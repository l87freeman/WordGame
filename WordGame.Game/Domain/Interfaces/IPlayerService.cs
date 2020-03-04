namespace WordGame.Game.Domain.Interfaces
{
    using System;
    using Models;

    public interface IPlayerService
    {
        event EventHandler<PlayerEventData> PlayersChanged;

        void Add(PlayerInfo player);

        void Remove(PlayerInfo player);

        PlayerInfo NextPlayer(PlayerInfo currentPlayer);
    }
}