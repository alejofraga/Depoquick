using Controllers;
using Controllers.ReservationExporter;
using DataLayer.repositories;
using DataLayer.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.Domain;

[TestClass]
public class ReservationExporter_Test
{
    private ReservationController _reservationController;
    private ClientController _clientController;
    private DepositController _depositController;

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
    }

    [TestMethod]
    public void ShouldCreateReservationCsvExporter()
    {
        var reservationExporter = new CsvExporter(_reservationController.DepositRepository);
        Assert.IsNotNull(reservationExporter);
    }

    [TestMethod]
    public void ShouldCreateReservationTxtExporter()
    {
        var reservationExporter = new TxtExporter(_reservationController.DepositRepository);
        Assert.IsNotNull(reservationExporter);
    }

    [TestMethod]
    public void ShouldExportReservationTxt()
    {
        var reservationExporter = new TxtExporter(_reservationController.DepositRepository);

        const string clientName = "Juan";
        const string email = "juan@gmail.com";
        const string password = "@sebaA1234@";
        _clientController.RegisterClient(clientName, email, password, password);
        var client = _clientController.GetClients().First();

        const string depositName = "Deposito";
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = true;
        var promotions = new List<PromotionInDepositManagementDto?>();
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        var deposit = _depositController.GetDeposits().First();

        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        _reservationController.CreateReservation(client, deposit, startDate, endDate);
        var reservations = _reservationController.GetReservations();

        var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        var filePath = Path.Combine(downloadsPath, "reservas.txt");

        try
        {
            reservationExporter.Export(reservations);
            Assert.IsTrue(File.Exists(filePath));
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void ShouldExportReservationCsv()
    {
        var reservationExporter = new CsvExporter(_reservationController.DepositRepository);

        const string clientName = "Juan";
        const string email = "juan@gmail.com";
        const string password = "@sebaA1234@";
        _clientController.RegisterClient(clientName, email, password, password);
        var client = _clientController.GetClients().First();

        const string depositName = "Deposito";
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = true;
        var promotions = new List<PromotionInDepositManagementDto?>();
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        var deposit = _depositController.GetDeposits().First();

        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        _reservationController.CreateReservation(client, deposit, startDate, endDate);
        var reservations = _reservationController.GetReservations();

        var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        var filePath = Path.Combine(downloadsPath, "reservas.csv");

        try
        {
            reservationExporter.Export(reservations);
            Assert.IsTrue(File.Exists(filePath));
        }
        finally
        {
            File.Delete(filePath);
        }
    }

    [TestMethod]
    public void ShouldCreateANewExportFileIfAlreadyExists()
    {
        var reservationExporter = new CsvExporter(_reservationController.DepositRepository);

        const string clientName = "Juan";
        const string email = "juan@gmail.com";
        const string password = "@sebaA1234@";
        _clientController.RegisterClient(clientName, email, password, password);
        var client = _clientController.GetClients().First();

        const string depositName = "Deposito";
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = true;
        var promotions = new List<PromotionInDepositManagementDto?>();
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        var deposit = _depositController.GetDeposits().First();

        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        _depositController.AddDateRangeDisponibilityToDeposit(startDate, endDate, deposit);

        _reservationController.CreateReservation(client, deposit, startDate, endDate);
        var reservation = _reservationController.GetReservations()[0];
        _reservationController.ConfirmReservation(reservation);

        var reservations = _reservationController.GetReservations();
        
        var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        var filePath = Path.Combine(downloadsPath, "reservas.csv");
        var expectedFilePath = Path.Combine(downloadsPath, "reservas_1.csv");

        try
        {
            reservationExporter.Export(reservations);
            reservationExporter.Export(reservations);
            Assert.IsTrue(File.Exists(filePath));
            Assert.IsTrue(File.Exists(expectedFilePath));
        }
        finally
        {
            File.Delete(filePath);
            File.Delete(expectedFilePath);
        }
    }
    [TestMethod]

    public void ShouldExportRejectedReservation()
    {
        var reservationExporter = new CsvExporter(_reservationController.DepositRepository);

        const string clientName = "Juan";
        const string email = "juan@gmail.com";
        const string password = "@sebaA1234@";
        _clientController.RegisterClient(clientName, email, password, password);
        var client = _clientController.GetClients().First();

        const string depositName = "Deposito";
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = true;
        var promotions = new List<PromotionInDepositManagementDto?>();
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        var deposit = _depositController.GetDeposits().First();

        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        _depositController.AddDateRangeDisponibilityToDeposit(startDate, endDate, deposit);

        _reservationController.CreateReservation(client, deposit, startDate, endDate);
        var reservation = _reservationController.GetReservations()[0];
        const string message = "No hay disponibilidad";
        _reservationController.RejectReservation(reservation,message);

        var reservations = _reservationController.GetReservations();
        
        var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        var filePath = Path.Combine(downloadsPath, "reservas.csv");
        var expectedFilePath = Path.Combine(downloadsPath, "reservas_1.csv");

        try
        {
            reservationExporter.Export(reservations);
            reservationExporter.Export(reservations);
            Assert.IsTrue(File.Exists(filePath));
            Assert.IsTrue(File.Exists(expectedFilePath));
        }
        finally
        {
            File.Delete(filePath);
            File.Delete(expectedFilePath);
        }
    }
}