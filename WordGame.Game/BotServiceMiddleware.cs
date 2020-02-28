namespace WordGame.Game
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class BotServiceMiddleware
    {
        private readonly ILogger<BotServiceMiddleware> logger;
        //private readonly IBotServiceProxy botService;

        public BotServiceMiddleware(ILogger<BotServiceMiddleware> logger/*, IBotServiceProxy botService*/)
        {
            this.logger = logger;
            //this.botService = botService;
        }

        public async Task Invoke(HttpContext context, RequestDelegate next)
        {
            if (this.ShouldAddBotAnswer(context))
            {
                this.CallBotService(context);
            }
            await next.Invoke(context);
        }

        private void CallBotService(HttpContext context)
        {
            throw new System.NotImplementedException();
        }

        private bool ShouldAddBotAnswer(HttpContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}