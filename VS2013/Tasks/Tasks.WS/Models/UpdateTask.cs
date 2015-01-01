using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tasks.WS.Models
{
    public class UpdateTask
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required]
        public DomainModel.TaskStatus Status { get; set; }
        public int EstimatedHours { get; set; }
        public int RemainingdHours { get; set; }
    }
}