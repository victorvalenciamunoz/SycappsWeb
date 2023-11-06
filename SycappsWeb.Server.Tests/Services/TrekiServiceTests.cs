using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Moq;
using SycappsWeb.Server.Data;
using SycappsWeb.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SycappsWeb.Server.Tests.Services;

public class TrekiServiceTests :IClassFixture<TrekiServiceFixture>
{
    private readonly TrekiServiceFixture fixture;
    private TrekiService? trekiService;

    public TrekiServiceTests(TrekiServiceFixture fixture)
    {
        this.fixture = fixture;
        
    }
    [Fact]
    public async Task Capture_ShouldReturnError_WhenUserDoesNotExist()
    {
        Mock<IConfiguration> mockConfiguration = new Mock<IConfiguration>();
        var mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

        IdentityUser? user = null;
        mockUserManager.Setup(service => service.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        //Arrange
        trekiService = new TrekiService(fixture.MockUoW.Object, mockConfiguration.Object, mockUserManager.Object);

        //Act
        var result = await trekiService.Capture(new Shared.Models.Un2Trek.CaptureTrekiRequest(), "");

        //Assert
        result.IsError.Should().Be(true);
        result.Errors[0].Equals(StringConstants.UserNotFoundErrorCode);
    }
}
