using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Tests.Unit
{
    public class SupplierServiceTests
    {
        private readonly Mock<IDeliveryDateCalculator> _deliveryDateCalculatorMock = new();
        private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();

        public SupplierServiceTests()
        {
            _deliveryDateCalculatorMock
                .Setup(x => x.Calculate(It.IsAny<int>()))
                .Returns(DateOnly.FromDateTime(DateTime.Now));

            _dateTimeProviderMock.Setup(x => x.Now).Returns(DateTime.Now);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void OrderCopies_WhenTitleIsNotvalid_ThrowsArgumentException(string title)
        {
            // Arrange
            var supplierService = new SupplierService(_deliveryDateCalculatorMock.Object, _dateTimeProviderMock.Object);

            // Act
            Action action = () => supplierService.OrderCopies(title, 1);

            // Assert
            action.Should().Throw<ArgumentException>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void OrderCopies_WhenCopiesCountIsNotValid_ThrowsArgumentOutOfRangeException(int copiesCount)
        {
            // Arrange
            var supplierService = new SupplierService(_deliveryDateCalculatorMock.Object, _dateTimeProviderMock.Object);

            // Act
            Action action = () => supplierService.OrderCopies("title", copiesCount);

            // Assert
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void OrderCopies_ShouldCallCalculateBusinessDaysToAdd()
        {
            // Arrange
            var supplierService = new SupplierService(_deliveryDateCalculatorMock.Object, _dateTimeProviderMock.Object);
            
            // Act
            supplierService.OrderCopies("title", 1);

            // Assert
            _deliveryDateCalculatorMock.Verify(x => x.CalculateBusinessDaysToAdd(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void OrderCopies_ShouldCallCalculate()
        {
            // Arrange
            var supplierService = new SupplierService(_deliveryDateCalculatorMock.Object, _dateTimeProviderMock.Object);

            // Act
            supplierService.OrderCopies("title", 1);

            // Assert
            _deliveryDateCalculatorMock.Verify(x => x.Calculate(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void OrderCopies_WhenDeliveryDateIsInThePast_ThrowsInvalidDeliveryDateException()
        {
            // Arrange
            var supplierService = new SupplierService(_deliveryDateCalculatorMock.Object, _dateTimeProviderMock.Object);

            _deliveryDateCalculatorMock
                .Setup(x => x.Calculate(It.IsAny<int>()))
                .Returns(DateOnly.FromDateTime(DateTime.Now.AddDays(-1)));

            // Act
            Action action = () => supplierService.OrderCopies("title", 1);

            // Assert
            action.Should().Throw<InvalidDeliveryDateException>();
        }

        [Theory]
        [InlineData("2024-01-01")]
        [InlineData("2024-01-02")]
        public void OrderCopies_ShouldReturnDeliveryDate(string date)
        {
            // Arrange
            var supplierService = new SupplierService(_deliveryDateCalculatorMock.Object, _dateTimeProviderMock.Object);
            _dateTimeProviderMock.Setup(x => x.Now).Returns(DateTime.Parse("2024-01-01"));
            var expectedDeliveryDate = DateOnly.Parse(date);
            _deliveryDateCalculatorMock.Setup(x => x.Calculate(It.IsAny<int>())).Returns(expectedDeliveryDate);

            // Act
            var deliveryDate = supplierService.OrderCopies("title", 1);

            // Assert
            deliveryDate.Should().Be(expectedDeliveryDate);
        }
    }
}
