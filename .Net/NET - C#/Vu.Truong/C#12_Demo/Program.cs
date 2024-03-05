// See https://aka.ms/new-console-template for more information
using C_12_Demo.Pieces;

Console.Clear();

var interceptorDemo = new InterceptorDemo();
var text = interceptorDemo.GetText("Hello");

Console.WriteLine("123" + text);

ExecRunner.Execute();