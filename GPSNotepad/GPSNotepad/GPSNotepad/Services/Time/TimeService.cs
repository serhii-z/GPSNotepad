using GeoTimeZone;
using GPSNotepad.Models;
using System;

namespace GPSNotepad.Services.Time
{
    public class TimeService : ITimeService
    {
        #region -- ITimeService implementation --

        public int GetTimeCorrection(PinViewModel pinViewModel)
        {
            var timeZone = TimeZoneLookup.GetTimeZone(pinViewModel.Latitude, pinViewModel.Longitude).Result;
            var timeUTC = DateTime.UtcNow;
            var zoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            var dateTimeZone = TimeZoneInfo.ConvertTimeFromUtc(timeUTC, zoneInfo);
            var hours = dateTimeZone.Subtract(DateTime.Now).Hours;

            return hours;
        }

        #endregion
    }
}
