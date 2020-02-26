namespace Game.ConsoleUI.Game.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces.Views;
    using Models;

    public class GameManagerView : IGameManagerView
    {
        private readonly IBaseView baseView;

        public GameManagerView(IBaseView baseView)
        {
            this.baseView = baseView;
        }

        public void Refresh(GameState gameState)
        {
            var suggestedWords = gameState.ChallengeHistory.SelectMany(ch => ch.HistoryOfSuggestedResolutions).ToList();
            var usedWords = gameState.ChallengeHistory.Where(ch => ch.ChallengeResolution != null)
                .Select(ch => ch.ChallengeResolution).ToList();

            var message = this.Format(suggestedWords, usedWords);
            this.baseView.Refresh(message);
        }

        private string Format(List<string> suggestedWords, List<string> usedWords)
        {
            var separator = Environment.NewLine;
            var formatted = $"List of suggested words:{separator}{string.Join(separator, suggestedWords)}{separator}"
                            + $"List of used words:{separator}{string.Join(separator, usedWords)}{separator}";
            return formatted;
        }
    }
}