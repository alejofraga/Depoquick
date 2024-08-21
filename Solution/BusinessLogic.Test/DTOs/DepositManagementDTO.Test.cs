using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class DepositInDepositManagementDTO_Test
{
    private DepositManagementDto _depositManagementDto;

    [TestInitialize]
    public void Setup()
    {
        _depositManagementDto = new DepositManagementDto();
    }

    [TestMethod]
    public void ShouldCreateDepositManagementDto()
    {
        Assert.IsNotNull(_depositManagementDto);
    }

    [TestMethod]
    public void ShouldSetDepositManagementDto()
    {
        const int expectedDepositId = 1;
        const char expectedDepositArea = 'A';
        const string expectedDepositSize = "Mediano";
        const string expectedDepositName = "DEPOSITO";
        const bool expectedDepositConditionig = true;
        var expectedPromotionList = new List<PromotionInDepositManagementDto?>();
        _depositManagementDto.Id = expectedDepositId;
        _depositManagementDto.Area = expectedDepositArea;
        _depositManagementDto.Size = expectedDepositSize;
        _depositManagementDto.Conditioning = expectedDepositConditionig;
        _depositManagementDto.Name = expectedDepositName;
        _depositManagementDto.Promotions = expectedPromotionList;
        Assert.AreEqual(expectedDepositId, _depositManagementDto.Id);
        Assert.AreEqual(expectedDepositArea, _depositManagementDto.Area);
        Assert.AreEqual(expectedDepositSize, _depositManagementDto.Size);
        Assert.AreEqual(expectedDepositConditionig, _depositManagementDto.Conditioning);
        Assert.AreEqual(expectedDepositName, _depositManagementDto.Name);
        Assert.IsNotNull(_depositManagementDto.Promotions);
    }

    [TestMethod]
    public void ShouldCreateDepositManagementDtoWithParameters()
    {
        const int expectedDepositId = 1;
        const char expectedDepositArea = 'A';
        const string expectedDepositSize = "Mediano";
        const bool expectedDepositConditionig = true;
        var expectedPromotionList = new List<PromotionInDepositManagementDto?>();
        var depositManagementDto =
            new DepositManagementDto(expectedDepositId, expectedDepositArea, expectedDepositSize,
                expectedDepositConditionig, expectedPromotionList);
        Assert.AreEqual(expectedDepositId, depositManagementDto.Id);
        Assert.AreEqual(expectedDepositArea, depositManagementDto.Area);
        Assert.AreEqual(expectedDepositSize, depositManagementDto.Size);
        Assert.AreEqual(expectedDepositConditionig, depositManagementDto.Conditioning);
        Assert.IsNotNull(depositManagementDto.Promotions);
    }
}