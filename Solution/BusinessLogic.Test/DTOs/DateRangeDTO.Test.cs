using BusinessLogic.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class DateRangeTest
{
    [TestMethod]
    public void ShouldCreateEmptyDateRange()
    {
        DateRange dateRange = new DateRange();
        Assert.IsNotNull(dateRange);
    }
    
    [TestMethod]
    public void ShouldSetAndGetCorrectStartDate()
    {
        DateRange dateRange = new DateRange();
        var expectedStartDate = DateTime.Today;
        dateRange.StartDate = expectedStartDate;
        Assert.AreEqual(expectedStartDate, dateRange.StartDate);
    }
    
    [TestMethod]
    public void ShouldSetAndGetCorrectEndDate()
    {
        DateRange dateRange = new DateRange();
        var expectedEndDate = DateTime.Today.AddDays(7);
        dateRange.EndDate = expectedEndDate;
        Assert.AreEqual(expectedEndDate, dateRange.EndDate);
    }
    
    [TestMethod]
    public void ShouldCreateDateRangeWithParameters()
    {
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(7);
        DateRange dateRange = new DateRange(expectedStartDate, expectedEndDate);
        
        Assert.IsNotNull(dateRange);
        Assert.AreEqual(expectedStartDate, dateRange.StartDate);
        Assert.AreEqual(expectedEndDate, dateRange.EndDate);
    }

    [TestMethod]
    public void ShouldSetDateRangeDto()
    {
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(7);
        var dateRangeDto = new DateRangeDto();
        dateRangeDto.StartDate = expectedStartDate;
        dateRangeDto.EndDate = expectedEndDate;
        Assert.AreEqual(expectedStartDate, dateRangeDto.StartDate);
        Assert.AreEqual(expectedEndDate, dateRangeDto.EndDate);
    }

}