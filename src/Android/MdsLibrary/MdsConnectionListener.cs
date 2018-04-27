using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace MdsLibrary
{
    public class MdsConnectionListener : Java.Lang.Object, Com.Movesense.Mds.IMdsConnectionListener
    {
        public event EventHandler<MdsConnectionListenerEventArgs> Connect;
        public event EventHandler<MdsConnectionListenerEventArgs> ConnectionComplete;
        public event EventHandler<MdsConnectionListenerEventArgs> Disconnect;

        public MdsConnectionListener()
        {
        }

        public void OnConnect(string MACaddress)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnConnect callback called: MACaddress {MACaddress}");
            Connect?.Invoke(this, new MdsConnectionListenerEventArgs(MACaddress));
        }

        public void OnConnectionComplete(string MACaddress, string serial)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnConnectionComplete callback called: MACaddress {MACaddress} Serial {serial}");
            ConnectionComplete?.Invoke(this, new MdsConnectionListenerEventArgs(MACaddress));
        }

        public void OnDisconnect(string MACaddress)
        {
            Debug.WriteLine($"SUCCESS MdsConnectionListener OnDisconnect callback called: MACaddress {MACaddress}");
            Disconnect?.Invoke(this, new MdsConnectionListenerEventArgs(MACaddress));
        }

        public void OnError(Com.Movesense.Mds.MdsException e)
        {
            throw new MdsException(e.ToString(), e);
        }
    }

    public class MdsConnectionListenerEventArgs : EventArgs
    {
        public string MACAddress { get; set; }

        public MdsConnectionListenerEventArgs(string macAddress)
        {
            MACAddress = macAddress;
        }
    }
}
