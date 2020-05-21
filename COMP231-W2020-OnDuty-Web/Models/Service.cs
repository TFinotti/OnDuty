using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace COMP231_W2020_OnDuty_Web.Models
{
    /// <summary>
    /// Service to be provided
    /// </summary>
    public class Service
    {
        [Key]
        public int Id { get; set; }
        // Used by the customer to provide more details about the service
        public string Details { get; set; }

        public string Status { get; set; }

        public string Category { get; set; }

        public User Customer { get; set; }

        public User Provider { get; set; }

        [Display(Name = "Date and Time")]
        public DateTime DateTime { get; set; }

        public int serviceEvaluation { get; set; }

    }
}
