using MediatR;
using SettlementBookingSystem.Application.Bookings.Dtos;
using System;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<BookingDto>
    {
        public string Name { get; set; }
        public DateTime BookingTime { get; set; }
    }
}
