using System.Diagnostics;
using WindowsDesktop.Interop;
using WindowsDesktop.Interop.Proxy;

namespace WindowsDesktop;

/// <summary>
/// Encapsulates Windows 11 (and Windows 10) virtual desktops.
/// </summary>
[DebuggerDisplay("{Name} ({Id})")]
public partial class VirtualDesktop
{
    #region instance members

    /// <summary>
    /// Gets the unique identifier for this virtual desktop.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets or sets the name of this virtual desktop.
    /// </summary>
    /// <remarks>
    /// This is not supported on Windows 10.
    /// </remarks>
    public string Name
    {
        get => _name;
        set
        {
            //Provider.VirtualDesktopManagerInternal.SetDesktopName(_source, value);
            _name = value;
        }
    }

    /// <summary>
    /// Switches to this virtual desktop.
    /// </summary>
    public void Switch()
    {
        //Provider.VirtualDesktopManagerInternal.SwitchDesktop(_source);
    }

    /// <summary>
    /// Removes this virtual desktop and switches to an available one.
    /// </summary>
    /// <remarks>If this is the last virtual desktop, a new one will be created to switch to.</remarks>
    // public void Remove()
    //     => Remove(GetRight() ?? GetLeft() ?? Create());

    /// <summary>
    /// Removes this virtual desktop and switches to <paramref name="fallbackDesktop" />.
    /// </summary>
    /// <param name="fallbackDesktop">A virtual desktop to be displayed after the virtual desktop is removed.</param>
    public void Remove(VirtualDesktop fallbackDesktop)
    {
        if (fallbackDesktop == null) throw new ArgumentNullException(nameof(fallbackDesktop));

        //Provider.VirtualDesktopManagerInternal.RemoveDesktop(_source, fallbackDesktop._source);
    }

    /// <summary>
    /// Returns the adjacent virtual desktop on the left, or null if there isn't one.
    /// </summary>
    // public VirtualDesktop? GetLeft()
    // {
    //     return SafeInvoke(
    //         () => Provider.VirtualDesktopManagerInternal
    //             .GetAdjacentDesktop(_source, AdjacentDesktop.LeftDirection)
    //             .ToVirtualDesktop(),
    //         HResult.TYPE_E_OUTOFBOUNDS);
    // }

    /// <summary>
    /// Returns the adjacent virtual desktop on the right, or null if there isn't one.
    /// </summary>
    // public VirtualDesktop? GetRight()
    // {
    //     return SafeInvoke(
    //         () => Provider.VirtualDesktopManagerInternal
    //             .GetAdjacentDesktop(_source, AdjacentDesktop.RightDirection)
    //             .ToVirtualDesktop(),
    //         HResult.TYPE_E_OUTOFBOUNDS);
    // }

    public override string ToString()
        => $"VirtualDesktopCore {Id} '{_name}'";

    #endregion

    #region static members (get or create)
    

    #endregion

    #region static members (others)

    #endregion
}
