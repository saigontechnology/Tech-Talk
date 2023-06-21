using System;
using System.Collections.Generic;

namespace ClientServerEvaluation.Models;

public partial class Student
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }
    public int Grade { get; set; }
}
