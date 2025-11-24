using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.DTOs
{
    public class AssignCourierRequestDto
    {
        [Required]
        public Guid CourierId { get; set; }
    }
}