using System;
using System.Collections.Generic;
using System.Text;

namespace Task_3.Domain.Models
{
    public class Job
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int RecruiterId { get; set; }

        public DateTime? ClosedAt { get; set; }

        public int? ClosedBy { get; set; }

        public ICollection<JobApplication> Applications { get; set; }
            = new List<JobApplication>();
    }
}
