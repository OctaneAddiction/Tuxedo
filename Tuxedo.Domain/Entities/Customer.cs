using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuxedo.Domain.Entities
{
    public class Customer
    {
        [Key]
        public Guid ObjectId { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public ICollection<CustomerSaving> CustomerSavings { get; set; } = new List<CustomerSaving>();
    }
}
