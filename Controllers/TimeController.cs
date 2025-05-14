using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/time")]
public class TimeController : ControllerBase
{
    [HttpGet("timezones")]
    public IActionResult GetTimezones()
    {
        var timezones = TimeZoneInfo.GetSystemTimeZones()
                                    .Select(tz => tz.Id)
                                    .ToList();
        return Ok(timezones);
    }

    [HttpGet]
    public IActionResult GetTime(string timezone)
    {
        try
        {
            var localTime = DateTime.Now;
            var selectedTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
            var selectedTime = TimeZoneInfo.ConvertTime(localTime, selectedTimeZone);

            return Ok(new
            {
                SelectedTime = selectedTime.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }
        catch (TimeZoneNotFoundException)
        {
            return BadRequest("Invalid timezone specified.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }
}