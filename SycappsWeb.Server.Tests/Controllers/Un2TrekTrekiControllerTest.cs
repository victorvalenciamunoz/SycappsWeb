using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SycappsWeb.Server.Controllers.v1;
using SycappsWeb.Server.Models.IntegrationModels;
using SycappsWeb.Server.Services;
using SycappsWeb.Server.Tests.Controllers;
using System.Security.Claims;
using System.Security.Principal;

namespace SycappsWeb.Server.Tests;

public class Un2TrekTrekiControllerTest : IClassFixture<TrekiControllerFixture>
{
    private readonly TrekiControllerFixture fixture;

    public Un2TrekTrekiControllerTest(TrekiControllerFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async Task Capture_OnSuccess_ReturnsOk()
    {   
        var captureRequest = new Shared.Models.Un2Trek.CaptureTrekiRequest();

        var captureResult = new ServiceResultSingleElement<bool>();
        captureResult.Errors.Clear();
        this.fixture.MockTrekiService.Setup(service => service.Capture(captureRequest, It.IsAny<string>())).ReturnsAsync(captureResult);

        //Arrange
        var sut = new Un2TrekTrekiController(this.fixture.MockTrekiService.Object);
        SetControllerContext(sut);

        // Act
        var result = (OkObjectResult)await sut.Capture(captureRequest);

        //Assert
        result.StatusCode.Should().Be(200);
        this.fixture.MockLoggerService.Invocations.Should().HaveCount(0);
    }

    [Fact]
    public async Task Capture_OnFail_ReturnsBadRequest()
    { 
        var captureRequest = new Shared.Models.Un2Trek.CaptureTrekiRequest();

        var captureResult = new ServiceResultSingleElement<bool>();
        captureResult.Errors.Add(string.Empty);
        this.fixture.MockTrekiService.Setup(service => service.Capture(captureRequest, It.IsAny<string>())).ReturnsAsync(captureResult);

        //Arrange
        var sut = new Un2TrekTrekiController(this.fixture.MockTrekiService.Object);
        SetControllerContext(sut);

        //Act
        var result = (BadRequestObjectResult)await sut.Capture(captureRequest);

        //Assert
        result.StatusCode.Should().Be(400);
        this.fixture.MockLoggerService.Invocations.Should().HaveCount(1);
    }

    private void SetControllerContext(Un2TrekTrekiController controller)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim("fullName", "Prueba prueba"),
                                   }, "Test"));
        controller.ControllerContext = new ControllerContext();
        controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
    }
}
