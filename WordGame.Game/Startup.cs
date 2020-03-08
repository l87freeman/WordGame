namespace WordGame.Game
{
    using System.Reflection;
    using AutoMapper;
    using Domain;
    using Domain.Interfaces;
    using Domain.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using WordGame.Game.Controllers;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.Configure<GameConfiguration>(this.Configuration.GetSection(nameof(GameConfiguration)));
            services.Configure<BotConfiguration>(this.Configuration.GetSection(nameof(BotConfiguration)));
            services.Configure<StateManagerConfiguration>(this.Configuration.GetSection(nameof(StateManagerConfiguration)));
            services.Configure<DictionaryProxyConfiguration>(this.Configuration.GetSection(nameof(DictionaryProxyConfiguration)));
            services.AddSingleton<ICommunicationProxy, CommunicationProxy>();
            services.AddSingleton<IPlayerService, PlayerService>();
            services.AddSingleton<IChallengeService, ChallengeService>();
            services.AddSingleton<IGameServiceFactory, GameServiceFactory>();
            services.AddSingleton<IChallengeResolutionValidator, ChallengeResolutionValidator>();
            services.AddSingleton<IDictionaryProvider, DictionaryProxy>();
            services.AddSingleton<IBotService, BotServiceRemote>();
            services.AddSingleton<IStateManager, StateManager>();

            services.AddControllers();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GameHub>("gameHub");
            });
        }
    }

   
}
