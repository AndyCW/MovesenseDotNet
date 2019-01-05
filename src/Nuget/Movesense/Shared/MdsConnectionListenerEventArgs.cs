using System;

namespace MdsLibrary
{
    /// <summary>
    /// Event args for MdsConnectionListener events, for reporting device connection/discopnnection to/from Mds Whiteboard
    /// </summary>
    public class MdsConnectionListenerEventArgs : EventArgs
    {
        /// <summary>
        /// Serial number of the device
        /// </summary>
        public string Serial { get; set; }

        /// <summary>
        /// Unique ID of the device
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// Create event args for reporting connection events
        /// </summary>
        /// <param name="serial">Serial number of the device</param>
        /// <param name="uuid">Unique ID of the device</param>
        public MdsConnectionListenerEventArgs(string serial, Guid uuid)
        {
            Serial = serial;
            Uuid = uuid;
        }
    }
}
