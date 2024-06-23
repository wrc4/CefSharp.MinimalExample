using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CefSharp.MinimalExample.WinForms
{
    internal class MyKeyboardHandler : IKeyboardHandler
    {
        private BrowserForm browserForm;

        // Constructor to accept a MainForm reference
        public MyKeyboardHandler(BrowserForm form)
        {
            this.browserForm = form;
        }

        public bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            // You can handle specific pre-key events here
            return false; // Return false to continue processing the event
        }

        public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            // Check if F11 is pressed to toggle fullscreen
            if (type == KeyType.KeyUp && windowsKeyCode == (int)Keys.F11)
            {
                // Assuming there's a method in the main form to toggle fullscreen
                //((BrowserForm)chromiumWebBrowser).ToggleFullScreen();
                //BrowserForm browserForm = chromiumWebBrowser.get.FindForm() as BrowserForm;
                browserForm?.ToggleFullScreen();
                return true; // Return true if the event is handled
            }
            return false;
        }
    }
}
