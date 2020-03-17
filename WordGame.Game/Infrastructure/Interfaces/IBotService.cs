namespace WordGame.Game.Infrastructure.Interfaces
{
    using System;

    public interface IBotService
    {
        void OnResolutionProvided(Action<string, Dto.Suggestion> onResolutionProvided);

        void OnApprovalProvided(Action<string, string> onApprovalProvided);
        
        void NewChallenge();

        void NeedApproval(Dto.Suggestion suggestion);
    }
}