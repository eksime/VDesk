namespace VDesk.Core;

public interface IVirtualDesktopProvider
{
    Guid[] GetDesktop();
    Guid Create();
    void MoveToDesktop(IntPtr hWnd, Guid virtualDesktopId);
    void Switch(Guid virtualDesktopId);
}