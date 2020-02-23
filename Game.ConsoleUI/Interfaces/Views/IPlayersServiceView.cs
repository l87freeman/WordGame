namespace Game.ConsoleUI.Interfaces.Views
{
    public interface IPlayersServiceView
    {
        bool ApproveResolution(string playerName, string resolutionWord, string currentPlayerName);

        string ResolveChallenge(string playerName, char challengeChallengeLetter);
    }
}