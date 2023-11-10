using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<EmployeeController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.Employee.FirstName} {compensation.Employee.LastName}'");

            var result = _compensationService.Create(compensation);

            // Employee didn't exist, try again!
            if (result == null)
                return BadRequest();

            return CreatedAtRoute("getCompensationById", new { id = result.Employee.EmployeeId }, result);
        }

        [HttpGet("{id}", Name = "getCompensationById")]
        public IActionResult GetCompensationById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var compensation = _compensationService.GetById(id);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }

    }
}
