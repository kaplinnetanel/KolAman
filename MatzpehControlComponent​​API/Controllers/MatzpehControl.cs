using MatzpehControlComponentAPI.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace MatzpehControlComponent​​API.Controllers;

[ApiController]
[Route("api/alerts")]
public class AlertsController : ControllerBase
{
    private readonly IReservoir _reservoir;
    public AlertsController(IReservoir reservoir)
    {
        _reservoir = reservoir;

    }

    string[] regions =
    {
        "NORTH", "SOUTH", "CENTER", "OVERSEAS"
    };

    [HttpGet("count")]
    public async Task<IActionResult> Count()
    {
        var result = _reservoir.Count();

        return Ok(result);
    }
}
