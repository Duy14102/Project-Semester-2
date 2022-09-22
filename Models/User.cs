using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstAspNetApp.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; } = null!;
        [Required]
        [Range(1, Int32.MaxValue)]
        public int Role { get; set; }
        [NotMapped]
        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; } = null!;


    }
}