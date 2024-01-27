using ArtVault.Components;
using ArtVault.DAOs;
using ArtVault.Business;

namespace ArtVault
{
    public class Program
    {
        public static void Main(string[] args)
        {
            

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddScoped<IArtVaultFacade>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();
            DAOConfig daoConfig = new DAOConfig();
            UtilizadorDAO UtiDAO = new UtilizadorDAO(daoConfig);

            // UtiDAO.InsertUtilizador("Gusto", "12345", "gusto@example.com", "Augusto Campos", "Viatodos, Barcelos", 123456789, 987654321, 1, true);
            app.Run();
        }

        
    }
}
