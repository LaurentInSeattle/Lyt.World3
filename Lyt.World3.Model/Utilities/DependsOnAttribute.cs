namespace Lyt.World3.Model.Utilities;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed  class DependsOnAttribute : Attribute
{
    public string Property; 

    public DependsOnAttribute(string property)
        => this.Property = property;
}
