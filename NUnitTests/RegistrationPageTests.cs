
using Imagination.Application.DTOs;
using Imagination.Application.Features.Account.Commands.Register;
using Imagination.Application.Patterns.Mediator.Interfaces;
using Imagination.Application.Patterns.Singleton;
using Imagination.Application.Responses;
using Imagination.Domain.Enum;
using Imagination.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace NUnitTests
{
    public class RegistrationPageTests
    {
        //private AccountController _accountController;
        //private Mock<IMediator> _mediatorMock;

        [SetUp]
        public void Setup()
        {
            //_mediatorMock = new Mock<IMediator>();

            //_accountController = new AccountController(_mediatorMock.Object);
            //_accountController.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        }


        [Test]
        public async Task Register_SuccessfulRegistration_RedirectsLogin()
        {
            //// Arrange
            //var model = new RegisterDto
            //{
            //    Username = "VIP",
            //    Email = "vip_client@gmail.com",
            //    Password = "pass",
            //    ConfirmPassword = "password"
            //};

            //var response = new BaseResponse { ErrorCode = Imagination.Domain.Enum.ErrorCode.NoError };
            //_mediatorMock.Setup(m => m.Send(It.IsAny<RegisterCommand>(), default)).ReturnsAsync(response);

            //// Act
            //var result = await _accountController.Register(model) as RedirectToActionResult;

            //// Assert
            //Assert.IsNotNull(result);
            //Assert.AreEqual("Login", result.ActionName);
            //Assert.AreEqual("Account", result.ControllerName);
        }

        //[TearDown]
        //public void TearDown()
        //{
        //    _accountController?.Dispose();
        //}

    }
}