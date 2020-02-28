namespace Game.ConsoleUI.WordGame.Views
{
    using Game.ConsoleUI.Interfaces.Views;
    using Game.ConsoleUI.WordGame.Models;

    public class GameStateServiceView : IGameStateServiceView
    {
        private readonly IBaseView baseView;

        public GameStateServiceView(IBaseView baseView)
        {
            this.baseView = baseView;
        }

        public bool ShouldRestoreGame(GameState gameState)
        {
            var message = $"There is a stored game, should it be restored:{gameState}";
            var conformed = this.baseView.WaitForConfirmation(message);

            return conformed;
        }

        public void DisplayNotStored()
        {
            var message = "Was not able to store game, please contact support";
            this.baseView.ShowWarning(message);
        }
    }
}