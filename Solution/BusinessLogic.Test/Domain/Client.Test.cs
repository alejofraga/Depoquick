
using BusinessLogic.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.Domain;

[TestClass]
public class RegisterClient
{
    [TestMethod]
    public void ShouldCreateClient()
    {
        var client = new Client();
        Assert.IsNotNull(client);
    }
    
    [TestMethod]
    public void ShouldThrowExceptionWhenCreatingClientWithTwoPasswordsThatAreNotEqual()
    {
        const string? expectedPassword1 = "Password@1";
        const string expectedPassword2 = "Password@2";
        const string? expectedName = "Agus";
        const string? expectedEmail = "Agustin@ort.edu.uy";
        
        Assert.ThrowsException<ArgumentException>(() => new Client(expectedName,expectedEmail,expectedPassword1, expectedPassword2,false));
    }
    
    
    [TestMethod]
    public void ShouldCreateClientWithPasswordIfAreThePasswordsEqual()
    {
        const string? expectedPassword1 = "Password@1";
        const string expectedPassword2 = "Password@1";
        const string? expectedName = "Agus";
        const string? expectedEmail = "Agustin@ort.edu.uy";
        
        var client = new Client(expectedName, expectedEmail, expectedPassword1, expectedPassword2,false);
        
        Assert.AreEqual(client.Password, expectedPassword1);
    }
    
    [TestMethod]
    public void ShouldSetCorrectName()
    {   
        var client = new Client();
        const string? expectedName = "Pedro";
        client.Name = expectedName;
        Assert.AreEqual(expectedName, client.Name);
    }

    [TestMethod]

    public void ShouldThrowExceptionIfSettedNameLengthIsGreaterThan100()
    {
        var client = new Client();
        const string? nameWithLengthGreaterThan100 =
            "AgustinAgustinAgustinAgustinAgustinAgustinAgustinAgustinAgustinAgustinAgustinAgustinAgustinAgustinAgustinAgustin";

        Assert.ThrowsException<ArgumentException>(() => client.Name = nameWithLengthGreaterThan100);
    }

    [TestMethod]
    public void ShouldSetCorrectEmail()
    {
        var client = new Client();
        const string? expectedEmail = "alejo@ort.edu.uy";
        client.Email = expectedEmail;
        Assert.AreEqual(expectedEmail, client.Email);
    }

    
    [TestMethod]
    public void ShouldThrowExceptionForInvalidEmail()
    {
        var client = new Client();
        Assert.ThrowsException<ArgumentException>(() => client.Email = "alejo");
    }
    
    
    [TestMethod]
    public void ShouldSetPassword()
    {
        var client = new Client();
        const string? expectedPassword = "P@ssw0rd";
        client.Password = expectedPassword;
        Assert.AreEqual(expectedPassword, client.Password);
    }
    
    [TestMethod]
    public void NameIsValidIfIsUpTo100Characters()
    {
        var client = new Client();
        var expectedName = new string('a', 100);
        Assert.IsTrue(client.NameIsUpTo100Characters(expectedName));
    }
    
    [TestMethod]
    public void NameIsInvalidIfIsMoreThan100Characters()
    {
        var client = new Client();
        var expectedName = new string('a', 101);
        Assert.IsFalse(client.NameIsUpTo100Characters(expectedName));
    }
    
    [TestMethod]
    public void PasswordIsValidIfIsMoreThan8Characters()
    {
        var client = new Client();
        var expectedPassword = new string('a', 9);
        Assert.IsTrue(client.PasswordIsMoreThan8Characters(expectedPassword));
    }
    
    [TestMethod]
    public void PasswordIsInvalidIfIsLessThan8Characters()
    {
        var client = new Client();
        var expectedPassword = new string('a', 7);
        Assert.IsFalse(client.PasswordIsMoreThan8Characters(expectedPassword));
    }
    
    [TestMethod]
    public void PasswordIsValidIfHaveAtLeastOneSymbol()
    {
        var client = new Client();
        var expectedPassword = "Pa$sword";
        Assert.IsTrue(client.PasswordHaveAtLeastOneSymbol(expectedPassword));
    }
    
    [TestMethod]
    public void PasswordIsInvalidIfDoesNotHaveAtLeastOneSymbol()
    {
        var client = new Client();
        var expectedPassword = "Password";
        Assert.IsFalse(client.PasswordHaveAtLeastOneSymbol(expectedPassword));
    }
    
    [TestMethod]
    public void PasswordIsValidIfHaveAtLeastOneLowercaseLetter()
    {
        var client = new Client();
        var expectedPassword = "Password";
        Assert.IsTrue(client.PasswordHaveAtLeastOneLowercaseLetter(expectedPassword));
    }
    
    [TestMethod]
    public void PasswordIsInvalidIfDoesNotHaveAtLeastOneLowercaseLetter()
    {
        var client = new Client();
        var expectedPassword = "PASSWORD";
        Assert.IsFalse(client.PasswordHaveAtLeastOneLowercaseLetter(expectedPassword));
    }
    
    [TestMethod]    
    public void PasswordIsValidIfHaveAtLeastOneUppercaseLetter()
    {
        var client = new Client();
        var expectedPassword = "Password";
        Assert.IsTrue(client.PasswordHaveAtLeastOneUppercaseLetter(expectedPassword));
    }
    
    [TestMethod]
    public void PasswordIsInvalidIfDoesNotHaveAtLeastOneUppercaseLetter()
    {
        var client = new Client();
        const string expectedPassword = "password";
        Assert.IsFalse(client.PasswordHaveAtLeastOneUppercaseLetter(expectedPassword));
    }

    [TestMethod]
    public void PasswordIsValidIfHaveAtLeastOneNumber()
    {   
        var client = new Client();
        const string expectedPassword = "Password1";
        Assert.IsTrue(client.PasswordHaveAtLeastOneNumber(expectedPassword));
    }
    
    [TestMethod]
    public void PasswordIsInvalidIfDoesNotHaveAtLeastOneNumber()
    {
        var client = new Client();
        const string expectedPassword = "Password";
        Assert.IsFalse(client.PasswordHaveAtLeastOneNumber(expectedPassword));
    }

    [TestMethod]

    public void ShouldThrowExceptionIfPasswordInvalid()
    {
        var client = new Client();
        Assert.ThrowsException<ArgumentException>(() => client.Password = "invalidPassword");
    }

    [TestMethod]
    public void ShouldThrowExceptionIfEmailLengthIsGraterThan254()
    {
        var client = new Client();
        var expectedEmail = new string('@', 300);
        Assert.ThrowsException<ArgumentException>(() => client.Email = expectedEmail);
    }
    
    [TestMethod]
    public void ShouldThrowExceptionIfEmailFormatIsInvalid()
    {
        var client = new Client();
        const string expectedEmail = ".agustin@$ort.edu.uy:p";
        Assert.ThrowsException<ArgumentException>(() => client.Email = expectedEmail);
    }
    
    [TestMethod]
    public void ShouldGetTrueIfClientIsAdmin()
    {
        var client = new Client();
        client.IsAdmin = true;
        Assert.IsTrue(client.IsAdmin);
    }
    [TestMethod]
    public void EqualFunctionShouldReturnFalseIfOneParameterIsNull()
    {
        var client = new Client();
        Assert.IsFalse(client.Equals(null));
    }
    
}