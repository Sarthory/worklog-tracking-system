using System.ComponentModel.DataAnnotations.Schema;

namespace WorklogTrackingSystem.Domain.Entities
{
    public class Worklog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int _id;
        private int _userId;
        private DateOnly _date;
        private int _workedHours;
        private int? _taskId;
        private string? _description;

        public int Id
        {
            get => _id;
            private set => _id = value;
        }

        [ForeignKey("User")]
        public int UserId
        {
            get => _userId;
            set => _userId = value > 0 
                ? value 
                : throw new ArgumentException("User ID must be greater than 0", nameof(value));
        }

        public DateOnly Date
        {
            get => _date;
            set => _date = value;
        }

        public int WorkedHours
        {
            get => _workedHours;
            set => _workedHours = value >= 0 
                ? value 
                : throw new ArgumentException("Worked hours cannot be negative", nameof(value));
        }

        public int? TaskId
        {
            get => _taskId;
            set => _taskId = value.HasValue && value.Value <= 0 
                ? throw new ArgumentException("Task ID must be greater than 0", nameof(value))
                : value;
        }

        public string? Description
        {
            get => _description;
            set => _description = value;
        }
    }
}