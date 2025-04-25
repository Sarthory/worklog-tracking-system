using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorklogTrackingSystem.Domain.DTOs
{
    public class WorklogDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateOnly Date { get; set; }

        public int WorkedHours { get; set; }

        public int? TaskId { get; set; }

        public string? Description { get; set; }

    }
}