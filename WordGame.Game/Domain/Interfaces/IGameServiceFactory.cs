namespace WordGame.Game.Domain.Interfaces
{
    public interface IGameServiceFactory
    {
        IGameService Create(ICommunicationProxy communicationProxy);
    }
}