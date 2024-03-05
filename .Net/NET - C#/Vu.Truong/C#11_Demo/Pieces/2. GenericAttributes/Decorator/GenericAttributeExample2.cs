namespace C_11_Demo.Pieces.Decorator;
internal class GenericAttributeExample2 : IExample
{
    public static int Order => 3;

    public static string Name => "Generic Attribute: Decorator";

    public static string Description => "Generic Attribute: Decorator";

    public static void Execute()
    {
        Console.WriteLine("Decorator: logger and counter");
        var actor = SceneFactory.GetActor<Actor>();
        actor.Action();
        Console.WriteLine();

        Console.WriteLine("Decorator: logger");
        actor = SceneFactory.GetActor<ActorWithLog>();
        actor.Action();
        Console.WriteLine();

        Console.WriteLine("Decorator: counter");
        actor = SceneFactory.GetActor<ActorWithCounter>();
        actor.Action();
        Console.WriteLine();
    }
}