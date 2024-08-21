using BusinessLogic.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BusinessLogic.Test.Domain;


[TestClass]
public class CreateReview
{
    [TestMethod]
    public void ShouldCreateReview()
    {
        var review = new Review();
        Assert.IsNotNull(review);
    }

    [TestMethod]
    public void ShouldSetCorrectValoration()
    {
        var review = new Review();
        const int expectedValoration = 5;
        review.Valoration = expectedValoration;
        Assert.AreEqual(review.Valoration, expectedValoration);
    }
    
    [TestMethod]
    public void ShouldThrowExceptionIfValorationGreaterThanFive()
    {
        var review = new Review();
        const int expectedValoration = 6;
        Assert.ThrowsException<ArgumentException>( () => review.Valoration = expectedValoration);
    }

    [TestMethod]
    public void ShouldSetCorrectComment()
    {
        var review = new Review();
        const string expectedComment = "Hi! That Deposit is wonderful Surely I'll recomend to my friends! :p";
        review.Comment = expectedComment;
        Assert.AreEqual(review.Comment, expectedComment);
    }

    [TestMethod]
    public void ShouldThrowExceptionIfCommentLengthIsGreaterThan500()
    {
        var review = new Review();
        const string commentWith505Characteres = 
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore " +
            "et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip " +
            "ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu " + 
            "fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit " +
            "anim id est laborum. Aditional characteres added to complet the five hundred!!!!";
        Assert.ThrowsException<ArgumentException>(() => review.Comment = commentWith505Characteres);
    }

    [TestMethod]

    public void ShouldSetCorrectId()
    {
        var review = new Review();
        const int expectedId = 1;
        review.Id = expectedId;
        Assert.AreEqual(review.Id, expectedId);
    }

    [TestMethod]
    public void ShouldThrowExceptionIfValorationIsLessThanOne()
    {
        var review = new Review();
        const int expectedValoration = 0;
        Assert.ThrowsException<ArgumentException>(() => review.Valoration = expectedValoration);
    }

    [TestMethod]
    public void ShouldHaveAConstructorWithParameters()
    {
        const int expectedValoration = 5;
        const string? expectedComment = "Hi! That Deposit is wonderful Surely I'll recommend to my friends! :p";
        var review = new Review(expectedValoration, expectedComment);
        Assert.AreEqual(review.Valoration, expectedValoration);
        Assert.AreEqual(review.Comment, expectedComment);
    }
    
    [TestMethod]
    
    public void EqualFunctionShouldReturnFalseIfOneParameterIsNullOrDifferentType()
    {
        var review = new Review();
        Assert.IsFalse(review.Equals(null));
    }
    
}