using WorklogTrackingSystem.Domain.Entities;

namespace WorklogTrackingSystem.Domain.DTOs
{
    public class WorklogSummaryDto
    {
        public DateOnly Date { get; set; }

        public int WorklogCount { get; set; }

        public string Situation { get; set; } = string.Empty;

        public double TotalWorkedHours { get; set; }

        public List<Worklog> Entries { get; set; } = [];
    }
}