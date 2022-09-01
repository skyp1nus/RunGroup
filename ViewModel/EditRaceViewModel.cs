﻿using MyProjectRunGroup.Data.Enum;
using MyProjectRunGroup.Models;

namespace MyProjectRunGroup.ViewModel
{
    public class EditRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string? ImageUrl { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }
        public RaceCategory RaceCategory { get; set; }
    }
}
