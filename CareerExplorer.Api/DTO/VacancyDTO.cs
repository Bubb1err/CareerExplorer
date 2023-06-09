﻿using CareerExplorer.Core.Entities;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace CareerExplorer.Api.DTO
{
    public class VacancyDTO 
    {
        public int Id { get; set; }
        [Required]
        [MinLength(200, ErrorMessage = "Provide at least 200 symbols.")]
        public string Description { get; set; }
        [Required]
        public bool IsAvailable { get; set; } = false;
        public bool IsAccepted { get; set; }
        public int CreatorId { get; set; }
        public string CreatorNickName { get; set; }
        public string CreatorName { get; set; }
        public string CreatorSurname { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDesciprion { get; set; }
        public PositionDTO? Position { get; set; } 
        public ICollection<SkillTagDTO>? Requirements { get; set; } 
        public DateTime CreatedDate { get; set; } 
        public bool IsApplied { get; set; }
    }
}
