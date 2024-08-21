using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class PromotionManagementDtoTest
{
    private PromotionManagementDto _promotionManagementDto;

    [TestInitialize]
    public void Setup()
    {
        _promotionManagementDto = new PromotionManagementDto();
    }

    [TestMethod]
    public void ShouldCreatePromotionManagementDto()
    {
        Assert.IsNotNull(_promotionManagementDto);
    }

    [TestMethod]
    public void ShouldSetPromotionManagementDto()
    {
        _promotionManagementDto.Id = 1;
        _promotionManagementDto.Tag = "tag";
        _promotionManagementDto.StartDate = DateTime.Today.AddDays(1);
        _promotionManagementDto.EndDate = DateTime.Today.AddDays(5);
        _promotionManagementDto.DiscountPercentage = 50;
        Assert.AreEqual(1, _promotionManagementDto.Id);
        Assert.AreEqual("tag", _promotionManagementDto.Tag);
        Assert.AreEqual(DateTime.Today.AddDays(1), _promotionManagementDto.StartDate);
        Assert.AreEqual(DateTime.Today.AddDays(5), _promotionManagementDto.EndDate);
        Assert.AreEqual(50, _promotionManagementDto.DiscountPercentage);
    }

    [TestMethod]
    public void ShouldCreatePromotionManagementDtoWithParameters()
    {
        const int expectedPromotionId = 1;
        const string expectedPromotionTag = "tag";
        var startDate = DateTime.Today.AddDays(1);
        var endDate = DateTime.Today.AddDays(5);
        const int expectedDiscountPercentage = 50;

        var promotionManagementDto =
            new PromotionManagementDto(expectedPromotionId, expectedPromotionTag, startDate, endDate,
                expectedDiscountPercentage);
        Assert.AreEqual(expectedPromotionId, promotionManagementDto.Id);
        Assert.AreEqual(expectedPromotionTag, promotionManagementDto.Tag);
        Assert.AreEqual(startDate, promotionManagementDto.StartDate);
        Assert.AreEqual(endDate, promotionManagementDto.EndDate);
        Assert.AreEqual(expectedDiscountPercentage, promotionManagementDto.DiscountPercentage);
    }
}