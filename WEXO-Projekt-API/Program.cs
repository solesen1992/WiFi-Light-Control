
using System.Security;
using WEXO_Project_BLL.BusinessLogic;
using WEXO_Project_BLL.Facade;
using WEXO_Projekt_DAL.DB;

namespace WEXO_Projekt_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //DAL Layer Injections
            //builder.Services.AddScoped<ILoginDB>(_ => new LoginDB());
            builder.Services.AddScoped<ILoginDB, LoginDB>();
            builder.Services.AddScoped<IBlackListDB, BlackListDB>();
            builder.Services.AddScoped<ISettingsDB, SettingsDB>();

            //BLL Layer Injections
            builder.Services.AddScoped<IBlackListBusinessLogic, BlackListBusinessLogic>();
            builder.Services.AddScoped<ISettingsBusinessLogic, SettingsBusinessLogic>();
            builder.Services.AddScoped<ILoginBusinessLogic, LoginBusinessLogic>();

            //Facade Layer Injection
            builder.Services.AddScoped<IAPIFacade, APIFacade>();
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Using CORS to talk from front end to API
            app.UseCors(builder =>
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
