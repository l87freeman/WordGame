namespace WordGame.GameState.Storage
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Interfaces;
    using Models;

    public class StateStorage : IStateStorage
    {
        readonly ConcurrentDictionary<DateTime, GameDto> stateStorage = new ConcurrentDictionary<DateTime, GameDto>();
        
        public Task SaveAsync(GameDto state)
        {
            this.stateStorage.TryAdd(DateTime.Now, state);
            return Task.CompletedTask;
        }
    }
}