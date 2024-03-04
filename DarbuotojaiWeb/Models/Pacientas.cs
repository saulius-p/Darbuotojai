using System;
using System.ComponentModel.DataAnnotations;

namespace DarbuotojaiWeb.Models
{
	public class Pacientas
	{
		[Required]
		public int PacientasId { get; set; }

		[Required]
		[MaxLength(12)]
		public string Vardas { get; set; }

		[Required]
		[MaxLength(15)]
		public string Pavarde { get; set; }

		// Galbūt gali būti užsieniečių asmens kodai?
		[Required]
		[MaxLength(14)]
		public string AsmensKodas { get; set; }

		[MaxLength(40)]
		public string Adresas { get; set; }

		[Required]
		public string Statusas { get; set; } = "aktyvus";

		public int? DarbuotojasId { get; set; }

		public Darbuotojas Darbuotojas { get; set; }

	}

}