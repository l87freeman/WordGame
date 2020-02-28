namespace WordGame.Game.Infrastructure.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface IRemoteService : IDisposable
    {
    void SetBaseUri(string baseUri);

    Task<TResponse> GetAsync<TResponse>(string path) where TResponse : class;

    Task<TResponse> PostAsync<TRequest, TResponse>(string path, TRequest data) where TResponse : class
        where TRequest : class;
    }
}