using BusinessLogic.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.Domain;

[TestClass]
public class CreateDeposit
{
    [TestMethod]
    public void ShouldCreateDeposit()
    {
        var deposit = new Deposit();
        Assert.IsNotNull(deposit);
    }

    [TestMethod]
    public void ShouldSetCorrectArea()
    {
        var deposit = new Deposit();
        const char expectedArea = 'A';
        deposit.Area = expectedArea;
        Assert.AreEqual(deposit.Area, expectedArea);
    }

    [TestMethod]
    public void ShouldThrowExceptionIfInvalidArea()
    {
        var deposit = new Deposit();
        const char expectedArea = 'X';
        Assert.ThrowsException<ArgumentException>(() => deposit.Area = expectedArea);
    }


    [TestMethod]
    public void ShouldSetCorrectSize()
    {
        var deposit = new Deposit();
        const string expectedSize = "Pequeño";
        deposit.Size = expectedSize;
        Assert.AreEqual(expectedSize, deposit.Size);
    }

    [TestMethod]
    public void ShouldThrowExceptionIfInvalidSize()
    {
        var deposit = new Deposit();
        const string expectedSize = "VeryVeryBig";
        Assert.ThrowsException<ArgumentException>(() => deposit.Size = expectedSize);
    }


    [TestMethod]
    public void ShouldSetCorrectConditioning()
    {
        var deposit = new Deposit();
        const bool expectedConditioning = false;
        deposit.Conditioning = expectedConditioning;
        Assert.AreEqual(expectedConditioning, deposit.Conditioning);
    }


    [TestMethod]
    public void ShouldCreateDepositWithCorrectAreaAndCorrectSizeAndCorrectConditioningAndId()
    {
        const char expectedArea = 'A';
        const string? expectedSize = "Pequeño";
        const bool expectedConditioning = false;
        const int expectedId = 1;
        const string depositName = "Deposito";
        var deposit = new Deposit
        {
            Area = expectedArea,
            Size = expectedSize,
            Conditioning = expectedConditioning,
            Id = expectedId,
            Name = depositName
        };
        Assert.AreEqual(deposit.Area, expectedArea);
        Assert.AreEqual(deposit.Size, expectedSize);
        Assert.AreEqual(deposit.Conditioning, expectedConditioning);
        Assert.AreEqual(deposit.Id, expectedId);
    }

    [TestMethod]
    public void ShouldSetCorrectId()
    {
        var deposit = new Deposit();
        const int expectedId = 1;
        deposit.Id = 1;
        Assert.AreEqual(expectedId, deposit.Id);
    }
    
    [TestMethod]
    public void EqualFunctionShouldReturnFalseIfOneParameterIsNull()
    {
        var deposit = new Deposit();
        Assert.IsFalse(deposit.Equals(null));
    }

    [TestMethod]
    public void ShouldSetDepositName()
    {
        var deposit = new Deposit();
        const string expectedName = "GreatDeposit";
        deposit.Name = expectedName;
    }

    [TestMethod]
    public void ShouldThrowExceptionIfDepositNameIsInvalid()
    {
        var deposit = new Deposit();
        const string invalidName = "Deposit1";
        Assert.ThrowsException<ArgumentException>(() => deposit.Name = invalidName);
    }

    [TestMethod]

    public void ShouldGetDepositNameInUpperCase()
    {
        var deposit = new Deposit();
        const string expectedName = "GreatDeposit";
        deposit.Name = expectedName;
        Assert.AreEqual(expectedName.ToUpper(), deposit.Name);
    }
}