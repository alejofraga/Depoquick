using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;


[TestClass]
public class ViewReservationDTOTest
{
    private ViewReservationDto _viewReservationDTO;
    [TestInitialize]
    public void Setup()
    {
        _viewReservationDTO = new ViewReservationDto();
    }
    [TestMethod]
    public void ShouldCreateViewReservationDto()
    {
        Assert.IsNotNull(_viewReservationDTO); 
    }
    
    [TestMethod]
    public void ShouldSetViewReservationDto()
    {
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(7);
        const bool expectedIsConfirmed = false;
        const bool expectedIsRejected =  false;
        const int expectedDepositId =  4;
        const bool expectedIsReviewed = false;
        const int expectedId = 6;
        const string expectedDepositName = "Deposito";
        const string expectedRejectedMessage = "Lo siento, rechazamos la reserva.";
        const int expectedPaymentStatus = 1;

        ViewReservationDto viewReservationDTO = new ViewReservationDto();
        
        viewReservationDTO.StartDate = expectedStartDate;
        viewReservationDTO.EndDate = expectedEndDate;
        viewReservationDTO.IsConfirmed = expectedIsConfirmed;
        viewReservationDTO.IsRejected = expectedIsRejected;
        viewReservationDTO.DepositId = expectedDepositId;
        viewReservationDTO.IsReviewed = expectedIsReviewed;
        viewReservationDTO.Id = expectedId;
        viewReservationDTO.DepositName = expectedDepositName;
        viewReservationDTO.RejectedMessage = expectedRejectedMessage;
        viewReservationDTO.PaymentStatus = expectedPaymentStatus;
        
        Assert.AreEqual(expectedStartDate,viewReservationDTO.StartDate);
        Assert.AreEqual(expectedEndDate,viewReservationDTO.EndDate);
        Assert.AreEqual(expectedIsConfirmed,viewReservationDTO.IsConfirmed);
        Assert.AreEqual(expectedIsRejected,viewReservationDTO.IsRejected);
        Assert.AreEqual(expectedDepositId,viewReservationDTO.DepositId);
        Assert.AreEqual(expectedIsReviewed,viewReservationDTO.IsReviewed);
        Assert.AreEqual(expectedId,viewReservationDTO.Id);
        Assert.AreEqual(expectedDepositName,viewReservationDTO.DepositName);
        Assert.AreEqual(expectedRejectedMessage,viewReservationDTO.RejectedMessage);
        Assert.AreEqual(1,viewReservationDTO.PaymentStatus);
    }

    [TestMethod]
    public void ShouldSetViewReservationDtoWithParameters()
    {
        
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(7);
        bool expectedIsConfirmed = false;
        bool expectedIsRejected =  false;
        int expectedDepositId =  4 ;
        bool expectedIsReviewed = false;
        int expectedId = 6;
        string expectedRejectedMessage = "Lo siento, rechazamos la reserva.";
        int expectedPaymentStatus = 1;
        string expectedDepositName = "Deposito";
        
        ViewReservationDto viewReservationDTO = new ViewReservationDto(expectedStartDate, expectedEndDate, expectedIsConfirmed,
            expectedIsRejected, expectedDepositId, expectedIsReviewed, expectedId,expectedDepositName, expectedRejectedMessage, expectedPaymentStatus);
        
        Assert.AreEqual(expectedStartDate,viewReservationDTO.StartDate);
        Assert.AreEqual(expectedEndDate,viewReservationDTO.EndDate);
        Assert.AreEqual(expectedIsConfirmed,viewReservationDTO.IsConfirmed);
        Assert.AreEqual(expectedIsRejected,viewReservationDTO.IsRejected);
        Assert.AreEqual(expectedDepositId,viewReservationDTO.DepositId);
        Assert.AreEqual(expectedIsReviewed,viewReservationDTO.IsReviewed);
        Assert.AreEqual(expectedId,viewReservationDTO.Id);
        Assert.AreEqual(expectedRejectedMessage,viewReservationDTO.RejectedMessage);
        Assert.AreEqual(expectedPaymentStatus,viewReservationDTO.PaymentStatus);
    }
}