﻿using System;
using System.Threading.Tasks;
using MabelBookshelf.Bookshelf.Application.Book.Commands;
using MabelBookshelf.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MabelBookshelf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("create")]
        [ProducesResponseType(typeof(BookInfoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> CreateBook([FromBody] CreateNewBookRequest request)
        {
            var command = new CreateBookCommand(request.ExternalId, "temp");
            var result = await _mediator.Send(command);
            return Ok(new BookInfoDto(result));
        }

        [Route("marknotfinished")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPatch]
        public async Task<ActionResult> MarkAsNotFinished([FromBody] MarkAsNotFinishedRequest request)
        {
            var command = new MarkAsNotFinishedCommand(request.BookId);
            var result = await _mediator.Send(command);
            if (result)
                return Ok();
            return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400))
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = 400
            };
        }

        [Route("startreading")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPatch]
        public async Task<ActionResult> StartReading([FromBody] StartReadingRequest request)
        {
            var command = new StartReadingCommand(request.Id ?? throw new ArgumentNullException());
            var result = await _mediator.Send(command);
            if (result)
                return Ok();
            return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400))
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = 400
            };
        }

        [Route("finish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPatch]
        public async Task<ActionResult> Finish([FromBody] FinishRequest request)
        {
            var command = new FinishBookCommand(request.Id ?? throw new ArgumentNullException());
            var result = await _mediator.Send(command);
            if (result)
                return Ok();
            return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400))
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = 400
            };
        }

        [Route("markwanted")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [HttpPatch]
        public async Task<ActionResult> MarkAsWanted([FromBody] MarkAsWantedRequest request)
        {
            var command = new MarkBookAsWantedCommand(request.Id ?? throw new ArgumentNullException());
            var result = await _mediator.Send(command);
            if (result)
                return Ok();
            return new ObjectResult(ProblemDetailsFactory.CreateProblemDetails(HttpContext, 400))
            {
                ContentTypes = { "application/problem+json" },
                StatusCode = 400
            };
        }
    }
}