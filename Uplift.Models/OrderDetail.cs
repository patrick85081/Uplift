using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Uplift.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int OrderHeaderId { get; set; }
        
        [ForeignKey(nameof(OrderHeaderId))]
        public OrderHeader OrderHeader { get; set; }
        
        [Required]
        public int ServiceId { get; set; }
        
        [ForeignKey(nameof(ServiceId))]
        public Service Service { get; set; }
        
        [Required]
        public string ServiceName { get; set; }
        
        [Required]
        public double Price { get; set; }
    }
}