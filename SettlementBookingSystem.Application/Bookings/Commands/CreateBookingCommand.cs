using MediatR;
using SettlementBookingSystem.Application.Bookings.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<BookingDto>
    {
        public string BookingId { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$")]
        [Required]
        public string Name { get; set; }
        public string BookingTime { get; set; }

        public string ExpiredTime { get; set; }
    }
}
