using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestNotes.Libraries;
using TestNotes.ViewModels;
using TestNotes.Views;

namespace TestNotes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IView
    {
        private WindowLockHook hook;
        public bool isLocked = false;
        public MainWindow()
        {
            InitializeComponent();
            Topmost = true;
            ((TestNoteViewModel) DataContext).View = this;
            hook = new WindowLockHook(this);

            var activationData = AppDomain.CurrentDomain.SetupInformation.ActivationArguments?.ActivationData;
            if (!string.IsNullOrWhiteSpace(activationData?[0]))
            {
                var uri = new Uri(activationData[0]);
                ((TestNoteViewModel) DataContext).LoadTestNote(uri.LocalPath);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            AppBarFunctions.SetAppBar(this, ABEdge.None);
            base.OnClosing(e);
        }

        public void LockWindow(bool lockWindow)
        {
            //isLocked = lockWindow;
            if (lockWindow)
            {
                ResizeMode = ResizeMode.NoResize;
            }
            else
            {
                ResizeMode = ResizeMode.CanResize;
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window) sender;
            window.Topmost = true;
        }
    }

    public class WindowLockHook
    {
        private readonly MainWindow Window;

        public WindowLockHook(MainWindow window)
        {
            Window = window;

            var source = PresentationSource.FromVisual(window) as HwndSource;
            if (source == null)
            {
                // If there is no hWnd, we need to wait until there is
                window.SourceInitialized += Window_SourceInitialized;
            }
            else
            {
                source.AddHook(WndProc);
            }
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            var source = (HwndSource) PresentationSource.FromVisual(Window);
            source.AddHook(WndProc);
        }

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == NativeMethods.WM_WINDOWPOSCHANGING && Window.isLocked)
            {
                var wp = Marshal.PtrToStructure<NativeMethods.WINDOWPOS>(lParam);
                wp.flags |= NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE;
                Marshal.StructureToPtr(wp, lParam, false);
            }

            return IntPtr.Zero;
        }
    }

    internal static class NativeMethods
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x;
            public int y;
            public int cx;
            public int cy;
            public int flags;
        }

        public const int
            SWP_NOMOVE = 0x0002,
            SWP_NOSIZE = 0x0001;

        public const int
            WM_WINDOWPOSCHANGING = 0x0046;
    }
}
