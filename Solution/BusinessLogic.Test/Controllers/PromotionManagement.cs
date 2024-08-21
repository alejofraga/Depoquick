using Controllers;
using DataLayer.repositories;
using DataLayer.Tests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.FacadeManagement;

[TestClass]
public class PromotionManagement
{
    private PromotionController _promotionController;
    private DepositController _depositController;
    [TestInitialize]
    public void OnInitialize()
    {
        var programTest = new ProgramTest();
        var scope = programTest.ServiceProvider.CreateScope();
        var promotionRepository = scope.ServiceProvider.GetRequiredService<PromotionRepository>();
        var depositRepository = scope.ServiceProvider.GetRequiredService<DepositRepository>();
        var reviewRepository = scope.ServiceProvider.GetRequiredService<ReviewRepository>();
        var reservationRepository = scope.ServiceProvider.GetRequiredService<ReservationRepository>();

        _promotionController = new PromotionController(promotionRepository);
        _depositController = new DepositController(depositRepository, promotionRepository,reservationRepository,reviewRepository);
    }

    [TestMethod]
    public void ShouldGetPromotions()
    {
        Assert.IsNotNull(_promotionController.GetPromotions());
    }

    [TestMethod]
    public void ShouldAddCorrectPromotion()
    {
        const string? tag = "promo 1";
        const int discountPercentage = 10;
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        _promotionController.AddPromotion(tag, discountPercentage, startDate, endDate);
        Assert.IsTrue(_promotionController.GetPromotions().Count == 1);
    }

    [TestMethod]
    public void ShouldThrowExceptionWhenAddingPromotionWithInvalidArgumentsFromFacade()
    {
        const string? tag = "promo 1";
        const int invalidDiscountPercentage = 0;
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        Assert.ThrowsException<ArgumentException>(() =>
            _promotionController.AddPromotion(tag, invalidDiscountPercentage, startDate, endDate));
    }

    [TestMethod]
    public void ShouldRemovePromotion()
    {
        const string? tag = "promo 1";
        const int discountPercentage = 10;
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        const int promotionId = 1;
        _promotionController.AddPromotion(tag, discountPercentage, startDate, endDate);
        _promotionController.RemovePromotion(promotionId);
        Assert.IsTrue(_promotionController.GetPromotions().Count == 0);
    }

    [TestMethod]
    public void ShouldUpdatePromotion()
    {
        const string? tag = "promo 1";
        const int discountPercentage = 10;
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        const int promotionId = 1;
        _promotionController.AddPromotion(tag, discountPercentage, startDate, endDate);
        const string? updatedTag = "Oferta de verano";
        const int updatedDiscountPercentage = 50;
        var updatedStartDate = DateTime.Now.AddDays(2);
        var updatedEndDate = DateTime.Now.AddDays(15);
        _promotionController.UpdatePromotion(updatedTag, updatedDiscountPercentage, updatedStartDate, updatedEndDate, promotionId);

        Assert.IsTrue(_promotionController.GetPromotions().Count == 1);
        Assert.IsTrue(_promotionController.GetPromotions().Find(p => p.Id == promotionId).Tag == updatedTag);
        Assert.IsTrue(_promotionController.GetPromotions().Find(p => p.Id == promotionId).DiscountPercentage ==
                      updatedDiscountPercentage);
        Assert.IsTrue(_promotionController.GetPromotions().Find(p => p.Id == promotionId).StartDate == updatedStartDate);
        Assert.IsTrue(_promotionController.GetPromotions().Find(p => p.Id == promotionId).EndDate == updatedEndDate);
    }

  

    [TestMethod]
    public void ShouldReturnFalseIfPromotionIsNotBeingUsedInDeposits()
    {
        const string? expectedPromotionTag = "Cool Promotion";
        const int expectedPromotionDiscount = 70;
        var expectedPromotionStartDate = DateTime.Today;
        var expectedPromotionEndDate = DateTime.Today.AddDays(7);

        _promotionController.AddPromotion(expectedPromotionTag, expectedPromotionDiscount, expectedPromotionStartDate,
            expectedPromotionEndDate);
        const int expectedPromotionId = 1;

        Assert.IsFalse(_promotionController.PromotionIsAplyingToAnyDeposit(expectedPromotionId
        ));
    }


    [TestMethod]
    public void ShouldThrowExceptionIfTriesToRemovePromotionThatBeingUsedInDeposits()
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

        _depositController.AddDeposit(depositName, depositExpectedArea, depositExpectedSize, depositExpectedConditioning,
            depositExpectedPromotions);


        Assert.ThrowsException<ArgumentException>(() => _promotionController.RemovePromotion(expectedPromotionId));
    }


    [TestMethod]
    public void ShouldRemovePromotionIsNotBeingUsedInDeposits()
    {
        const int expectedPromotionId = 1;
        const string? expectedPromotionTag = "Cool Promotion";
        const int expectedPromotionDiscount = 70;
        var expectedPromotionStartDate = DateTime.Today;
        var expectedPromotionEndDate = DateTime.Today.AddDays(7);

        _promotionController.AddPromotion(expectedPromotionTag, expectedPromotionDiscount, expectedPromotionStartDate,
            expectedPromotionEndDate);
        _promotionController.RemovePromotion(expectedPromotionId);

        Assert.IsTrue(_promotionController.GetPromotions().Count == 0);
    }

    [TestMethod]
    public void ShouldGetPromotionFromFacadeIfPromotionExists()
    {
        const int expectedPromotionId = 1;
        const string? expectedPromotionTag = "Cool Promotion";
        const int expectedPromotionDiscount = 70;
        var expectedPromotionStartDate = DateTime.Today;
        var expectedPromotionEndDate = DateTime.Today.AddDays(7);

        _promotionController.AddPromotion(expectedPromotionTag, expectedPromotionDiscount, expectedPromotionStartDate,
            expectedPromotionEndDate);

        Assert.IsNotNull(_promotionController.GetPromotionById(expectedPromotionId));
        Assert.IsTrue(_promotionController.GetPromotionById(expectedPromotionId).Tag == expectedPromotionTag);
        Assert.IsTrue(_promotionController.GetPromotionById(expectedPromotionId).DiscountPercentage == expectedPromotionDiscount);
        Assert.IsTrue(_promotionController.GetPromotionById(expectedPromotionId).StartDate == expectedPromotionStartDate);
        Assert.IsTrue(_promotionController.GetPromotionById(expectedPromotionId).EndDate == expectedPromotionEndDate);
    }


    [TestMethod]
    public void ShouldThrowExceptionWhenAttemptingToGetNonExistantPromotionFromFacade()
    {
        const int expectedPromotionId = 1;
        Assert.ThrowsException<NullReferenceException>(() => _promotionController.GetPromotionById(expectedPromotionId));
    }

    [TestMethod]
    public void ShouldTrowExceptionWhenAttemptingToUpdatePromotionThatBeingUsedInDeposits()
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

        _depositController.AddDeposit(depositName, depositExpectedArea, depositExpectedSize, depositExpectedConditioning,
            depositExpectedPromotions);

        Assert.ThrowsException<ArgumentException>(() => _promotionController.UpdatePromotion(expectedPromotionTag,
            expectedPromotionDiscount, expectedPromotionStartDate, expectedPromotionEndDate, expectedPromotionId));
    }
}