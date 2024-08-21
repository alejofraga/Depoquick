using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class ReservationInDepositReviewDTO_Test
{
    [TestMethod]
    public void ShouldCreateReservationInDepositReviewDto()
    {
        ReviewReservationDto reviewReservationDto = new ReviewReservationDto();
        Assert.IsNotNull(reviewReservationDto); 
    }
    
    [TestMethod]
    public void ShouldSetReservationInDepositReviewDto()
    {
        var expectedReservationId = 1;
        const string expectedDepositName = "deposito";
        var expectedDepositId = 1;
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(5);
        var expectedCost = 100;
        
        ReviewReservationDto reviewReservationDto = new ReviewReservationDto();
        reviewReservationDto.ReservationId = expectedReservationId;
        reviewReservationDto.DepositId = expectedDepositId;
        reviewReservationDto.StartDate = expectedStartDate;
        reviewReservationDto.EndDate = expectedEndDate;
        reviewReservationDto.Cost = expectedCost;
        reviewReservationDto.DepositName = expectedDepositName;
        Assert.AreEqual(expectedReservationId, reviewReservationDto.ReservationId);
        Assert.AreEqual(expectedDepositId, reviewReservationDto.DepositId);
        Assert.AreEqual(expectedStartDate, reviewReservationDto.StartDate);
        Assert.AreEqual(expectedEndDate, reviewReservationDto.EndDate);
        Assert.AreEqual(expectedCost, reviewReservationDto.Cost);
        Assert.AreEqual(expectedDepositName, reviewReservationDto.DepositName);
    }
    
    [TestMethod]
    public void ShouldCreateReservationInDepositReviewDtoWithParameters()
    {
        var expectedReservationId = 1;
        var expectedDepositId = 1;
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(5);
        var expectedCost = 100;
        const string expectedDepositName = "Deposito";
        
        ReviewReservationDto reviewReservationDto = new ReviewReservationDto(expectedReservationId, expectedDepositId, expectedDepositName,expectedStartDate, expectedEndDate, expectedCost);
        Assert.AreEqual(expectedReservationId, reviewReservationDto.ReservationId);
        Assert.AreEqual(expectedDepositId, reviewReservationDto.DepositId);
        Assert.AreEqual(expectedStartDate, reviewReservationDto.StartDate);
        Assert.AreEqual(expectedEndDate, reviewReservationDto.EndDate);
    }
}