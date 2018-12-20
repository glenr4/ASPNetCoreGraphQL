using GraphiQl;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using NHLStats.Api.Models;
using NHLStats.Core.Data;
using NHLStats.Data;
using NHLStats.Data.Repositories;

namespace NHLStats.Api
{
    public class Startup
    {
        // Log everything
        //public static readonly LoggerFactory MyLoggerFactory
        //	= new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });

        // Log only query execution
        //public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[]
        //                                        {
        //                                            new ConsoleLoggerProvider((category, level)
        //                                                => category == DbLoggerCategory.Database.Command.Name
        //                                                   && level == LogLevel.Information, true)
        //                                        });

        public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new[]
                                                {
                                                    new DebugLoggerProvider( (category, level) =>
                                                        category == DbLoggerCategory.Database.Command.Name
                                                           && level == LogLevel.Information)
                                                });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<NHLStatsContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:NHLStatsDb"])
                                                                        .UseLoggerFactory(MyLoggerFactory)  // Warning: Do not create a new ILoggerFactory instance each time
                                                                        );

            services.AddTransient<IPlayerRepository, PlayerRepository>();
            services.AddTransient<ISkaterStatisticRepository, SkaterStatisticRepository>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<NHLStatsQuery>();
            services.AddSingleton<NHLStatsMutation>();
            services.AddSingleton<PlayerType>();
            services.AddSingleton<PlayerInputType>();
            services.AddSingleton<SkaterStatisticType>();
            services.AddSingleton<AddressType>();

            ServiceProvider sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new NHLStatsSchema(new FuncDependencyResolver(type => sp.GetService(type))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, NHLStatsContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphiQl();
            app.UseMvc();
            db.EnsureSeedData();
        }
    }
}