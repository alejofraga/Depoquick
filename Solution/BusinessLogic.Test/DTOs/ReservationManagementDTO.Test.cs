using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class ReservationManagementDtoTest
{
    
    private ReservationManagementDTO _reservationManagementDto;
    [TestInitialize]
    public void Setup()
    {
        _reservationManagementDto = new ReservationManagementDTO();
    }
    [TestMethod]
    public void ShouldCreateReservationManagementDto()
    {
        Assert.IsNotNull(_reservationManagementDto);
    }
    
    [TestMethod]
    public void ShouldSetReservationManagementDto()
    {
        _reservationManagementDto.DepositId = 1;
        _reservationManagementDto.ReservationId = 1;
        _reservationManagementDto.ClientName = "Pepe el crack";
        _reservationManagementDto.ClientMail = "alejofraga22v2@gmail.com";
        _reservationManagementDto.StartDate = DateTime.Today;
        _reservationManagementDto.EndDate = DateTime.Today.AddDays(5);

        const int expectedDepositId = 1;
        const int expectedReservationId = 1;
        const string expectedClientName = "Pepe el crack";
        const string expectedClientMail = "alejofraga22v2@gmail.com";
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(5);
        
        Assert.AreEqual(expectedDepositId, _reservationManagementDto.DepositId);
        Assert.AreEqual(expectedReservationId, _reservationManagementDto.ReservationId);
        Assert.AreEqual(expectedClientName, _reservationManagementDto.ClientName);
        Assert.AreEqual(expectedClientMail, _reservationManagementDto.ClientMail);
        Assert.AreEqual(expectedStartDate, _reservationManagementDto.StartDate);
        Assert.AreEqual(expectedEndDate, _reservationManagementDto.EndDate);
        
    }

    [TestMethod]

    public void ShouldCreateReservationManagementDtoWithParameters()
    {
        
        const int expectedDepositId = 1;
        const string expectedDepositName = "Deposito";
        const int expectedReservationId = 1;
        const string expectedClientName = "Pepe el crack";
        const string expectedClientMail = "alejofraga22v2@gmail.com";
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(5);
        
        var reservationManagementDto = new ReservationManagementDTO(expectedDepositId, expectedReservationId, expectedDepositName,expectedClientName, expectedClientMail, expectedStartDate, expectedEndDate);
        
        Assert.AreEqual(expectedDepositId, reservationManagementDto.DepositId);
        Assert.AreEqual(expectedReservationId, reservationManagementDto.ReservationId);
        Assert.AreEqual(expectedClientName, reservationManagementDto.ClientName);
        Assert.AreEqual(expectedClientMail, reservationManagementDto.ClientMail);
        Assert.AreEqual(expectedStartDate, reservationManagementDto.StartDate);
        Assert.AreEqual(expectedEndDate, reservationManagementDto.EndDate);
        Assert.AreEqual(expectedDepositName, reservationManagementDto.DepositName);
        
    }
}