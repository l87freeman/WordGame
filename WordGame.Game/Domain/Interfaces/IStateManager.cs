namespace WordGame.Game.Domain.Interfaces
{
    using Dto;

    public interface IStateManager
    {
        void SaveState(GameDto gameDto);
    }
}