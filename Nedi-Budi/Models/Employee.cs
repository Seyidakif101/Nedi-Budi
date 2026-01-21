using Nedi_Budi.Models.Common;

namespace Nedi_Budi.Models
{
    public class Employee:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
