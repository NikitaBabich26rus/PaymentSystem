﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Models;

namespace PaymentSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}