using System;
using System.Collections.Generic;

namespace ExpenseManagementMVC.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string Lastname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}
