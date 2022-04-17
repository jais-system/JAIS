using Common;
using Common.Services.AppManager;
using Common.Services.AppManager.Entities;
using Microsoft.AspNetCore.Mvc;

namespace RestServer.Controllers.V1;

[Route("api/v1/apps")]
[ApiController]
public class AppsController : ControllerBase
{
    private readonly IAppManager _appManager;

    public AppsController()
    {
        _appManager = DependencyInjection.Resolve<IAppManager>();
    }

    [HttpPost]
    [Route("upload")]
    [RequestSizeLimit(5000000000)]
    public async Task<ActionResult> InstallApp([FromHeader] string appName, [FromHeader] string fileName, [FromHeader] string appVersion, [FromHeader] string author)
    {
        bool result = _appManager.RequestSideloadingApp(new SideloadingRequest(appName, author, appVersion));

        if (!result)
        {
            return StatusCode(403);
        }

        InstallAppResult installResult = await _appManager.InstallApp(fileName, Request.Body);

        return installResult == InstallAppResult.Success ? Ok() : UnprocessableEntity();
    }
}