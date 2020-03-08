using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WordGame.Dictionary.Controllers
{
    using System.Threading.Tasks;
    using Infrastructure.Interfaces;

    [ApiController]
    [Route("[controller]")]
    public class DictionaryController : ControllerBase
    {
        private readonly ILogger<DictionaryController> logger;
        private readonly IWordStorage storage;

        public DictionaryController(ILogger<DictionaryController> logger, IWordStorage storage)
        {
            this.logger = logger;
            this.storage = storage;
        }

        [HttpGet("check/{word}")]
        public Task<bool> IsWordExists(string word)
        {
            this.logger.LogDebug($"If word exits request received for word {word}");
            return Task.Run<bool>(() => this.storage.IsWordExists(word));
        }

        [HttpGet("words/{letter}")]
        public Task<ISet<string>> GetWords(char letter)
        {
            this.logger.LogDebug($"Received request to get all words on {letter}");
            return Task.Run<ISet<string>>(() => this.storage.GetWords(letter));
        }
    }
}
