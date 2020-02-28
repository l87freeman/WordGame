namespace WordGame.Game
{
    using System.Reflection;
    using AutoMapper;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using WordGame.Game.Controllers;
    using WordGame.Game.Domain.Interfaces;
    using WordGame.Game.Domain.Services;

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
            services.Configure<GameConfiguration>(this.Configuration.GetSection(nameof(GameConfiguration)));

            services.AddControllers();
            services.AddSignalR();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddSingleton<IGameManager, GameManager>();
            services.AddSingleton<IGameStateProvider, GameStateProvider>();
            services.AddSingleton<IGameChallengeValidator, GameChallengeValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PlayersHub>("/gameHub");
            });
        }
    }

   
}
