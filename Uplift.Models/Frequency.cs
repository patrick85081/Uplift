using System.ComponentModel.DataAnnotations;

namespace Uplift.Models
{
    public class Frequency
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FrequencyCount { get; set; }

        [Required]
        [Display(Name = "Frequency Name")]
        public string Name { get; set; }
    }
}