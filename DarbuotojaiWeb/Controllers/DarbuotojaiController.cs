using DarbuotojaiWeb.Data;
using DarbuotojaiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DarbuotojaiWeb.Controllers
{
	public class DarbuotojaiController : Controller
	{
		private readonly ApplicationDbContext _db;
		public DarbuotojaiController(ApplicationDbContext db)
		{
			_db = db;
		}
		public IActionResult Index()
		{
			List<Darbuotojas> objDarbuotojaiSar = _db.Darbuotojai.ToList();
			return View(objDarbuotojaiSar);
		}

		[HttpPost]
		public IActionResult Index(string vardas, string pavarde, string statusas)
		{
			List<Darbuotojas> objDarbuotojaiSar = _db.Darbuotojai.ToList();
			List<Darbuotojas> filtruotasSar = objDarbuotojaiSar
				.Where(d => string.IsNullOrEmpty(vardas) || d.Vardas.Contains(vardas))
				.Where(d => string.IsNullOrEmpty(pavarde) || d.Pavarde.Contains(pavarde))
				.Where(d => string.IsNullOrEmpty(statusas) || d.Statusas.Contains(statusas))
				.ToList();
			return View(filtruotasSar);
		}

		public IActionResult Prideti()
		{
			List<Pareiga> objPareigosSar = _db.Pareigos.ToList();
			ViewBag.GalimosPareigos = objPareigosSar;

			return View();
		}

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
				Debug.WriteLine("Error: darbuotojas is null");
				return View("Error");
			}

		}

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
			Debug.WriteLine("TESTAS");
			Debug.WriteLine(darbuotojas.Vardas);
			Debug.WriteLine(darbuotojas.DarbuotojasPareigos.ElementAtOrDefault(2));

			if (darbuotojas == null)
			{
				return NotFound();
			}

			List<Pareiga> objPareigosSar = _db.Pareigos.ToList();
			ViewBag.GalimosPareigos = objPareigosSar;

			return View(darbuotojas);
		}

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
			Debug.WriteLine("TESTAS");
			Debug.WriteLine(darbuotojas.Vardas);
			Debug.WriteLine(darbuotojas.DarbuotojasPareigos.ElementAtOrDefault(2));

			if (darbuotojas == null)
			{
				return NotFound();
			}

			List<Pareiga> objPareigosSar = _db.Pareigos.ToList();
			ViewBag.GalimosPareigos = objPareigosSar;

			return View(darbuotojas);
		}

		[HttpPost, ActionName("Pasalinti")]
		public IActionResult PasalintiPOST (int? id)
		{
			Darbuotojas? darbuotojas = _db.Darbuotojai.Find(id);
			if (darbuotojas == null)
			{ 
				return NotFound(); 
			}
			darbuotojas.Statusas = "neaktyvus";
			_db.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}