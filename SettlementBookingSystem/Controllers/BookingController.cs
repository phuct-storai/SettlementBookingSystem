using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.ProblemDetails;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SettlementBookingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly String url = "https://localhost:44364";

        public BookingController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookingDto>> CreateAsync([FromBody] CreateBookingCommand command)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);

                CreateBookingCommand bookingRequest = CreateBookingRequest(command);

                var bookingRequestDto = System.Text.Json.JsonSerializer.Serialize(bookingRequest);

                var content = new StringContent(bookingRequestDto, Encoding.UTF8, "application/json");

                var result = await client.PostAsync("/Main/get-booking", content);

                if (result.StatusCode == HttpStatusCode.BadRequest)
                {
                    return BadRequest();
                }

                if (result.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new ConflictException("Already exits a booking in this time range. Try again!");
                }

                return await GetResponseAsync(bookingRequest, result);
            };
        }

        private static CreateBookingCommand CreateBookingRequest(CreateBookingCommand command)
        {
            return new CreateBookingCommand
            {
                BookingId = "",
                Name = command.Name,
                BookingTime = command.BookingTime,
                ExpiredTime = "",
            };
        }

        private async Task<ActionResult<BookingDto>> GetResponseAsync(CreateBookingCommand bookingRequest, HttpResponseMessage result)
        {
            bookingRequest.BookingId = await result.Content.ReadAsStringAsync();
            bookingRequest.ExpiredTime = DateTime.Parse(bookingRequest.BookingTime).AddHours(1).ToString();
            return Ok(bookingRequest.BookingId.ToString());
        }
    }
}
