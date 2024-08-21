using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class PromotionCreateReservationDtoTest
{
    private PromotionInDepositCreateReservationDto _promotionInDepositCreateReservationDto;
    [TestInitialize]
    public void Setup()
    {
        _promotionInDepositCreateReservationDto = new PromotionInDepositCreateReservationDto();
    }
    [TestMethod]
    public void ShouldCreatePromotionCreateReservationDto()
    {
        Assert.IsNotNull(_promotionInDepositCreateReservationDto); 
    }

    [TestMethod]
    public void ShouldSetCorrectPromotionCreateReservationDto()
    {
        const string expectedPromotionTag = "super promo";
        const int expectedPromotionDiscountPercentage = 60;
        _promotionInDepositCreateReservationDto.Tag = expectedPromotionTag;
        _promotionInDepositCreateReservationDto.DiscountPercentage = expectedPromotionDiscountPercentage;
        
        Assert.AreEqual(expectedPromotionTag, _promotionInDepositCreateReservationDto.Tag);
        Assert.AreEqual(expectedPromotionDiscountPercentage, _promotionInDepositCreateReservationDto.DiscountPercentage);
    }
    
    [TestMethod]
    public void ShouldSetCorrectPromotionCreateReservationDtoWithParameters()
    {
        const string expectedPromotionTag = "super promo";
        const int expectedPromotionDiscountPercentage = 60;
       
        var promotionInCreateReservationDto = new PromotionInDepositCreateReservationDto(expectedPromotionTag, expectedPromotionDiscountPercentage);
        
        Assert.AreEqual(expectedPromotionTag, promotionInCreateReservationDto.Tag);
        Assert.AreEqual(expectedPromotionDiscountPercentage, promotionInCreateReservationDto.DiscountPercentage);
    }
    
}