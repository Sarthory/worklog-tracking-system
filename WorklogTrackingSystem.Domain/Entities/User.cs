using System.ComponentModel.DataAnnotations.Schema;
using WorklogTrackingSystem.Domain.Enums;

namespace WorklogTrackingSystem.Domain.Entities
{
    public class User
    {
        private int _id;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _login = string.Empty;
        private string _passwordHash = string.Empty;
        private Role _role;
        private int _dailyMinHours;
        private int _dailyMaxHours;
        private string? _refreshToken;
        private DateTime? _refreshTokenExpiryTime;

        public User()
        {
        }

        public User(int id, string firstName, string lastName, string login, string passwordHash, Role role, int dailyMinHours, int dailyMaxHours)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Login = login;
            PasswordHash = passwordHash;
            Role = role;
            DailyMinHours = dailyMinHours;
            DailyMaxHours = dailyMaxHours;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get => _id;
            private set => _id = value;
        }

        public string FirstName
        {
            get => _firstName;
            set => _firstName = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("First name cannot be null or whitespace", nameof(value));
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Last name cannot be null or whitespace", nameof(value));
        }

        public string Login
        {
            get => _login;
            set => _login = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Login cannot be null or whitespace", nameof(value));
        }

        public string PasswordHash
        {
            get => _passwordHash;
            set => _passwordHash = !string.IsNullOrWhiteSpace(value)
                ? value
                : throw new ArgumentException("Password hash cannot be null or whitespace", nameof(value));
        }

        public Role Role
        {
            get => _role;
            set => _role = value;
        }

        public int DailyMinHours
        {
            get => _dailyMinHours;
            set => _dailyMinHours = value >= 0
                ? value
                : throw new ArgumentException("Daily minimum hours cannot be negative", nameof(value));
        }

        public int DailyMaxHours
        {
            get => _dailyMaxHours;
            set => _dailyMaxHours = value >= _dailyMinHours
                ? value
                : throw new ArgumentException("Daily maximum hours cannot be less than minimum hours", nameof(value));
        }

        public string? RefreshToken
        {
            get => _refreshToken;
            set => _refreshToken = value;
        }

        public DateTime? RefreshTokenExpiryTime
        {
            get => _refreshTokenExpiryTime;
            set => _refreshTokenExpiryTime = value;
        }

        public static IEnumerable<object> GenerateStaticUsers()
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

            var random = new Random();
            var users = new List<User>();

            for (int i = 1; i <= 100; i++)
            {
                users.Add(new User
                {
                    Id = i,
                    FirstName = firstNames[random.Next(firstNames.Length)],
                    LastName = lastNames[random.Next(lastNames.Length)],
                    Login = $"user{i}",
                    PasswordHash = "AQAAAAIAAYagAAAAEPeOCNHUSpTCOfi7PNQhTUI4UNcaLe+3iwGgcEsiyCyIW5jGpx06dv9VFeSEft1jrQ==",
                    Role = (Role)random.Next(1, 3), // 1 = Admin, 2 = User
                    DailyMinHours = random.Next(2, 7), // Between 2 and 6
                    DailyMaxHours = random.Next(6, 13), // Between 6 and 12
                    RefreshToken = string.Empty,
                    RefreshTokenExpiryTime = (DateTime?)null
                });
            }

            return users;
        }
    }
}