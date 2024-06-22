﻿// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using CefSharp.WinForms;
using Moverio_Windows_App;
using System;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Sensors;

namespace CefSharp.MinimalExample.WinForms
{
    public static class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {

#if ANYCPU
            CefRuntime.SubscribeAnyCpuAssemblyResolver();
#endif

            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };

            //Example of setting a command line argument
            //Enables WebRTC
            // - CEF Doesn't currently support permissions on a per browser basis see https://bitbucket.org/chromiumembedded/cef/issues/2582/allow-run-time-handling-of-media-access
            // - CEF Doesn't currently support displaying a UI for media access permissions
            //
            //NOTE: WebRTC Device Id's aren't persisted as they are in Chrome see https://bitbucket.org/chromiumembedded/cef/issues/2064/persist-webrtc-deviceids-across-restart
            settings.CefCommandLineArgs.Add("enable-media-stream");
            //https://peter.sh/experiments/chromium-command-line-switches/#use-fake-ui-for-media-stream
            settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
            //For screen sharing add (see https://bitbucket.org/chromiumembedded/cef/issues/2582/allow-run-time-handling-of-media-access#comment-58677180)
            settings.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");

            // Allow loading local files
            settings.CefCommandLineArgs.Add("disable-web-security", "1");
            settings.CefCommandLineArgs.Add("allow-file-access-from-files", "1");

            //Perform dependency check to make sure all relevant resources are in our output directory.
            var initialized = Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            if (!initialized)
            {
                MessageBox.Show("Cef.Initialized failed, check the log file for more details.");

                return 0;
            }

            Application.EnableVisualStyles();
            Application.Run(new BrowserForm());

            return 0;
        }
    }
}