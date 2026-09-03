using API.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        return Unauthorized();
    }

    [HttpGet("badrequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("This is a bad request.");
    }

    [HttpGet("notfound")]
    public IActionResult GetNotFound()
    {
        return NotFound("This resource was not found.");
    }

    [HttpGet("servererror")]
    public IActionResult GetServerError()
    {
        return StatusCode(500, "This is a server error.");
    }

    [HttpPost("validationerror")]
    public IActionResult GetValidationError(CreateProductDto product)
    {
        return BadRequest("This is a validation error.");
    }

    [HttpGet("internalerror")]
    public IActionResult GetError()
    {
        throw new Exception("This is an internal server error.");
    }        
}
