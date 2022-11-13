using System.ComponentModel.DataAnnotations;

namespace AFDemo.Models
{
    public class ProcessOrderStatus
    {
        public int Id { get; set; }

        [MaxLength(10)]
        public string JobId { get; set; }

        [MaxLength(10)]
        public string Status { get; set; }
    }
}