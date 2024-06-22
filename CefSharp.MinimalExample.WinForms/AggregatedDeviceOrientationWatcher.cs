using System;
using System.Collections.Generic;
using Windows.Devices.Sensors;

namespace Moverio_Windows_App
{
    internal class AggregatedDeviceOrientationWatcher : MoverioWatcher
    {
        public AggregatedDeviceOrientationWatcher() : base(OrientationSensor.GetDeviceSelector(SensorReadingType.Absolute))
        {
        }
    }
}
