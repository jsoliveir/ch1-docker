using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api.Core.Subscriptions.Models
{
    public class SubscriptionViewModel
    {
        [MaxLength(20, ErrorMessage = "Not a valid name")]
        public string FirstName { get; set; }

        public Gender Gender { get; set; } = Gender.NA;

        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public bool Consent { get; set; }

        [Required]
        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Not a valid id")]
        public int NewsletterId { get; set; }

    }
}
