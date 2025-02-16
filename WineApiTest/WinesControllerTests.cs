using WineApi.Controllers;
using WineApi.Database.Entities;
using WineApi.Database.Repositories;
using WineApi.Models;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;

namespace WineApi.Tests
{
    public class WinesControllerTests
    {
        private readonly Mock<IRepository<Wine>> _mockRepository;
        private readonly Mock<IValidator<WineDto>> _mockValidator;
        private readonly Mock<IMapper> _mockMapper;
        private readonly WinesController _controller;

        public WinesControllerTests()
        {
            _mockRepository = new Mock<IRepository<Wine>>();
            _mockValidator = new Mock<IValidator<WineDto>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new WinesController(null!, _mockRepository.Object, _mockValidator.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetAllWines_ReturnsOkResult_WithListOfWineDto()
        {
            // Arrange
            var wines = new List<Wine>
            {
                new() { Id = 1, Title = "Chardonnay", Year = 2020, Brand = "Brand A", Type = "White" },
                new() { Id = 2, Title = "Cabernet Sauvignon", Year = 2018, Brand = "Brand B", Type = "Red" },
                new() { Id = 3, Title = "Merlot", Year = 2019, Brand = "Brand C", Type = "Red" },
                new() { Id = 4, Title = "Pinot Noir", Year = 2021, Brand = "Brand D", Type = "Red" },
                new() { Id = 5, Title = "Riesling", Year = 2017, Brand = "Brand E", Type = "White" },
            };

            var wineDtos = new List<WineDto>
            {
                new() { Title = "Chardonnay", Year = 2020, Brand = "Brand A", Type = "White" },
                new() { Title = "Cabernet Sauvignon", Year = 2018, Brand = "Brand B", Type = "Red" },
                new() { Title = "Merlot", Year = 2019, Brand = "Brand C", Type = "Red" },
                new() { Title = "Pinot Noir", Year = 2021, Brand = "Brand D", Type = "Red" },
                new() { Title = "Riesling", Year = 2017, Brand = "Brand E", Type = "White" },
            };

            _mockRepository.Setup(r => r.GetAll()).Returns(wines);
            _mockMapper.Setup(m => m.Map<IEnumerable<WineDto>>(It.IsAny<IEnumerable<Wine>>())).Returns(wineDtos);

            // Act
            var result = _controller.GetAllWines();

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(wineDtos);
        }

        [Fact]
        public void GetWineById_ReturnsNotFound_WhenWineDoesNotExist()
        {
            // Arrange
            int wineId = 1;
            _mockRepository.Setup(r => r.GetById(wineId)).Returns((Wine)null!);

            // Act
            var result = _controller.GetWineById(wineId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetWineById_ReturnsOkResult_WithWineDto_WhenWineExists()
        {
            // Arrange
            int wineId = 1;
            var wine = new Wine { Id = 1, Title = "Chardonnay", Year = 2020, Brand = "Brand A", Type = "White" };
            var wineDto = new WineDto { Title = "Chardonnay", Year = 2020, Brand = "Brand A", Type = "White" };

            _mockRepository.Setup(r => r.GetById(wineId)).Returns(wine);
            _mockMapper.Setup(m => m.Map<WineDto>(It.IsAny<Wine>())).Returns(wineDto);

            // Act
            var result = _controller.GetWineById(wineId);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeEquivalentTo(wineDto);
        }

        [Fact]
        public void AddWine_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var wineDto = new WineDto { Title = "", Year = 2020, Brand = "Brand A", Type = "White" };
            _mockValidator.Setup(v => v.Validate(wineDto)).Returns(new ValidationResult(new[] { new ValidationFailure("Title", "Title is required") }));

            // Act
            var result = _controller.AddWine(wineDto);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void AddWine_ReturnsCreatedAtAction_WhenWineIsAdded()
        {
            // Arrange
            var wineDto = new WineDto { Title = "Chardonnay", Year = 2020, Brand = "Brand A", Type = "White" };
            var wine = new Wine { Id = 1, Title = "Chardonnay", Year = 2020, Brand = "Brand A", Type = "White" };

            _mockValidator.Setup(v => v.Validate(wineDto)).Returns(new ValidationResult());
            _mockMapper.Setup(m => m.Map<Wine>(wineDto)).Returns(wine);
            _mockRepository.Setup(r => r.Add(wine));
            _mockRepository.Setup(r => r.Save());

            // Act
            var result = _controller.AddWine(wineDto);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public void UpdateWine_ReturnsNotFound_WhenWineDoesNotExist()
        {
            // Arrange
            int wineId = 1;
            var wineDto = new WineDto { Title = "Chardonnay", Year = 2020, Brand = "Brand A", Type = "White" };

            _mockValidator.Setup(v => v.Validate(wineDto)).Returns(new ValidationResult());
            _mockRepository.Setup(r => r.GetById(wineId)).Returns((Wine)null!);

            // Act
            var result = _controller.UpdateWine(wineId, wineDto);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void UpdateWine_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            int wineId = 1;
            var wineDto = new WineDto { Title = "", Year = 2020, Brand = "Brand A", Type = "White" };
            _mockValidator.Setup(v => v.Validate(wineDto)).Returns(new FluentValidation.Results.ValidationResult(new[] { new FluentValidation.Results.ValidationFailure("Name", "Name is required") }));

            // Act
            var result = _controller.UpdateWine(wineId, wineDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void DeleteWine_ReturnsOkResult_WhenWineIsDeleted()
        {
            // Arrange
            int wineId = 1;
            var wine = new Wine { Id = 1, Title = "Chardonnay", Year = 2020, Brand = "Brand A", Type = "White" };

            _mockRepository.Setup(r => r.GetById(wineId)).Returns(wine);
            _mockRepository.Setup(r => r.Delete(wineId));
            _mockRepository.Setup(r => r.Save());

            // Act
            var result = _controller.DeleteWine(wineId);

            // Assert
            result.Should().BeOfType<OkResult>();
        }
    }
}