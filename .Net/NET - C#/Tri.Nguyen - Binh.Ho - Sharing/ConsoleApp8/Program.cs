
static void IncreaseValue(int value)
{

    value++;

}

//static void IncreaseValue(ref int value)
//{

//    value++;

//}

//static void IncreaseValue(out int value)
//{

//    value = 0;

//    value++;

//}

static void Main(string[] args)
{

    int value = 5;



    Console.WriteLine("Value before increase: {0}", value);



    IncreaseValue(value);
    //IncreaseValue(ref value);
    //IncreaseValue(out value);

    Console.WriteLine("Value after increase: {0}", value);



    Console.ReadKey();

}


