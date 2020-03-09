namespace WordGame.Game.Infrastructure.Interfaces
{
    using Dto;

    public interface IStateManager
    {
        void SaveState(GameDto gameDto);
    }
}