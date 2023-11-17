using System.Reflection;
using System.Runtime.CompilerServices;

namespace VDesk.Core.Interop;

public class ComBaseObject<TInterface>
{
    private readonly Dictionary<string, MethodInfo> _methods = new();

    public Type ComInterfaceType => typeof(TInterface);

    public object ComObject { get; }

    private protected ComBaseObject()
    {
        var instance = CreateInstance(typeof(TInterface), null);
        ComObject = instance;
    }


    private protected ComBaseObject(Guid clsid)
    {
        var instance = CreateInstance(typeof(TInterface), clsid);

        this.ComObject = instance;
    }

    private protected ComBaseObject(object comObject)
    {
        this.ComObject = comObject;
    }

    protected static object?[] Args(params object?[] args)
        => args;

    protected void InvokeMethod(object?[]? parameters = null, [CallerMemberName] string methodName = "")
        => this.InvokeMethod<object>(parameters, methodName);

    protected T? InvokeMethod<T>(object?[]? parameters = null, [CallerMemberName] string methodName = "")
    {
        if (this._methods.TryGetValue(methodName, out var methodInfo)
            || (methodInfo = this.ComInterfaceType.GetMethod(methodName)) != null)
        {
            this._methods[methodName] = methodInfo;
        }
        else
            throw new NotSupportedException(
                $"Method '{methodName}' is not supported in COM interface '{typeof(TInterface).Name}'.");

        try
        {
            return (T?)methodInfo.Invoke(this.ComObject, parameters);
        }
        catch (TargetInvocationException ex) when (ex.InnerException != null)
        {
            throw ex.InnerException;
        }
    }

    private static object CreateInstance(Type type, Guid? guidService)
    {
        var shellType = Type.GetTypeFromCLSID(CLSID.ImmersiveShell)
                        ?? throw new Exception($"Type of ImmersiveShell ('{CLSID.ImmersiveShell}') is not found.");
        var shell = Activator.CreateInstance(shellType) as IServiceProvider
                    ?? throw new Exception("Failed to create an instance of ImmersiveShell.");

        try
        {
            return shell.QueryService(guidService ?? type.GUID, type.GUID);
        }
        catch (Exception e)
        {
            Console.WriteLine($"{type} - {e.Message}");
            throw;
        }
    }
}