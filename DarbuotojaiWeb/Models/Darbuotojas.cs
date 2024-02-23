using System.ComponentModel.DataAnnotations;

namespace DarbuotojaiWeb.Models
{
	public class Darbuotojas
	{
		[Required]
		public int DarbuotojasId { get; set; }

		[Required]
		[MaxLength(12)]
		public string Vardas { get; set; }

		[Required]
		[MaxLength(15)]
		public string Pavarde { get; set; }

		[Required]
		public DateTime GimimoData { get; set; }

		[MaxLength(30)]
		public string Adresas { get; set; }

		[Required]
		public string Statusas { get; set; } = "aktyvus";

		public ICollection<DarbuotojasPareiga> DarbuotojasPareigos { get; set; }
	}

}
