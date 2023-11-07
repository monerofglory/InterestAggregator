using Ical.Net;
using System.Net.Http;
using System;
using System.Threading.Tasks;
using Ical.Net.CalendarComponents;

namespace InterestAggregatorFunction.Services.IcsReader
{
    public class IcsReader
    {
        public (string fixtureName, DateTime kickoff) CheckFootballFixtures(string team)
        {
            Calendar cal = GetICalAsync($"https://sports.yahoo.com/soccer/teams/{team}/ical.ics").Result;
            CalendarEvent tomorrow = GetTomorowsEvent(cal);
            if (tomorrow != null)
            {
                return (tomorrow.Summary, tomorrow.DtStart.AsSystemLocal);
            }
            return (string.Empty, DateTime.MinValue);
        }

        private CalendarEvent? GetTomorowsEvent(Calendar calendar)
        {
            var tomorrow = DateTime.Now.AddDays(1).Date;
            foreach (CalendarEvent calEvent in calendar.Events)
            {
                var calEventStartDate = calEvent.Start.Date;
                if (calEventStartDate == tomorrow) {
                    return calEvent;
                }
            }
            return null;
        }

        private static async Task<Calendar> GetICalAsync(string url)
        {
            Calendar calendar;  
            using (var client = new HttpClient())
            {
                using (var stream = await client.GetStreamAsync("https://sports.yahoo.com/soccer/teams/chelsea/ical.ics"))
                {
                    calendar = Calendar.Load(stream);
                    // Do something with the calendar object
                }
            }
            return calendar;

        }
    }
}
