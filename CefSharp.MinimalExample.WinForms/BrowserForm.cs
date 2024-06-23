// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.DevTools.IO;
using CefSharp.MinimalExample.WinForms.Controls;
using CefSharp.WinForms;
using Moverio_Windows_App;
using System;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Sensors;

namespace CefSharp.MinimalExample.WinForms
{
    public partial class BrowserForm : Form
    {
#if DEBUG
        private const string Build = "Debug";
#else
        private const string Build = "Release";
#endif
        private readonly string title = "CefSharp.MinimalExample.WinForms (" + Build + ")";
        private readonly ChromiumWebBrowser browser;

        private static readonly AggregatedDeviceOrientationWatcher watcher = new AggregatedDeviceOrientationWatcher();
        private static OrientationSensor sensor;

        // Euler angles in degrees, convert Euler angles to radians
        static float radX = DegreesToRadians(-90.0f);
        static float radY = DegreesToRadians(0.0f);
        static float radZ = DegreesToRadians(-90.0f);

        // Create quaternion from Euler angles
        static Quaternion rotation = Quaternion.CreateFromYawPitchRoll(radY, radX, radZ);

        private static ChromiumWebBrowser TargetWebBrowser = null;

        public BrowserForm()
        {
            InitializeComponent();

            Text = title;
            WindowState = FormWindowState.Maximized;

            browser = new ChromiumWebBrowser()
            {
                Dock = DockStyle.Fill // Ensure the browser fills the form
            };// "www.google.com");
            browser.KeyboardHandler = new MyKeyboardHandler(this);
            toolStripContainer.ContentPanel.Controls.Add(browser);

            // Load local HTML file
            //string filePath = "file://" + Application.StartupPath + @"\html\index.html";
            string filePath = "file:///D:/dev/MWXR_sample/index.html";
            browser.Load(filePath);

            browser.IsBrowserInitializedChanged += OnIsBrowserInitializedChanged;
            browser.LoadingStateChanged += OnLoadingStateChanged;
            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            browser.AddressChanged += OnBrowserAddressChanged;
            browser.LoadError += OnBrowserLoadError;
            browser.Disposed += Browser_Disposed;

            //Wait for the MainFrame to finish loading
            browser.FrameLoadEnd += (sender, args) =>
            {
                //Wait for the MainFrame to finish loading
                //if (args.Frame.IsMain)
                //{
                //    args.Frame.ExecuteJavaScriptAsync("alert('MainFrame finished loading');");
                //}
                TargetWebBrowser = browser;
            };

            var version = string.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}",
               Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion);

#if NETCOREAPP
            // .NET Core
            var environment = string.Format("Environment: {0}, Runtime: {1}",
                System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant(),
                System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);
#else
            // .NET Framework
            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            var environment = String.Format("Environment: {0}", bitness);
#endif

            DisplayOutput(string.Format("{0}, {1}", version, environment));

            GoFullScreen();
        }

        private void Browser_Disposed(object sender, EventArgs e)
        {
            // TargetWebBrowser = null;
            // CleanupDeviceWatcher();
        }

        private void InitializeDeviceWatcher()
        {
            try
            {
                watcher.Add += Watcher_AddAsync;
                watcher.Remove += Watcher_Remove;
                watcher.Start();
            }
            catch (Exception ex)
            {
                String s = ex.Message;
                CleanupDeviceWatcher();
            }
        }

        private void CleanupDeviceWatcher()
        {
            watcher.Stop();
            watcher.Add -= Watcher_AddAsync;
            watcher.Remove -= Watcher_Remove;
        }

        private void OnBrowserLoadError(object sender, LoadErrorEventArgs e)
        {
            //Actions that trigger a download will raise an aborted error.
            //Aborted is generally safe to ignore
            if (e.ErrorCode == CefErrorCode.Aborted)
            {
                return;
            }

            var errorHtml = string.Format("<html><body><h2>Failed to load URL {0} with error {1} ({2}).</h2></body></html>",
                                              e.FailedUrl, e.ErrorText, e.ErrorCode);

            _ = e.Browser.SetMainFrameDocumentContentAsync(errorHtml);

            //AddressChanged isn't called for failed Urls so we need to manually update the Url TextBox
            //this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = e.FailedUrl);
        }

        private void OnIsBrowserInitializedChanged(object sender, EventArgs e)
        {
            InitializeDeviceWatcher();

            var b = ((ChromiumWebBrowser)sender);

            this.InvokeOnUiThreadIfRequired(() => b.Focus());
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args)
        {
            DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
        }

        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            SetCanGoBack(args.CanGoBack);
            SetCanGoForward(args.CanGoForward);

            this.InvokeOnUiThreadIfRequired(() => SetIsLoading(!args.CanReload));
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args)
        {
            this.InvokeOnUiThreadIfRequired(() => Text = title + " - " + args.Title);
        }

        private void OnBrowserAddressChanged(object sender, AddressChangedEventArgs args)
        {
            //this.InvokeOnUiThreadIfRequired(() => urlTextBox.Text = args.Address);
        }

        private void SetCanGoBack(bool canGoBack)
        {
            //this.InvokeOnUiThreadIfRequired(() => backButton.Enabled = canGoBack);
        }

        private void SetCanGoForward(bool canGoForward)
        {
            //this.InvokeOnUiThreadIfRequired(() => forwardButton.Enabled = canGoForward);
        }

        private void SetIsLoading(bool isLoading)
        {
            //goButton.Text = isLoading ?
            //    "Stop" :
            //    "Go";
            //goButton.Image = isLoading ?
            //    Properties.Resources.nav_plain_red :
            //    Properties.Resources.nav_plain_green;

            HandleToolStripLayout();
        }

        public void DisplayOutput(string output)
        {
            this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
        }

        private void HandleToolStripLayout(object sender, LayoutEventArgs e)
        {
            HandleToolStripLayout();
        }

        private void HandleToolStripLayout()
        {
            /*
            var width = toolStrip1.Width;
            foreach (ToolStripItem item in toolStrip1.Items)
            {
                if (item != urlTextBox)
                {
                    width -= item.Width - item.Margin.Horizontal;
                }
            }
            urlTextBox.Width = Math.Max(0, width - urlTextBox.Margin.Horizontal - 18);*/
        }

        private void ExitMenuItemClick(object sender, EventArgs e)
        {
            TargetWebBrowser = null;
            CleanupDeviceWatcher();

            browser.Dispose();
            Cef.Shutdown();
            Close();
        }

        private void GoButtonClick(object sender, EventArgs e)
        {
            //LoadUrl(urlTextBox.Text);
        }

        private void BackButtonClick(object sender, EventArgs e)
        {
            browser.Back();
        }

        private void ForwardButtonClick(object sender, EventArgs e)
        {
            browser.Forward();
        }

        private void UrlTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            LoadUrl(urlTextBox.Text);*/
        }

        private void LoadUrl(string urlString)
        {
            // No action unless the user types in some sort of url
            if (string.IsNullOrEmpty(urlString))
            {
                return;
            }

            Uri url;

            var success = Uri.TryCreate(urlString, UriKind.RelativeOrAbsolute, out url);

            // Basic parsing was a success, now we need to perform additional checks
            if (success)
            {
                // Load absolute urls directly.
                // You may wish to validate the scheme is http/https
                // e.g. url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps
                if (url.IsAbsoluteUri)
                {
                    browser.LoadUrl(urlString);

                    return;
                }

                // Relative Url
                // We'll do some additional checks to see if we can load the Url
                // or if we pass the url off to the search engine
                var hostNameType = Uri.CheckHostName(urlString);

                if (hostNameType == UriHostNameType.IPv4 || hostNameType == UriHostNameType.IPv6)
                {
                    browser.LoadUrl(urlString);

                    return;
                }

                if (hostNameType == UriHostNameType.Dns)
                {
                    try
                    {
                        var hostEntry = Dns.GetHostEntry(urlString);
                        if (hostEntry.AddressList.Length > 0)
                        {
                            browser.LoadUrl(urlString);

                            return;
                        }
                    }
                    catch (Exception)
                    {
                        // Failed to resolve the host
                    }
                }
            }

            // Failed parsing load urlString is a search engine
            var searchUrl = "https://www.google.com/search?q=" + Uri.EscapeDataString(urlString);

            browser.LoadUrl(searchUrl);
        }

        private void ShowDevToolsMenuItemClick(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

        public void ToggleFullScreen()
        {
            if (this.FormBorderStyle == FormBorderStyle.None)
            {
                this.InvokeOnUiThreadIfRequired(() =>
                {
                    // Switch to windowed mode
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Normal;
                    this.TopMost = false;
                });
            }
            else
            {
                this.InvokeOnUiThreadIfRequired(() =>
                {
                    GoFullScreen();
                });
            }
        }

        private void GoFullScreen()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        #region MoverioWatcher
        private static async void Watcher_AddAsync(object sender, string deviceId)
        {
            await EnableAsync(deviceId).ConfigureAwait(false);
        }

        private static void Watcher_Remove(object sender, string deviceId)
        {
            Disable();
        }

        private static async Task EnableAsync(string deviceId)
        {
            if (sensor != null)
            {
                Disable();
            }

            sensor = await OrientationSensor.FromIdAsync(deviceId);
            sensor.ReadingChanged += Sensor_ReadingChangedAsync;
        }

        private static void Disable()
        {
            if (sensor == null)
            {
                return;
            }

            sensor.ReadingChanged -= Sensor_ReadingChangedAsync;
            sensor = null;
        }

        private static void Sensor_ReadingChangedAsync(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
        {
            // Your original quaternion to rotate
            // This is an example, replace with your actual quaternion
            Quaternion readingQuaternion = new Quaternion(args.Reading.Quaternion.X, args.Reading.Quaternion.Y, args.Reading.Quaternion.Z, args.Reading.Quaternion.W);

            // Rotate the original quaternion by the rotation quaternion
            Quaternion rotatedQuaternion = Quaternion.Multiply(rotation, readingQuaternion);

            // Normalize the resulting quaternion
            // rotatedQuaternion = Quaternion.Normalize(rotatedQuaternion);

            if (TargetWebBrowser != null)
            {
                // Format: {type:"MWXR",c:"<command>", p:{"x":0,"y":0,"z":0,"w":0}}. where command can be: 1: Reset position (sensor covered) 
                String message = String.Format("{{\"x\":{0},\"y\":{1},\"z\":{2},\"w\":{3}}}", rotatedQuaternion.X, rotatedQuaternion.Y, rotatedQuaternion.Z, rotatedQuaternion.W);
                String scriptText = "window.postMessage({\"type\":\"MWXR\",\"q\":" + message + "}, '*');";
                TargetWebBrowser.ExecuteScriptAsync(scriptText);
            }

            // WriteMessage(Console.OpenStandardOutput(), String.Format("{{\"x\":{0},\"y\":{1},\"z\":{2},\"w\":{3}}}", rotatedQuaternion.X, rotatedQuaternion.Y, rotatedQuaternion.Z, rotatedQuaternion.W));
        }


        private static float DegreesToRadians(float degrees)
        {
            return (float)(degrees * Math.PI / 180.0);
        }
        #endregion

    }
}
