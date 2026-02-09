using Fly_Away.Data;
using Fly_Away.DTOs;
using Fly_Away.Models;
using Fly_Away.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fly_Away.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly PasswordService _passwords;

    public AuthController(AppDbContext db, PasswordService passwords)
    {
        _db = db;
        _passwords = passwords;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req)
    {
        var email = req.Email.Trim().ToLower();

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest("Email and password required.");

        var exists = await _db.Accounts.AnyAsync(a => a.Email.ToLower() == email);
        if (exists) return Conflict("Account already exists.");

        var account = new Account
        {
            Email = email,
            Password = _passwords.Hash(req.Password)
        };

        _db.Accounts.Add(account);
        await _db.SaveChangesAsync();

        return Ok(new { account.Account_ID, account.Email });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req)
    {
        var email = req.Email.Trim().ToLower();

        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Email.ToLower() == email);
        if (account is null) return Unauthorized("Invalid email/password.");

        if (!_passwords.Verify(account.Password, req.Password))
            return Unauthorized("Invalid email/password.");

        HttpContext.Session.SetInt32("AccountId", account.Account_ID);

        return Ok(new { message = "Logged in", accountId = account.Account_ID, account.Email });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("AccountId");
        return Ok(new { message = "Logged out" });
    }

    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var id = HttpContext.Session.GetInt32("AccountId");
        if (id is null) return Unauthorized("Not logged in.");

        var account = await _db.Accounts.FindAsync(id.Value);
        if (account is null) return Unauthorized("Not logged in.");

        return Ok(new { account.Account_ID, account.Email });
    }
}
