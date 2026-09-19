using System;
using System.Collections.Generic;
using System.Text;
using Task_3.Domain.Enums;

namespace Task_3.Domain.Models
{


    public class JobApplication
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public int CandidateId { get; set; }

        public ApplicationStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? CancelledAt { get; set; }

        public Job Job { get; set; } = null!;
    }
}
