using System.ComponentModel.DataAnnotations;

namespace MakiMora.Core.DTOs
{
    public class CourierEarningDto
    {
        public Guid Id { get; set; }
        public Guid CourierId { get; set; }
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string EarningType { get; set; } = string.Empty; // delivery_fee, bonus, penalty
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateCourierEarningRequestDto
    {
        [Required]
        public Guid CourierId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(20)]
        public string EarningType { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }
    }

    public class UpdateCourierEarningRequestDto
    {
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(20)]
        public string EarningType { get; set; } = string.Empty;
    }
}