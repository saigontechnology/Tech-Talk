using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ExampleCSharp
{
    public class StringCSharp
    {
         string oldPath = "c:\\Program Files\\Microsoft Visual Studio \"8.0\"";

        // Initialize with a verbatim string literal.
         string newPath = @"c:\Program Files\Microsoft Visual Studio ""9.0""";

        public void FormatString()
        {
            string name = "Ed Sheeran";
            string food = "Apple";

            string strFormat = String.Format("{0:C} eats {1}", name, food);
            Console.WriteLine(strFormat);

            string strFormat1 = String.Format("{0, 20}", "Programiz");
            Console.WriteLine(strFormat1);

            // Number formatting  
            int num = 302;
            string numStr = String.Format("Number {0, 0:D5}", num);
            Console.WriteLine(numStr);

            // Decimal formatting  
            decimal money = 99.95m;
            string moneyStr = String.Format("Money {0, 0:C2}", money);
            Console.WriteLine(moneyStr);

            // DateTime formatting  
            DateTime now = DateTime.Now;
            string dtStr = String.Format("{0:d} at {0:t}", now);
            Console.WriteLine(dtStr);
        }

        enum MyEnum
        {
            a = 1 
        }

        public void StringInterpolation()
        {
            string name = "Huy";
            string location = "Ho Chi Minh";
            //FormattableString
            DateTime date = DateTime.Now;
            string msg = $@"Hi {name}, today is {date.DayOfWeek}.
                            Welcome to {location}";
            Console.WriteLine(msg);
        }

        public void StringWithTernaryOperator()
        {
            string name = "Huy";
            //DateTime date = DateTime.Now;
            string msg = $@"Hi, Welcome {(name == "Huy" ? "Admin" : "Guest")}";
            Console.WriteLine(msg);
        }

        public void StringInterpolationWithFunc()
        {
            double a = 5.2;
            double b = 10.5;
            string msg = $"Tjis is a result {Double(a, b)}";

            double Double(double a, double b)
            {
                return a * b;
            }
        }

        public void StringWithFormatStandard()
        {

            DateTime date = DateTime.Now;
            string msg = $"Hi, Today is: {date:dddd/MMmmm/yyyy / HH/ss}";
            Console.WriteLine(msg);

            // Currency
            var decimalValue = 120.23456;
            var asCurrency = $"It costs {decimalValue:C}";
            Console.WriteLine(msg);

            var asCurrencyC3 = $"It costs {decimalValue:C3}";
            Console.WriteLine(msg);
        }

        public void StringWithCommonMethod()
        {
            string str1 = "Welcome, to-C|Sharp";
            var str2 = str1.Split(new char[] { ',', '-', '|' }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < str2.Length; j++)
            {
                Console.WriteLine(str2[j]);
            }

            Console.WriteLine("SubString: {0}", str1.Substring(7));
            Console.WriteLine("Substring with Length: {0}", str1.Substring(3, 7));

            string s = new String('a', 3);
            Console.WriteLine("The initial string: '{0}'", s);
            s = s.Replace('a', 'b').Replace('b', 'c').Replace('c', 'd');
            Console.WriteLine("The final string: '{0}'", s);

            string text = "Saigon Technology Lantern Your Way";
            string searchText = "Technology";
            bool result = text.Contains(searchText);
            Console.WriteLine("The string '{0}' contains '{1}'? -> '{2}'", text, searchText, result); // True


            string[] s1 = { "Saigon", "Technology", "Lantern", "Your", "Way" };

            var strConcat = string.Concat(s1);
            Console.WriteLine(strConcat);

            var strJoin = string.Join(",", s1);
            var strJoin1 = string.Join(",", s1, 1,3);
            Console.WriteLine(strJoin);
            Console.WriteLine(strJoin1);

            Console.ReadLine();

        }

        public void CompareStringAndStringBuilder()
        {
            ObjectIDGenerator idGenerator1 = new ObjectIDGenerator();
            bool flag = new bool();

            Stopwatch s1 = Stopwatch.StartNew();
            String str = "Sun";
            Console.WriteLine("String = {0}", str);
            Console.WriteLine("Instance Id : {0}", idGenerator1.GetId(str, out flag));
            //flag will be True for new instance otherwise it will be False
            Console.WriteLine("This instance is new : {0}\n", flag);
            //concatenating strings
            str += " rises";
            Console.WriteLine("String = {0}", str);
            Console.WriteLine("Instance Id : {0}", idGenerator1.GetId(str, out flag));
            Console.WriteLine("this instance is new : {0}\n", flag);
            str += " in";
            Console.WriteLine("String = {0}", str);
            Console.WriteLine("Instance Id : {0}", idGenerator1.GetId(str, out flag));
            Console.WriteLine("this instance is new : {0}\n", flag);
            str += " the";
            Console.WriteLine("String = {0}", str);
            Console.WriteLine("Instance Id : {0}", idGenerator1.GetId(str, out flag));
            Console.WriteLine("this instance is new : {0}\n", flag);
            str += " east";
            Console.WriteLine("String = {0}", str);
            Console.WriteLine("Instance Id : {0}", idGenerator1.GetId(str, out flag));
            Console.WriteLine("this instance is new : {0}\n", flag);

            s1.Stop();
            Console.WriteLine($"Time taken in string concatenation: {s1.ElapsedMilliseconds} MS \n");

            Console.WriteLine($"------------------------ \n");
            //initializing string using StringBuilder
            Stopwatch s2 = Stopwatch.StartNew();
            StringBuilder stringBuilder = new StringBuilder("Sun");
            ObjectIDGenerator idGenerator2 = new ObjectIDGenerator();
            Console.WriteLine("StringBuilder = {0}", stringBuilder);
            Console.WriteLine("Instance Id : {0}", idGenerator2.GetId(stringBuilder, out flag));
            Console.WriteLine("This instance is new : {0}\n", flag);
            stringBuilder.Append(" rises");
            Console.WriteLine("StringBuilder = {0}", stringBuilder);
            Console.WriteLine("Instance Id : {0}", idGenerator2.GetId(stringBuilder, out flag));
            Console.WriteLine("This instance is new : {0}\n", flag);
            stringBuilder.Append(" in");
            Console.WriteLine("StringBuilder = {0}", stringBuilder);
            Console.WriteLine("Instance Id : {0}", idGenerator2.GetId(stringBuilder, out flag));
            Console.WriteLine("This instance is new : {0}\n", flag);
            stringBuilder.Append(" the");
            Console.WriteLine("StringBuilder = {0}", stringBuilder);
            Console.WriteLine("Instance Id : {0}", idGenerator2.GetId(stringBuilder, out flag));
            Console.WriteLine("This instance is new : {0}\n", flag);
            stringBuilder.Append(" east");
            Console.WriteLine("StringBuilder = {0}", stringBuilder);
            Console.WriteLine("Instance Id : {0}", idGenerator2.GetId(stringBuilder, out flag));
            Console.WriteLine("This instance is new : {0}\n", flag);
            s2.Stop();
            Console.WriteLine($"Time taken in StringBuilder concatenation: {s2.ElapsedMilliseconds} MS");
        }

    }
}
