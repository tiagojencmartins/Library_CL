using Library.Domain.Enums;
using Library.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Library.Application.Controllers
{
    public class ControllerBase : Controller
    {
        [NonAction]
        public IActionResult GetActionStatus(CommandResult result)
        {
            switch (result.Status)
            {
                case StatusEnum.NotFound:
                    return NotFound(result.Description);
                case StatusEnum.NotModified:
                    return Problem(result.Description);
                case StatusEnum.Ok:
                default:
                    break;
            }

            return Ok(result.Description);
        }
    }
}
