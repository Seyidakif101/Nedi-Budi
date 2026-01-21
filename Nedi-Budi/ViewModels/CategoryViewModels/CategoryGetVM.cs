using System.ComponentModel.DataAnnotations;

namespace Nedi_Budi.ViewModels.CategoryViewModels
{
    public class CategoryGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
