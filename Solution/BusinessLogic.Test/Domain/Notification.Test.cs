using BusinessLogic.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.Domain;

[TestClass]
public class DetailedNotification_Test
{
    [TestMethod]
    public void ShouldCreateDetailedNotification()
    {
        var detailedNotification = new Notification();
        Assert.IsNotNull(detailedNotification);
    }

    [TestMethod]
    public void ShouldSetDetailedNotification()
    {
        var detailedNotification = new Notification();
        const ValidNotifications notification = ValidNotifications.ReservationConfirmed;
        const int notificationType = (int) notification;
        const string depositName = "pepes";
        DateTime startDate = DateTime.Today;
        DateTime endDate = DateTime.Today.AddDays(5);

        detailedNotification.NotificationType = notificationType;
        detailedNotification.DepositName = depositName;
        detailedNotification.StartDate = startDate;
        detailedNotification.EndDate = endDate;
        
        Assert.AreEqual(detailedNotification.NotificationType, notificationType);
        Assert.AreEqual(detailedNotification.DepositName, depositName);
        Assert.AreEqual(detailedNotification.StartDate, startDate);
        Assert.AreEqual(detailedNotification.EndDate, endDate);
    }

    [TestMethod]
    public void ShouldCreateDetailedNotificationWithParameters()
    {
        const ValidNotifications notification = ValidNotifications.ReservationConfirmed;
        const int notificationType = (int) notification;
        const string depositName = "pepes";
        DateTime startDate = DateTime.Today;
        DateTime endDate = DateTime.Today.AddDays(5);
        var detailedNotification = new Notification(notificationType, depositName, startDate, endDate);
        Assert.AreEqual(detailedNotification.NotificationType, notificationType);
        Assert.AreEqual(detailedNotification.DepositName, depositName);
        Assert.AreEqual(detailedNotification.StartDate, startDate);
        Assert.AreEqual(detailedNotification.EndDate, endDate);
    }
    
}