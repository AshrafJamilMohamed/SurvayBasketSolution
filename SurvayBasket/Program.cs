

using Microsoft.IdentityModel.Tokens;
using SurvayBasket.DependancyInjection;
using SurvayBasket.ErrorHandling;
using SurvayBasket.Service;
using System.Text;

namespace SurvayBasket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDependancyInjection(builder.Configuration);


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


            app.MapControllers();
            app.UseExceptionHandler();

            app.Run();
        }
    }
}
