namespace WordGame.Game.Domain.Interfaces
{
    using System;
    using Models;
    using Models.Players;

    public interface IPlayerService
    {
        event EventHandler<PlayerEventData> PlayersChanged;

        void Add(PlayerInfo player);

        void Remove(PlayerInfo player);

        void NextPlayer();
    }
}