using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class DepositDisponibilityDtoTest
{
    [TestMethod]
    public void ShouldCreateCorrectDepositDisponibilityDto()
    {
        DepositDisponibilityDto depositDisponibilityDto = new DepositDisponibilityDto();
        Assert.IsNotNull(depositDisponibilityDto);
    }
    
    
    [TestMethod]
    public void ShouldSetAndGetCorrectDepositDisponibilityDto()
    {
        DepositDisponibilityDto depositDisponibilityDto = new DepositDisponibilityDto();
        var expectedDepositDtoName = "DEPOSITO NUEVO";
        var expectedDepositDtoArea = 'E';
        var expectedDepositDtoSize = "Grande";
        var expectedDepositDtoConditioning = true;
        
        var expectedPromotionDtoTag = "Nueva promo";
        var expectedPromotionDtoDiscountPercentage = 10;

        PromotionDisponibilityDto promotionDto = new(expectedPromotionDtoTag, expectedPromotionDtoDiscountPercentage);
        depositDisponibilityDto.PromotionDtoList.Add(promotionDto);
        
        var expectedStartDate = DateTime.Today;
        var expectedEndDate = DateTime.Today.AddDays(7);
        
        DateRangeDto dateRange = new DateRangeDto(expectedStartDate, expectedEndDate);
        depositDisponibilityDto.DateRangesList.Add(dateRange);
            
        var expectedDepositHasPromotions = true;
        var expectedAreDisponibilityRangeRegistered = true;

        depositDisponibilityDto.Name = expectedDepositDtoName;
        depositDisponibilityDto.Area = expectedDepositDtoArea;
        depositDisponibilityDto.Size = expectedDepositDtoSize;
        depositDisponibilityDto.Conditioning = expectedDepositDtoConditioning;
        depositDisponibilityDto.HasPromotions = expectedDepositHasPromotions;
        depositDisponibilityDto.AreDisponibilityRangesRegistered = expectedAreDisponibilityRangeRegistered;
        
        
        Assert.AreEqual(depositDisponibilityDto.Name,expectedDepositDtoName);
        Assert.AreEqual(depositDisponibilityDto.Area ,expectedDepositDtoArea);
        Assert.AreEqual(depositDisponibilityDto.Size,expectedDepositDtoSize);
        Assert.AreEqual(depositDisponibilityDto.Conditioning,expectedDepositDtoConditioning);
        Assert.AreEqual(depositDisponibilityDto.HasPromotions,expectedDepositHasPromotions);
        Assert.AreEqual(depositDisponibilityDto.AreDisponibilityRangesRegistered,expectedAreDisponibilityRangeRegistered);
        Assert.IsTrue(depositDisponibilityDto.PromotionDtoList.Count == 1);
        Assert.IsTrue(depositDisponibilityDto.DateRangesList.Count == 1);
    }
    
    [TestMethod]
    public void ShouldCreateCorrectDepositDisponibilityDtoWithParameters()
    {   
        var expectedDepositDtoName = "DEPOSITO NUEVO";
        var expectedDepositDtoArea = 'E';
        var expectedDepositDtoSize = "Grande";
        var expectedDepositDtoConditioning = true;
        
        var expectedDepositHasPromotions = true;
        var expectedAreDisponibilityRangeRegistered = true;
        
        DepositDisponibilityDto depositDisponibilityDto = new DepositDisponibilityDto(expectedDepositDtoName,
                                                                                        expectedDepositDtoArea,
                                                                                        expectedDepositDtoSize, 
                                                                                        expectedDepositDtoConditioning, 
                                                                                        expectedDepositHasPromotions, 
                                                                                        expectedAreDisponibilityRangeRegistered);
        Assert.IsNotNull(depositDisponibilityDto);
        Assert.AreEqual(depositDisponibilityDto.Name,expectedDepositDtoName);
        Assert.AreEqual(depositDisponibilityDto.Area ,expectedDepositDtoArea);
        Assert.AreEqual(depositDisponibilityDto.Size,expectedDepositDtoSize);
        Assert.AreEqual(depositDisponibilityDto.Conditioning,expectedDepositDtoConditioning);
        Assert.AreEqual(depositDisponibilityDto.HasPromotions,expectedDepositHasPromotions);
        Assert.AreEqual(depositDisponibilityDto.AreDisponibilityRangesRegistered,expectedAreDisponibilityRangeRegistered);
    }
}