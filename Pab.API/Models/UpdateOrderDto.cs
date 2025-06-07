namespace Pab.API.Models
{
    // DTO do aktualizacji nagłówka zamówienia (np. zmiana UserId, Date, ewentualnie kwoty)
    public class UpdateOrderDto
    {
        public string UserId { get; set; } = null!;

        // Jeśli chcesz pozwolić na zmianę daty zamówienia lub kwoty, dodaj tu te pola:
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Zakładam uproszczony scenariusz: nie edytujemy pozycje w tym DTO.
        // Jeśli chcesz edytować pozycje, musisz rozbudować ten model o listę pozycji.
    }
}
