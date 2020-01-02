using System;
using System.ComponentModel.DataAnnotations;

namespace DDD.School.API.DTOs
{

    public class CreateCourseDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(250, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string Title { get; set; }
    }
}
