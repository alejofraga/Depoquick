using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

using BusinessLogic;
using Controllers;
using DataLayer;
using DataLayer.repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContextFactory<DataAccessContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        providerOptions => providerOptions.EnableRetryOnFailure())
);
builder.Services.AddSingleton<ClientRepository>();
builder.Services.AddSingleton<DepositRepository>();
builder.Services.AddSingleton<NotificationsRepository>();
builder.Services.AddSingleton<PromotionRepository>();
builder.Services.AddSingleton<ReservationRepository>();
builder.Services.AddSingleton<ReviewRepository>();
builder.Services.AddSingleton<LogRepository>();
builder.Services.AddSingleton<ClientController>();
builder.Services.AddSingleton<DepositController>();
builder.Services.AddSingleton<ReservationController>();
builder.Services.AddSingleton<PromotionController>();

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

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

