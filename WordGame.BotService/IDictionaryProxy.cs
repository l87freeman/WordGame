namespace WordGame.BotService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDictionaryProxy
    {
        Task<ISet<string>> GetWords(char letter);
    }
}