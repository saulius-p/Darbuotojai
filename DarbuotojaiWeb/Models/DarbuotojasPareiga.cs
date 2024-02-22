namespace DarbuotojaiWeb.Models
{
    public class DarbuotojasPareiga
    {
        public int DarbuotojasId { get; set; }
        public Darbuotojas Darbuotojas { get; set; }

        public int PareigaId { get; set; }
        public Pareiga Pareiga { get; set; }

		public override string ToString()
		{
			return $"DarbuotojasPareiga: DarbuotojasId={DarbuotojasId}, PareigaId={PareigaId}.";

		}
	}
}