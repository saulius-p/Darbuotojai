using DarbuotojaiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DarbuotojaiWeb.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}

		public DbSet<Darbuotojas> Darbuotojai { get; set; }
		public DbSet<Pareiga> Pareigos { get; set; }
		public DbSet<DarbuotojasPareiga> DarbuotojasPareigos { get; set; }
		public DbSet<Pacientas> Pacientai { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder); // Optional.

			// Configure many-to-many relationship
			modelBuilder.Entity<DarbuotojasPareiga>()
				.HasKey(dp => new { dp.DarbuotojasId, dp.PareigaId });

			modelBuilder.Entity<DarbuotojasPareiga>()
				.HasOne(dp => dp.Darbuotojas)
				.WithMany(d => d.DarbuotojasPareigos)
				.HasForeignKey(dp => dp.DarbuotojasId);

			modelBuilder.Entity<DarbuotojasPareiga>()
				.HasOne(dp => dp.Pareiga)
				.WithMany(p => p.DarbuotojasPareigos)
				.HasForeignKey(dp => dp.PareigaId);

			modelBuilder.Entity<Pareiga>().HasData(
				new Pareiga { PareigaId = 1, Pavadinimas = "Administratorius(-ė)" },
				new Pareiga { PareigaId = 2, Pavadinimas = "Analitikas(-ė)" },
				new Pareiga { PareigaId = 3, Pavadinimas = "Bendrosios praktikos slaugytoja" },
				new Pareiga { PareigaId = 4, Pavadinimas = "Laborantas(-ė)" },
				new Pareiga { PareigaId = 5, Pavadinimas = "Vadovas(-ė)" },
				new Pareiga { PareigaId = 6, Pavadinimas = "Apskaitininkas(-ė)" },
				new Pareiga { PareigaId = 7, Pavadinimas = "Programuotojas(-a)" }
			);
		}
	}
}