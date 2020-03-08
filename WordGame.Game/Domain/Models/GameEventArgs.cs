namespace WordGame.Game.Domain.Models
{
    using System;
    using Players;

    public abstract class GameEventArgs : EventArgs
    {
        public Player EventByPlayer { get; set; }

        public EventType EventType { get; set; }
    }
}