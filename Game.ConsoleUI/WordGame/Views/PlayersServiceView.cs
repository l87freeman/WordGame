namespace Game.ConsoleUI.WordGame.Views
{
    using Game.ConsoleUI.Interfaces.Views;

    public class PlayersServiceView : IPlayersServiceView
    {
        private readonly IBaseView baseView;

        public PlayersServiceView(IBaseView baseView)
        {
            this.baseView = baseView;
        }

        public bool ApproveResolution(string playerName, string resolutionWord, string currentPlayer)
        {
            var agreed = this.baseView.WaitForConfirmation($"{playerName} please agree resolution from {currentPlayer} - {resolutionWord}");

            return agreed;
        }

        public string ResolveChallenge(string playerName, char challengeChallengeLetter)
        {
            var resolution =
                this.baseView.WaitForInput($"{playerName} please provide valid word starting on {challengeChallengeLetter}");

            return resolution;
        }
    }
}