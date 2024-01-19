using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Tests.Unit
{
    public class CartItemTests
    {
        [Fact]
        public void Constructor_Should_SetProperties_When_Parameters_Are_Valid()
        {
            // Arrange
            var book = new Book("The Lord Of The Rings", 19.99);
            var quantity = 10;

            // Act
            var cartItem = new CartItem(book, quantity);

            // Assert
            Assert.Equal(book, cartItem.Book);
            Assert.Equal(quantity, cartItem.Quantity);
        }

        [Fact]
        public void Constructor_Should_ThrowArgumentNullException_When_Book_IsNull()
        {
            // Arrange
            Book book = null;
            var quantity = 10;

            // Act
            var action = new Action(() => new CartItem(book, quantity));

            // Assert
            Assert.ThrowsAny<ArgumentNullException>(action);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Constructor_Should_ThrowArgumentOutOfRangeException_When_Quantity_IsInvalid(int quantity)
        {
            // Arrange
            var book = new Book("The Lord Of The Rings", 19.99);

            // Act
            var action = new Action(() => new CartItem(book, quantity));

            // Assert
            Assert.ThrowsAny<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        public void IncreaseQuantity_Should_IncreaseCartItemQuantity_When_Quantity_IsPositive()
        {
            // Arrange
            var book = new Book("The Lord Of The Rings", 19.99);
            var quantity = 10;
            var cartItem = new CartItem(book, quantity);
            var quantityToAdd = 5;

            // Act
            cartItem.IncreaseQuantity(quantityToAdd);

            // Assert
            Assert.Equal(quantity + quantityToAdd, cartItem.Quantity);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void IncreaseQuantity_Should_ThrowArgumentOutOfRangeException_When_Quantity_IsInvalid(int quantity)
        {
            // Arrange
            var book = new Book("The Lord Of The Rings", 19.99);
            var cartItem = new CartItem(book, 10);

            // Act
            var action = new Action(() => cartItem.IncreaseQuantity(quantity));

            // Assert
            Assert.ThrowsAny<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        public void DecreaseQuantity_Should_DecreaseCartItemQuantity_When_Quantity_IsPositive()
        {
            // Arrange
            var book = new Book("The Lord Of The Rings", 19.99);
            var quantity = 10;
            var cartItem = new CartItem(book, quantity);
            var quantityToRemove = 5;

            // Act
            cartItem.DecreaseQuantity(quantityToRemove);

            // Assert
            Assert.Equal(quantity - quantityToRemove, cartItem.Quantity);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void DecreaseQuantity_Should_ThrowArgumentOutOfRangeException_When_Quantity_IsInvalid(int quantity)
        {
            // Arrange
            var book = new Book("The Lord Of The Rings", 19.99);
            var cartItem = new CartItem(book, 10);

            // Act
            var action = new Action(() => cartItem.DecreaseQuantity(quantity));

            // Assert
            Assert.ThrowsAny<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        public void Subtotal_Should_ReturnCorrectValue()
        {
            // Arrange
            var book = new Book("The Lord Of The Rings", 19.99);
            var quantity = 10;
            var cartItem = new CartItem(book, quantity);

            // Act
            var subtotal = cartItem.Subtotal;

            // Assert
            Assert.Equal(book.Price * quantity, subtotal);
        }

    }
}
