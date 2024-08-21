using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.Domain;

[TestClass]
public class DateTimeProvider_Test
{
    [TestMethod]
    public void ShouldReturnCurrentDateTime()
    {
        var currentDateTime = DateTimeProvider.GetCurrentDateTime();
        var expectedDateTime = DateTime.Today;
        Assert.AreEqual(expectedDateTime, currentDateTime);
    }

    [TestMethod]
    public void ShouldSetCurrentDateTime()
    {
        DateTimeProvider.SetCurrentDateTime(DateTime.Today.AddDays(1));
        var currentDateTime = DateTimeProvider.GetCurrentDateTime();
        var expectedDateTime = DateTime.Today.AddDays(1);
        Assert.AreEqual(expectedDateTime, currentDateTime);
    }
}