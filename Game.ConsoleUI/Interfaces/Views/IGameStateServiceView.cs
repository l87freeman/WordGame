namespace Game.ConsoleUI.Interfaces.Views
{
    using Game.Models;

    public interface IGameStateServiceView
    {
        bool ShouldRestoreGame(GameState gameState);

        void DisplayNotStored();
    }
}