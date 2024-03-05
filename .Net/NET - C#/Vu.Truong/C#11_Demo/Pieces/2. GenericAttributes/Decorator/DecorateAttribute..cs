namespace C_11_Demo.Pieces.Decorator;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
internal class DecorateAttribute<T> : Attribute where T : IAction
{
}