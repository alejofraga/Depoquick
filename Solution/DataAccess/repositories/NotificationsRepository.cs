using BusinessLogic;
using BusinessLogic.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.repositories;

public class NotificationsRepository
{
    private readonly IDbContextFactory<DataAccessContext> _contextFactory;

    public NotificationsRepository(IDbContextFactory<DataAccessContext> dataAccessContext)
    {
        _contextFactory = dataAccessContext;
    }

    public Notification InsertNotification(Notification newNotification)
    {
        using var context = _contextFactory.CreateDbContext();
        var notification = context.Notifications.Add(newNotification);
        context.SaveChanges();
        return notification.Entity;
    }
    
    public void DeleteNotification(Notification notification)
    {
        using var context = _contextFactory.CreateDbContext();
        context.Notifications.Remove(notification);
        context.SaveChanges();
    }

    
    public List<Notification> GetClientNotifications(string clientEmail)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Notifications.Where(n => n.ClientEmail == clientEmail).ToList();
    }
    
    
    
}