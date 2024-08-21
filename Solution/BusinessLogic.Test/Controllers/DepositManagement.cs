using Controllers;
using DataLayer.repositories;
using DataLayer.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.FacadeManagement;

[TestClass]
public class DepositManagement
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


    [TestMethod]
    public void ShouldReturnTrueIfPromotionIsBeingUsedInDeposits()
    {
        const int expectedPromotionId = 1;
        const string? expectedPromotionTag = "Cool Promotion";
        const int expectedPromotionDiscount = 70;
        var expectedPromotionStartDate = DateTime.Today;
        var expectedPromotionEndDate = DateTime.Today.AddDays(7);
        _promotionController.AddPromotion(expectedPromotionTag, expectedPromotionDiscount, expectedPromotionStartDate,
            expectedPromotionEndDate);
        const char depositExpectedArea = 'A';
        const string? depositExpectedSize = "Pequeño";
        const bool depositExpectedConditioning = true;
        var depositExpectedPromotions = _promotionController.GetPromotionInDepositManagementDtos();
        const string depositName = "deposito";

        _depositController.AddDeposit(depositName, depositExpectedArea, depositExpectedSize,
            depositExpectedConditioning, depositExpectedPromotions);

        Assert.IsTrue(_promotionController.PromotionIsAplyingToAnyDeposit(expectedPromotionId));
    }

    [TestMethod]
    public void ShouldAddCorrectDeposit()
    {
        const char area = 'A';
        const string? size = "Pequeño";
        const bool conditioning = false;
        const int expectedDepositId = 1;
        const string name = "newDepo";
        var promotions = new List<PromotionInDepositManagementDto?>();
        _depositController.AddDeposit(name, area, size, conditioning, promotions);
        Assert.IsNotNull(_depositController.GetDepositById(expectedDepositId));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenTryingToAddDepositWithExistingName()
    {
        const char area = 'A';
        const string? size = "Pequeño";
        const bool conditioning = false;
        const string expectedName = "newDepo";
        _depositController.AddDeposit(expectedName, area, size, conditioning,
            new List<PromotionInDepositManagementDto?>());
        Assert.ThrowsException<ArgumentException>(() =>
            _depositController.AddDeposit(expectedName, area, size, conditioning,
                new List<PromotionInDepositManagementDto?>()));
    }


    [TestMethod]
    public void ShouldThrowExceptionWhenTryingToAddNonExistingPromotionToDeposit()
    {
        const char area = 'A';
        const string? size = "Pequeño";
        const bool conditioning = false;
        const string? tag = "promo 1";
        const int discountPercentage = 10;
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        const int promotion1Id = 1;
        const int nonExistantPromotionId = 2;
        const string name = "newDepo";

        _promotionController.AddPromotion(tag, discountPercentage, startDate, endDate);

        var promotion1 = new PromotionInDepositManagementDto(promotion1Id, "Promo 1");
        var nonExistingPromotion =
            new PromotionInDepositManagementDto(nonExistantPromotionId, "nonExistingPromo");
        var promotions = new List<PromotionInDepositManagementDto?>
        {
            promotion1,
            nonExistingPromotion
        };
        Assert.ThrowsException<NullReferenceException>(() =>
            _depositController.AddDeposit(name, area, size, conditioning, promotions));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenTryingToAddInvalidDeposit()
    {
        const char invalidArea = 'Z';
        const string? size = "Pequeño";
        const bool conditioning = false;
        const string? tag = "promo 1";
        const int discountPercentage = 10;
        const string name = "newDepo";
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        const int promotion1Id = 1;

        _promotionController.AddPromotion(tag, discountPercentage, startDate, endDate);

        var promotion1 = new PromotionInDepositManagementDto(promotion1Id, "Promo1");
        var promotions = new List<PromotionInDepositManagementDto?>
        {
            promotion1,
        };
        Assert.ThrowsException<ArgumentException>(() =>
            _depositController.AddDeposit(name, invalidArea, size, conditioning, promotions));
    }

    [TestMethod]
    public void ShouldAddCorrectDepositWithMultiplePromotions()
    {
        const char area = 'A';
        const string? size = "Pequeño";
        const bool conditioning = false;
        const string? tag1 = "Promo 1";
        const string? tag2 = "Promo 2";
        const int discountPercentage = 10;
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        const int promotion1Id = 1;
        const int promotion2Id = 2;
        const string name = "newDepo";

        _promotionController.AddPromotion(tag1, discountPercentage, startDate, endDate);
        _promotionController.AddPromotion(tag2, discountPercentage, startDate, endDate);
        var promo1 = new PromotionInDepositManagementDto(promotion1Id, tag1);
        var promo2 = new PromotionInDepositManagementDto(promotion2Id, tag2);
        var promotions = new List<PromotionInDepositManagementDto?>
        {
            promo1,
            promo2
        };
        const int expectedDepositId = 1;
        _depositController.AddDeposit(name, area, size, conditioning, promotions);
        var depositPromotions = _depositController.GetDepositPromotions(expectedDepositId);
        Assert.IsNotNull(depositPromotions.Find(p => p.Tag == tag1));
        Assert.IsNotNull(depositPromotions.Find(p => p.Tag == tag2));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToRemoveDepositThatHasReservations()
    {
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = false;
        const string depositName = "newDepo";
        var promotions = new List<PromotionInDepositManagementDto?>();

        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const string email = "alejofraga22v2@gmail.com";
        const string name = "alejo";
        const string password1 = "P@ssW0rd2024";
        const string password2 = "P@ssW0rd2024";
        _clientController.RegisterAdmin(name, email, password1, password2);
        var client = _clientController.GetClient(email);
        var deposit = _depositController.GetDeposits()[0];
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(1);
        _reservationController.CreateReservation(client, deposit, startDate, endDate);
        const int expectedDepositId = 1;
        Assert.ThrowsException<ArgumentException>(() => _depositController.RemoveDeposit(expectedDepositId));
    }

    [TestMethod]
    public void ShouldRemoveDepositThatHasNoReservations()
    {
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = false;
        const string depositName = "newDepo";
        var promotions = new List<PromotionInDepositManagementDto?>();
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const string email = "alejofraga22v2@gmail.com";
        const string name = "alejo";
        const string password1 = "P@ssW0rd2024";
        const string password2 = "P@ssW0rd2024";
        const int expectedDepositId = 1;
        _clientController.RegisterAdmin(name, email, password1, password2);
        _depositController.RemoveDeposit(expectedDepositId);
        Assert.IsTrue((_depositController.GetDeposits().Count == 0));
    }

    [TestMethod]
    public void ShouldReturnTrueIfDepositHasReservations()
    {
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = false;
        var promotions = new List<PromotionInDepositManagementDto?>();
        const string depositName = "newDepo";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        const string email = "alejofraga22v2@gmail.com";
        const string name = "alejo";
        const string password1 = "P@ssW0rd2024";
        const string password2 = "P@ssW0rd2024";
        _clientController.RegisterClient(name, email, password1, password2);
        var client = _clientController.GetClient(email);
        const int expectedDepositId = 1;
        var deposit = _depositController.GetDepositById(expectedDepositId);
        var startDate = DateTime.Today;
        var endDate = DateTime.Today.AddDays(7);

        _reservationController.CreateReservation(client, deposit, startDate, endDate);

        Assert.IsTrue(_depositController.DepositHasGotReservations(expectedDepositId));
    }
    
    [TestMethod]
    public void ShouldThrowExceptionWhenTryingToGetNonExistantDeposit()
    {
        const int nonExistantDepositId = 1;
        Assert.ThrowsException<NullReferenceException>(() => _clientController.GetDepositById(nonExistantDepositId));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenTryingToAddNonExistantPromotionTODeposit()
    {
        const char area = 'A';
        const string? size = "Pequeño";
        const bool conditioning = false;
        var nonExistantPromotion = new PromotionInDepositManagementDto();
        nonExistantPromotion.Id = 15;
        var promotions = new List<PromotionInDepositManagementDto?>();
        promotions.Add(nonExistantPromotion);
        const string depositName = "newDepo";
        Assert.ThrowsException<NullReferenceException>(()=>_depositController.AddDeposit(depositName, area, size, conditioning, promotions));
    }
    
    [TestMethod]
    public void ShouldThrowExceptionWhenTryingToAddDepositWithExistantName()
    {
        const char area = 'A';
        const string? size = "Pequeño";
        const bool conditioning = false;
        var promotions = new List<PromotionInDepositManagementDto?>();
        const string depositName = "newDepo";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        Assert.ThrowsException<ArgumentException>(()=>_depositController.AddDeposit(depositName, area, size, conditioning, promotions));
    }

    [TestMethod]
    public void ShouldThrowExceptionsWhenAttemptingToGetNonExistantPromotion()
    {
        const int nonExistantPromotionId = 1;
        Assert.ThrowsException<NullReferenceException>(() => _depositController.GetPromotionById(nonExistantPromotionId));
    }

    [TestMethod]
    public void ShouldReturnFalseIfDepositDoesntHaveReservations()
    {
        const char area = 'A';
        const string size = "Pequeño";
        const bool conditioning = false;
        var promotions = new List<PromotionInDepositManagementDto?>();
        const string depositName = "newDepo";
        _depositController.AddDeposit(depositName
            , area, size, conditioning, promotions);
        const int expectedDepositId = 1;
        Assert.IsFalse(_depositController.DepositHasGotReservations(expectedDepositId));
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToGetNonExistantDepositFromFacade()
    {
        const int unregisteredDepositId = 1;
        Assert.ThrowsException<NullReferenceException>(() => _depositController.GetDepositById(unregisteredDepositId));
    }

    [TestMethod]
    public void ShouldReturnTrueIfAreRegisteredDepositsIntoFacade()
    {
        const char area = 'A';
        const string? size = "Pequeño";
        const bool conditioning = false;
        var promotions = new List<PromotionInDepositManagementDto?>();
        const string depositName = "newDepo";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        Assert.IsTrue(_depositController.AreDepositsRegistered());
    }

    [TestMethod]
    public void ShouldBeFalseIfNotAreRegisteredDepositsIntoFacade()
    {
        Assert.IsFalse(_depositController.AreDepositsRegistered());
    }

    [TestMethod]
    public void ShouldGetDepositFromFacade()
    {
        const char area = 'A';
        const string? size = "Pequeño";
        const bool conditioning = false;
        var promotions = new List<PromotionInDepositManagementDto?>();
        const int expectedDepositId = 1;
        const string depositName = "newDepo";
        _depositController.AddDeposit(depositName, area, size, conditioning, promotions);
        Assert.IsNotNull(_depositController.GetDepositById(expectedDepositId));
        Assert.IsTrue(_depositController.GetDepositById(expectedDepositId).Area == area);
        Assert.IsTrue(_depositController.GetDepositById(expectedDepositId).Size == size);
        Assert.IsTrue(_depositController.GetDepositById(expectedDepositId).Conditioning == conditioning);
    }
}