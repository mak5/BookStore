using FluentAssertions;
using Moq;

namespace BookStore.Tests.Unit
{
    public class CartTests
    {
        private readonly Mock<ISupplierService> _supplierServiceMock;

        public CartTests()
        {
            _supplierServiceMock = new();
            _supplierServiceMock
                .Setup(s => s.OrderCopies(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(DateOnly.FromDateTime(DateTime.Now.AddDays(2)));
        }

        [Fact]
        public void Constructor_Should_SetProperties_When_No_Parameters_Are_Provided()
        {
            // Arrange & Act
            var cart = new Cart();

            // Assert
            Assert.StartsWith("Customer_", cart.CustomerName);
            Assert.False(cart.Completed);
            Assert.Empty(cart.Items);
        }
        
        [Fact]
        public void Constructor_Should_SetProperties_When_Parameters_Are_Valid()
        {
            // Arrange
            var customerName = "John Doe";

            // Act
            var cart = new Cart(customerName);

            // Assert
            Assert.Equal(customerName, cart.CustomerName);
            Assert.False(cart.Completed);
            Assert.Empty(cart.Items);
        }

        [Fact]
        public void Constructor_Should_SetProperties_When_CustomerName_IsNull()
        {
            // Arrange
            string customerName = null;

            // Act
            var cart = new Cart(customerName);

            // Assert
            Assert.StartsWith("Customer_", cart.CustomerName);
            Assert.False(cart.Completed);
            Assert.Empty(cart.Items);
        }

        [Fact]
        public void AddItem_Should_AddNewCartItem_With_Quantity_When_NoCartItemContainsTheBook()
        {
            // Arrange
            var cart = new Cart();
            var book = new Book("The Lord Of The Rings", 19.99);
            var quantity = 10;

            // Act
            cart.AddItem(book, quantity);

            // Assert
            Assert.Single(cart.Items);
            Assert.Equal(book, cart.Items[0].Book);
            Assert.Equal(quantity, cart.Items[0].Quantity);
        }

        [Fact]
        public void AddItem_Should_IncreaseQuantity_When_CartItemContainsTheBook()
        {
            // Arrange
            var cart = new Cart();
            var book = new Book("The Lord Of The Rings", 19.99);
            var quantity = 10;
            cart.AddItem(book, quantity);

            // Act
            cart.AddItem(book, 1);

            // Assert
            Assert.Single(cart.Items);
            Assert.Equal(book, cart.Items[0].Book);
            Assert.Equal(quantity + 1, cart.Items[0].Quantity);
        }

        [Fact]
        public void AddItem_Should_ThrowInvalidOperationException_When_CartIsCompleted()
        {
            // Arrange
            var cart = new Cart();
            var book = new Book("The Lord Of The Rings", 19.99);
            var quantity = 10;
            cart.AddItem(book, quantity);
            cart.Checkout(_supplierServiceMock.Object);

            // Act
            var action = new Action(() => cart.AddItem(book, 1));

            // Assert
            Assert.ThrowsAny<InvalidOperationException>(action);
        }

        [Fact]
        public void RemoveItem_Should_DecreaseQuantity_When_CartItemContainsTheBook()
        {
            // Arrange
            var cart = new Cart();
            var book = new Book("The Lord Of The Rings", 19.99);
            var quantity = 10;
            cart.AddItem(book, quantity);

            // Act
            cart.RemoveItem(book, 1);

            // Assert
            Assert.Single(cart.Items);
            Assert.Equal(book, cart.Items[0].Book);
            Assert.Equal(quantity - 1, cart.Items[0].Quantity);
        }

        [Fact]
        public void RemoveItem_Should_RemoveCartItem_When_CartItemContainsTheBookAndQuantityIsZero()
        {
            // Arrange
            var cart = new Cart();
            var book = new Book("The Lord Of The Rings", 19.99);
            var quantity = 10;
            cart.AddItem(book, quantity);

            // Act
            cart.RemoveItem(book, quantity);

            // Assert
            Assert.Empty(cart.Items);
        }

        [Fact]
        public void RemoveItem_Should_ThrowInvalidOperationException_When_CartIsCompleted()
        {
            // Arrange
            var cart = new Cart();
            var book = new Book("The Lord Of The Rings", 19.99);
            var quantity = 10;
            cart.AddItem(book, quantity);
            cart.Checkout(_supplierServiceMock.Object);

            // Act
            var action = new Action(() => cart.RemoveItem(book, 1));

            // Assert
            Assert.ThrowsAny<InvalidOperationException>(action);
        }

        [Fact]
        public void Checkout_Should_SetCompletedToTrue_When_CartIsNotCompleted()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.Checkout(_supplierServiceMock.Object);

            // Assert
            Assert.True(cart.Completed);
        }

        [Fact]
        public void TotalPrice_Should_ReturnZero_When_CartIsEmpty()
        {
            // Arrange
            var cart = new Cart();

            // Act
            var totalPrice = cart.TotalPrice;

            // Assert
            Assert.Equal(0, totalPrice);
        }

        [Fact]
        public void TotalPrice_Should_ReturnTotalPrice_When_CartIsNotEmpty()
        {
            // Arrange
            var cart = new Cart();
            var book1 = new Book("The Lord Of The Rings", 19.99)
            {
                Id = 1
            };
            var quantity1 = 10;
            var book2 = new Book("The Hobbit", 9.99)
            {
                Id = 2
            };
            var quantity2 = 5;
            cart.AddItem(book1, quantity1);
            cart.AddItem(book2, quantity2);
            var expectedTotalPrice = book1.Price * quantity1 + book2.Price * quantity2;

            // Act
            var totalPrice = cart.TotalPrice;

            // Assert
            Assert.Equal(expectedTotalPrice, totalPrice);
        }

        [Fact]
        public void Checkout_Should_CallOrderCopies_When_QuantityIsGreaterThanBookQuantity()
        {
            // Arrange
            var cart = new Cart();
            var book = new Book("The Lord Of The Rings", 19.99);
            book.AddCopies(1);
            var quantity = 2;
            cart.AddItem(book, quantity);

            // Act
            cart.Checkout(_supplierServiceMock.Object);

            // Assert
            _supplierServiceMock.Verify(s => s.OrderCopies(book.Title, It.IsAny<int>()), Times.Once);
        }

        [Theory]
        [InlineData(9)]
        [InlineData(10)]
        public void Checkout_Should_NotCallOrderCopies_When_QuantityIsLessThanOrEqualBookQuantity(int quantity)
        {
            // Arrange
            var cart = new Cart();
            var book = new Book("The Lord Of The Rings", 19.99);
            book.AddCopies(10);
            cart.AddItem(book, quantity);

            // Act
            cart.Checkout(_supplierServiceMock.Object);

            // Assert
            _supplierServiceMock.Verify(s => s.OrderCopies(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(9)]
        [InlineData(10)]
        public void Checkout_Should_ReturnDeliveryDate_As_Today_When_QuantityIsLessThanOrEqualBookQuantity(int quantity)
        {
            // Arrange
            var cart = new Cart();
            var book = new Book("The Lord Of The Rings", 19.99);
            book.AddCopies(10);
            cart.AddItem(book, quantity);

            // Act
            var deliveryDate = cart.Checkout(_supplierServiceMock.Object);

            // Assert
            Assert.Equal(DateOnly.FromDateTime(DateTime.Now), deliveryDate);
        }

        [Fact]
        public void Checkout_Should_ReturnDeliveryDate_GreaterThan_Today_When_QuantityIsGreaterThanBookQuantity()
        {
            // Arrange
            var cart = new Cart();
            var book = new Book("The Lord Of The Rings", 19.99);
            book.AddCopies(5);
            var quantity = 10;
            cart.AddItem(book, quantity);

            // Act
            var deliveryDate = cart.Checkout(_supplierServiceMock.Object);

            // Assert
            deliveryDate.Should().BeAfter(DateOnly.FromDateTime(DateTime.Now));
        }
    }
}
