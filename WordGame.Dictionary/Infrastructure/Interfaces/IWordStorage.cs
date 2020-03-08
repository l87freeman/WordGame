namespace WordGame.Dictionary.Infrastructure.Interfaces
{
    using System.Collections.Generic;

    public interface IWordStorage
    {
        ISet<string> GetWords(char startLetter);

        bool IsWordExists(string word);
    }
}