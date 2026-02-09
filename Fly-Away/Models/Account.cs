namespace Fly_Away.Models;

public class Account
{
    public int Account_ID { get; set; }
    public string Email { get; set; } = string.Empty;

    // store hash (not plaintext)
    public string Password { get; set; } = string.Empty;
}
