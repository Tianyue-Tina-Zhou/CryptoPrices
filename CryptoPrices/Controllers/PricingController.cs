using CryptoPrices.Models;
using CryptoPrices.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoPrices.Controllers
{
    public class PricingController : Controller
    {
        private readonly ILogger<PricingController> _logger;
        IPriceComparisonService _priceComparisonService;

        //Injection into constructor
        public PricingController(ILogger<PricingController> logger, IPriceComparisonService priceComparisonService)
        {
            _logger = logger;
            _priceComparisonService = priceComparisonService;
        }

        public IActionResult Index()
        {
            ViewData["Prices"] = _priceComparisonService.GetPriceRecords();
            ViewData["Results"] = _priceComparisonService.GetPriceComparisonResults();
            return View();
        }
    }
}
