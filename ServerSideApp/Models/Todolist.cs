using System;
using System.Collections.Generic;

namespace ServerSideApp.Models;

public partial class Todolist
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Task { get; set; } = null!;

    public virtual Cpr User { get; set; } = null!;
}
