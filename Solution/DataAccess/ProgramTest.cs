using BusinessLogic;
using BusinessLogic.Domain;
using DataLayer.repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Tests;

public class ProgramTest
{
    private SqliteConnection _inMemorySqlite;

    public readonly ServiceProvider ServiceProvider;
    
    public ProgramTest()
    {
        _inMemorySqlite = new SqliteConnection("Data Source=:memory:");
        _inMemorySqlite.Open();
        var services = new ServiceCollection();
        services.AddDbContextFactory<DataAccessContext>(options => { options.UseSqlite(_inMemorySqlite); });
        services.AddSingleton<ClientRepository>();
        services.AddSingleton<DepositRepository>();
        services.AddSingleton<NotificationsRepository>();
        services.AddSingleton<PromotionRepository>();
        services.AddSingleton<ReservationRepository>();
        services.AddSingleton<ReviewRepository>();
        services.AddSingleton<LogRepository>();
        services.AddSingleton<Notification>();
        ServiceProvider = services.BuildServiceProvider();
    }
}