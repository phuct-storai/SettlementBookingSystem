using SettlementBookingSystem.Application.Bookings.Commands;
using System;

namespace SettlementBookingSystem.Application.Bookings.Dtos
{
    public class BookingDto : CreateBookingCommand
    {
        public BookingDto()
        {
            BookingId = Guid.Empty;
        }

        public Guid BookingId { get; }
    }
}
