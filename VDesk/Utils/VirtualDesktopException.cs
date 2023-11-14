namespace VDesk.Utils
{
    public class VirtualDesktopException : Exception
    {
        public VirtualDesktopException(Exception exception)
        : base("An error occurs with Virtual Desktop", exception)
        {
            
        }

    }
}