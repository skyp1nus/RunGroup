﻿using System.ComponentModel.DataAnnotations;

namespace MyProjectRunGroup.Models
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        public int? Pace { get; set; }
        public int? Milage { get; set; }
        public Address Address { get; set; }
        public ICollection<Club> Clubs { get; set; }
        public ICollection<Race> Races { get; set; }
    }
}
