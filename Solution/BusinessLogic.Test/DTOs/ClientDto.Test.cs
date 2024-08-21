using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class ClientDtoTest
{
    private ClientDto _clientDto;

    [TestInitialize]
    public void Setup()
    {
        _clientDto = new ClientDto();
    }

    [TestMethod]
    public void ShouldCreateClientDto()
    {
        Assert.IsNotNull(_clientDto);
    }

    [TestMethod]
    public void ShouldSetClientDto()
    {
        const string expectedName = "Pepe el crack";
        const string expectedMail = "Pepe@gmail.com";
        const string expectedPassword = "#theBestAdmin1234";
        const bool expectedIsAdmin = true;

        var clientDto = new ClientDto(expectedName, expectedMail, expectedPassword, expectedIsAdmin);

        Assert.AreEqual(expectedName, clientDto.Name);
        Assert.AreEqual(expectedMail, clientDto.Email);
        Assert.AreEqual(expectedPassword, clientDto.Password);
        Assert.AreEqual(expectedIsAdmin, clientDto.IsAdmin);
    }
}