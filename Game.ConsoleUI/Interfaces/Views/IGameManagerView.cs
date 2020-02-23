namespace Game.ConsoleUI.Interfaces.Views
{
    using Game.Models;

    public interface IGameManagerView
    {
        void Refresh(GameState gameState);
    }
}