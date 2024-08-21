using BusinessLogic.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.Domain;

[TestClass]
public class CreateReservation
{

    [TestInitialize]
    public void TestInitialize()
    {
        DateTimeProvider.SetCurrentDateTime(DateTime.Today);
    }
    [TestMethod]
    public void ShouldCreateReservation()
    {
        var reservation = new Reservation();
        Assert.IsNotNull(reservation);
    }

    [TestMethod]
    public void ShouldSetCorrectStartReservationDate()
    {
        var reservation = new Reservation();
        var expectedDate = DateTime.Now;
        reservation.StartDate = expectedDate;
        Assert.AreEqual(reservation.StartDate, expectedDate);
    }


    [TestMethod]
    public void ShouldThrowExceptionIfSettedStartDateIsBeforeActualDate()
    {
        var reservation = new Reservation();
        var invalidDate = DateTime.Now.AddDays(-1);
        Assert.ThrowsException<ArgumentException>(() => reservation.StartDate = invalidDate);
    }

    [TestMethod]
    public void ShouldSetCorrectEndReservationDate()
    {
        var reservation = new Reservation();
        var expectedDate = DateTime.Today;
        reservation.EndDate = expectedDate;
        Assert.AreEqual(reservation.EndDate, expectedDate);
    }



    [TestMethod]
    public void ShouldSetCorrectIsConfirmedFlag()
    {
        var reservation = new Reservation();
        const bool expectedFlagValue = true;
        reservation.IsConfirmed = expectedFlagValue;
        Assert.AreEqual(reservation.IsConfirmed, expectedFlagValue);
    }
    
    
    [TestMethod]
    public void ShouldThrowExceptionIfRejectedMessageIsGreaterThan300Characters()
    {
        var reservation = new Reservation();
        var invalidMessage = new string('a', 301);
        Assert.ThrowsException<ArgumentException>(() => reservation.RejectedMessage = invalidMessage);
    }

    [TestMethod]
    public void ShouldGetTrueIfReservationIsReviewed()
    {
        var reservation = new Reservation();
        reservation.IsReviewed = true;
        Assert.IsTrue(reservation.IsReviewed);
    }


    [TestMethod]
    public void ShouldThrowExceptionIfReservationsIsLessThanOneDayLong()
    {
        var startDate = DateTime.Now;
        var endDate = DateTime.Now;
        Assert.ThrowsException<ArgumentException>(() => new Reservation(startDate, endDate, false, false));
    }
    
    [TestMethod]
    
    public void EqualFunctionShouldReturnFalseIfOneParameterIsNull()
    {
        var reservation = new Reservation();
        Assert.IsFalse(reservation.Equals(null));
    }
    
    [TestMethod]
    public void ShouldSetAndRetrievePaidPaymentStatus()
    {
        var reservation = new Reservation();

        reservation.PaymentStatus = PaymentStatus.Paid;

        Assert.AreEqual(PaymentStatus.Paid, reservation.PaymentStatus);
    }

    [TestMethod]
    public void ShouldBeNoPaymentTheDefaultPaymentStatus()
    {
        var reservation = new Reservation();
        
        Assert.AreEqual(PaymentStatus.NoPayment, reservation.PaymentStatus);
    }
    
    [TestMethod]
    public void ShouldSetAndRetrievePendingPaymentStatus()
    {
        var reservation = new Reservation();
        reservation.PaymentStatus = PaymentStatus.Pending;
        Assert.AreEqual(PaymentStatus.Pending, reservation.PaymentStatus);
    }
}