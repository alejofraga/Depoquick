using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test;


[TestClass]
public class ReviewCreateReservationDTOTest
{
    private ReviewInDepositCreateReservationDto _reviewInDepositCreateReservationDto;
    [TestInitialize]
    public void Setup()
    {
        _reviewInDepositCreateReservationDto = new ReviewInDepositCreateReservationDto();
    }
    [TestMethod]
    public void ShouldCreateReviewCreateReservationDto()
    {
        Assert.IsNotNull(_reviewInDepositCreateReservationDto);
    }

    [TestMethod]
    public void ShouldSetCorrectReviewCreateReservationDto()
    {
        const int expectedValoration = 5;
        const string expectedComment = "Que deposito tan genial";

        _reviewInDepositCreateReservationDto.Valoration = expectedValoration;
        _reviewInDepositCreateReservationDto.Comment = expectedComment;
        Assert.AreEqual(_reviewInDepositCreateReservationDto.Valoration, expectedValoration);
        Assert.AreEqual(_reviewInDepositCreateReservationDto.Comment, expectedComment);
    }
    
    [TestMethod]
    public void ShouldSetCorrectReviewCreateReservationDtoWithParameters()
    {
        const int expectedValoration = 5;
        const string expectedComment = "Que deposito tan genial";

        var reviewInDepositCreateReservationDto = new ReviewInDepositCreateReservationDto(expectedValoration, expectedComment);
        
        Assert.AreEqual(reviewInDepositCreateReservationDto.Valoration, expectedValoration);
        Assert.AreEqual(reviewInDepositCreateReservationDto.Comment, expectedComment);
    }
}