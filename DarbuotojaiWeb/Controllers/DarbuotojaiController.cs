using DarbuotojaiWeb.Data;
using DarbuotojaiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DarbuotojaiWeb.Controllers
{
	/// <summary>
	/// Klasė su drabuotojų sąrašu susijusiems veiksmams valdyti.
	/// </summary>
	public class DarbuotojaiController : Controller
	{
		private readonly ApplicationDbContext _db;

		/// <summary>
		/// Klasės DarbuotojaiController konstruktorius.
		/// </summary>
		/// <param name="db">Klasės ApplicationDbContext objektas prieigai prie duomenų bazės.</param>
		public DarbuotojaiController(ApplicationDbContext db)
		{
			_db = db;
		}

		/// <summary>
		/// Pagal paieškos kriterijus pateikiamas puslapiuotas darbuotojų sąrašas.
		/// </summary>
		/// <param name="vardas">Paieškos tekstas ieškojimui pagal vardą.</param>
		/// <param name="pavarde">Paieškos tekstas ieškojimui pagal pavardę.</param>
		/// <param name="statusas">Paieškos tekstas ieškojimui pagal statusą.</param>
		/// <param name="pusl">Puslapio, į kurį sunaviguota, numeris.</param>
		/// <returns>Rodinys su palapiuotu darbuotojų sąrašu.</returns>
		public IActionResult Index(string vardas, string pavarde, string statusas, int pusl = 1)
		{

			int elSkPuslapyje = 7;
			IQueryable<Darbuotojas> darbuotojai = _db.Darbuotojai.AsQueryable();

			darbuotojai = darbuotojai
				.Where(d => string.IsNullOrEmpty(vardas) || d.Vardas.ToLower().Contains(vardas.ToLower()))
				.Where(d => string.IsNullOrEmpty(pavarde) || d.Pavarde.ToLower().Contains(pavarde.ToLower()))
				.Where(d => string.IsNullOrEmpty(statusas) || d.Statusas == statusas);

			var visoDarbuotoju = darbuotojai.Count();

			darbuotojai = darbuotojai.Skip((pusl - 1) * elSkPuslapyje).Take(elSkPuslapyje);

			var puslapiuotiDarbuotojai = darbuotojai.ToList();

			var viewModel = new PaginatedList<Darbuotojas>(puslapiuotiDarbuotojai, visoDarbuotoju, pusl, elSkPuslapyje);

			ViewData["VardasFilter"] = vardas;
			ViewData["PavardeFilter"] = pavarde;
			ViewData["StatusasFilter"] = statusas;

			return View(viewModel);
		}

		/// <summary>
		/// Atvaizduoja formą naujo darbuotojo pridėjimui.
		/// </summary>
		/// <returns>Rodinys naujo darbuotojo pridėjimui.</returns>
		public IActionResult Prideti()
		{
			List<Pareiga> objPareigosSar = _db.Pareigos.ToList();
			ViewBag.GalimosPareigos = objPareigosSar;

			return View();
		}

		/// <summary>
		/// Įvykdoma HTTP POST užklausa pridėti naują darbuotoją.
		/// </summary>
		/// <param name="darbuotojas">Klasės Darbuotojas objektas su duomenimis iš formos.</param>
		/// <param name="pasirinktosPareigos">Darbuotojui priskirtų pareigų sąrašas.</param>
		/// <returns>Nukreipimas (redirect) į darbuotojų sąrašą ARBA klaidos rodinys.</returns>
		[HttpPost]
		public IActionResult Prideti(Darbuotojas darbuotojas, List<int> pasirinktosPareigos)
		{
			//List<int> pasirinktosPareigos = new List<int> { 1, 3 };
			var selectedPareigos = _db.Pareigos.Where(p => pasirinktosPareigos.Contains(p.PareigaId)).ToList();

			if (darbuotojas != null)
			{
				darbuotojas.DarbuotojasPareigos = new List<DarbuotojasPareiga>();

				foreach (var pareiga in selectedPareigos)
				{
					darbuotojas.DarbuotojasPareigos.Add(new DarbuotojasPareiga
					{
						PareigaId = pareiga.PareigaId
					});
				}

				_db.Darbuotojai.Add(darbuotojas);
				_db.SaveChanges();

				return RedirectToAction("Index");
			}
			else
			{
				Debug.WriteLine("Error: darbuotojas yra null");
				return View("Error");
			}

		}

		/// <summary>
		/// Atvaizduojama forma esamo darbuotojo duomenų koregavimui.
		/// </summary>
		/// <param name="id">Darbuotojo, kurio duomenis koreguosime, id.</param>
		/// <returns>
		/// Jeigu darbuotojo ID yra null arba 0, grąžiname "NotFound".
		/// Jeigu darbuotojo su tokiu ID nėra duomenų bazėje, grąžiname "NotFound".
		/// Kitu atveju atvaizduojama forma su informacija apie darbuotoją.
		/// </returns>
		public IActionResult Koreguoti(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Darbuotojas? darbuotojas = _db.Darbuotojai
										  .Include(d => d.DarbuotojasPareigos)
										  .ThenInclude(dp => dp.Pareiga)
										  .FirstOrDefault(u => u.DarbuotojasId == id);

			if (darbuotojas == null)
			{
				return NotFound();
			}

			Debug.WriteLine("TESTAS");
			Debug.WriteLine(darbuotojas.Vardas);
			Debug.WriteLine(darbuotojas.DarbuotojasPareigos.ElementAtOrDefault(2));
			List<Pareiga> objPareigosSar = _db.Pareigos.ToList();
			ViewBag.GalimosPareigos = objPareigosSar;

			return View(darbuotojas);
		}

		/// <summary>
		/// Įvykdoma HTTP POST užklausa esamo darbuotojo informacijos atnaujinimui duomenų bazėje.
		/// </summary>
		/// <param name="darbuotojasIsFormos">Klasės Darbuotojas objektas su duomenimis iš formos.</param>
		/// <param name="pasirinktosPareigos">Darbuotojui (galimai naujai) priskirtų pareigų sąrašas.</param>
		/// <returns>Darbuotojo informacijos duomenų bazėje atnaujinimas ir nukreipimas (redirect) į darbuotojų sąrašą ARBA klaidos rodinys.</returns>

		[HttpPost]
		public IActionResult Koreguoti(Darbuotojas darbuotojasIsFormos, List<int> pasirinktosPareigos)
		{
			if (darbuotojasIsFormos != null)
			{
				var darbuotojas = _db.Darbuotojai
					.Include(d => d.DarbuotojasPareigos)
					.FirstOrDefault(u => u.DarbuotojasId == darbuotojasIsFormos.DarbuotojasId);

				if (darbuotojas != null)
				{
					darbuotojas.Vardas = darbuotojasIsFormos.Vardas;
					darbuotojas.Pavarde = darbuotojasIsFormos.Pavarde;
					darbuotojas.GimimoData = darbuotojasIsFormos.GimimoData;
					darbuotojas.Adresas = darbuotojasIsFormos.Adresas;

					darbuotojas.DarbuotojasPareigos.Clear();

					var pareigos = _db.Pareigos.Where(p => pasirinktosPareigos.Contains(p.PareigaId)).ToList();
					foreach (var pareiga in pareigos)
					{
						darbuotojas.DarbuotojasPareigos.Add(new DarbuotojasPareiga
						{
							PareigaId = pareiga.PareigaId
						});
					}

					// Update Darbuotojas with the new DarbuotojasPareigos
					_db.Darbuotojai.Update(darbuotojas);
					_db.SaveChanges();

					return RedirectToAction("Index");
				}
				else
				{
					Debug.WriteLine("Error: existingDarbuotojas is null");
					return View("Error");
				}
			}
			else
			{
				Debug.WriteLine("Error: darbuotojas is null");
				return View("Error");
			}
		}

		/// <summary>
		/// Atvaizduojamas rodinys esamo darbuotojo pašalinimui (statuso pakeitimui).
		/// </summary>
		/// <param name="id">Darbuotojo, kurį norima pašalinti, id.</param>
		/// <returns>
		/// Jeigu darbuotojo ID yra null arba 0, grąžiname "NotFound".
		/// Jeigu darbuotojo su tokiu ID nėra duomenų bazėje, grąžiname "NotFound".
		/// Kitu atveju atvaizduojama neveiksni forma su informacija apie darbuotoją.
		/// </returns>
		public IActionResult Pasalinti(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Darbuotojas? darbuotojas = _db.Darbuotojai
										  .Include(d => d.DarbuotojasPareigos)
										  .ThenInclude(dp => dp.Pareiga)
										  .FirstOrDefault(u => u.DarbuotojasId == id);

			if (darbuotojas == null)
			{
				return NotFound();
			}

			return View(darbuotojas);
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
			Darbuotojas? darbuotojas = _db.Darbuotojai
										  .Include(d => d.Pacientai)
										  .FirstOrDefault(u => u.DarbuotojasId == id);
			if (darbuotojas == null)
			{
				return NotFound();
			}
			darbuotojas.Statusas = "neaktyvus";

			foreach (var pacientas in darbuotojas.Pacientai)
			{
				pacientas.Statusas = "neaktyvus";
				pacientas.DarbuotojasId = null;
			}
			_db.SaveChanges();
			return RedirectToAction("Index");
		}

		/// <summary>
		/// Atvaizduojamas rodinys su informacija apie darbuotoją.
		/// </summary>
		/// <param name="id">Darbuotojo id.</param>
		/// <returns>
		/// Jeigu darbuotojo ID yra null arba 0, grąžiname "NotFound".
		/// Jeigu darbuotojo su tokiu ID nėra duomenų bazėje, grąžiname "NotFound".
		/// Kitu atveju atvaizduojama neveiksni forma su informacija apie darbuotoją.
		/// </returns>
		public IActionResult Ziureti(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			Darbuotojas? darbuotojas = _db.Darbuotojai
										  .Include(d => d.DarbuotojasPareigos)
										  .ThenInclude(dp => dp.Pareiga)
										  .Include(d => d.Pacientai)
										  .FirstOrDefault(u => u.DarbuotojasId == id);

			if (darbuotojas == null)
			{
				return NotFound();
			}

			return View(darbuotojas);
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