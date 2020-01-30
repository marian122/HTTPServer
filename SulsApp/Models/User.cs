using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SulsApp.Models
{
    public class User
    {
        public User()
        {
            this.Submissions = new HashSet<Submission>();
        }
        public string Id { get; set; }

        [MaxLength(20),Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public ICollection<Submission> Submissions { get; set; }

    }
}
