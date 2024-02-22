using System.ComponentModel.DataAnnotations;

namespace DarbuotojaiWeb.Models
{
    public class Pareiga
    {
        [Required]
        public int PareigaId { get; set; }
        [Required]
        public string Pavadinimas { get; set; }

        public ICollection<DarbuotojasPareiga> DarbuotojasPareigos { get; set; }
    }
}