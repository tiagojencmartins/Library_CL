using Library.Application.Mediator.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DummyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DummyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Logs the user in and gets his associated bearer token
        /// </summary>
        /// <returns>
        /// Bearer token
        /// </returns>
        /// /// <param name="email">User email.</param>
        /// /// <param name="password">User password.</param>
        /// <response code="200">Bearer token</response>
        /// <response code="401">Unauthorized.</response>
        /// <response code="204">No user found with the given credentials</response>
        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _mediator.Send(new LoginCommand(email, password));

            return GetActionStatus(result);
        }
    }
}