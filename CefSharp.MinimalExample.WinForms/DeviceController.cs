using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CefSharp.MinimalExample.WinForms
{
    public class DeviceController
    {
        RJCP.IO.Ports.SerialPortStream _port = null;

        private DeviceInfo _deviceInfo = null;

        public event EventHandler<CommandInfo> OnCommandResult;

        public void Init(DeviceInfo deviceInfo)
        {
            _deviceInfo = deviceInfo;
            connect();
        }

        public void Destroy()
        {
            closePort();
        }

        public void Get2D3D()
        {
            if (_port != null)
                _port.WriteLine(CommandInfo.CMD_GET2D3D);
        }

        public void Set2D3D(string val)
        {
            if (_port != null)
                _port.WriteLine(CommandInfo.CMD_SET2D3D + " " + val);
        }

        public void GetBright()
        {
            if (_port != null)
                _port.WriteLine(CommandInfo.CMD_GETBRIGHT);
        }

        public void SetBright(string val)
        {
            if (_port != null)
                _port.WriteLine(CommandInfo.CMD_SETBRIGHT + " " + val);
        }

        public void GetDistance()
        {
            if (_port != null)
                _port.WriteLine(CommandInfo.CMD_GETDISTANCE);
        }

        public void SetDistance(string val)
        {
            if (_port != null)
                _port.WriteLine(CommandInfo.CMD_SETDISTANCE + " " + val);
        }

        private void connect()
        {
            if (_deviceInfo == null)
                return;

            _port = new RJCP.IO.Ports.SerialPortStream(_deviceInfo.PortName);
            _port.DataReceived += _port_DataReceived;
            _port.OpenDirect();
            if (_port.IsOpen)
            {
                Get2D3D();
            }
        }

        private void closePort()
        {
            if (_port != null)
            {
                _port.DataReceived -= _port_DataReceived;
                if (_port.IsOpen)
                {
                    _port.Close();
                }
            }
            _port = null;
        }

        private void _port_DataReceived(object sender, RJCP.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string result = _port.ReadExisting();
            handleCommandResult(result);
        }

        private string getCommandValue(string result, string command)
        {
            string command_crlf = command + "\r\n";
            if (result.Contains(command_crlf))
            {
                string val_start = result.Substring(result.IndexOf(command_crlf) + command_crlf.Length);
                string val = val_start.Remove(val_start.IndexOf("\r\n\r\n"));
                //while (true)
                //{
                //    val2 = val.TrimEnd('\r', '\n');
                //    if (val2.Length < val.Length)
                //    {
                //        val = val2;
                //    }
                //    else
                //        break;
                //}
                return val;
            }

            return "";
        }

        private void handleCommandResult(string result)
        {
            Debug.WriteLine(" ---- handle: " + result);

            string[] commandsToTest = { CommandInfo.CMD_GET2D3D, CommandInfo.CMD_GETBRIGHT, CommandInfo.CMD_GETDISTANCE };

            foreach (var command in commandsToTest)
            {
                string testResult = getCommandValue(result, command);
                if (testResult.Length > 0)
                {
                    OnCommandResult?.Invoke(this, new CommandInfo(command, testResult));
                }
            }
        }

    }
}
