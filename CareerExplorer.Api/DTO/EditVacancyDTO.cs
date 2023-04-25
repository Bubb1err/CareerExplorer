﻿using CareerExplorer.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace CareerExplorer.Api.DTO
{
    public class EditVacancyDTO
    {
        public int Id { get; set; }
        [Required]
        [MinLength(200, ErrorMessage = "Provide at least 200 symbols.")]
        public string Description { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        [Required]
        [Range(0, 20000, ErrorMessage = "Provide number between 0 and 20 000.")]
        public int? Salary { get; set; }
        public int? WorkType { get; set; }
        public int? EnglishLevel { get; set; }
        public int? ExperienceYears { get; set; } = 0;
    }
}
