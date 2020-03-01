namespace WordGame.ConsoleUI.Domain
{
    using Interfaces;
    using Views.Interfaces;

    public class PlayerNameProvider : IPlayerNameProvider
    {
        private readonly IBaseView baseView;

        public PlayerNameProvider(IBaseView baseView)
        {
            this.baseView = baseView;
        }

        public string GetPlayerName()
        {
            //TODO add call to server in order to check if name unique;
            string playerName = null;
            while (string.IsNullOrWhiteSpace(playerName) || playerName.Length < 2)
            {
                playerName = baseView.WaitForInput("Enter your name (more than 2 symbols and not whitespaces)");
            }

            return playerName;
        }
    }
}