namespace C_11_Demo.Pieces.Validator;
internal class GenericAttributeExample : IExample
{
    public static int Order => 2;

    public static string Name => "Generic Attribute: Validator";

    public static string Description => "Generic Attribute: Validator";

    public static void Execute()
    {
        var factory = new FactoryService();
        try
        {
            Console.WriteLine("Add product with validation");
            factory.AddProduct();
            Console.WriteLine("Add product success");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Add product fail: {ex.Message}");
        }

        Console.WriteLine();

        try
        {
            Console.WriteLine("Update product with validation");
            factory.UpdateProduct();
            Console.WriteLine("Update product success");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Update product fail: {ex.Message}");
        }

        Console.WriteLine();

        try
        {
            Console.WriteLine("Update product without validation");
            factory.UpdateProductNoValidation();
            Console.WriteLine("Update product success");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Update product fail: {ex.Message}");
        }
    }
}