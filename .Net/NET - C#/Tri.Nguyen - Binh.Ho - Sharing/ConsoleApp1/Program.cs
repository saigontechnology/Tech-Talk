using AbstractClassAndInterface;

public class AnimalDemo
{

    public static void Main(string[] args)
    {

        // Create a CanEat object.
        // An object declared as CanEat
        // Actually it is the Cat.
        ICanEat canEat1 = new Cat("Tom");

        // An object declared as CanEat
        // Actually it is the Mouse.
        ICanEat canEat2 = new Mouse();

        // Polymorphism shown here.
        // CSharp know the actual type of an object.
        // ==> Tom cat eat ...
        canEat1.Eat();

        // ==> Mouse eat ...
        canEat2.Eat();

        bool isCat = canEat1 is Cat;// true

        Console.WriteLine("catEat1 is Cat? " + isCat);

        // Check 'canEat2' Is the mouse or not?.
        if (canEat2 is Mouse)
        {
            // Cast
            Mouse mouse = (Mouse)canEat2;

            // Call Drink() method (Inherited from CanDrink).
            mouse.Drink();
        }

        Console.ReadLine();
    }
}