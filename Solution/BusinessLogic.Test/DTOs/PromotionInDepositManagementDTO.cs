using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class PromotionIdAndTagDtoTest
{
    private PromotionInDepositManagementDto _promotionInDepositManagementDto;
    
    [TestInitialize]
    public void Setup()
    {
        _promotionInDepositManagementDto = new PromotionInDepositManagementDto();
    }
    [TestMethod]
    public void ShouldCreatePromotionInDepositManagementDto()
    {
        Assert.IsNotNull(_promotionInDepositManagementDto); 
    }
    
    [TestMethod]
    public void ShouldSetPromotionInDepositManagementDto()
    {
        _promotionInDepositManagementDto.Id = 1;
        _promotionInDepositManagementDto.Tag = "tag";
        Assert.AreEqual(1, _promotionInDepositManagementDto.Id);
        Assert.AreEqual("tag", _promotionInDepositManagementDto.Tag);
    }

    [TestMethod]
    public void ShouldCreatePromotionInDepositManagementDtoWithParameters()
    {
        var promotionInDepositManagementDto = new PromotionInDepositManagementDto(1, "tag");
        Assert.AreEqual(1, promotionInDepositManagementDto.Id);
        Assert.AreEqual("tag", promotionInDepositManagementDto.Tag);
    }
}