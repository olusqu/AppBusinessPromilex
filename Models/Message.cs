using System;

namespace WebBusinessPromilexApp.Models;

public partial class Message
{
    public int Id { get; set; }
    public string SenderName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public string Content { get; set; } = null!;
    public DateTime SentDate { get; set; }

    // Status przeczytania wiadomości we "wbudowanej" skrzynce odbiorczej
    public bool IsRead { get; set; } = false;
}