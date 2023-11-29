using SettlementBookingSystem.Application.Bookings.Commands;
using System;

namespace SettlementBookingSystem.Application.Bookings.Dtos
{
    public class BookingDto : CreateBookingCommand
    {
        public BookingDto()
        {
            BookingId = Guid.NewGuid();
        }

        public Guid BookingId { get; }
    }
}
