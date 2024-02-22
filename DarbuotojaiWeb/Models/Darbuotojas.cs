using System.ComponentModel.DataAnnotations;

namespace DarbuotojaiWeb.Models
{
	public class Darbuotojas
	{
		[Required]
		public int DarbuotojasId { get; set; }
		[Required]
		public string Vardas { get; set; }
		[Required]
		public string Pavarde { get; set; }
		[Required]
		public DateTime GimimoData { get; set; }
		public string Adresas { get; set; }
		[Required]
		public string Statusas { get; set; } = "aktyvus";

		public ICollection<DarbuotojasPareiga> DarbuotojasPareigos { get; set; }
	}

}
