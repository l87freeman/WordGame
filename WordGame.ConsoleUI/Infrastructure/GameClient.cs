namespace WordGame.ConsoleUI.Infrastructure
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Domain.Interfaces;
    using Domain.Models;
    using Interfaces;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    public class GameClient : IGameClient
    {
        private HubConnection connection;
        private readonly IPlayerNameProvider playerNameProvider;
        private readonly IGameManager gameManager;
        private readonly GameConfiguration config;
        private string connectionId;

        public GameClient(IOptions<GameConfiguration> gameOptions, IPlayerNameProvider playerNameProvider, IGameManager gameManager)
        {
            this.config = gameOptions.Value;
            this.playerNameProvider = playerNameProvider;
            this.gameManager = gameManager;
        }

        public void Start()
        {
            var playerName = this.playerNameProvider.GetPlayerName();
            this.BuildConnectionForPlayer(playerName);
            this.StartSession();
        }

        private void StartSession()
        {
            this.connection.StartAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                        task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected to game");
                    this.CreateSubscriptions();
                }
            }).Wait();
        }

        private void BuildConnectionForPlayer(string playerName)
        {
            this.connection = new HubConnectionBuilder()
                .WithAutomaticReconnect()
                .WithUrl(this.config.ServerAddress, options =>
                {
                    options.Cookies = new CookieContainer();
                    options.Cookies.Add(new Uri(this.config.ServerAddress), new Cookie("userName", playerName));
                })
                .Build();

            this.connection.Closed += this.Connection_Closed;
            this.connection.Reconnecting += this.Connection_Reconnecting;
            this.connection.Reconnected += this.Connection_Reconnected;
        }

        private Task Connection_Reconnected(string arg)
        {
            Console.WriteLine($"Reconnected with: {arg}, old value: {this.connectionId}");
            return Task.CompletedTask;
        }

        private Task Connection_Reconnecting(Exception arg)
        {
            Console.WriteLine($"Connection started reconnecting due to an error: {arg}");
            return Task.CompletedTask;
        }

        private Task Connection_Closed(Exception arg)
        {
            if (arg == null)
            {
                Console.WriteLine("Connection closed without error.");
            }
            else
            {
                Console.WriteLine($"Connection closed due to an error: {arg}");
            }
            return Task.CompletedTask;
        }

        private void CreateSubscriptions()
        {
            this.connection.On<Game>("GameUpdated", game =>
            {
                this.gameManager.RefreshUi(game);
            });
            this.connection.On<Suggestion>("NeedApproval", suggestion => { this.gameManager.Approve(suggestion); });
            this.connection.On<Challenge>("NewChallenge", challenge => { this.gameManager.Resolve(challenge); });
            this.connection.On<string>("Notify", message =>
            {
                this.gameManager.Display(message);
            });
            this.gameManager.Approved +=
                (sender, arg) =>
                {
                    this.connectionId ??= this.connection.ConnectionId;
                    this.connection.InvokeAsync<string>("Approved", arg.ToString()).GetAwaiter().GetResult();
                };
            this.gameManager.Resolved +=
                (sender, arg) =>
                {
                    this.connection.InvokeAsync<Suggestion>("Resolved", arg).GetAwaiter().GetResult();
                };
        }

        public void Dispose()
        {
            this.connection.StopAsync().Wait();
        }
    }
}