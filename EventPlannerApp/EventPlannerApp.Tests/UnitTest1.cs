using EventPlannerApp.Controllers;
using EventPlannerApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace EventPlannerApp.Tests
{
    public class UnitTest1
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<SignInManager<ApplicationUser>> _signInManagerMock;
        private readonly Mock<ILogger<AccountController>> _loggerMock;
        private readonly AccountController _controller;
        public UnitTest1()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var optionsMock = new Mock<IOptions<IdentityOptions>>();
            var loggerMock = new Mock<ILogger<SignInManager<ApplicationUser>>>();
            var schemesMock = new Mock<IAuthenticationSchemeProvider>();
            var confirmationMock = new Mock<IUserConfirmation<ApplicationUser>>();

            _signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
                _userManagerMock.Object, contextAccessorMock.Object, claimsFactoryMock.Object,
                optionsMock.Object, loggerMock.Object, schemesMock.Object, confirmationMock.Object);

            _loggerMock = new Mock<ILogger<AccountController>>();

            _controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Login_ValidModel_RedirectsToHomeIndex()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Name = "testuser",
                Password = "password",
                RememberMe = false
            };

            _signInManagerMock
                .Setup(s => s.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            // Act
            var result = await _controller.Login(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }

        [Fact]
        public async Task Login_InvalidModel_ReturnsViewWithModel()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            var model = new LoginViewModel();

            // Act
            var result = await _controller.Login(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public async Task Login_InvalidCredentials_AddsModelErrorAndReturnsView()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Name = "testuser",
                Password = "wrongpassword",
                RememberMe = false
            };

            _signInManagerMock
                .Setup(s => s.PasswordSignInAsync(model.Name, model.Password, model.RememberMe, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _controller.Login(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
            Assert.False(_controller.ModelState.IsValid);
            Assert.True(_controller.ModelState.ContainsKey(string.Empty));
        }
    }
}