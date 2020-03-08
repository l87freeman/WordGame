﻿namespace WordGame.Game
{
    using Microsoft.Extensions.Options;

    public class BotConfiguration : IOptions<BotConfiguration>
    {
        public string Address { get; set; }

        public BotConfiguration Value => this;
    }
}