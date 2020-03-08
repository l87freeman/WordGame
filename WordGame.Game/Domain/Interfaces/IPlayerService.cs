namespace WordGame.Game.Domain.Interfaces
{
    using System.Collections.Generic;
    using Models.Players;

    public interface IPlayerService
    {
        Player CurrentPlayer { get; }
        
        List<Player> ActivePlayers { get; }
        
        List<Player> ActivePlayersNoCurrent { get; }

        bool IsGameWithBot { get; }

        Player GetPlayer(string playerId);

        void Add(Player player);

        void Remove(Player player);

        void NextPlayer();

        void Reset();
    }
}