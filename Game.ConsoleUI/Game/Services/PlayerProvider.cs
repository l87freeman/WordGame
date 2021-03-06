﻿namespace Game.ConsoleUI.Game.Services
{
    using System.Collections.Generic;
    using global::Game.ConsoleUI.Interfaces.Services;
    using global::Game.ConsoleUI.Interfaces.Views;
    using Models;
    using Serilog;

    public class PlayerProvider : BaseServiceWithLogger<PlayerProvider>, IPlayerProvider
    {
        private readonly IPlayerProviderView view;

        public PlayerProvider(ILogger logger, IPlayerProviderView view) : base(logger)
        {
            this.view = view;
        }

        public List<GamePlayer> GetPlayers()
        {
            var playersNames = this.view.GetPlayersNames();
            var isGameWithBot = this.view.ShouldIncludeBot();
            var players = new List<GamePlayer>();

            foreach (var playersName in playersNames)
            {
                players.Add(new GamePlayer(playersName));
            }
            if (isGameWithBot)
            {
                players.Add(new GamePlayerBot());
            }

            return players;
        }
    }
}