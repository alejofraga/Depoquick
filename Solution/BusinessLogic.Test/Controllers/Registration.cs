using Controllers;
using DataLayer.repositories;
using DataLayer.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.FacadeManagement;

[TestClass]
public class Registration
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
    public void ShouldRegisterAdminIfNotRegistered()
    {
        const string? adminExpectedPassword1 = "#theBestAdmin1234";
        const string adminExpectedPassword2 = "#theBestAdmin1234";
        const string? adminExpectedName = "AdministratorName";
        const string? adminExpectedEmail = "theBestAdminEmail@gmail.com";

        _clientController.RegisterAdmin(adminExpectedName, adminExpectedEmail, adminExpectedPassword1, adminExpectedPassword2);

        Assert.AreEqual(_clientController.GetAdmin().Email, adminExpectedEmail);
    }


    [TestMethod]
    public void ShouldRegisterClientIfNotRegistered()
    {
        const string clientExpectedPassword1 = "#theBestClient1234";
        const string clientExpectedPassword2 = "#theBestClient1234";
        const string clientExpectedName = "ClientThatIsNoAdmin";
        const string clientExpectedEmail = "theBestClientEmail@gmail.com";

        _clientController.RegisterClient(clientExpectedName, clientExpectedEmail, clientExpectedPassword1,
            clientExpectedPassword2);
        var client = _clientController.GetClient(clientExpectedEmail);
        Assert.IsTrue(_clientController.ClientIsRegistered(client));
    }

    [TestMethod]
    public void ShouldThrowExceptionIfTriesToRegisterClientAndIsRegistered()
    {
        const string clientExpectedPassword1 = "#theBestClient1234";
        const string clientExpectedPassword2 = "#theBestClient1234";
        const string clientExpectedName = "ClientThatIsNoAdmin";
        const string clientExpectedEmail = "theBestClientEmail@gmail.com";

        _clientController.RegisterClient(clientExpectedName, clientExpectedEmail, clientExpectedPassword1,
            clientExpectedPassword2);

        Assert.ThrowsException<DbUpdateException>(() => _clientController.RegisterClient(clientExpectedName, clientExpectedEmail,
            clientExpectedPassword1, clientExpectedPassword2));
    }

    [TestMethod]
    public void ShouldThrowExceptionIfTriesToRegisterAdminAndIsAlreadyRegistered()
    {
        const string? adminExpectedPassword1 = "#theBestAdmin1234";
        const string adminExpectedPassword2 = "#theBestAdmin1234";
        const string? adminExpectedName = "AdministratorName";
        const string? adminExpectedEmail = "theBestAdminEmail@gmail.com";

        _clientController.RegisterAdmin(adminExpectedName, adminExpectedEmail, adminExpectedPassword1, adminExpectedPassword2);

        Assert.ThrowsException<ArgumentException>(() => _clientController.RegisterAdmin(adminExpectedName, adminExpectedEmail,
            adminExpectedPassword1, adminExpectedPassword2));
    }
}