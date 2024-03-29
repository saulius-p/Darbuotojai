﻿using System;
using System.Collections.Generic;
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

		[MaxLength(40)]
		public string Adresas { get; set; }

		[Required]
		public string Statusas { get; set; } = "aktyvus";

		public ICollection<DarbuotojasPareiga> DarbuotojasPareigos { get; set; }

		public ICollection<Pacientas> Pacientai {  get; set; }

		public override string ToString()
		{
			string pareiguSarasas = "";
			if (DarbuotojasPareigos != null)
			{
				foreach (var darbuotojasPareiga in DarbuotojasPareigos)
				{
					pareiguSarasas += $"{darbuotojasPareiga.Pareiga.Pavadinimas}, ";
				}

				pareiguSarasas = pareiguSarasas.TrimEnd(',', ' ');
			}

			return $"{Vardas} {Pavarde} ({pareiguSarasas})";
		}
	}

}
