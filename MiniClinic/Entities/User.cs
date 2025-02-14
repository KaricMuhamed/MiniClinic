﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniClinic.Entities
{
    [Index(nameof(Email), IsUnique  = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Admin,
        Doctor
    }
}
