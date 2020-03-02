namespace WordGame.ConsoleUI.Infrastructure
{
    using System;
    using System.Net;
    using Domain.Interfaces;
    using Domain.Models;
    using Interfaces;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Options;

    public class GameClient : IGameClient
    {
        private HubConnection connection;
        private readonly IPlayerNameProvider playerNameProvider;
        private readonly IGameManager gameManager;
        private readonly GameConfiguration config;

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
        }

        private void CreateSubscriptions()
        {
            this.connection.On<Game>("Updated", game => { this.gameManager.RefreshUi(game); });
            this.connection.On<Suggestion>("NeedApproval", suggestion => { this.gameManager.Approve(suggestion); });
            this.connection.On<Challenge>("Challenge", challenge => { this.gameManager.Resolve(challenge); });
            this.connection.On<string>("Notify", message => { this.gameManager.Display(message); });
            this.gameManager.Approved +=
                (sender, arg) => this.connection.InvokeAsync<bool>("Approved", arg);
            this.gameManager.BotInteractionChanged +=
                (sender, arg) => this.connection.InvokeAsync("ChangedBotInteraction");
            this.gameManager.Resolved +=
                (sender, arg) => this.connection.InvokeAsync<string>("Resolved", arg);
        }

        public void Dispose()
        {
            this.connection.StopAsync().Wait();
        }
    }
}