using System;
using System.Collections.Generic;
using System.Text;

namespace CefSharp.MinimalExample.WinForms
{
    public class CommandInfo
    {
        public const string CMD_GET2D3D = "get2d3d";
        public const string CMD_SET2D3D = "set2d3d";
        public const string CMD_GETBRIGHT = "getbright";
        public const string CMD_SETBRIGHT = "setbright";
        public const string CMD_GETDISTANCE = "getdisplaydistance";
        public const string CMD_SETDISTANCE = "setdisplaydistance";

        public const string VAL_2D = "0";
        public const string VAL_3D = "1";
        public const int VAL_MID_BRIGHT = 11;
        public const int VAL_MAX_BRIGHT = 20;
        public const int VAL_AUTO_BRIGHT = 50;

        public string Command;
        public string Result;

        public CommandInfo(string command, string result)
        {
            Command = command;
            Result = result;
        }
    }
}
