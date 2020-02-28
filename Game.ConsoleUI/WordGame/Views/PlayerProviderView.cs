namespace Game.ConsoleUI.WordGame.Views
{
    using System.Collections.Generic;
    using Game.ConsoleUI.Interfaces.Views;

    public class PlayerProviderView : IPlayerProviderView
    {
        private readonly IBaseView baseView;

        public PlayerProviderView(IBaseView baseView)
        {
            this.baseView = baseView;
        }

        public List<string> GetPlayersNames()
        {
            var playersList = this.baseView.WaitForInputList("Please provide players list");
            if (playersList.Count == 0)
            {
                playersList = this.GetPlayersNames();
            }
            return playersList;
        }

        public bool ShouldIncludeBot()
        {
            var shouldIncludeBot = this.baseView.WaitForConfirmation("Do you want to play with bot?");
            return shouldIncludeBot;
        }
    }
}