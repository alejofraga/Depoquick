using BusinessLogic.Domain;
using Controllers;
using DataLayer.repositories;
using DataLayer.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class NotificationsManagement
{
    private ClientController _clientController;
    [TestInitialize]
    public void OnInitialize()
    {
        var programTest = new ProgramTest();
        var scope = programTest.ServiceProvider.CreateScope();
        var promotionRepository = scope.ServiceProvider.GetRequiredService<PromotionRepository>();
        var depositRepository = scope.ServiceProvider.GetRequiredService<DepositRepository>();
        var reservationRepository = scope.ServiceProvider.GetRequiredService<ReservationRepository>();
        var clientRepository = scope.ServiceProvider.GetRequiredService<ClientRepository>();
        var logRepository = scope.ServiceProvider.GetRequiredService<LogRepository>();
        var notificationsRepository = scope.ServiceProvider.GetRequiredService<NotificationsRepository>();
        _clientController = new ClientController(clientRepository, depositRepository,
            promotionRepository, notificationsRepository,
            logRepository, reservationRepository);
    }
    
    [TestMethod]
    public void ShouldCreateNotifications()
    {
        var notifications = new Notification();
        Assert.IsNotNull(notifications);
    }
    
    [TestMethod]
    public void ShouldSetNewNotification()
    {
        const ValidNotifications notification = ValidNotifications.ReservationConfirmed;
        const int notificationType = (int) notification;
        const string depositName = "pepes";
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(5);
        var detailedNotification = new Notification
        {
            NotificationType = notificationType,
            DepositName = depositName,
            StartDate = startDate,
            EndDate = endDate
        };
        Assert.IsTrue(detailedNotification.NotificationType == notificationType);
        Assert.IsTrue(detailedNotification.DepositName == depositName);
        Assert.IsTrue(detailedNotification.StartDate == startDate);
        Assert.IsTrue(detailedNotification.EndDate == endDate);

    }
    
    
    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToAddNonExistentNotifications()
    {
        const ValidNotifications nonExistentNotification = (ValidNotifications) 4;
        const int nonExistentNotificationType = (int) nonExistentNotification;
        const string depositName = "pepes";
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(5);
        Assert.ThrowsException<ArgumentException>(() => new Notification(nonExistentNotificationType, depositName, startDate, endDate));
    }

    [TestMethod]
    public void ShouldAddNotificationToClient()
    {
        const string name = "Sebastian";
        const string email = "seba@gmail.com";
        const string password1 = "@Seba1234@";
        const string password2 = "@Seba1234@";
        _clientController.RegisterClient(name, email, password1, password2);
        const ValidNotifications notification = ValidNotifications.ReservationConfirmed;
        const int notificationType = (int) notification;
        const string depositName = "pepes";
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(5);
        _clientController.AddNotificationToClient(email, notificationType, depositName, startDate, endDate);
        var notifications = _clientController.GetClientNotifications(email);
        Assert.IsNotNull(notifications.Find(notificationLog => notificationLog.NotificationType == notificationType));
    }

    [TestMethod]
    public void ShouldAddMultipleNotificationsToClient()
    {
        const string name = "Sebastian";
        const string email = "seba@gmail.com";
        const string password1 = "@Seba1234@";
        const string password2 = "@Seba1234@";
        _clientController.RegisterClient(name, email, password1, password2);
        const ValidNotifications notification1 = ValidNotifications.ReservationConfirmed;
        const int notificationType1 = (int) notification1;
        const ValidNotifications notification2 = ValidNotifications.ReservationRejected;
        const int notificationType2 = (int) notification2;
        const string depositName = "pepes";
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(5);
        _clientController.AddNotificationToClient(email,notificationType1, depositName, startDate, endDate);
        _clientController.AddNotificationToClient(email,notificationType2, depositName, startDate, endDate);
        var notifications = _clientController.GetClientNotifications(email);
        Assert.IsNotNull(notifications.Find(notificationLog => notificationLog.NotificationType == notificationType1));
        Assert.IsNotNull(notifications.Find(notificationLog => notificationLog.NotificationType == notificationType2));
    }

    [TestMethod]
    public void ShouldDeleteNotificationToClient()
    {
        const string name = "Sebastian";
        const string email = "seba@gmail.com";
        const string password1 = "@Seba1234@";
        const string password2 = "@Seba1234@";
        _clientController.RegisterClient(name, email, password1, password2);
        const ValidNotifications notification = ValidNotifications.ReservationConfirmed;
        const int notificationType = (int) notification;
        const string depositName = "pepes";
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(5);
        _clientController.AddNotificationToClient(email,notificationType, depositName, startDate, endDate);
        var notifications = _clientController.GetClientNotifications(email);
        var notificationToDelete = notifications[0];
        _clientController.RemoveNotification(notificationToDelete);
        Assert.IsFalse(_clientController.GetClientNotifications(email).Contains(notificationToDelete));
    }
    
    [TestMethod]
    public void ShouldGetClientNotifications()
    {
        const string name = "Sebastian";
        const string email = "seba@gmail.com";
        const string password1 = "@Seba1234@";
        const string password2 = "@Seba1234@";
        _clientController.RegisterClient(name, email, password1, password2);
        const ValidNotifications notification1 = ValidNotifications.ReservationConfirmed;
        const int notificationType1 = (int) notification1;
        const string depositName = "pepes";
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(5);
        _clientController.AddNotificationToClient(email, notificationType1, depositName, startDate, endDate);
        const ValidNotifications notification2 = ValidNotifications.ReservationRejected;
        const int notificationType2 = (int) notification2;
        _clientController.AddNotificationToClient(email, notificationType2, depositName, startDate, endDate);
        var notifications = _clientController.GetClientNotifications(email);
        Assert.IsNotNull(notifications.Find(notificationLog => notificationLog.NotificationType == notificationType1));
        Assert.IsNotNull(notifications.Find(notificationLog => notificationLog.NotificationType == notificationType2));
    }
    
}