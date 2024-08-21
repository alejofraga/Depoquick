using BusinessLogic.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.Domain;

[TestClass]
public class PromotionTests
{
    [TestMethod]
    public void ShouldCreatePromotion()
    {
        var promotion = new Promotion();
        Assert.IsNotNull(promotion);
    }

    [TestMethod]
    public void ShouldSetAndGetTag()
    {
        var promotion = new Promotion();
        const string expectedTag = "tag 02";
        promotion.Tag = expectedTag;
        Assert.AreEqual(expectedTag, promotion.Tag);
    }
    
    [TestMethod]
    public void ShouldSetAndGetCorrectDiscountPercentage()
    {
        var promotion = new Promotion();
        const int expectedDiscountPercentage = 20;
        promotion.DiscountPercentage = expectedDiscountPercentage;
        Assert.AreEqual(expectedDiscountPercentage, promotion.DiscountPercentage);
    }
    
    [TestMethod]
    public void ShouldSetAndGetCorrectStartDate()
    {
        const string expectedTag = "tag 02";
        const int expectedDiscountPercentage = 20;
        var expectedStartDate = DateTime.Now;
        var expectedEndDate = DateTime.Now.AddDays(1);

        var promotion = new Promotion(expectedTag, expectedDiscountPercentage, expectedStartDate,expectedEndDate);
        Assert.AreEqual(expectedStartDate, promotion.StartDate);
    }

    [TestMethod]
    public void ShouldSetAndGetCorrectEndDate()
    {
        const string? expectedTag = "tag 02";
        const int expectedDiscountPercentage = 20;
        var expectedStartDate = DateTime.Now;
        var expectedEndDate = DateTime.Now.AddDays(5);

        var promotion = new Promotion(expectedTag, expectedDiscountPercentage, expectedStartDate,expectedEndDate);
        Assert.AreEqual(expectedEndDate, promotion.EndDate);
    }
    
    
    [TestMethod]
    public void PercentageIsValidIfIsUpTo75()
    {
        var promotion = new Promotion();
        const int expectedDiscountPercentage = 75;
        Assert.IsTrue(promotion.DiscountPercentageIsUpTo75(expectedDiscountPercentage));
    }
    
    [TestMethod]
    public void PercentageIsInvalidIfIsMoreThan75()
    {
        var promotion = new Promotion();
        const int expectedDiscountPercentage = 76;
        Assert.IsFalse(promotion.DiscountPercentageIsUpTo75(expectedDiscountPercentage ));
    }

    [TestMethod]
    public void PercentageIsValidIfIsGreaterThanOrEqualTo5()
    {
        var promotion = new Promotion();
        const int expectedDiscountPercentage = 5;
        Assert.IsTrue(promotion.DiscountPercentageIsGreaterThanOrEqualTo5(expectedDiscountPercentage));
    }

    [TestMethod]
    public void PercentageIsInvalidIfIsLessThan5()
    {
        var promotion = new Promotion();
        const int expectedDiscountPercentage = 4;
        Assert.IsFalse(promotion.DiscountPercentageIsGreaterThanOrEqualTo5(expectedDiscountPercentage));
    }

    [TestMethod]
    public void ShouldThrowExceptionIfTagIsInvalid()
    {
        var promotion = new Promotion();
        const string? expectedTag = "tag02!";
        Assert.ThrowsException<ArgumentException>( () => promotion.Tag = expectedTag);
    }
    
    [TestMethod]
    public void ShouldThrowExceptionIfDiscountPercentageIsInvalid()
    {
        var promotion = new Promotion();
        const int expectedDiscountPercentage = 76;
        Assert.ThrowsException<ArgumentException>( () => promotion.DiscountPercentage = expectedDiscountPercentage);
    }

    [TestMethod]
    public void StartDateIsInvalidIfItIsAfterTheEndDate()
    {
        var promotion = new Promotion();
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now;
        Assert.IsFalse(promotion.StartDateIsBeforeEndDate(startDate, endDate));
    }
    
    [TestMethod]
    public void StartDateIsValidIfItIsBeforeTheEndDate()
    {
        var promotion = new Promotion();
        var startDate = DateTime.Now;
        var endDate = DateTime.Now.AddDays(5);
        Assert.IsTrue(promotion.StartDateIsBeforeEndDate(startDate, endDate));
    }
    
    [TestMethod]
    public void ShouldThrowExceptionIfStartDateIsAfterEndDate()
    {
        const string? expectedTag = "tag 02";
        const int expectedDiscountPercentage = 20;
        var startDate = DateTime.Now.AddDays(1);
        var endDate = DateTime.Now;
        Assert.ThrowsException<ArgumentException>( () => new Promotion(expectedTag,expectedDiscountPercentage,startDate, endDate));
    }
    
    [TestMethod]
    public void DateIsValidIfItIsTodayOrInTheFuture()
    {
        var promotion = new Promotion();
        var date = DateTime.Now;
        Assert.IsTrue(promotion.DateIsTodayOrInTheFuture(date));
    }
    
    [TestMethod]
    public void DateIsInvalidIfItIsInThePast()
    {
        var promotion = new Promotion();
        var date = DateTime.Now.AddDays(-1);
        Assert.IsFalse(promotion.DateIsTodayOrInTheFuture(date));
    }
    
    [TestMethod]
    public void ShouldThrowExceptionIfStartDateIsInThePast()
    {
        const string? expectedTag = "tag 02";
        const int expectedDiscountPercentage = 20;
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now;
        Assert.ThrowsException<ArgumentException>( () => new Promotion(expectedTag,expectedDiscountPercentage,startDate, endDate));
    }
    
    [TestMethod]
    
    public void EqualFunctionShouldReturnFalseIfOneParameterIsNull()
    {
        var promotion = new Promotion();
        Promotion? other = null;
        Assert.IsFalse(promotion.Equals(other));
    }

}