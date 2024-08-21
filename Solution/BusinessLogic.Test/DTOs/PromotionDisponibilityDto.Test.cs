using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class PromotionDisponibilityDtoTest
{
    [TestMethod]
    public void ShouldCreatePromotionDisponibilityDto()
    {
        PromotionDisponibilityDto promotionDisponibilityDto = new PromotionDisponibilityDto();
        Assert.IsNotNull(promotionDisponibilityDto);
    }


    [TestMethod]
    public void ShouldSetAndGetCorrectPromotionDisponibilityDto()
    {
        PromotionDisponibilityDto promotionDisponibilityDto = new PromotionDisponibilityDto();
        var expectedPromotionDtoTag = "Nueva promo";
        var exectedPromotionDtoDiscountPercentage = 10;

        promotionDisponibilityDto.Tag = expectedPromotionDtoTag;
        promotionDisponibilityDto.DiscountPercentage = exectedPromotionDtoDiscountPercentage;
        
        Assert.AreEqual(expectedPromotionDtoTag, promotionDisponibilityDto.Tag);
        Assert.AreEqual(exectedPromotionDtoDiscountPercentage, promotionDisponibilityDto.DiscountPercentage);
    }
    
    [TestMethod]
    public void ShouldCreatePromotionDisponibilityDtoWithParameters()
    {   
        
        var expectedPromotionDtoTag = "Nueva promo";
        var exectedPromotionDtoDiscountPercentage = 10;
        
        PromotionDisponibilityDto promotionDisponibilityDto = new PromotionDisponibilityDto(expectedPromotionDtoTag, exectedPromotionDtoDiscountPercentage);
       
        Assert.AreEqual(expectedPromotionDtoTag, promotionDisponibilityDto.Tag);
        Assert.AreEqual(exectedPromotionDtoDiscountPercentage, promotionDisponibilityDto.DiscountPercentage);
    }
}