using DarbuotojaiWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DarbuotojaiWeb.Controllers
{
	public class PradziaController : Controller
	{
		private readonly ILogger<PradziaController> _logger;

		public PradziaController(ILogger<PradziaController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privatumas()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
