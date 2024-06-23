using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace CefSharp.MinimalExample.WinForms
{
    public class DeviceInfo
    {
        public string DeviceID;
        public string ModelName;
        public string PortName;

        static Dictionary<string, string> supportedModels = new Dictionary<string, string>();

        static DeviceInfo()
        {
            supportedModels.Add("VID_04B8&PID_0C0C", "BT-30C");
            supportedModels.Add("VID_04B8&PID_0D12", "BT-40");
        }

        public static List<DeviceInfo> getDetectedDevices()
        {
            List<DeviceInfo> detectedDevices = new List<DeviceInfo>();

            try
            {
                ManagementObjectCollection mgmtObjCol;
                ManagementObjectSearcher mgmtObjSearcher;
                mgmtObjSearcher = new ManagementObjectSearcher("Select * from Win32_SerialPort");
                mgmtObjCol = mgmtObjSearcher.Get();

                foreach (ManagementObject mgmtObj in mgmtObjCol)
                {
                    string portName = mgmtObj["DeviceID"].ToString();
                    string pnpDeviceID = mgmtObj["PNPDeviceID"].ToString();
                    DeviceInfo deviceInfo = DeviceInfo.getDeviceInfo(pnpDeviceID, portName);
                    if (deviceInfo != null)
                    {
                        detectedDevices.Add(deviceInfo);
                    }
                }
            }
            catch (PlatformNotSupportedException ex)
            {
                Console.WriteLine("This functionality is not supported on this platform: " + ex.Message);
                Console.WriteLine("OS Description: " + RuntimeInformation.OSDescription);
                Console.WriteLine("OS Architecture: " + RuntimeInformation.OSArchitecture);
                Console.WriteLine("Framework Description: " + RuntimeInformation.FrameworkDescription);
                Console.WriteLine("Process Architecture: " + RuntimeInformation.ProcessArchitecture);

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Console.WriteLine("Running on Windows");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Console.WriteLine("Running on Linux");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Console.WriteLine("Running on macOS");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
                {
                    Console.WriteLine("Running on FreeBSD");
                }

                string[] portNames = SerialPort.GetPortNames();
                foreach (var portName in portNames)
                {
                    Console.WriteLine("Serial port found: " + portName);
                }
            }
            catch (ManagementException ex)
            {
                Console.WriteLine("An error occurred while querying for WMI data: " + ex.Message);
            }
            return detectedDevices;
        }


        DeviceInfo(string deviceID, string modelName, string portName)
        {
            DeviceID = deviceID;
            ModelName = modelName;
            PortName = portName;
        }

        public static DeviceInfo getDeviceInfo(string deviceID, string portName)
        {
            foreach (string modelDeviceID in supportedModels.Keys)
            {
                if (deviceID.Contains(modelDeviceID))
                {
                    return new DeviceInfo(modelDeviceID, supportedModels[modelDeviceID], portName);
                }
            }
            return null;
        }

        public override int GetHashCode()
        {
            return DeviceID.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DeviceInfo))
                return false;
            return GetHashCode() == ((DeviceInfo)obj).GetHashCode();
        }
    }
}
