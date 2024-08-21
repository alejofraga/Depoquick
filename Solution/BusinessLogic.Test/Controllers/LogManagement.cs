using BusinessLogic.Domain;
using Controllers;
using DataLayer.repositories;
using DataLayer.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.FacadeManagement;

[TestClass]
public class LogManagement
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
    public void ShouldCreateLog()
    {
        var log = new Log();
        Assert.IsNotNull(log);
    }
    

    

    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToAddNonExistentActions()
    {
        const string nonExistentAction = "el cliente tomo mate";
        var time = DateTime.Now;
        
        const string name = "alejo";
        const string email = "alejofraga22v2@gmail.com";
        const string password1 = "@Lejo1234@";
        const string password2 = "@Lejo1234@";
        _clientController.RegisterClient(name, email,password1,password2);
        
        Assert.ThrowsException<ArgumentException>(() =>  _clientController.AddActionToClient(email, nonExistentAction, time));
    }

    [TestMethod]
    public void ShouldAddActionToClientFromFacade()
    {
        const string? adminExpectedPassword1 = "#theBestAdmin1234";
        const string adminExpectedPassword2 = "#theBestAdmin1234";
        const string? adminExpectedName = "AdministratorName";
        const string? adminExpectedEmail = "theBestAdminEmail@gmail.com";
        _clientController.RegisterAdmin(adminExpectedName, adminExpectedEmail, adminExpectedPassword1, adminExpectedPassword2);
        
        const string action = "Inicio de sesión";
        DateTime time = DateTime.Now;
        _clientController.AddActionToClient(adminExpectedEmail, action, time);
        var actualAction = _clientController.GetClientLog(adminExpectedEmail).First();
        Assert.AreEqual($"{time}: {action}", actualAction.Description);
    }

    [TestMethod]
    public void AddNewReviewShouldBeAValidAction()
    {
        const string? adminExpectedPassword1 = "#theBestAdmin1234";
        const string adminExpectedPassword2 = "#theBestAdmin1234";
        const string? adminExpectedName = "AdministratorName";
        const string? adminExpectedEmail = "theBestAdminEmail@gmail.com";
        _clientController.RegisterAdmin(adminExpectedName, adminExpectedEmail, adminExpectedPassword1, adminExpectedPassword2);
        
        const string action = "Agregó una nueva reseña";
        DateTime time = DateTime.Now;
        _clientController.AddActionToClient(adminExpectedEmail, action, time);
        var actualAction = _clientController.GetClientLog(adminExpectedEmail).First();
        Assert.AreEqual($"{time}: {action}", actualAction.Description);
    }
}