using BusinessLogic.Domain;
using Controllers;
using DataLayer.repositories;
using DataLayer.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.FacadeManagement;

[TestClass]
public class ReservationManagement
{
    const string email = "alejo@gmail.com";
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


        _clientController.RegisterClient(name, email, password1, password2);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        DateTimeProvider.SetCurrentDateTime(DateTime.Today);
    }

    [TestMethod]
    public void ShouldAddCorrectReservationToClient()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var startDate = DateTime.Now.AddDays(5);
        var endDate = DateTime.Now.AddDays(9);
        var client = _clientController.GetClient(email);

        _reservationController.CreateReservation(client, deposit, startDate, endDate);

        var reservation = _clientController.GetClientReservations(client)[0];

        Assert.AreEqual(deposit.Id, reservation.DepositId);
        Assert.IsTrue(reservation.StartDate == startDate);
        Assert.IsTrue(reservation.EndDate == endDate);
    }

    [TestMethod]
    public void ShouldGetReservations()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        var startDate2 = DateTime.Now.AddDays(10);
        var endDate2 = DateTime.Now.AddDays(15);
        var startDate3 = DateTime.Now.AddDays(16);
        var endDate3 = DateTime.Now.AddDays(90);

        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        _reservationController.CreateReservation(client, deposit, startDate2, endDate2);
        _reservationController.CreateReservation(client, deposit, startDate3, endDate3);
        var reservations = _reservationController.GetReservations();
        Assert.IsTrue(reservations.Count == 3);
        Assert.IsNotNull(reservations.Find(reservation => reservation.Id == 1));
        Assert.IsNotNull(reservations.Find(reservation => reservation.Id == 2));
        Assert.IsNotNull(reservations.Find(reservation => reservation.Id == 3));
    }

    [TestMethod]
    public void ShouldConfirmReservation()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);

        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1, endDate1,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));

        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        var reservation = _reservationController.GetReservations()[0];
        _reservationController.ConfirmReservation(reservation);
        Assert.IsTrue(reservation.IsConfirmed);
    }

    [TestMethod]
    public void ShouldGetConfirmedReservations()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        var startDate2 = DateTime.Now.AddDays(10);
        var endDate2 = DateTime.Now.AddDays(15);
        var startDate3 = DateTime.Now.AddDays(16);
        var endDate3 = DateTime.Now.AddDays(90);


        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1, endDate1,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate2, endDate2,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate3, endDate3,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));

        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        _reservationController.CreateReservation(client, deposit, startDate2, endDate2);
        _reservationController.CreateReservation(client, deposit, startDate3, endDate3);

        var reservation1 = _reservationController.GetReservationById(1);
        var reservation3 = _reservationController.GetReservationById(3);


        _reservationController.ConfirmReservation(reservation1);
        _reservationController.ConfirmReservation(reservation3);

        var confirmedReservations = _reservationController.GetConfirmedReservations();
        Assert.IsTrue(confirmedReservations.Count == 2);
        Assert.IsNotNull(confirmedReservations.Find(reservation => reservation.Id == 1));
        Assert.IsNotNull(confirmedReservations.Find(reservation => reservation.Id == 3));
    }


    [TestMethod]
    public void ShouldGetClientReservations()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        var startDate2 = DateTime.Now.AddDays(16);
        var endDate2 = DateTime.Now.AddDays(90);
        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        _reservationController.CreateReservation(client, deposit, startDate2, endDate2);
        const int expectedReservation1Id = 1;
        const int expectedReservation2Id = 2;

        var clientReservations = _clientController.GetClientReservations(client);
        Assert.IsTrue(clientReservations.Count == 2);
        Assert.IsNotNull(clientReservations.Find(reservation => reservation.Id == expectedReservation1Id));
        Assert.IsNotNull(clientReservations.Find(reservation => reservation.Id == expectedReservation2Id));
    }


    [TestMethod]
    public void ShouldGetPendingReservations()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        var startDate2 = DateTime.Now.AddDays(10);
        var endDate2 = DateTime.Now.AddDays(15);
        var startDate3 = DateTime.Now.AddDays(16);
        var endDate3 = DateTime.Now.AddDays(90);

        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1, endDate1,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate2, endDate2,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate3, endDate3,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));

        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        _reservationController.CreateReservation(client, deposit, startDate2, endDate2);
        _reservationController.CreateReservation(client, deposit, startDate3, endDate3);
        const int expectedReservation1Id = 1;
        const int expectedReservation2Id = 2;
        const int expectedReservation3Id = 3;
        var reservation1 = _reservationController.GetReservations()
            .Find(reservation => reservation.Id == expectedReservation1Id);
        var reservation3 = _reservationController.GetReservations()
            .Find(reservation => reservation.Id == expectedReservation3Id);


        _reservationController.ConfirmReservation(reservation1);
        _reservationController.ConfirmReservation(reservation3);


        var pendingReservations = _reservationController.GetPendingReservations();
        Assert.IsTrue(pendingReservations.Count == 1);
        Assert.IsNull(pendingReservations.Find(reservation => reservation.Id == expectedReservation1Id));
        Assert.IsNull(pendingReservations.Find(reservation => reservation.Id == expectedReservation3Id));
        Assert.IsNotNull(pendingReservations.Find(reservation => reservation.Id == expectedReservation2Id));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToRejectConfirmedReservation()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1, endDate1,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));

        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        var reservation = _reservationController.GetReservations()[0];
        _reservationController.ConfirmReservation(reservation);
        const string message = "I don't like your reservation";

        Assert.ThrowsException<ArgumentException>(() => _reservationController.RejectReservation(reservation, message));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToConfirmRejectedReservation()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        var reservation = _reservationController.GetReservations()[0];
        const string message = "I don't like your reservation";
        _reservationController.RejectReservation(reservation, message);

        Assert.ThrowsException<ArgumentException>(() => _reservationController.ConfirmReservation(reservation));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToAddReservationOfSameDepositFromSameUserWithDaysInCommon()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        var startDate2ContainedInStartDate1 = DateTime.Now.AddDays(6);
        Assert.ThrowsException<ArgumentException>(() =>
            _reservationController.CreateReservation(client, deposit, startDate2ContainedInStartDate1, endDate1));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToAddSameReservationFromSameUser()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        Assert.ThrowsException<ArgumentException>(
            () => _reservationController.CreateReservation(client, deposit, startDate1, endDate1));
    }

    [TestMethod]
    public void
        ShouldThrowExceptionWhenAttemptingToAddReservationFromSameUserWithStartDateEqualToPastReservationEndDate()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        Assert.ThrowsException<ArgumentException>(() =>
            _reservationController.CreateReservation(client, deposit, endDate1, endDate1.AddDays(5)));
    }

    [TestMethod]
    public void ShouldAddReservationFromSameUserWithStartDateGreaterByLittleToPastReservationEndDate()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        var startDate2 = DateTime.Now.AddDays(9);
        var endDate2 = DateTime.Now.AddDays(14);
        _reservationController.CreateReservation(client, deposit, startDate2, endDate2);
        var reservation = _reservationController.GetReservations()[1];
        Assert.AreEqual(deposit.Id, reservation.DepositId);
        Assert.IsTrue(reservation.StartDate == startDate2);
        Assert.IsTrue(reservation.EndDate == endDate2);
    }

    [TestMethod]
    public void ShouldGetRejectedReservations()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        var startDate2 = DateTime.Now.AddDays(10);
        var endDate2 = DateTime.Now.AddDays(15);
        var startDate3 = DateTime.Now.AddDays(16);
        var endDate3 = DateTime.Now.AddDays(90);

        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1, endDate1,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate2, endDate2,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate3, endDate3,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));

        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        _reservationController.CreateReservation(client, deposit, startDate2, endDate2);
        _reservationController.CreateReservation(client, deposit, startDate3, endDate3);
        const int expectedReservation1Id = 1;
        const int expectedReservation2Id = 2;
        const int expectedReservation3Id = 3;
        var reservation1 = _reservationController.GetReservations()
            .Find(reservation => reservation.Id == expectedReservation1Id);
        var reservation3 = _reservationController.GetReservations()
            .Find(reservation => reservation.Id == expectedReservation3Id);
        const string message = "I don't like your reservation";

        _reservationController.RejectReservation(reservation1, message);
        _reservationController.ConfirmReservation(reservation3);

        var rejectedReservations = _reservationController.GetRejectedReservations();
        Assert.IsTrue(rejectedReservations.Count == expectedReservation1Id);
        Assert.IsNotNull(rejectedReservations.Find(reservation => reservation.Id == expectedReservation1Id));
        Assert.IsNull(rejectedReservations.Find(reservation => reservation.Id == expectedReservation3Id));
        Assert.IsNull(rejectedReservations.Find(reservation => reservation.Id == expectedReservation2Id));
    }


    [TestMethod]
    public void ShouldGetReservationCost()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        var cost = _reservationController.CalculateReservationCost(deposit, startDate1, endDate1);
        const int expectedCost = 300;
        Assert.AreEqual(cost, expectedCost);
    }

    [TestMethod]
    public void ShouldAddMessageWhenRejectingReservation()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Now.AddDays(5);
        var endDate1 = DateTime.Now.AddDays(9);
        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        const string? message = "I don't like your reservation";
        var reservation = _reservationController.GetReservations()[0];
        _reservationController.RejectReservation(reservation, message);
        reservation = _reservationController.GetReservations()[0];

        Assert.IsTrue(reservation.IsRejected);
        Assert.IsTrue(reservation.RejectedMessage == message);
    }


    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToCreateReservationWithNonRegisteredClient()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        const string name = "alejo fraga";
        const string email = "wrongEmail@gmail.com";
        const string password1 = "P@ssW0rd2024";
        const string password2 = "P@ssW0rd2024";
        const bool isAdmin = false;
        var client = new Client(name, email, password1, password2, isAdmin);
        var startDate = DateTime.Now.AddDays(5);
        var endDate = DateTime.Now.AddDays(9);

        Assert.ThrowsException<ArgumentException>(() =>
            _reservationController.CreateReservation(client, deposit, startDate, endDate));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToCreateReservationWithNonRegisteredDeposit()
    {
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const int expectedDepositId = 1;
        const string depositName = "Deposito";
        var deposit = new Deposit
        {
            Area = area,
            Size = size,
            Conditioning = conditioning,
            Id = expectedDepositId,
            Name = depositName
        };
        var client = _clientController.GetClient(email);
        var startDate = DateTime.Now.AddDays(5);
        var endDate = DateTime.Now.AddDays(9);

        Assert.ThrowsException<ArgumentException>(() =>
            _reservationController.CreateReservation(client, deposit, startDate, endDate));
    }

    [TestMethod]
    public void ShouldGetExpiredClientReservations() // fixear test
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate1 = DateTime.Today;
        var endDate1 = DateTime.Today.AddDays(2);

        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate1, endDate1,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));

        _reservationController.CreateReservation(client, deposit, startDate1, endDate1);
        var reservation = _reservationController.GetReservations()[0];
        _reservationController.ConfirmReservation(reservation);
        const int expectedReservationId = 1;
        DateTimeProvider.SetCurrentDateTime(DateTime.Today.AddDays(10));

        var client1ExpiredReservations = _clientController.GetClientExpiredReservations(client);

        Assert.IsTrue(client1ExpiredReservations.Count == 1);
        Assert.IsNotNull(client1ExpiredReservations.Find(reservation => reservation.Id == expectedReservationId));
    }

    [TestMethod]
    public void ShouldAddCorrectReview()
    {
        const int stars = 5;
        const string? comment = "This is a test review.";
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const int expectedDepositId = 1;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning,
            new List<PromotionInDepositManagementDto?>());
        var deposit = _depositController.GetDepositById(expectedDepositId);

        const string name = "alejo fraga";
        const string email = "juan@gmail.com";
        const string password1 = "P@asWor323";
        const string password2 = "P@asWor323";

        _clientController.RegisterClient(name, email, password1, password2);
        var client = _clientController.GetClient(email);

        const int expectedReservationId = 1;
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(5);
        ReviewReservationDto reviewReservationDto = new ReviewReservationDto();
        reviewReservationDto.ReservationId = expectedReservationId;
        reviewReservationDto.StartDate = startDate;
        reviewReservationDto.EndDate = endDate;
        reviewReservationDto.DepositId = expectedDepositId;
        reviewReservationDto.Cost = 100;

        _reservationController.CreateReservation(client, deposit, startDate, endDate);

        _depositController.AddReviewToDeposit(deposit.Id, stars, comment, reviewReservationDto);

        Assert.IsTrue(_depositController.GetDepositReviews(deposit.Id).Count == 1);
    }

    [TestMethod]
    public void ShouldGetClientByReservation()
    {
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var client = _clientController.GetClient(email);
        var startDate = DateTime.Now.AddDays(5);
        var endDate = DateTime.Now.AddDays(9);

        _reservationController.CreateReservation(client, deposit, startDate, endDate);
        var reservation = _reservationController.GetReservations()[0];

        Assert.AreEqual(_clientController.GetClientByReservation(reservation), client);
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToGetTheClientOfAReservationThatIsNotRegistered()
    {
        var nonRegisteredReservation = new Reservation();
        Assert.ThrowsException<NullReferenceException>(() =>
            _clientController.GetClientByReservation(nonRegisteredReservation));
    }


    [TestMethod]
    public void ShouldGetDepositAvailableForReservationBetweenDates()
    {
        var currentClient = _clientController.GetClient(email);

        var reservationDeposit1StartDate = DateTime.Today;
        var reservationDeposit1EndDate = DateTime.Today.AddDays(7);

        var reservationAvailableStart = reservationDeposit1StartDate;
        var reservationAvailableEndDate = reservationDeposit1EndDate;

        var Deposit1promotions = new List<PromotionInDepositManagementDto?>();
        const char Deposit1area = 'A';
        const string Deposit1size = "Mediano";
        const bool Deposit1conditioning = false;
        const string Deposit1depositName = "Deposito uno";
        _depositController.AddDeposit(Deposit1depositName, Deposit1area, Deposit1size, Deposit1conditioning,
            Deposit1promotions);

        var deposit1 = _depositController.GetDeposits()[0];

        var Deposit2promotions = new List<PromotionInDepositManagementDto?>();
        const char Deposit2area = 'E';
        const string Deposit2size = "Grande";
        const bool Deposit2conditioning = false;
        const string Deposit2depositName = "Deposito dos";
        _depositController.AddDeposit(Deposit2depositName, Deposit2area, Deposit2size, Deposit2conditioning,
            Deposit2promotions);

        var deposit2 = _depositController.GetDeposits()[1];

        var expectedReservationId = 1;
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(reservationAvailableStart,
            reservationAvailableEndDate, _depositController.ConvertDepositToDepositDisponibilityDto(deposit1));
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(reservationAvailableStart,
            reservationAvailableEndDate, _depositController.ConvertDepositToDepositDisponibilityDto(deposit2));

        _reservationController.CreateReservation(currentClient, deposit1, reservationDeposit1StartDate,
            reservationDeposit1EndDate);
        _reservationController.ConfirmReservation(_reservationController.GetReservationById(expectedReservationId));

        List<Deposit> depositsAvailable =
            _depositController.GetDepositsAvailableBetweentDates(reservationAvailableStart,
                reservationAvailableEndDate);
        const int expectedAvailableDeposits = 1;
        Assert.AreEqual(expectedAvailableDeposits, depositsAvailable.Count);
    }

    [TestMethod]
    public void ShouldCalculateReservationCostBySize()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        const string size = "Mediano";
        var deposit = new Deposit();
        deposit.Size = size;
        var reservation = new Reservation();
        reservation.StartDate = startDate;
        reservation.EndDate = endDate;
        reservation.Deposit = deposit;

        const int expectedReservationCost = 75;

        Assert.AreEqual(expectedReservationCost,
            _reservationController.CalculateReservationCost(deposit, startDate, endDate));
    }


    [TestMethod]
    public void ShouldMupliplyCostByDuration()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(2);
        const string size = "Mediano";
        var deposit = new Deposit();
        deposit.Size = size;
        var reservation = new Reservation();
        reservation.StartDate = startDate;
        reservation.EndDate = endDate;
        reservation.Deposit = deposit;
        var duration = (endDate - startDate).Days;
        const int sizeCost = 75;

        var expectedReservationCost = sizeCost * duration;

        Assert.AreEqual(expectedReservationCost,
            _reservationController.CalculateReservationCost(deposit, startDate, endDate));
    }

    [TestMethod]
    public void ShouldCalculateExtraCostWithConditioning()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(2);
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = true;
        const int id = 1;
        const string depositName = "Deposito";

        _depositController.AddDeposit(depositName, area, size, conditioning,
            _promotionController.GetPromotionInDepositManagementDtos());
        var deposit = _depositController.GetDeposits()[0];
        const bool isConfirmed = false;
        const bool isReviewed = false;
        var duration = (endDate - startDate).Days;
        const int conditioningCost = 20;
        const int sizeCost = 75;

        var expectedReservationCost = (sizeCost + conditioningCost) * duration;

        Assert.AreEqual(expectedReservationCost,
            _reservationController.CalculateReservationCost(deposit, startDate, endDate));
    }


    [TestMethod]
    public void ShouldApplyPromotionToCostIfDepositHasValidPromotion()
    {
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(2);
        const int promotionDiscount = 55;
        const string tag = "promo1";
        var promotionStartDate = DateTime.Today;
        var promotionEndDate = DateTime.Today.AddDays(1);
        _promotionController.AddPromotion(tag, promotionDiscount, promotionStartDate, promotionEndDate);
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = true;
        const string depositName = "Deposito";
        var promotions = _promotionController.GetPromotionInDepositManagementDtos();
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        var deposit = _depositController.GetDeposits()[0];
        var duration = (endDate - startDate).Days;
        const int conditioningCost = 20;
        const int expectedTotalPromotionDiscount = 55;
        const float multiplicationFactor = (100f - expectedTotalPromotionDiscount) / 100f;

        var expectedReservationCost = ((75 + conditioningCost) * duration) * (multiplicationFactor);

        Assert.AreEqual(expectedReservationCost,
            _reservationController.CalculateReservationCost(deposit, startDate, endDate));
    }


    [TestMethod]
    public void ShouldApplyOnlyValidPromotionsToCost()
    {
        var startDate = DateTime.Now.AddDays(5);
        var endDate = DateTime.Now.AddDays(10);
        const int promotionDiscount = 55;
        var expiredStartDate = DateTime.Now;
        var expiredEndDate = DateTime.Now.AddDays(1);
        const string tag = "promo1";
        _promotionController.AddPromotion(tag, promotionDiscount, expiredStartDate, expiredEndDate);
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = true;
        const int id = 1;
        const string depositName = "Deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning,
            _promotionController.GetPromotionInDepositManagementDtos());
        var deposit = _depositController.GetDeposits()[0];
        const bool isConfirmed = false;
        const bool isReviewed = false;

        var duration = (endDate - startDate).Days;
        const int conditioningCost = 20;
        const int expectedTotalPromotionDiscount = 0;
        const float multiplicationFactor = (100f - expectedTotalPromotionDiscount) / 100f;
        const int sizeCost = 75;


        var expectedReservationCost = ((sizeCost + conditioningCost) * duration) * (multiplicationFactor);
        Assert.AreEqual(expectedReservationCost,
            _reservationController.CalculateReservationCost(deposit, startDate, endDate));
    }


    [TestMethod]
    public void TotalPromotionDiscountShouldBeUpTo100()
    {
        var startDate = DateTime.Now.AddDays(5);
        var endDate = DateTime.Now.AddDays(10);
        const int promotion1Discount = 55;
        const int promotion2Discount = 65;

        var promotionStartDate = DateTime.Now;
        var promotionEndDate = DateTime.Now.AddDays(6);
        const string tag1 = "promo1";
        const string tag2 = "promo2";

        _promotionController.AddPromotion(tag1, promotion1Discount, promotionStartDate, promotionEndDate);
        _promotionController.AddPromotion(tag2, promotion2Discount, promotionStartDate, promotionEndDate);

        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = true;
        const int id = 1;
        const string depositName = "Deposito";

        _depositController.AddDeposit(depositName, area, size, conditioning,
            _promotionController.GetPromotionInDepositManagementDtos());
        var deposit = _depositController.GetDeposits()[0];
        var duration = (endDate - startDate).Days;
        const int conditioningCost = 20;
        const int expectedTotalPromotionDiscount = 100;
        var multiplicationFactor = (100f - expectedTotalPromotionDiscount) / 100f;
        const int sizeCost = 75;

        var expectedReservationCost = ((sizeCost + conditioningCost) * duration) * (multiplicationFactor);
        Assert.AreEqual(expectedReservationCost,
            _reservationController.CalculateReservationCost(deposit, startDate, endDate));
    }

    [TestMethod]
    public void ShouldAddDiscountByDays()
    {
        var startDate = DateTime.Now.AddDays(5);
        var endDate = DateTime.Now.AddDays(15);
        const int promotion1Discount = 55;
        var promotionStartDate = DateTime.Now;
        var promotionEndDate = DateTime.Now.AddDays(6);
        const string tag1 = "promo1";
        _promotionController.AddPromotion(tag1, promotion1Discount, promotionStartDate, promotionEndDate);
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = true;
        const int id = 1;
        const string depositName = "Deposito";
        var promotions = _promotionController.GetPromotionInDepositManagementDtos();
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        var deposit = _depositController.GetDeposits()[0];
        var duration = (endDate - startDate).Days;
        const int conditioningCost = 20;
        const int expectedTotalPromotionDiscount = 55;
        const int expectedDiscountByDays = 5;
        const int totalDiscount = expectedTotalPromotionDiscount + expectedDiscountByDays;
        const int sizeCost = 50;

        const float multiplicationFactor = (100f - totalDiscount) / 100f;


        var expectedReservationCost = ((sizeCost + conditioningCost) * duration) * (multiplicationFactor);
        Assert.AreEqual(expectedReservationCost,
            _reservationController.CalculateReservationCost(deposit, startDate, endDate));
    }


    [TestMethod]
    public void ShouldCalculateCost()
    {
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(15);
        const int promotion1Discount = 55;
        var promotionStartDate = DateTime.Today;
        var promotionEndDate = DateTime.Today.AddDays(6);
        const string tag1 = "promo1";
        _promotionController.AddPromotion(tag1, promotion1Discount, promotionStartDate, promotionEndDate);
        const char area = 'A';
        const string size = "Grande";
        const bool conditioning = true;
        const int id = 1;
        const string depositName = "Deposito";
        var promotions = _promotionController.GetPromotionInDepositManagementDtos();
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        var deposit = _depositController.GetDeposits()[0];
        var duration = (endDate - startDate).Days;
        const int conditioningCost = 20;
        const int expectedTotalPromotionDiscount = 55;
        const int expectedDiscountByDays = 10;
        const int totalDiscount = expectedTotalPromotionDiscount + expectedDiscountByDays;
        const int sizeCost = 100;

        const float multiplicationFactor = (100f - totalDiscount) / 100f;


        var expectedReservationCost = ((sizeCost + conditioningCost) * duration) * (multiplicationFactor);
        Assert.AreEqual(expectedReservationCost,
            _reservationController.CalculateReservationCost(deposit, startDate, endDate));
    }

    [TestMethod]
    public void ShoulThrowExceptionWhenAttemptingToGetNonExistantDeposit()
    {
        const int nonExistantDepositId = 1;
        Assert.ThrowsException<NullReferenceException>(() => _depositController.GetDepositById(nonExistantDepositId));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToGetNonExistantDeposit()
    {
        const int nonExistantDepositId = 1;
        Assert.ThrowsException<NullReferenceException>(
            () => _reservationController.GetDepositById(nonExistantDepositId));
    }

    [TestMethod]
    public void ShouldRejectReservationIfDepositIsNotAvailableWhenCOnfirmingReservation()
    {
        const string clientName = "Juan";
        const string email = "juan@gmail.com";
        const string email2 = "juan2@gmail.com";
        const string password = "@sebaA1234@";
        _clientController.RegisterClient(clientName, email, password, password);
        _clientController.RegisterClient(clientName, email2, password, password);
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
        const int reservationId = 1;
        var reservation = _reservationController.GetReservationById(reservationId);
        _reservationController.ConfirmReservation(reservation);
        var client2 = _clientController.GetClient(email2);

        _reservationController.CreateReservation(client2, deposit, startDate, endDate);
        const int invalidReservationId = 2;
        var invalidReservation = _reservationController.GetReservationById(invalidReservationId);
        Assert.ThrowsException<ArgumentException>(() => _reservationController.ConfirmReservation(invalidReservation));
    }


    [TestMethod]
    public void ShouldFacadeExportReservationTxt()
    {
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
        const int reservationId = 1;
        var reservation = _reservationController.GetReservationById(reservationId);
        _reservationController.ConfirmReservation(reservation);

        var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        var filePath = Path.Combine(downloadsPath, "reservas.txt");
        var expectedFilePath = Path.Combine(downloadsPath, "reservas_1.txt");


        try
        {
            _reservationController.ExportReservation("txt");
            _reservationController.ExportReservation("txt");

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
    public void ShouldExportRejectedReservationsOnTxt()
    {
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
        const int reservationId = 1;
        var reservation = _reservationController.GetReservationById(reservationId);
        const string message = "I don't like your reservation";
        _reservationController.RejectReservation(reservation, message);

        var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        var filePath = Path.Combine(downloadsPath, "reservas.txt");
        var expectedFilePath = Path.Combine(downloadsPath, "reservas_1.txt");


        try
        {
            _reservationController.ExportReservation("txt");
            _reservationController.ExportReservation("txt");
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
    public void ShouldFacadeExportReservationCsv()
    {
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

        var downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        var filePath = Path.Combine(downloadsPath, "reservas.csv");

        try
        {
            _reservationController.ExportReservation("csv");
            Assert.IsTrue(File.Exists(filePath));
        }
        finally
        {
            File.Delete(filePath);
        }
    }
}