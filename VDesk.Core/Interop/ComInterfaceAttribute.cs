namespace VDesk.Core.Interop;

[AttributeUsage(AttributeTargets.Interface)]
internal class ComInterfaceAttribute : Attribute
{
    public string? InterfaceName { get; }

    public ComInterfaceAttribute()
    {
    }

    public ComInterfaceAttribute(string interfaceName)
    {
        this.InterfaceName = interfaceName;
    }
}
