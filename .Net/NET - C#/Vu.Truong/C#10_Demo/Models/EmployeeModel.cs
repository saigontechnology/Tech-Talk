namespace C_10_Demo.Models;
public class EmployeeModel : BaseModel
{
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Title { get; set; }   
    public int Age { get; set; }

    public OfficeModel Office { get; set; }
    public PositionModel Position { get; set; }
}

public class OfficeModel
{
    public string Name { get; set; }
    public string Address { get; set; }
    public OfficeType Type { get; set; }
}

public class PositionModel
{
    public string Name { get; set; }
    public int Level { get; set; }

    public string Description { get; set; }
}

public enum OfficeType
{
    Base,
    Extended,
    Side
}