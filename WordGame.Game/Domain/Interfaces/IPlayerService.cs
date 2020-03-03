namespace WordGame.Game.Domain.Interfaces
{
    using System;
    using Models;

    public interface IPlayerService
    {
        event EventHandler<string> PlayersChanged;

        event EventHandler<EventArgs> OnePlayerLeft;

        void Add(PlayerInfo player);

        void Remove(PlayerInfo player);

        PlayerInfo NextPlayer(PlayerInfo currentPlayer);
    }
}