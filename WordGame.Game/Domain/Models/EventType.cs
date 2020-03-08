namespace WordGame.Game.Domain.Models
{
    public enum EventType
    {
        PlayerLeft,
        PlayerJoined,
        NewChallenge,
        ResolutionProvided,
        ResolutionChecked,
        ApproveReceived
    }
}