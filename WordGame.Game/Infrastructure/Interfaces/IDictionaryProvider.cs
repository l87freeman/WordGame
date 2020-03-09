namespace WordGame.Game.Infrastructure.Interfaces
{
    using System.Collections.Generic;

    public interface IDictionaryProvider
    {
        List<string> GetWords(char challengeLetter);

        bool IsWordExists(string word);
    }
}