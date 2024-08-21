using Controllers;
using DataLayer.repositories;
using DataLayer.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.Domain;

[TestClass]

public class StatisticsTest
{
    const string name = "alejo fraga";
    const string password1 = "P@ssW0rd2024";
    const string password2 = "P@ssW0rd2024";
    private ReservationController _reservationController;
    private ClientController _clientController;
    private DepositController _depositController;
    private PromotionController _promotionController;

    [TestInitialize]
    public void OnInitialize()
    {
        var programTest = new ProgramTest();
        var scope = programTest.ServiceProvider.CreateScope();
        var promotionRepository = scope.ServiceProvider.GetRequiredService<PromotionRepository>();
        var clientRepository = scope.ServiceProvider.GetRequiredService<ClientRepository>();
        var reservationRepository = scope.ServiceProvider.GetRequiredService<ReservationRepository>();
        var depositRepository = scope.ServiceProvider.GetRequiredService<DepositRepository>();
        var notificationRepository = scope.ServiceProvider.GetRequiredService<NotificationsRepository>();
        var logRepository = scope.ServiceProvider.GetRequiredService<LogRepository>();
        var reviewRepository = scope.ServiceProvider.GetRequiredService<ReviewRepository>();
        _reservationController = new ReservationController(reservationRepository, depositRepository, clientRepository,
            promotionRepository);
        _clientController = new ClientController(clientRepository, depositRepository, promotionRepository,
            notificationRepository, logRepository, reservationRepository);
        _depositController = new DepositController(depositRepository, promotionRepository, reservationRepository,
            reviewRepository);
        _promotionController = new PromotionController(promotionRepository);
    }
    
    [TestMethod]
    public void ShouldGetMoneyGeneretedBetweenDatesByDepositArea()
    {
        const string clientEmail = "alejo@gmail.com";
        _clientController.RegisterClient("alejo fraga", clientEmail, "P@ssW0rd2024", "P@ssW0rd2024");
        
        var promotions = new List<PromotionInDepositManagementDto>();
        var area = 'A';
        const string depositName1 = "DepositoUno";
        const string depositName2 = "DepositoDos";
        const string depositName3 = "DepositoTres";
        
        _depositController.AddDeposit(depositName1, area, "Mediano", true, promotions);
        _depositController.AddDeposit(depositName2, area, "Grande", true, promotions);
        _depositController.AddDeposit(depositName3, area, "Mediano", true, promotions);
        
        var deposit1 = _depositController.GetDeposits()[0];
        var deposit2 = _depositController.GetDeposits()[1];
        var deposit3 = _depositController.GetDeposits()[2];
        var client = _clientController.GetClients().FirstOrDefault(client => client.Email == clientEmail);
        
        var startDate1 = DateTime.Now;
        var endDate1 = DateTime.Now.AddDays(1);
        var endDate2 = DateTime.Now.AddDays(5);
        var endDate3 = DateTime.Now.AddDays(3);
        
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1,endDate1, _depositController.ConvertDepositToDepositDisponibilityDto(deposit1));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1,endDate1, _depositController.ConvertDepositToDepositDisponibilityDto(deposit2));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1, endDate2, _depositController.ConvertDepositToDepositDisponibilityDto(deposit3));
        
        _reservationController.CreateReservation(client,deposit1, startDate1, endDate1);
        _reservationController.CreateReservation(client,deposit2, startDate1, endDate1);
        _reservationController.CreateReservation(client,deposit3, startDate1, endDate2);
        var reservation1 = _reservationController.GetReservations()[0];
        var reservation2 = _reservationController.GetReservations()[1];
        var reservation3 = _reservationController.GetReservations()[2];
        _reservationController.ConfirmReservation(reservation1);
        _reservationController.ConfirmReservation(reservation2);
        _reservationController.ConfirmReservation(reservation3);
        
        Assert.AreEqual(690, _reservationController.GetMoneyGeneratedBetweenDatesByDepositArea(area, startDate1, endDate3));
    }

    [TestMethod]
    public void ShouldGetReservationAmountOfConfirmedReservationsByDepositArea()
    {
        const string clientEmail = "alejo@gmail.com";
        _clientController.RegisterClient("alejo fraga", clientEmail, "P@ssW0rd2024", "P@ssW0rd2024");
        
        var promotions = new List<PromotionInDepositManagementDto>();

        var areaA = 'A';
        var areaC = 'C';
        const string depositName1 = "DepositoUno";
        const string depositName2 = "DepositoDos";
        const string depositName3 = "DepositoTres";

        _depositController.AddDeposit(depositName1, areaA, "Mediano", true, promotions);
        _depositController.AddDeposit(depositName2, areaA, "Grande", true, promotions);
        _depositController.AddDeposit(depositName3, areaC, "Mediano", true, promotions);
        
        var deposit1 = _depositController.GetDeposits()[0];
        var deposit2 = _depositController.GetDeposits()[1];
        var deposit3 = _depositController.GetDeposits()[2];
        var client = _clientController.GetClient(clientEmail);
        
        var startDate1 = DateTime.Now;
        var endDate1 = DateTime.Now.AddDays(1);
        var endDate2 = DateTime.Now.AddDays(5);
        
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1,endDate1, _depositController.ConvertDepositToDepositDisponibilityDto(deposit1));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1,endDate1, _depositController.ConvertDepositToDepositDisponibilityDto(deposit2));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1, endDate2, _depositController.ConvertDepositToDepositDisponibilityDto(deposit3));
        
        _reservationController.CreateReservation(client,deposit1, startDate1, endDate1);
        _reservationController.CreateReservation(client,deposit2, startDate1, endDate1);
        _reservationController.CreateReservation(client,deposit3, startDate1, endDate2);
        var reservation1 = _reservationController.GetReservations()[0];
        var reservation2 = _reservationController.GetReservations()[1];
        var reservation3 = _reservationController.GetReservations()[2];
        _reservationController.ConfirmReservation(reservation1);
        _reservationController.ConfirmReservation(reservation2);
        _reservationController.ConfirmReservation(reservation3);
        
        Assert.AreEqual(2, _reservationController.GetAmountOfConfirmedReservationsByDepositArea(areaA));
    }
    
}