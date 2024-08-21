using BusinessLogic;
using BusinessLogic.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.repositories;

public class ClientRepository
{
    private readonly IDbContextFactory<DataAccessContext> _contextFactory;
    
    public ClientRepository(IDbContextFactory<DataAccessContext> context)
    {
        _contextFactory = context;
    }
    
    public Client GetClient(string email)
    {
        using var context = _contextFactory.CreateDbContext();
        var client = context.Clients.Find(email);
        if (client != null)
        {
            return client;
        }
        throw new NullReferenceException();
    }
    
    public Client InsertClient(Client newClient)
    {
        using var context = _contextFactory.CreateDbContext();
        var client = context.Clients.Add(newClient);
        context.SaveChanges();
        return client.Entity;
    }

    public Client? GetAdmin()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Clients.FirstOrDefault(client => client.IsAdmin);
    }

    public bool IsAdminRegistered()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Clients.Any(client => client.IsAdmin);
    }
    
    
    public List<Client> GetClients()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Clients.ToList();
    }
}