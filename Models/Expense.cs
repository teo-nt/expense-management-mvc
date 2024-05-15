using System;
using System.Collections.Generic;

namespace ExpenseManagementMVC.Models;

public partial class Expense
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal Amount { get; set; }

    public string Category { get; set; } = null!;

    public DateOnly Date { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
