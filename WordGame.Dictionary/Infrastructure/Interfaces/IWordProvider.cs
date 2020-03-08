namespace WordGame.Dictionary.Infrastructure.Interfaces
{
    using System.Collections.Generic;

    public interface IWordProvider
    {
        Dictionary<char, HashSet<string>> GetWords();
    }
}