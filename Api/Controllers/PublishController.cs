﻿using Business.Discrete;
using Data.Login;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly IPublishService _publishService;

        public PublishController(IPublishService publishService)
        {
            _publishService = publishService;
        }

        [HttpPost(nameof(RegisterViaRabbitMQAsync))]
        public async Task<IActionResult> RegisterViaRabbitMQAsync(Register register)
        {
            await _publishService.RegisterViaRabbitMQAsync(register);
            return Created();
        }
    }
}