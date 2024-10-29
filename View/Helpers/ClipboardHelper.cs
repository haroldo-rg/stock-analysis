using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Classe utilitária para copiar os dados para a área de transferência
/// Utiliza as APIs do Windows
/// Necessária pois o Clipboard é nativo de aplicações Windows Forms
/// </summary>
public class ClipboardHelper
{
    [DllImport("user32.dll")]
    private static extern IntPtr OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll")]
    private static extern bool CloseClipboard();

    [DllImport("user32.dll")]
    private static extern bool EmptyClipboard();

    [DllImport("user32.dll")]
    private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GlobalAlloc(uint uFlags, uint dwBytes);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern bool GlobalUnlock(IntPtr hMem);

    [DllImport("kernel32.dll")]
    private static extern uint GlobalSize(IntPtr hMem);

    private const uint CF_TEXT = 1;
    private const uint GMEM_MOVEABLE = 0x0002;

    public static void CopyToClipboard(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        // Open the clipboard
        OpenClipboard(IntPtr.Zero);

        // Clear the clipboard
        EmptyClipboard();

        // Allocate global memory for the text
        IntPtr hGlobal = GlobalAlloc(GMEM_MOVEABLE, (uint)(text.Length + 1));
        IntPtr lpGlobal = GlobalLock(hGlobal);

        // Copy the text to the global memory
        Marshal.Copy(Encoding.Latin1.GetBytes(text), 0, lpGlobal, text.Length);
        Marshal.WriteByte(lpGlobal, text.Length, 0); // Null-terminate the string

        // Unlock the global memory
        GlobalUnlock(hGlobal);

        // Set the clipboard data
        SetClipboardData(CF_TEXT, hGlobal);

        // Close the clipboard
        CloseClipboard();
    }
}
