using GPSNotepad.Models;

namespace GPSNotepad.Services.Time
{
    public interface ITimeService
    {
        int GetTimeCorrection(PinViewModel pinViewModel);
    }
}