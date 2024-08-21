using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;

[TestClass]
public class DepositCreateReservationDTOTest
{
    private DepositCreateReservationDto _depositCreateReservationDto;

    [TestInitialize]
    public void Setup()
    {
        _depositCreateReservationDto = new DepositCreateReservationDto();
    }

    [TestMethod]
    public void ShouldCreateDepositCreateReservationDto()
    {
        Assert.IsNotNull(_depositCreateReservationDto);
    }
    
    [TestMethod]
    public void ShouldSetCorrectDepositCreateReservationDto()
    {
        const int expectedDepositId = 1;
        const char expectedDepositArea = 'A';
        const string expectedDepositSize = "Grande";
        const bool expectedDepositConditionig = false;

        const string expectedPromotionTag = "promo brava";
        const int expectedPromotionDiscountPercentage = 5;
        var expectedPromotionIn =
            new PromotionInDepositCreateReservationDto(expectedPromotionTag, expectedPromotionDiscountPercentage);

        var expectedPromotionList = new List<PromotionInDepositCreateReservationDto>();
        expectedPromotionList.Add(expectedPromotionIn);

        const string expectedReviewComment = "Eu gostei muito do deposito! Muito obrigado!";
        const int expectedReviewValoration = 5;
        var expectedReviewInDeposit =
            new ReviewInDepositCreateReservationDto(expectedReviewValoration, expectedReviewComment);

        var expectedReviewList = new List<ReviewInDepositCreateReservationDto>();
        expectedReviewList.Add(expectedReviewInDeposit);

        const bool expectedContainsAnyReviews = true;
        const bool expectedHasPromotions = true;

        _depositCreateReservationDto.Id = expectedDepositId;
        _depositCreateReservationDto.Area = expectedDepositArea;
        _depositCreateReservationDto.Size = expectedDepositSize;
        _depositCreateReservationDto.Conditionig = expectedDepositConditionig;
        _depositCreateReservationDto.PromotionDtoList = expectedPromotionList;
        _depositCreateReservationDto.ReviewDtoList = expectedReviewList;
        _depositCreateReservationDto.ContainsAnyReviews = expectedContainsAnyReviews;
        _depositCreateReservationDto.HasPromotions = expectedHasPromotions;

        Assert.AreEqual(expectedDepositId, _depositCreateReservationDto.Id);
        Assert.AreEqual(expectedDepositArea, _depositCreateReservationDto.Area);
        Assert.AreEqual(expectedDepositSize, _depositCreateReservationDto.Size);
        Assert.AreEqual(expectedDepositConditionig, _depositCreateReservationDto.Conditionig);
        Assert.AreEqual(expectedPromotionList.Count, _depositCreateReservationDto.PromotionDtoList.Count);
        Assert.AreEqual(expectedReviewList.Count, _depositCreateReservationDto.ReviewDtoList.Count);
        Assert.AreEqual(expectedContainsAnyReviews, _depositCreateReservationDto.ContainsAnyReviews);
        Assert.AreEqual(expectedHasPromotions, _depositCreateReservationDto.HasPromotions);
    }

    [TestMethod]
    public void ShouldSetCorrectDepositCreateReservationDtoWithParameters()
    {
        const int expectedDepositId = 1;
        const char expectedDepositArea = 'A';
        const string expectedDepositSize = "Grande";
        const bool expectedDepositConditionig = false;

        const string expectedPromotionTag = "promo brava";
        const int expectedPromotionDiscountPercentage = 5;
        var expectedPromotionIn =
            new PromotionInDepositCreateReservationDto(expectedPromotionTag, expectedPromotionDiscountPercentage);

        var expectedPromotionList = new List<PromotionInDepositCreateReservationDto>();
        expectedPromotionList.Add(expectedPromotionIn);

        const string expectedReviewComment = "Eu gostei muito do deposito! Muito obrigado!";
        const int expectedReviewValoration = 5;
        var expectedReviewInDeposit =
            new ReviewInDepositCreateReservationDto(expectedReviewValoration, expectedReviewComment);

        var expectedReviewList = new List<ReviewInDepositCreateReservationDto>();
        expectedReviewList.Add(expectedReviewInDeposit);

        const bool expectedContainsAnyReviews = true;
        const bool expectedHasPromotions = true;
        const string expectedDepositName = "Deposito";

        var depositCreateReservationDto = new DepositCreateReservationDto(expectedDepositId,
            expectedDepositArea,
            expectedDepositSize,
            expectedDepositConditionig,
            expectedPromotionList,
            expectedReviewList,
            expectedContainsAnyReviews,
            expectedHasPromotions);
        depositCreateReservationDto.Name = expectedDepositName;


        Assert.AreEqual(expectedDepositId, depositCreateReservationDto.Id);
        Assert.AreEqual(expectedDepositArea, depositCreateReservationDto.Area);
        Assert.AreEqual(expectedDepositSize, depositCreateReservationDto.Size);
        Assert.AreEqual(expectedDepositConditionig, depositCreateReservationDto.Conditionig);
        Assert.AreEqual(expectedPromotionList.Count, depositCreateReservationDto.PromotionDtoList.Count);
        Assert.AreEqual(expectedReviewList.Count, depositCreateReservationDto.ReviewDtoList.Count);
        Assert.AreEqual(expectedContainsAnyReviews, depositCreateReservationDto.ContainsAnyReviews);
        Assert.AreEqual(expectedHasPromotions, depositCreateReservationDto.HasPromotions);
        Assert.AreEqual(expectedDepositName, depositCreateReservationDto.Name);
    }
}