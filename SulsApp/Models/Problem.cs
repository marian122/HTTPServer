using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SulsApp.Models
{
    public class Problem
    {
        public Problem()
        {
            this.Submissions = new HashSet<Submission>();
        }
        public string Id { get; set; }

        [MaxLength(20), Required]
        public string Name { get; set; }

        [Range(50, 300), Required]
        public int Points { get; set; }

        public ICollection<Submission> Submissions { get; set; }

    }
}
