﻿namespace WordGame.Game.Domain.Models.Players
{
    using System;

    public class PlayerEventData : EventArgs
    {
        public PlayerInfo EventByPlayer { get; set; }

        public PlayerEventType EventType { get; set; }

        public int PlayersCount { get; set; }
    }
}