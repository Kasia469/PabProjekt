using System.ComponentModel.DataAnnotations;
namespace Pab.WebAdmin.Models
{

    public class ProductDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        public string Name { get; set; } = null!;

        [Range(0.01, 1000000, ErrorMessage = "Cena musi być większa od 0")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock nie może być ujemny")]
        public int Stock { get; set; }
    }
}