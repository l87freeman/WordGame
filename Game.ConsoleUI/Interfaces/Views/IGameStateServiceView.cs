namespace Game.ConsoleUI.Interfaces.Views
{
    using Game.ConsoleUI.WordGame.Models;

    public interface IGameStateServiceView
    {
        bool ShouldRestoreGame(GameState gameState);

        void DisplayNotStored();
    }
}