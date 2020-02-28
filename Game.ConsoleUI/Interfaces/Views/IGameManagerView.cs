namespace Game.ConsoleUI.Interfaces.Views
{
    using Game.ConsoleUI.WordGame.Models;

    public interface IGameManagerView
    {
        void Refresh(GameState gameState);
    }
}