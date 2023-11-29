using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Bookings.Dtos;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
        public async Task<ActionResult<BookingDto>> Create([FromBody] CreateBookingCommand command)
        {
            //var result =  await _mediator.Send(command);
            //return Ok(result);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);

                var bookingRequest = new CreateBookingCommand
                {
                    BookingId = "",
                    Name = command.Name,
                    BookingTime = DateTime.Now.ToString("HH:mm:ss"),
                    ExpiredTime = ""
                    //BookingTime = "18:23:34"
                };

                var bookingRequestDto = System.Text.Json.JsonSerializer.Serialize(bookingRequest);

                var content = new StringContent(bookingRequestDto, Encoding.UTF8, "application/json");

                var result = await client.PostAsync("/Main/get-booking", content);

                if (result.IsSuccessStatusCode)
                {
                    bookingRequest.BookingId = await result.Content.ReadAsStringAsync();
                    bookingRequest.ExpiredTime = DateTime.Parse(bookingRequest.BookingTime).AddHours(1).ToString();
                    return Ok(bookingRequest.BookingId.ToString());
                }
                else
                {
                    return BadRequest();
                }
            };
        }

    }
}
