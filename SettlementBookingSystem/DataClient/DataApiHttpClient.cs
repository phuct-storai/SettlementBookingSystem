using System.Net.Http;
using System;

namespace SettlementBookingSystem.DataClient
{
    public class DataApiHttpClient
    {
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://localhost:44364"),
    };
}
}
