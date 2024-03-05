namespace C_10_Demo.Pieces;
public class ExtendedPropertyPatterns
{
    public static void Execute()
    {
        var employee = new EmployeeModel
        {
            Name = "Employee 1",
            Age = 40,
            FirstName = "Employee",
            LastName = "1",
            Office = new OfficeModel
            {
                Name = "Office 1",
                Type = OfficeType.Base,
                Address = "Office Address 1",
            },
            Position = new PositionModel
            {
                Level = 1,
                Name = "Developer",
                Description = "Writing Code and Debug with 💖"
            },
            Title = "Developer"
        };

        EmployeeModel employee2 = new()
        {
            Name = "Employee 2",
            Age = 35,
            FirstName = "Employee",
            LastName = "2",
            Office = new OfficeModel
            {
                Name = "Office 2",
                Type = OfficeType.Extended,
                Address = "Office Address 2",
            },
            Position = new PositionModel
            {
                Level = 10,
                Name = "Universal Developer",
                Description = "Writing Code and Debug with 💖 💖 💖 💖 💖 💖 💖"
            },
            Title = "DeveloperSieuCapDzuTru"
        };

        // Old ways, from C# 8
        if (employee is { Office: { Type: OfficeType.Base } })
        {
            Console.WriteLine(employee.Name);
        }

        // New ways, from C# 10
        if (employee2 is { Office.Type: OfficeType.Extended, Age: > 10, Position.Level: > 4 })
        {
            Console.WriteLine(employee2.Name);
        }
    }
}
