using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tuxedo.Domain.Entities
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;

        // Navigation property
        public ICollection<ValueTracker> ValueTrackers { get; set; } = new List<ValueTracker>();
    }
}
