using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class LogDtoTest
{
    [TestMethod]
    public void ShouldCreateLogDto()
    {
        LogDto newLogDto = new LogDto();
        Assert.IsNotNull(newLogDto);
    }
    
    [TestMethod]
    public void ShouldSetCorrectLogDto()
    {
        LogDto newLogDto = new LogDto();
        List<string> expectedActionsList = new List<string>();
        const string expectedDescription = "Ingreso del cliente al sistema";
        newLogDto.ActionDescriptions = expectedActionsList;
        newLogDto.ActionDescriptions.Add(expectedDescription);
        
        Assert.AreEqual(newLogDto.ActionDescriptions[0], expectedDescription);
    }
    
    [TestMethod]
    public void ShouldCreateCorrectLogDtoWithParameters()
    {   
        List<string> expectedActionsList = new List<string>();
        const string expectedDescription = "Ingreso del cliente al sistema";
        expectedActionsList.Add(expectedDescription);
        
        LogDto newLogDto = new LogDto(expectedActionsList);
        
        Assert.AreEqual(newLogDto.ActionDescriptions[0], expectedDescription);
    }
}