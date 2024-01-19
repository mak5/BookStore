using FluentAssertions;

namespace BookStore.Tests.Unit
{
    public class BookTests
    {
        [Fact]
        public void Constructor_Should_SetProperties_When_Parameters_Are_Valid()
        {
            // Arrange
            var title = "The Lord Of The Rings";
            var price = 19.99;

            // Act
            var book = new Book(title, price);

            // Assert
            book.Title.Should().Be(title);
            book.Price.Should().Be(price);
            book.Quantity.Should().Be(0);

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_Should_ThrowArgumentException_When_Title_IsInvalid(string title)
        {
            // Arrange
            var price = 19.99;

            // Act
            var action = new Action(() => new Book(title, price));

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Constructor_Should_ThrowArgumentOutOfRangeException_When_Price_IsInvalid(double price)
        {
            // Arrange
            var title = "The Lord Of The Rings";

            // Act
            var action = new Action(() => new Book(title, price));

            // Assert
            Assert.ThrowsAny<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        public void AddCopies_Should_IncreaseQuantity_When_Quantity_IsValid()
        {
            // Arrange
            var title = "The Lord Of The Rings";
            var price = 19.99;
            var book = new Book(title, price);
            var quantity = 10;

            // Act
            book.AddCopies(quantity);

            // Assert
            Assert.Equal(quantity, book.Quantity);
        }

        [Fact]
        public void RemoveCopies_Should_DecreaseQuantity_When_Quantity_IsValid()
        {
            // Arrange
            var title = "The Lord Of The Rings";
            var price = 19.99;
            var book = new Book(title, price);
            var quantity = 10;
            book.AddCopies(quantity);

            // Act
            book.RemoveCopies(quantity);

            // Assert
            Assert.Equal(0, book.Quantity);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void AddCopies_Should_ThrowArgumentOutOfRangeException_When_Quantity_IsInvalid(int quantity)
        {
            // Arrange
            var title = "The Lord Of The Rings";
            var price = 19.99;
            var book = new Book(title, price);

            // Act
            var action = new Action(() => book.AddCopies(quantity));

            // Assert
            Assert.ThrowsAny<ArgumentOutOfRangeException>(action);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void RemoveCopies_Should_ThrowArgumentOutOfRangeException_When_Quantity_IsInvalid(int quantity)
        {
            // Arrange
            var title = "The Lord Of The Rings";
            var price = 19.99;
            var book = new Book(title, price);

            // Act
            var action = new Action(() => book.RemoveCopies(quantity));

            // Assert
            Assert.ThrowsAny<ArgumentOutOfRangeException>(action);
        }          
    }
}