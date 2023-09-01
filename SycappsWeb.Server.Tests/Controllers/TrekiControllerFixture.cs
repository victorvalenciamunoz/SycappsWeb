using Microsoft.Extensions.Logging;
using Moq;
using SycappsWeb.Server.Controllers.v1;
using SycappsWeb.Server.Services;

namespace SycappsWeb.Server.Tests.Controllers;

public class TrekiControllerFixture : IDisposable
{
    public Mock<ITrekiService> MockTrekiService { get; private set; }
    public Mock<ILogger<Un2TrekTrekiController>> MockLoggerService { get; private set; }

    public TrekiControllerFixture()
    {
        MockTrekiService = new Mock<ITrekiService>();
        MockLoggerService = new Mock<ILogger<Un2TrekTrekiController>>();
    }
    public void Dispose()
    {
        MockTrekiService = null;
        MockLoggerService = null;
    }
}
