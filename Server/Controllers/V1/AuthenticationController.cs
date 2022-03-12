using Common;
using Common.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Entities;
using Server.Services.KeyService;

namespace Server.Controllers.V1;

[Route("api/v1/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpPost]
    [Route("authenticate")]
    public ActionResult<string> GetToken([FromBody] AuthenticationRequest request)
    {
        if (request.Key != "test")
        {
            return Unauthorized();
        }

        var keyService = Ioc.Resolve<IKeyService>();

        return keyService.GenerateJwtToken();
    }

    [HttpGet]
    [Authorize]
    [Route("test")]
    public ActionResult<IEnumerable<string>> Get()
    {
        return new string[] { "value1", "value2", "value3", "value4", "value5" };
    }
}