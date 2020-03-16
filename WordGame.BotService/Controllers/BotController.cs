using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WordGame.BotService.Controllers
{
    using System.Threading.Tasks;
    using Models;

    [ApiController]
    [Route("[controller]")]
    public class BotController : ControllerBase
    {
        private readonly ILogger<BotController> logger;
        private readonly IDictionaryProxy proxy;

        public BotController(ILogger<BotController> logger, IDictionaryProxy proxy)
        {
            this.logger = logger;
            this.proxy = proxy;
        }

        [HttpGet("resolve")]
        public async Task<Suggestion> Resolve([FromBody] ChallengeDto challenge)
        {
            var words = (await this.proxy.GetWords(challenge.Challenge)).Select(w => w.ToLower());
            var notUsed = words.Except(challenge.Suggestions.Select(s => s.Word.ToLower()));
            var suggestion = this.GetRandomSuggestion(notUsed);

            return new Suggestion { Word = suggestion, IsNotProvided = suggestion == null };
        }

        [HttpGet("words/{letter}")]
        public async Task<IEnumerable<string>> Test(char letter)
        {
            var words = (await this.proxy.GetWords(letter)).Select(w => w.ToLower());

            return words;
        }

        private string GetRandomSuggestion(IEnumerable<string> notUsed)
        {
            var list = notUsed.ToList();
            string result = string.Empty;
            if (list.Count > 0)
            {
                var resultIndex = new Random().Next(0, list.Count - 1);
                result = list[resultIndex];
            }

            return result;
        }
    }
}
