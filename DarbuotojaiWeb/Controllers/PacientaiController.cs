using DarbuotojaiWeb.Data;
using DarbuotojaiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using static DarbuotojaiWeb.Controllers.DarbuotojaiController;

namespace DarbuotojaiWeb.Controllers
{
	/// <summary>
	/// Klasė su pacientų sąrašu susijusiems veiksmams valdyti.
	/// </summary>
	public class PacientaiController : Controller
	{
		private readonly ApplicationDbContext _db;
		/// <summary>
		/// Klasės PacientaiController konstruktorius.
		/// </summary>
		/// <param name="db">Klasės ApplicationDbContext objektas prieigai prie duomenų bazės.</param>
		public PacientaiController(ApplicationDbContext db)
		{
			_db = db;
		}

        /// <summary>
        /// Pagal paieškos kriterijus pateikiamas puslapiuotas pacientų sąrašas.
        /// </summary>
        /// <param name="vardas">Paieškos tekstas ieškojimui pagal vardą.</param>
        /// <param name="pavarde">Paieškos tekstas ieškojimui pagal pavardę.</param>
        /// <param name="statusas">Paieškos tekstas ieškojimui pagal statusą.</param>
        /// <param name="pusl">Puslapio, į kurį sunaviguota, numeris.</param>
        /// <returns>Rodinys su palapiuotu darbuotojų sąrašu.</returns>
        public IActionResult Index(string vardas, string pavarde, string statusas, int pusl = 1)
        {

            int elSkPuslapyje = 7;
            IQueryable<Pacientas> pacientai = _db.Pacientai.Include(p => p.Darbuotojas)
														   .ThenInclude(d => d.DarbuotojasPareigos)
										                   .ThenInclude(dp => dp.Pareiga)
														   .AsQueryable();

            pacientai = pacientai
                .Include(p => p.Darbuotojas)
				.Where(d => string.IsNullOrEmpty(vardas) || d.Vardas.ToLower().Contains(vardas.ToLower()))
                .Where(d => string.IsNullOrEmpty(pavarde) || d.Pavarde.ToLower().Contains(pavarde.ToLower()))
                .Where(d => string.IsNullOrEmpty(statusas) || d.Statusas == statusas);

            var visoPacientu = pacientai.Count();

            pacientai = pacientai.Skip((pusl - 1) * elSkPuslapyje).Take(elSkPuslapyje);

            var puslapiuotiPacientai = pacientai.ToList();

            var viewModel = new PaginatedList<Pacientas>(puslapiuotiPacientai, visoPacientu, pusl, elSkPuslapyje);

            ViewData["VardasFilter"] = vardas;
            ViewData["PavardeFilter"] = pavarde;
            ViewData["StatusasFilter"] = statusas;

            return View(viewModel);
        }

        /// <summary>
        /// Atvaizduoja formą naujo paciento pridėjimui.
        /// </summary>
        /// <returns>Rodinys naujo paciento pridėjimui.</returns>
        public IActionResult Prideti()
		{
			List<Darbuotojas> objDarbuotojuSar = _db.Darbuotojai.Include(d => d.DarbuotojasPareigos)
														        .ThenInclude(dp => dp.Pareiga)
																.Where(d => d.Statusas == "aktyvus" &&
																			d.DarbuotojasPareigos.Any(dp => dp.Pareiga.Pavadinimas.Contains("Gydytoj") || 
																			                                dp.Pareiga.Pavadinimas == "Administratorius(-ė)")).ToList();
			ViewBag.DarbuotojuSar = objDarbuotojuSar;

			return View();
		}

		/// <summary>
		/// Įvykdoma HTTP POST užklausa pridėti naują pacientą.
		/// </summary>
		/// <param name="pacientas">Klasės Pacientas objektas su duomenimis iš formos.</param>
		/// <param name="pasirinktasDarbuotojas">Pacientui priskirto darbuotojo ID.</param>
		/// <returns>Nukreipimas (redirect) į pacientų sąrašą ARBA klaidos rodinys.</returns>
		[HttpPost]
		public IActionResult Prideti(Pacientas pacientas, int pasirinktasDarbuotojas)
		{

			Debug.WriteLine("TESTAS");
			Debug.WriteLine(pasirinktasDarbuotojas);

			Darbuotojas? darbuotojas = _db.Darbuotojai.FirstOrDefault(u => u.DarbuotojasId == pasirinktasDarbuotojas);
            
			Debug.WriteLine("TESTAS");
            Debug.WriteLine(darbuotojas);

			if (darbuotojas != null)
			{
				pacientas.Darbuotojas = darbuotojas;

				_db.Pacientai.Add(pacientas);
				_db.SaveChanges();
				TempData["success"] = "Paciento įrašas sukurtas!";
				return RedirectToAction("Index");
			}
			else
			{
				ModelState.AddModelError("pasirinktasDarbuotojas", "Pasirinkite darbuotoją.");
				List<Darbuotojas> objDarbuotojuSar = _db.Darbuotojai.Include(d => d.DarbuotojasPareigos)
																    .ThenInclude(dp => dp.Pareiga)
																    .Where(d => d.Statusas == "aktyvus" &&
																			    d.DarbuotojasPareigos.Any(dp => dp.Pareiga.Pavadinimas.Contains("Gydytoj") ||
																											    dp.Pareiga.Pavadinimas == "Administratorius(-ė)")).ToList();
				ViewBag.DarbuotojuSar = objDarbuotojuSar;

                return View();
			}
		}

		/// <summary>
		/// Atvaizduojama forma esamo paciento duomenų koregavimui.
		/// </summary>
		/// <param name="id">Paciento, kurio duomenis koreguosime, id.</param>
		/// <returns>
		/// Jeigu paciento ID yra null arba 0, grąžiname "NotFound".
		/// Jeigu paciento su tokiu ID nėra duomenų bazėje, grąžiname "NotFound".
		/// Kitu atveju atvaizduojama forma su informacija apie pacientą.
		/// </returns>
		public IActionResult Koreguoti(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Pacientas? pacientas = _db.Pacientai
									  .Include(d => d.Darbuotojas)
								      .FirstOrDefault(u => u.PacientasId == id);

			if (pacientas == null)
			{
				return NotFound();
			}

			Debug.WriteLine("TESTAS");
			Debug.WriteLine(pacientas.Vardas);
			Debug.WriteLine(pacientas.Darbuotojas.ToString());

			List<Darbuotojas> objDarbuotojuSar = _db.Darbuotojai.Include(d => d.DarbuotojasPareigos)
																.ThenInclude(dp => dp.Pareiga)
																.Where(d => d.Statusas == "aktyvus" &&
																			d.DarbuotojasPareigos.Any(dp => dp.Pareiga.Pavadinimas.Contains("Gydytoj") ||
																											dp.Pareiga.Pavadinimas == "Administratorius(-ė)")).ToList();
			ViewBag.DarbuotojuSar = objDarbuotojuSar;

			return View(pacientas);
		}

		/// <summary>
		/// Įvykdoma HTTP POST užklausa esamo paciento informacijos atnaujinimui duomenų bazėje.
		/// </summary>
		/// <param name="pacientasIsFormos">Klasės Pacientas objektas su duomenimis iš formos.</param>
		/// <param name="pasirinktasDarbuotojas">Pacientui (galimai naujai) priskirtas darbuotojas.</param>
		/// <returns>Paciento informacijos duomenų bazėje atnaujinimas ir nukreipimas (redirect) į pacientų sąrašą ARBA klaidos rodinys.</returns>

		[HttpPost]
		public IActionResult Koreguoti(Pacientas pacientasIsFormos, int pasirinktasDarbuotojas)
		{
			if (pacientasIsFormos != null)
			{
				var pacientas = _db.Pacientai
					.Include(d => d.Darbuotojas)
					.FirstOrDefault(u => u.PacientasId == pacientasIsFormos.PacientasId);

				if (pacientas != null)
				{
					Darbuotojas? darbuotojas = _db.Darbuotojai.FirstOrDefault(d => d.DarbuotojasId == pasirinktasDarbuotojas);

					pacientas.Vardas = pacientasIsFormos.Vardas;
					pacientas.Pavarde = pacientasIsFormos.Pavarde;
					pacientas.AsmensKodas = pacientasIsFormos.AsmensKodas;
					pacientas.Adresas = pacientasIsFormos.Adresas;
					pacientas.Darbuotojas = darbuotojas;

					_db.Pacientai.Update(pacientas);
					_db.SaveChanges();

					return RedirectToAction("Index");
				}
				else
				{
					Debug.WriteLine("Error: pacientas yra null");
					return View("Error");
				}
			}
			else
			{
				Debug.WriteLine("Error: pacientasIsFormos yra null");
				return View("Error");
			}
		}

		/// <summary>
		/// Atvaizduojamas rodinys esamo paciento pašalinimui (statuso pakeitimui).
		/// </summary>
		/// <param name="id">Paciento, kurį norima pašalinti, id.</param>
		/// <returns>
		/// Jeigu paciento ID yra null arba 0, grąžiname "NotFound".
		/// Jeigu paciento su tokiu ID nėra duomenų bazėje, grąžiname "NotFound".
		/// Kitu atveju atvaizduojama neveiksni forma su informacija apie pacientą.
		/// </returns>
		public IActionResult Pasalinti(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Pacientas? pacientas = _db.Pacientai
									  .Include(d => d.Darbuotojas)
									  .ThenInclude(d => d.DarbuotojasPareigos)
									  .ThenInclude(dp => dp.Pareiga)
									  .FirstOrDefault(u => u.PacientasId == id);

			if (pacientas == null)
			{
				return NotFound();
			}

			return View(pacientas);
		}

		/// <summary>
		/// Įvykdoma HTTP POST užklausa darbuotojo pašalinimui (statuso pakeitimui).
		/// </summary>
		/// <param name="id">Darbuotojo, kurį norima pašalinti, id.</param>
		/// <returns>
		/// Jeigu darbuotojo su tokiu ID nėra duomenų bazėje, grąžiname "NotFound".
		/// Darbuotojo statusas duomenų bazėje pakeičiamas į "neaktyvus" ir nukreipiama (redirect) į darbuotojų sąrašą.
		/// </returns>
		[HttpPost, ActionName("Pasalinti")]
		public IActionResult PasalintiPOST(int? id)
		{
			Pacientas? pacientas = _db.Pacientai.Find(id);
			if (pacientas == null)
			{
				return NotFound();
			}
			pacientas.Statusas = "neaktyvus";
			pacientas.DarbuotojasId = null;

			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Atvaizduojamas rodinys su informacija apie pacientą.
		/// </summary>
		/// <param name="id">Paciento id.</param>
		/// <returns>
		/// Jeigu paciento ID yra null arba 0, grąžiname "NotFound".
		/// Jeigu paciento su tokiu ID nėra duomenų bazėje, grąžiname "NotFound".
		/// Kitu atveju atvaizduojama neveiksni forma su informacija apie pacientą.
		/// </returns>
		public IActionResult Ziureti(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Pacientas? pacientas = _db.Pacientai
									  .Include(d => d.Darbuotojas)
									  .ThenInclude(d => d.DarbuotojasPareigos)
									  .ThenInclude(dp => dp.Pareiga)
									  .FirstOrDefault(u => u.PacientasId == id);

			if (pacientas == null)
			{
				return NotFound();
			}

			return View(pacientas);
		}

		/// <summary>
		/// Klasė reprezentuojanti puslapiuotą elementų sąrašą.
		/// </summary>
		/// <typeparam name="T">Sąraše esančių elementų tipas.</typeparam>
		// Idėją pasiėmiau iš interneto. Kol kas palieku angliškai.
		public class PaginatedList<T>
        {
            public List<T> Items { get; set; }
            public int TotalItems { get; set; }
            public int CurrentPage { get; set; }
            public int PageSize { get; set; }
            public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

            /// <summary>
            /// Inicijuoja PaginatedList klasės objektą.
            /// </summary>
            /// <param name="items">Elementų skaičius dabartiniame puslapyje.</param>
            /// <param name="totalItems">Sąrašo ilgis.</param>
            /// <param name="currentPage">Dabartinis puslapis.</param>
            /// <param name="pageSize">Pasirinktas elementų skaičius viename puslapyje.</param>
            public PaginatedList(List<T> items, int totalItems, int currentPage, int pageSize)
            {
                Items = items;
                TotalItems = totalItems;
                CurrentPage = currentPage;
                PageSize = pageSize;
            }
        }
    }
}
