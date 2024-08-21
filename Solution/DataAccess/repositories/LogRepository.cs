using BusinessLogic;
using BusinessLogic.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.repositories;

public class LogRepository
{
    private readonly IDbContextFactory<DataAccessContext> _contextFactory;
    
    public LogRepository(IDbContextFactory<DataAccessContext> dataAccessContext)
    {
        _contextFactory = dataAccessContext;
    }
    
    public Log InsertAction(Log newAction)
    {
        using var context = _contextFactory.CreateDbContext();
        var action = context.Actions.Add(newAction);
        context.SaveChanges();
        return action.Entity;
    }
    
    public List<Log> GetClientActionInLog(string clientEmail)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Actions.Where(n => n.ClientEmail == clientEmail).ToList();
    }
}