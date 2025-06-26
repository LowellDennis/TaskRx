using System.Runtime.InteropServices;

namespace TaskRx.Utilities
{
    /// <summary>
    /// Helper class for displaying message boxes that are centered on the parent form or screen
    /// </summary>
    public static class MessageBoxHelper
    {
        // Import necessary Windows API functions
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Constants for SetWindowPos
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOACTIVATE = 0x0010;

        // Message box hook to center it on parent
        private class MessageBoxCenteringHook : IDisposable
        {
            private IntPtr _parentHandle;
            private IntPtr _hookHandle = IntPtr.Zero;
            private HookProc _hookProc;

            // Delegate for the hook procedure
            private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

            // Import necessary Windows API functions for hooks
            [DllImport("user32.dll")]
            private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, uint threadId);

            [DllImport("user32.dll")]
            private static extern bool UnhookWindowsHookEx(IntPtr hhk);

            [DllImport("user32.dll")]
            private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

            [DllImport("kernel32.dll")]
            private static extern uint GetCurrentThreadId();

            // Constants for hooks
            private const int WH_CBT = 5;
            private const int HCBT_ACTIVATE = 5;

            public MessageBoxCenteringHook(IntPtr parentHandle)
            {
                _parentHandle = parentHandle;
                _hookProc = new HookProc(HookCallback);
                _hookHandle = SetWindowsHookEx(WH_CBT, _hookProc, IntPtr.Zero, GetCurrentThreadId());
            }

            private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
            {
                if (nCode == HCBT_ACTIVATE)
                {
                    // Get the message box window handle
                    IntPtr msgBoxHandle = wParam;

                    // Center the message box on the parent window
                    CenterWindowOnParent(msgBoxHandle, _parentHandle);

                    // Unhook after centering
                    UnhookWindowsHookEx(_hookHandle);
                    _hookHandle = IntPtr.Zero;
                }

                return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }

            private void CenterWindowOnParent(IntPtr childHandle, IntPtr parentHandle)
            {
                // Get parent window dimensions
                RECT parentRect;
                GetWindowRect(parentHandle, out parentRect);
                int parentWidth = parentRect.Right - parentRect.Left;
                int parentHeight = parentRect.Bottom - parentRect.Top;
                int parentCenterX = parentRect.Left + (parentWidth / 2);
                int parentCenterY = parentRect.Top + (parentHeight / 2);

                // Get child window dimensions
                RECT childRect;
                GetWindowRect(childHandle, out childRect);
                int childWidth = childRect.Right - childRect.Left;
                int childHeight = childRect.Bottom - childRect.Top;

                // Calculate new position
                int newX = parentCenterX - (childWidth / 2);
                int newY = parentCenterY - (childHeight / 2);

                // Move the window
                SetWindowPos(childHandle, IntPtr.Zero, newX, newY, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);
            }

            public void Dispose()
            {
                if (_hookHandle != IntPtr.Zero)
                {
                    UnhookWindowsHookEx(_hookHandle);
                    _hookHandle = IntPtr.Zero;
                }
            }
        }

        /// <summary>
        /// Shows a message box that is centered on the specified owner form
        /// </summary>
        public static DialogResult ShowCentered(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            IntPtr ownerHandle = owner?.Handle ?? GetActiveWindow();

            // Use a hook to center the message box on its parent
            using (var hook = new MessageBoxCenteringHook(ownerHandle))
            {
                // Show the message box with the owner
                return MessageBox.Show(owner, text, caption, buttons, icon);
            }
        }

        /// <summary>
        /// Shows a message box that is centered on the screen
        /// </summary>
        public static DialogResult ShowCentered(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            // Get the active window as the parent
            IntPtr activeWindow = GetActiveWindow();

            if (activeWindow != IntPtr.Zero)
            {
                // Use a hook to center the message box on the active window
                using (var hook = new MessageBoxCenteringHook(activeWindow))
                {
                    return MessageBox.Show(null, text, caption, buttons, icon);
                }
            }
            else
            {
                // If no active window, just show the message box normally
                return MessageBox.Show(text, caption, buttons, icon);
            }
        }
    }
}