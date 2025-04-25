using WorklogTrackingSystem.Domain.Entities;
using WorklogTrackingSystem.Domain.Enums;

namespace WorklogTrackingSystem.Infrastructure.Data
{
    public static class DatabaseSeeder
    {
        public static void SeedUsers(UserDbContext context)
        {
            if (!context.Users.Any())
            {
                var firstNames = new[]
                {
                    "John", "Jane", "Michael", "Emily", "Chris", "Sarah", "David", "Laura", "James", "Anna",
                    "Robert", "Linda", "William", "Elizabeth", "Richard", "Barbara", "Joseph", "Susan", "Thomas", "Jessica",
                    "Charles", "Karen", "Daniel", "Nancy", "Matthew", "Lisa", "Anthony", "Betty", "Mark", "Sandra",
                    "Donald", "Ashley", "Steven", "Kimberly", "Paul", "Donna", "Andrew", "Carol", "Joshua", "Michelle"
                };

                var lastNames = new[]
                {
                    "Smith", "Johnson", "Brown", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris", "Martin",
                    "Thompson", "Garcia", "Martinez", "Robinson", "Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall",
                    "Allen", "Young", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores", "Green",
                    "Adams", "Nelson", "Baker", "Carter", "Mitchell", "Perez", "Roberts", "Turner", "Phillips", "Campbell"
                };

                var loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

                var random = new Random();

                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Role = Role.Admin,
                    Login = "admin",
                    PasswordHash = "AQAAAAIAAYagAAAAEPeOCNHUSpTCOfi7PNQhTUI4UNcaLe+3iwGgcEsiyCyIW5jGpx06dv9VFeSEft1jrQ==",
                    DailyMinHours = 4,
                    DailyMaxHours = 8,
                    RefreshToken = null,
                    RefreshTokenExpiryTime = null
                };

                context.Users.Add(adminUser);
                context.SaveChanges();

                var users = Enumerable.Range(1, 10000).Select(i => new User
                {
                    FirstName = firstNames[random.Next(firstNames.Length)],
                    LastName = lastNames[random.Next(lastNames.Length)],
                    Role = Role.User,
                    Login = $"user{i}",
                    PasswordHash = "AQAAAAIAAYagAAAAEPeOCNHUSpTCOfi7PNQhTUI4UNcaLe+3iwGgcEsiyCyIW5jGpx06dv9VFeSEft1jrQ==",
                    DailyMinHours = random.Next(2, 7),
                    DailyMaxHours = random.Next(6, 9),
                    RefreshToken = null,
                    RefreshTokenExpiryTime = null
                }).ToList();

                context.Users.AddRange(users);
                context.SaveChanges();

                // Generate worklogs for each user
                var worklogs = users.SelectMany(user =>
                {
                    var worklogDates = Enumerable.Range(0, (DateTime.Now - new DateTime(2025, 4, 1)).Days + 1)
                                                 .Select(offset => new DateTime(2025, 4, 1).AddDays(offset))
                                                 .OrderBy(_ => random.Next())
                                                 .Take(random.Next(3, 7))
                                                 .ToList();

                    return worklogDates.SelectMany(date =>
                    {
                        var worklogCount = random.Next(1, 4);
                        return Enumerable.Range(1, worklogCount).Select(_ => new Worklog
                        {
                            UserId = user.Id,
                            Date = DateOnly.FromDateTime(date),
                            WorkedHours = random.Next(2, 5),
                            TaskId = random.Next(1000, 5001),
                            Description = loremIpsum[..100]
                        });
                    });
                }).ToList();

                context.Worklogs.AddRange(worklogs);
                context.SaveChanges();
            }
        }
    }
}