using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAspNetApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string UserName { get; set; } = null!;
        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Password { get; set; } = null!;
        [Required]
        [StringLength(500, MinimumLength = 3)]
        public string Fullname { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        [Range(1, 2)]
        public int Role { get; set; }
        [NotMapped]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;

        public DateTime UserDate { get; set; }

        public string? Image { get; set; }
    }
}