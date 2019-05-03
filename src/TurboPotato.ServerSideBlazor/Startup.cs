using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Jil;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TurboPotato.ServerSideBlazor.Data;

namespace TurboPotato.ServerSideBlazor
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddHttpClient();

            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<InMemoryChatMessageRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            InMemoryChatMessageRepository chatMessageRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/api"))
                {
                    if (context.Request.Method == "PUT")
                    {
                        using (var body = context.Request.Body)
                        using (var reader = new StreamReader(body))
                        {
                            var json = await reader.ReadToEndAsync();
                            Console.WriteLine("PUT: " + json);

                            chatMessageRepository.AddChatMessage(
                                JSON.Deserialize<ChatMessage>(json));
                        }
                    } else
                    {
                        var json = JSON.Serialize(
                            chatMessageRepository.GetChatMessages());
                        Console.WriteLine("GET: " + json);

                        await context.Response.WriteAsync(json);
                    }
                }
                else
                {
                    await next();
                }
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
