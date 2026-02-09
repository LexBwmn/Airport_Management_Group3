using Fly_Away.Data;
using Fly_Away.DTOs;
using Fly_Away.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fly_Away.Controllers;

public class AuthPageController : Controller
{
    private readonly AppDbContext _db;
    private readonly PasswordService _passwords;

    public AuthPageController(AppDbContext db, PasswordService passwords)
    {
        _db = db;
        _passwords = passwords;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var email = (req.Email ?? "").Trim().ToLower();

        var user = await _db.Accounts.FirstOrDefaultAsync(a => a.Email.ToLower() == email);

        if (user == null || !_passwords.Verify(user.Password, req.Password))
        {
            ViewBag.Error = "Invalid email/password";
            return View();
        }

        HttpContext.Session.SetInt32("AccountId", user.Account_ID);
        return RedirectToAction("Index", "Home"); // or MyBookings if you want
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        var email = (req.Email ?? "").Trim().ToLower();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(req.Password))
        {
            ViewBag.Error = "Email and password required";
            return View();
        }

        var exists = await _db.Accounts.AnyAsync(a => a.Email.ToLower() == email);
        if (exists)
        {
            ViewBag.Error = "Account already exists";
            return View();
        }

        var account = new Fly_Away.Models.Account
        {
            Email = email,
            Password = _passwords.Hash(req.Password)
        };

        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();

        HttpContext.Session.SetInt32("AccountId", account.Account_ID);
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
