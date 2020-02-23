namespace Game.ConsoleUI.Interfaces.Views
{
    using System.Collections.Generic;

    public interface IPlayerProviderView
    {
        List<string> GetPlayersNames();

        bool ShouldIncludeBot();
    }
}