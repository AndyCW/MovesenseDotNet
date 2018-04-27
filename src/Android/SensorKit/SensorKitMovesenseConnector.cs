using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCLStorage;
using System.IO;
using System.Numerics;
using Plugin.BLE.Abstractions.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SensorKitSDK
{
    public class SensorKitMovesenseConnector : ViewModel, IConnector
    {
        SensorModel Data;
        IDevice _device;
        bool isUpdating = false;
        DateTime _connectedTime = DateTime.MinValue;
        string currentPacket = null;
        string currentData = "";

        // Movesense
        public static Guid MOVESENSE_SERVICE = new Guid("61353090-8231-49cc-b57a-886370740041");
        public static Guid MOVESENSE_WRITE = new Guid("34802252-7185-4d5d-b431-630e7050e8f0");
        public static Guid MOVESENSE_NOTIFY = new Guid("34802252-7185-4d5d-b431-630e7050e8f0");

        private ICharacteristic _movesenseWrite;
        private ICharacteristic _movesenseNotify;

        const string PACKET_START = "RX-";
        const string PACKET_END = "TX-";
        const string SYNC_TIME = "SX-";
        const string SYNC_COMMAND = @"/";


        bool _isConnected = false;
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                SetValue(ref _isConnected, value, "IsConnected");
            }
        }

        public SensorKitMovesenseConnector(SensorModel data)
        {
            Data = data;
        }

        #region IConnector

        public async Task Subscribe()
        {
           
            try
            {
                Debug.WriteLine($"***SUBSCRIBING {Data.Name}...");

                if (Data.Id != null)
                {
                    _device = await SensorKit.Instance.Adapter.ConnectToKnownDeviceAsync(Data.Id);
                    if (_device != null)
                    {
                        _connectedTime = DateTime.Now;

                        var uart = await _device.GetServiceAsync(MOVESENSE_SERVICE);
                        if (uart != null)
                        {
                            _movesenseNotify = await uart.GetCharacteristicAsync(MOVESENSE_NOTIFY);
                            _movesenseWrite = await uart.GetCharacteristicAsync(MOVESENSE_WRITE);

                            _movesenseNotify.ValueUpdated += (s, e) =>
                            {
                                var received = Encoding.UTF8.GetString(e.Characteristic.Value, 0, e.Characteristic.Value.Length);
                                //ParsePacket(received);
                            };

                            IsConnected = true;

                            await _movesenseNotify.StartUpdatesAsync();
                            //await _uartTX.WriteAsync(Encoding.UTF8.GetBytes(SYNC_COMMAND));
                        }
                    }

                    InvokeHelper.Invoke(() =>
                    {
                        Data.NotifyPropertyChanged("IsSubscribed");
                    });

                }
            }catch(Exception x)
            {
                Debug.WriteLine(x);
            }
        }
       
        public async Task Unsubscribe()
        {
            try
            {
                if (_device != null)
                {
                    await SensorKit.Instance.Adapter?.DisconnectDeviceAsync(_device);
                }
            }catch(Exception x)
            {
                Debug.WriteLine(x);
            }
            _connectedTime = DateTime.MinValue;
            IsConnected = false;
        }

#endregion

        #region Data parsing

        private void ParsePacket(string received)
        {
            Debug.WriteLine($"CHUNK: {received}");
            if (!String.IsNullOrEmpty(received))
            {
                if (currentPacket == null)
                {
                    string packetStart = ParsePacketStart(received);
                    if (!String.IsNullOrEmpty(packetStart))
                    {
                        Debug.WriteLine($"PACKET START: {packetStart}");
                        currentPacket = packetStart;
                        currentData = "";
                    }
                    else
                    {
                        var syncMs = ParseEndSync(received);
                        if (syncMs.HasValue)
                        {
                            Data?.SynchronizeTime(_connectedTime, syncMs.Value);
                            Data?.Save();
                        }
                    }
                }
                else
                {
                    string packetEnd = ParsePacketEnd(received);
                    if (!String.IsNullOrEmpty(packetEnd))
                    {
                        Debug.WriteLine($"PACKET END: {packetEnd}");
                        if (packetEnd == currentPacket)
                        { // packet complete
                            //DataBuffer.Enqueue(currentData);
                            PacketToSensorData(currentData);
                            Debug.WriteLine($"DATA: {currentData}");
                        }

                        currentPacket = null;
                        currentData = "";
                    }
                    else
                    {
                        // keep appending to the buffer
                        currentData += received;
                    }
                }
            }
        }

        private long? ParseEndSync(string s)
        {
            if (!String.IsNullOrEmpty(s) && s.StartsWith(SYNC_TIME))
            {
                var t = s.Substring(SYNC_TIME.Length);
                long result;
                if (long.TryParse(t, out result))
                {
                    return result;
                }
            }
            return null;
        }

        private string ParsePacketStart(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                int start = s.LastIndexOf(PACKET_START); // any RX in the string?
                if (start != -1)
                {
                    return s.Substring(start + PACKET_START.Length);
                }
            }
            return null;
        }

        private string ParsePacketEnd(string s)
        {
            if (!String.IsNullOrEmpty(s))
            {
                int start = s.LastIndexOf(PACKET_END); // any TX in the string?
                if (start != -1)
                {
                    return s.Substring(start + PACKET_END.Length);
                }
            }
            return null;
        }

        private SensorItem PacketToSensorData(string json)
        {
            try
            {
                
                var item = JsonConvert.DeserializeObject<SensorData>(json);
                if (Data != null)
                {
                    if (item?.airdata != null)
                    {
                        var reading = new SensorItem
                        {
                            offsetMs = item.airdata.t,
                            itemType = SensorItemTypes.Airtime,
                            duration = item.airdata.dt,
                            altitude = item.airdata.alt,
                            force = item.airdata.g
                        };

                        Data.Append(reading);
                    }

                    if (item?.summary != null)
                    {
                        Data.Summary = item.summary;
                    }
                }

            }
            catch(Exception x)
            {
                Debug.WriteLine(x);
            }
            return null;
        }

        #endregion


    }
}
