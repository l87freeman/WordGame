namespace WordGame.Game.Infrastructure.Interfaces
{
    public interface IGameServiceFactory
    {
        IGameService Create(ICommunicationProxy communicationProxy);
    }
}