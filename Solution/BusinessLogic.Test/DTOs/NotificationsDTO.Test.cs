using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class NotificationsDTO_Test
{
    private NotificationsDto _notificationsDto;

    [TestInitialize]
    public void Setup()
    {
        _notificationsDto = new NotificationsDto();
    }
    
    [TestMethod]
    public void ShouldCreateNotificationsDto()
    {
        Assert.IsNotNull(_notificationsDto);
    }
    
    [TestMethod]
    public void ShouldSetNotificationsDto()
    {
        const int expectedNotificationType = 1;
        const string expectedDepositName = "Pepes";
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(5);
        const string expectedClientEmail = "seba@gmail.com";
        _notificationsDto.ClientEmail = expectedClientEmail;
        _notificationsDto.NotificationType = expectedNotificationType;
        _notificationsDto.DepositName = expectedDepositName;
        _notificationsDto.StartDate = expectedStartDate;
        _notificationsDto.EndDate = expectedEndDate;
        Assert.AreEqual(expectedClientEmail, _notificationsDto.ClientEmail);
        Assert.AreEqual(expectedNotificationType, _notificationsDto.NotificationType);
        Assert.AreEqual(expectedDepositName, _notificationsDto.DepositName);
        Assert.AreEqual(expectedStartDate, _notificationsDto.StartDate);
        Assert.AreEqual(expectedEndDate, _notificationsDto.EndDate);
    }
    
    [TestMethod]
    public void ShouldCreateNotificationsDtoWithParameters()
    {
        const int expectedNotificationType = 1;
        const string expectedDepositName = "Pepes";
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(5);
        const string expectedClientEmail = "seba@gmail.com";
        var notificationsDto = new NotificationsDto(expectedClientEmail, expectedNotificationType, expectedDepositName, expectedStartDate, expectedEndDate);
        Assert.AreEqual(expectedClientEmail, notificationsDto.ClientEmail);
        Assert.AreEqual(expectedNotificationType, notificationsDto.NotificationType);
        Assert.AreEqual(expectedDepositName, notificationsDto.DepositName);
        Assert.AreEqual(expectedStartDate, notificationsDto.StartDate);
        Assert.AreEqual(expectedEndDate, notificationsDto.EndDate);
    }
}