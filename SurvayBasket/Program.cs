using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using SurvayBasket.DependancyInjection;

namespace SurvayBasket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDependancyInjection(builder.Configuration);

            //  builder.Services.AddResponseCaching();
            builder.Services.AddDistributedMemoryCache();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseCors("ServayBasketPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            // app.UseResponseCaching();

            app.MapControllers();
            app.UseExceptionHandler();
            app.UseRateLimiter();
            app.MapHealthChecks("health",new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
              

            });

            app.Run();
        }
    }
}
