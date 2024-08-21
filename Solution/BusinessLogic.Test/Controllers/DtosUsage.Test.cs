using BusinessLogic.Domain;
using Controllers;
using DataLayer.repositories;
using DataLayer.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.FacadeManagement;

[TestClass]
public class DtosUsage
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
    public void ShouldConvertPromotionListOnPromotionIdAndTagDtoList()
    {
        var promotions = _promotionController.GetPromotions();
        var promotionIdAndTagDtoList = _promotionController.GetPromotionInDepositManagementDtos();
        Assert.AreEqual(promotions.Count, promotionIdAndTagDtoList.Count);
    }

    [TestMethod]
    public void ShouldConvertPromotionListOnPromotionFullDataDtoList()
    {
        const string tag = "super promo";
        const int discountPercentage = 60;
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(7);
        _promotionController.AddPromotion(tag, discountPercentage, startDate, endDate);
        var promotions = _promotionController.GetPromotions();
        var promotionFullDataDtoList = _promotionController.GetPromotionInPromotionManagementDtos();
        Assert.AreEqual(promotions.Count, promotionFullDataDtoList.Count);
    }

    [TestMethod]
    public void ShouldGetClientViewReservationDTOList()
    {
        string expectedName = "cliente";
        string expectedClientEmail = "cliente@gmail.com";
        string expectedClientPassword = "12345678#Aa";
        string expectedClientPasswordConfirmation = "12345678#Aa";

        ClientDto clientDto = new ClientDto(expectedName, expectedClientEmail, expectedClientPassword, false);

        _clientController.RegisterClient(expectedName, expectedClientEmail, expectedClientPassword,
            expectedClientPasswordConfirmation);

        char expectedArea = 'A';
        string? expectedSize = "Grande";
        bool expectedConditioning = true;
        int expectedDepositId = 1;
        const string depositName = "deposito";

        _depositController.AddDeposit(depositName, expectedArea, expectedSize, expectedConditioning,
            new List<PromotionInDepositManagementDto?>());

        DateTime expectedStartDate = DateTime.Today;
        DateTime expecteEndDate = DateTime.Today.AddDays(7);

        _reservationController.CreateReservation(_clientController.GetClient(expectedClientEmail), _depositController.GetDepositById(expectedDepositId),
            expectedStartDate, expecteEndDate);

        var clientReservations = _clientController.GetClientReservations(_clientController.GetClient(expectedClientEmail));

        var clientReservationsDtos = _clientController.GetClientViewReservationDtOs(clientDto);

        Assert.AreEqual(clientReservations.Count, clientReservationsDtos.Count);
    }

    [TestMethod]
    public void ShouldGetPendingReservationsAndConvertToReservationManagementDtoList()
    {
        const int expectedDepositId = 1;
        const string expectedClientName = "Pepe el crack";
        const string expectedClientMail = "alejofraga22v2@gmail.com";
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(5);
        const string clientPassword1 = "#theBestAdmin1234";
        const string clientPassword2 = "#theBestAdmin1234";
        _clientController.RegisterClient(expectedClientName, expectedClientMail, clientPassword1, clientPassword2);
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "deposito";

        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);

        _reservationController.CreateReservation(_clientController.GetClient(expectedClientMail), _depositController.GetDepositById(expectedDepositId),
            expectedStartDate, expectedEndDate);
        var pendingReservations = _reservationController.GetPendingReservations();
        var reservationManagementDtos = _reservationController.GetReservationManagementDtos();
        Assert.AreEqual(pendingReservations.Count, reservationManagementDtos.Count);
    }

    [TestMethod]
    public void ShouldConfirmReservationGivenAReservationManagementDto()
    {
        const int expectedDepositId = 1;
        const int expectedReservationId = 1;
        const string expectedClientName = "Pepe el crack";
        const string expectedClientMail = "alejofraga22v2@gmail.com";
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(5);
        const string clientPassword1 = "#theBestAdmin1234";
        const string clientPassword2 = "#theBestAdmin1234";
        _clientController.RegisterClient(expectedClientName, expectedClientMail, clientPassword1, clientPassword2);
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "deposito";

        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);

        _reservationController.CreateReservation(_clientController.GetClient(expectedClientMail), _depositController.GetDepositById(expectedDepositId),
            expectedStartDate, expectedEndDate);

        var deposit = _depositController.GetDeposits()[0];

        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(expectedStartDate, expectedEndDate,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));

        var reservationManagementDto = new ReservationManagementDTO(expectedDepositId, expectedReservationId,
            depositName,
            expectedClientName, expectedClientMail, expectedStartDate, expectedEndDate);

        _reservationController.ConfirmReservationByReservationManagementDto(reservationManagementDto);

        var reservations = _reservationController.GetReservations();

        Assert.IsTrue(
            reservations.Find(reservation => reservation.Id == expectedReservationId).IsConfirmed);
    }

    [TestMethod]
    public void ShouldRejectReservationGivenAReservationManagementDto()
    {
        const int expectedDepositId = 1;
        const int expectedReservationId = 1;
        const string expectedClientName = "Pepe el crack";
        const string expectedClientMail = "alejofraga22v2@gmail.com";
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(5);
        const string clientPassword1 = "#theBestAdmin1234";
        const string clientPassword2 = "#theBestAdmin1234";
        _clientController.RegisterClient(expectedClientName, expectedClientMail, clientPassword1, clientPassword2);
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        _reservationController.CreateReservation(_clientController.GetClient(expectedClientMail), _depositController.GetDepositById(expectedDepositId),
            expectedStartDate, expectedEndDate);


        var reservationManagementDto = new ReservationManagementDTO(expectedDepositId, expectedReservationId,
            depositName,
            expectedClientName, expectedClientMail, expectedStartDate, expectedEndDate);

        _reservationController.RejectReservationByReservationManagementDto(reservationManagementDto, "No hay disponibilidad");

        Assert.IsTrue(_reservationController.GetReservations().Find(reservation => reservation.Id == expectedReservationId)
            .IsRejected);
    }

    [TestMethod]
    public void ShouldGetPromotionsInDepositInPromotionManagementDto()
    {
        const string tag = "super promo";
        const int discountPercentage = 60;
        var startDateP = DateTime.Today;
        var endDateP = DateTime.Today.AddDays(70);
        _promotionController.AddPromotion(tag, discountPercentage, startDateP, endDateP);
        var promotions = _promotionController.GetPromotionInDepositManagementDtos();
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = false;
        const string depositName = "deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        
        Assert.AreEqual(promotions.Count, _depositController.GetPromotionsInDepositInPromotionManagementDto(1).Count);
    }

    [TestMethod]
    public void ShouldGetDepositInDepositManagementDto()
    {
        const string tag = "super promo";
        const int discountPercentage = 60;
        var startDateP = DateTime.Today;
        var endDateP = DateTime.Today.AddDays(70);
        _promotionController.AddPromotion(tag, discountPercentage, startDateP, endDateP);
        var promotions = _promotionController.GetPromotionInDepositManagementDtos();
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = false;
        const string depositName = "deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        var deposits = _depositController.GetDeposits();
        var depositInDepositManagementDtoList = _depositController.GetDepositManagementDtos();
        Assert.AreEqual(deposits.Count, depositInDepositManagementDtoList.Count);
    }

    [TestMethod]
    public void ShouldGetExpiredReservationsInDepositReviewDto()
    {
        var expectedName = "Pepe";
        var expectedMail = "pepe@gmail.com";
        var expectedMail2 = "pepe2@gmail.com";
        var expectedPassword1 = "#theBestAdmin1234";
        var expectedPassword2 = "#theBestAdmin1234";
        _clientController.RegisterClient(expectedName, expectedMail, expectedPassword1, expectedPassword2);
        _clientController.RegisterClient(expectedName, expectedMail2, expectedPassword1, expectedPassword2);

        var client = _clientController.GetClients()[0];
        var client2 = _clientController.GetClients()[1];

        const string tag = "super promo";
        const int discountPercentage = 60;
        var startDateP = DateTime.Today;
        var endDateP = DateTime.Today.AddDays(70);
        _promotionController.AddPromotion(tag, discountPercentage, startDateP, endDateP);
        _promotionController.AddPromotion(tag, discountPercentage, startDateP, endDateP);
        var promotions = _promotionController.GetPromotionInDepositManagementDtos();
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = false;
        const string depositName = "deposito";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        var deposit = _depositController.GetDeposits()[0];

        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(8);
        var startDate2 = DateTime.Today.AddDays(9);
        var endDate2 = DateTime.Today.AddDays(36);

        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate, endDate,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));
        
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate2, endDate2,_depositController.ConvertDepositToDepositDisponibilityDto(deposit));

        _reservationController.CreateReservation(client, deposit, startDate, endDate);
        _reservationController.CreateReservation(client2, deposit, startDate2, endDate2);

        var reservation = _reservationController.GetReservations()[0];
        var reservation2 = _reservationController.GetReservations()[1];
        _reservationController.ConfirmReservation(reservation);
        _reservationController.ConfirmReservation(reservation2);

        DateTimeProvider.SetCurrentDateTime(DateTime.Today.AddDays(50));
        
        var reservationInDepositReviewDtoList =
            _clientController.GetClientExpiredAndNotReviewedReservationInDepositReviewDtos(client);
        
        var reservationInDepositReviewDtoList2 =
            _clientController.GetClientExpiredAndNotReviewedReservationInDepositReviewDtos(client2);
        Assert.AreNotEqual(0, reservationInDepositReviewDtoList.Count);
        Assert.AreNotEqual(0, reservationInDepositReviewDtoList2.Count);
    }

    [TestMethod]
    public void ShouldGetClientLogDtoByClientDto()
    {
        string expectedName = "Pepe el crack";
        string expectedMail = "pepe@gmail.com";
        string expectedPassword = "#theBestAdmin1234";
        bool expectedIsAdmin = true;
        var clientDto = new ClientDto(expectedName, expectedMail, expectedPassword, expectedIsAdmin);
        _clientController.RegisterClient(clientDto.Name, clientDto.Email, clientDto.Password, clientDto.Password);
        _clientController.AddActionToClient(clientDto.Email, "Inicio de sesión",DateTime.Now);
        var expectedActionLog = _clientController.GetClientLog(expectedMail);
        var logDto = _clientController.GetClientLogDtoByClientDto(clientDto);
        var actionLog = logDto.ActionDescriptions;
        Assert.AreEqual(expectedActionLog.Count, actionLog.Count);
    }

    [TestMethod]
    public void ShouldRemoveNotificationByNotificationDto()
    {
        const string expectedName = "Sebastian";
        const string expectedMail = "seba@gmail.com";
        const string expectedPassword = "#@sebaA12345";
        _clientController.RegisterClient(expectedName, expectedMail, expectedPassword, expectedPassword);
        const ValidNotifications notification = ValidNotifications.ReservationConfirmed;
        const int notificationType = (int)notification;
        const string depositName = "pepes";
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(5);
        _clientController.AddNotificationToClient(expectedMail, notificationType, depositName, startDate, endDate);
        var notificationDto = _clientController.GetClientNotificationsDtos(expectedMail)[0];
        _clientController.RemoveNotificationDto(notificationDto);
        Assert.AreEqual(0, _clientController.GetClientNotificationsDtos(expectedMail).Count);
    }
    [TestMethod]
    public void ShouldGetClientDtoList()
    {
        var clients = _clientController.GetClients();
        var clientDtoList = _clientController.GetClientDtos();
        Assert.AreEqual(clients.Count, clientDtoList.Count);
    }

    [TestMethod]
    public void ShouldGetClientLogGivenAClientDto()
    {
        string expectedName = "Pepe el crack";
        string expectedMail = "pepe@gmail.com";
        string expectedPassword = "#theBestAdmin1234";
        bool expectedIsAdmin = true;
        var clientDto = new ClientDto(expectedName, expectedMail, expectedPassword, expectedIsAdmin);
        _clientController.RegisterClient(clientDto.Name, clientDto.Email, clientDto.Password, clientDto.Password);
        var expectedActionLog = _clientController.GetClientLog(expectedMail);
        var actionLog = _clientController.GetClientLogByClientDto(clientDto);
        CollectionAssert.AreEqual(expectedActionLog, actionLog);
    }

    [TestMethod]
    public void ShouldGetActiveUserByClientDto()
    {
        const string expectedName = "Pepe el crack";
        const string expectedMail = "pepe@gmail.com";
        const string expectedPassword = "#theBestAdmin1234";
        const bool expectedIsAdmin = true;
        var clientDto = new ClientDto(expectedName, expectedMail, expectedPassword, expectedIsAdmin);
        _clientController.RegisterClient(clientDto.Name, clientDto.Email, clientDto.Password, clientDto.Password);
        _clientController.Login(expectedMail, expectedPassword);
        var expectedActiveUser = _clientController.GetActiveUser();
        var activeUserByClientDto = _clientController.GetActiveUserByClientDto();

        Assert.AreEqual(expectedActiveUser.Email, activeUserByClientDto.Email);
    }
    
    [TestMethod]
    public void ShouldGetNullIfNoActiveUse()
    {
        var expectedActiveUser = _clientController.GetActiveUser();
        var activeUserByClientDto = _clientController.GetActiveUserByClientDto();
        Assert.IsNull(activeUserByClientDto);
        Assert.IsNull(expectedActiveUser);
    }


    [TestMethod]
    public void ShouldConvertDepositPromotionsListToPromotionsCreateReservationDtoList()
    {
        const string expectedPromotionTag = "super promo";
        const int expectedPromotionDiscountPercentage = 60;
        DateTime expectedStartDate = DateTime.Today;
        DateTime expectedEndDate = DateTime.Today.AddDays(7);
        const int expectedPromotionId = 1;

        _promotionController.AddPromotion(expectedPromotionTag, expectedPromotionDiscountPercentage, expectedStartDate,
            expectedEndDate);
        const int expectedDepositId = 1;
        const char expectedDepositArea = 'A';
        const string expectedDepositSize = "Grande";
        const bool expectedDepositConditioning = false;
        const string depositName = "deposito";

        _depositController.AddDeposit(depositName, expectedDepositArea, expectedDepositSize, expectedDepositConditioning,
            _promotionController.ConvertPromotionsOnPromotionInDepositManagementDtos(_promotionController.GetPromotions()));

        var expectedDepositPromotionsList = _depositController.GetDepositPromotions(expectedDepositId);
        var expectedPromotionCreateReservationDtoList =
            _promotionController.ConvertPromotionsToPromotionCreateReservationDto(expectedDepositPromotionsList);


        Assert.AreEqual(expectedDepositPromotionsList.Count, expectedPromotionCreateReservationDtoList.Count);
    }

    [TestMethod]
    public void ShouldConvertDepositReviewListToReviewCreateReservationDtoList()
    {
        const string expectedReviewComment = "Muy buen deposito";
        const int expectedReviewValoration = 5;

        const string name = "cliente";
        const string email = "alejofraga22v2@gmail.com";
        const string password1 = "@DfG57254908";
        const string password2 = "@DfG57254908";

        _clientController.RegisterClient(name, email, password1, password2);

        const int expectedDepositId = 1;
        const char expectedDepositArea = 'A';
        const string expectedDepositSize = "Grande";
        const bool expectedDepositConditioning = false;
        const string depositName = "deposito";
        _depositController.AddDeposit(depositName, expectedDepositArea, expectedDepositSize, expectedDepositConditioning,
            _promotionController.ConvertPromotionsOnPromotionInDepositManagementDtos(_promotionController.GetPromotions()));

        _reservationController.CreateReservation(_clientController.GetClient(email), _depositController.GetDepositById(expectedDepositId), DateTime.Today,
            DateTime.Today.AddDays(5));
        var deposit = _reservationController.GetDeposits()[0];
        _depositController.AddDateRangeDisponibilityToDeposit(DateTime.Today, DateTime.Today.AddDays(5), deposit);
        _reservationController.ConfirmReservation(_reservationController.GetReservations()[0]);
        DateTimeProvider.SetCurrentDateTime(DateTime.Today.AddDays(10));
        var reviewReservationDto =
            _clientController.GetClientExpiredAndNotReviewedReservationInDepositReviewDtos(_clientController.GetClient(email))[0];
        _depositController.AddReviewToDeposit(expectedDepositId, expectedReviewValoration, expectedReviewComment,reviewReservationDto);

        var expectedDepositReviewList = _depositController.GetDepositReviews(expectedDepositId);
        var expectedReviewCreateReservationDtoList =
            _depositController.ConvertReviewsToReviewCreateReservationDto(expectedDepositReviewList);


        Assert.AreEqual(expectedDepositReviewList.Count, expectedReviewCreateReservationDtoList.Count);
    }

    [TestMethod]
    public void ShouldConvertDepositToDepositCreateReservationDto()
    {
        
        const string name = "cliente";
        const string email = "alejofraga22v2@gmail.com";
        const string password1 = "@DfG57254908";
        const string password2 = "@DfG57254908";

        _clientController.RegisterClient(name, email, password1, password2);
        
        const string expectedPromotionTag = "super promo";
        const int expectedPromotionDiscountPercentage = 60;
        DateTime expectedStartDate = DateTime.Today;
        DateTime expectedEndDate = DateTime.Today.AddDays(7);
        _promotionController.AddPromotion(expectedPromotionTag, expectedPromotionDiscountPercentage, expectedStartDate,
            expectedEndDate);


        const char expectedDepositArea = 'A';
        const string expectedDepositSize = "Grande";
        const bool expectedDepositConditioning = false;
        const string depositName = "deposito";
        

        _depositController.AddDeposit(depositName, expectedDepositArea, expectedDepositSize, expectedDepositConditioning,
            _promotionController.ConvertPromotionsOnPromotionInDepositManagementDtos(_promotionController.GetPromotions()));

        
        var deposit = _depositController.GetDeposits()[0];
        _depositController.AddDateRangeDisponibilityToDeposit( DateTime.Today, DateTime.Today.AddDays(5),deposit);

        var expectedDepositId = deposit.Id;
        _reservationController.CreateReservation(_clientController.GetClient(email), _depositController.GetDepositById(expectedDepositId), DateTime.Today,
            DateTime.Today.AddDays(5));
        var reservation = _reservationController.GetReservations()[0];
        _reservationController.ConfirmReservation(reservation);
        
        const string expectedReviewComment = "Muy buen deposito";
        const int expectedReviewValoration = 5;
        
        DateTimeProvider.SetCurrentDateTime(DateTime.Today.AddDays(10));
        var client = _clientController.GetClient(email);
        var reviewReservationDto =
            _clientController.GetClientExpiredAndNotReviewedReservationInDepositReviewDtos(client)[0];
        _depositController.AddReviewToDeposit(expectedDepositId, expectedReviewValoration, expectedReviewComment,
            reviewReservationDto);

        var expectedDeposit = _depositController.GetDepositById(expectedDepositId);
        DepositCreateReservationDto expectedDepositCreateReservationDto =
            _depositController.ConvertDepositToDepositCreateReservationDto(expectedDeposit);

        Assert.AreEqual(expectedDeposit.Id, expectedDepositCreateReservationDto.Id);
        Assert.AreEqual(expectedDeposit.Size, expectedDepositCreateReservationDto.Size);
        Assert.AreEqual(expectedDeposit.Area, expectedDepositCreateReservationDto.Area);
        Assert.AreEqual(expectedDeposit.Conditioning, expectedDepositCreateReservationDto.Conditionig);
        Assert.AreEqual(_depositController.GetDepositPromotions(expectedDepositId).Count,
            expectedDepositCreateReservationDto.PromotionDtoList.Count);
        Assert.AreEqual(_depositController.GetDepositReviews(expectedDepositId).Count, expectedDepositCreateReservationDto.ReviewDtoList.Count);
        Assert.AreEqual(_depositController.GetDepositReviews(expectedDepositId).Any(), expectedDepositCreateReservationDto.ContainsAnyReviews);
        Assert.AreEqual(_depositController.GetDepositPromotions(expectedDepositId).Any(), expectedDepositCreateReservationDto.HasPromotions);
    }

    [TestMethod]
    public void ShouldGetAvailableDepositListInDepositCreateReservationDtoList()
    {
        const string expectedPromotionTag = "super promo";
        const int expectedPromotionDiscountPercentage = 60;
        DateTime expectedStartDate = DateTime.Today;
        DateTime expectedEndDate = DateTime.Today.AddDays(7);

        const char expectedDepositArea = 'A';
        const string expectedDepositSize = "Grande";
        const bool expectedDepositConditioning = false;
        const string depositName = "Deposito uno";

        DateTime startDateForCreateReservation = DateTime.Today;
        DateTime endDateForCreateReservation = DateTime.Today.AddDays(7);

        _promotionController.AddPromotion(expectedPromotionTag, expectedPromotionDiscountPercentage, expectedStartDate,
            expectedEndDate);

        _depositController.AddDeposit(depositName, expectedDepositArea, expectedDepositSize, expectedDepositConditioning,
            _promotionController.ConvertPromotionsOnPromotionInDepositManagementDtos(_promotionController.GetPromotions()));

        var deposit1 = _depositController.GetDeposits()[0];

        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDateForCreateReservation,
            endDateForCreateReservation, _depositController.ConvertDepositToDepositDisponibilityDto(deposit1));

        List<DepositCreateReservationDto> expectedDepositCreateReservationDtoList =
            _depositController.GetAvailableDepositListInCreateReservationDto(startDateForCreateReservation,
                endDateForCreateReservation);

        Assert.AreEqual(_depositController.GetDeposits().Count, expectedDepositCreateReservationDtoList.Count);
    }

    [TestMethod]
    public void ShouldCalculateReservationCostWithDepositCreateReservationDto()
    {
        const string expectedPromotionTag = "super promo";
        const int expectedPromotionDiscountPercentage = 60;
        DateTime expectedStartDate = DateTime.Today;
        DateTime expectedEndDate = DateTime.Today.AddDays(7);

        const int expectedDepositId = 1;
        const char expectedDepositArea = 'A';
        const string expectedDepositSize = "Grande";
        const bool expectedDepositConditioning = false;
        const string depositName = "deposito";


        DateTime startReservationDate = DateTime.Today;
        DateTime endReservationDate = DateTime.Today.AddDays(7);

        _promotionController.AddPromotion(expectedPromotionTag, expectedPromotionDiscountPercentage, expectedStartDate,
            expectedEndDate);
        _depositController.AddDeposit(depositName, expectedDepositArea, expectedDepositSize, expectedDepositConditioning,
            _promotionController.ConvertPromotionsOnPromotionInDepositManagementDtos(_promotionController.GetPromotions()));

        Deposit expectedDeposit = _depositController.GetDepositById(expectedDepositId);

        float expectedCalculatedCost = _reservationController.CalculateReservationCost(expectedDeposit,
            startReservationDate, endReservationDate);

        DepositCreateReservationDto depositDto = new DepositCreateReservationDto();
        depositDto.Id = expectedDepositId;

        float calculatedCostWithDto = _reservationController.CalculateReservationCostWithDepositCreateReservationDto(depositDto,
            startReservationDate, endReservationDate);

        Assert.AreEqual(calculatedCostWithDto, expectedCalculatedCost);
    }

    [TestMethod]
    public void ShouldCreateReservationWithDepositCreateReservationDto()
    {
        const string expectedPromotionTag = "super promo";
        const int expectedPromotionDiscountPercentage = 60;
        DateTime expectedStartDate = DateTime.Today;
        DateTime expectedEndDate = DateTime.Today.AddDays(7);
        const int expectedPromotionId = 1;

        const int expectedDepositId = 1;
        const char expectedDepositArea = 'A';
        const string expectedDepositSize = "Grande";
        const bool expectedDepositConditioning = false;

        DateTime startReservationDate = DateTime.Today;
        DateTime endReservationDate = DateTime.Today.AddDays(7);


        string expectedName = "cliente";
        string expectedClientEmail = "cliente@gmail.com";
        string expectedClientPassword = "12345678#Aa";
        string expectedClientPasswordConfirmation = "12345678#Aa";
        const string depositName = "deposito";


        _clientController.RegisterClient(expectedName, expectedClientEmail, expectedClientPassword,
            expectedClientPasswordConfirmation);
        _promotionController.AddPromotion(expectedPromotionTag, expectedPromotionDiscountPercentage, expectedStartDate,
            expectedEndDate);
        _depositController.AddDeposit(depositName, expectedDepositArea, expectedDepositSize, expectedDepositConditioning,
            _promotionController.ConvertPromotionsOnPromotionInDepositManagementDtos(_promotionController.GetPromotions()));

        DepositCreateReservationDto depositDto = new DepositCreateReservationDto();
        depositDto.Id = expectedDepositId;


        ClientDto clientDto = new ClientDto();
        clientDto.Email = expectedClientEmail;

        var expectedDeposit = _depositController.GetDeposits()[0];

        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startReservationDate, endReservationDate,
            _depositController.ConvertDepositToDepositDisponibilityDto(expectedDeposit));

        _reservationController.CreateReservationWithDepositCreateReservationDto(clientDto, depositDto, startReservationDate,
            endReservationDate);

        Assert.IsTrue(_reservationController.GetReservations().Count == 1);
    }

    [TestMethod]
    public void ShouldGetClientExpiredAndNotReviewedReservationInDepositReviewDtosByClientDto()
    {
        const string expectedName = "Pepe el crack";
        const string expectedMail = "pepe@gmail.com";
        const string expectedPassword = "#theBestAdmin1234";
        const bool expectedIsAdmin = true;
        var clientDto = new ClientDto(expectedName, expectedMail, expectedPassword, expectedIsAdmin);
        _clientController.RegisterClient(clientDto.Name, clientDto.Email, clientDto.Password, clientDto.Password);
        var promotions = new List<PromotionInDepositManagementDto?>();
        const char area = 'A';
        const string size = "Mediano";
        const bool conditioning = false;
        const string depositName = "deposito";

        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);

        DateTime startDate = DateTime.Today;
        DateTime endDate = DateTime.Today.AddDays(5);

        var deposit = _depositController.GetDepositByName(depositName.ToUpper());
        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(startDate, endDate,
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit));


        _reservationController.CreateReservation(_clientController.GetClient(expectedMail), deposit, startDate,
            endDate);

        const int expectedReservationId = 1;
        var reservation = _reservationController.GetReservationById(expectedReservationId);


        _reservationController.ConfirmReservation(reservation);
        DateTimeProvider.SetCurrentDateTime(DateTime.Today.AddDays(10));
        var expiredAndNotReviewedReservations =
            _clientController.GetClientExpiredAndNotReviewedReservationInDepositReviewDtosByClientDto(clientDto);
        Assert.IsTrue(expiredAndNotReviewedReservations.Count == 1);
    }

    [TestMethod]
    public void ShouldGetAdminAsAClientDto()
    {
        const string expectedName = "Sebastian";
        const string expectedMail = "seba@gmail.com";
        const string expectedPassword = "#theBestAdmin1234";
        _clientController.RegisterAdmin(expectedName, expectedMail, expectedPassword, expectedPassword);
        var admin = _clientController.GetAdmin();
        var adminDto = _clientController.GetAdminByClientDto();
        Assert.AreEqual(admin.Email, adminDto.Email);
    }
    
    [TestMethod]
    public void ShouldGetNullIfNoAdminIsRegistered()
    {
        var admin = _clientController.GetAdmin();
        var adminDto = _clientController.GetAdminByClientDto();
        Assert.IsNull(admin);
        Assert.IsNull(adminDto);
    }

    [TestMethod]
    public void ShouldGetNotificationsDtoByClient()
    {
        const string expectedName = "Sebastian";
        const string expectedMail = "seba@gmail.com";
        const string expectedPassword = "#@sebaA12345";
        _clientController.RegisterClient(expectedName, expectedMail, expectedPassword, expectedPassword);
        var client = _clientController.GetClient(expectedMail);
        const ValidNotifications notification = ValidNotifications.ReservationConfirmed;
        const int notificationType = (int)notification;
        const string depositName = "pepes";
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(5);
        _clientController.AddNotificationToClient(expectedMail, notificationType, depositName, startDate, endDate);
        var notifications = _clientController.GetClientNotifications(client.Email);
        var notificationsDto = _clientController.GetClientNotificationsDtos(client.Email);
        Assert.AreEqual(notifications.Count(), notificationsDto.Count());
    }

    [TestMethod]
    public void ShouldGetDepositsInDepositDisponibilityDto()
    {
        var expectedDepositName = "DEPOSITO NUEVO";
        var expectedDepositArea = 'E';
        var expectedDepositSize = "Grande";
        var expectedDepositConditioning = true;

        var expectedDepositPromotionTag = "Nueva promo";
        var exectedDepositPromotionDiscountPercentage = 10;
        var expectedStartDatePromotion = DateTime.Today;
        var expectedEndDatePromotion = DateTime.Today.AddDays(7);

        _promotionController.AddPromotion(expectedDepositPromotionTag, exectedDepositPromotionDiscountPercentage,
            expectedStartDatePromotion, expectedEndDatePromotion);
        _depositController.AddDeposit(expectedDepositName, expectedDepositArea, expectedDepositSize, expectedDepositConditioning,
            _promotionController.ConvertPromotionsOnPromotionInDepositManagementDtos(_promotionController.GetPromotions()));

        Assert.AreEqual(_depositController.GetDeposits().Count, _depositController.GetDepositsInDepositDisponibilityDto().Count);
    }


    [TestMethod]
    public void ShouldAddDateRangeDisponibilityToDepositWithDisponibilityDepositDto()
    {
        var expectedDepositName = "DEPOSITO NUEVO";
        var expectedDepositArea = 'E';
        var expectedDepositSize = "Grande";
        var expectedDepositConditioning = true;
        var expectedDepositId = 1;

        var expectedDateRangeStartDate = DateTime.Today;
        var expectedDateRangeEndDate = DateTime.Today.AddDays(7);

        List<PromotionInDepositManagementDto> promotionInDepositManagementDto =
            new List<PromotionInDepositManagementDto>();

        _depositController.AddDeposit(expectedDepositName, expectedDepositArea, expectedDepositSize, expectedDepositConditioning,
            promotionInDepositManagementDto);

        var deposit = _depositController.GetDeposits()[0];
        DepositDisponibilityDto selectedDepositDto =
            _depositController.ConvertDepositToDepositDisponibilityDto(deposit);

        _depositController.AddDateRangeDisponibilityToDepositWithDisponibilityDepositDto(expectedDateRangeStartDate,
            expectedDateRangeEndDate, selectedDepositDto);

        Assert.IsTrue(_depositController.DepositHasDisponibility(deposit.Id));
    }
}