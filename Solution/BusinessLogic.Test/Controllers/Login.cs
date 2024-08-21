using System.Security;
using Controllers;
using DataLayer.repositories;
using DataLayer.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.FacadeManagement;

[TestClass]
public class Login
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
    public void ShouldReturnFalseIfThereIsNoAdmin()
    {
        Assert.IsFalse(_clientController.AdminRegistered());
    }

    [TestMethod]
    public void ShouldLogOut()
    {
        const string? adminExpectedPassword1 = "#theBestAdmin1234";
        const string adminExpectedPassword2 = "#theBestAdmin1234";
        const string? adminExpectedName = "AdministratorName";
        const string? adminExpectedEmail = "theBestAdminEmail@gmail.com";
        _clientController.RegisterAdmin(adminExpectedName, adminExpectedEmail, adminExpectedPassword1, adminExpectedPassword2);
        _clientController.SetActiveUser(adminExpectedEmail);
        _clientController.LogOut();
        Assert.IsNull(_clientController.GetActiveUser());
    }
    [TestMethod]
    public void ShouldSetActiveUser()
    {
        const string? adminExpectedPassword1 = "#theBestAdmin1234";
        const string adminExpectedPassword2 = "#theBestAdmin1234";
        const string? adminExpectedName = "AdministratorName";
        const string? adminExpectedEmail = "theBestAdminEmail@gmail.com";
        _clientController.RegisterAdmin(adminExpectedName, adminExpectedEmail, adminExpectedPassword1, adminExpectedPassword2);
        _clientController.SetActiveUser(adminExpectedEmail);
        Assert.AreEqual(_clientController.GetActiveUser().Email, adminExpectedEmail);
    }

    [TestMethod]

    public void ShouldLoginIfCredentialsAreOk()
    {
        const string? adminExpectedPassword1 = "#theBestAdmin1234";
        const string adminExpectedPassword2 = "#theBestAdmin1234";
        const string? adminExpectedName = "AdministratorName";
        const string? adminExpectedEmail = "theBestAdminEmail@gmail.com";
        _clientController.RegisterAdmin(adminExpectedName, adminExpectedEmail, adminExpectedPassword1, adminExpectedPassword2);
        const string passwordRecieved = "#theBestAdmin1234";
        const string emailRecieved = "theBestAdminEmail@gmail.com";
        _clientController.Login(emailRecieved, passwordRecieved);
        Assert.IsTrue(_clientController.GetActiveUser().Email == emailRecieved);
    }
    
    [TestMethod]

    public void ShouldThrowExceptionWhenAttemptingToLoginAndCredentialsAreIncorrect()
    {
        const string? adminExpectedPassword1 = "#theBestAdmin1234";
        const string adminExpectedPassword2 = "#theBestAdmin1234";
        const string? adminExpectedName = "AdministratorName";
        const string? adminExpectedEmail = "theBestAdminEmail@gmail.com";
        _clientController.RegisterAdmin(adminExpectedName, adminExpectedEmail, adminExpectedPassword1, adminExpectedPassword2);
        const string wrongPassword = "#WrongPassword";
        const string emailRecieved = "theBestAdminEmail@gmail.com";
        Assert.ThrowsException<SecurityException>(() => _clientController.Login(emailRecieved, wrongPassword));
    }
    
    [TestMethod]

    public void ShouldThrowExceptionWhenAttemptingToLoginAndNotRegistered()
    {
        const string passwordRecieved = "#theBestAdmin1234";
        const string emailRecieved = "theBestAdminEmail@gmail.com";
        Assert.ThrowsException<NullReferenceException>(() => _clientController.Login(emailRecieved, passwordRecieved));
    }
}