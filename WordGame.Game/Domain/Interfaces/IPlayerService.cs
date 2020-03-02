namespace WordGame.Game.Domain.Interfaces
{
    using System;
    using Models;

    public interface IPlayerService
    {
        event EventHandler<string> PlayersChanged;

        void Add(PlayerInfo player);

        void Remove(PlayerInfo player);
    }
}