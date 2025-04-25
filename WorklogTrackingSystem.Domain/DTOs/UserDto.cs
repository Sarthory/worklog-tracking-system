using WorklogTrackingSystem.Domain.Enums;

namespace WorklogTrackingSystem.Domain.DTOs
{
    public class UserDto
    {
        public int Id { get; set; } = 0;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int DailyMinHours { get; set; } = 0;

        public int DailyMaxHours { get; set; } = 0;

        public string Role { get; set; } = string.Empty;

    }
}