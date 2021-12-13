using System;

using UIKit;
using CoreBluetooth;
using System.Timers;
using Foundation;
using CoreFoundation;
using Movesense;

namespace NothingApp2
{
    public partial class DetailViewController : UIViewController
    {
        public readonly string UUID = "0000fdf3-0000-1000-8000-00805ff9b34fb";
        public object DetailItem { get; set; }

        protected DetailViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.

        }

        public void SetDetailItem(object newDetailItem)
        {
            if (DetailItem != newDetailItem)
            {
                DetailItem = newDetailItem;

                // Update the view
                ConfigureView();

            }
        }

        void ConfigureView()
        {
            // Update the user interface for the detail item
            if (IsViewLoaded && DetailItem != null)
            {
                detailDescriptionLabel.Text = DetailItem.ToString();

                var myDel = new MySimpleCBCentralManagerDelegate();
                var myMgr = new CBCentralManager(myDel, DispatchQueue.CurrentQueue);

                System.Threading.Thread.Sleep(200);

                MDSWrapper wrapper = new MDSWrapper();
                MDSResponseBlock block = new MDSResponseBlock(async (arg0) => DoThing(arg0, wrapper));
                MDSEventBlock eventBlock = (MDSEvent arg0) => DoTheOtherThing(arg0, wrapper);
                wrapper.DoSubscribe("MDS/ConnectedDevices", new Foundation.NSDictionary(), block, eventBlock);
            }

        }

        public void DoTheOtherThing(MDSEvent anEvent, MDSWrapper wrapper)
        {
            System.Diagnostics.Debug.WriteLine("Mooooo ");

            var m = anEvent.BodyDictionary.ValueForKey(new NSString("Method"));
            System.Diagnostics.Debug.WriteLine(m);
            var serial = anEvent.BodyDictionary.ValueForKey(new NSString("Body")).ValueForKey(new NSString("Serial")).ToString();

            //if (!serial.EndsWith("08")) return;

            //var body = new NSDictionary();

            var ledOn = NSDictionary.FromObjectsAndKeys(new object[] {
                    true,
                    0
                }, new object[] {
                    "IsOn",
                    "LedColor"
            });

            var body = NSDictionary.FromObjectAndKey(ledOn, new NSString("LedState"));

            //wrapper.DoGet(String.Format("{0}/Component/Leds", serial), new NSDictionary(), (resp) =>
            //{
            //    wrapper.DisconnectPeripheralWithUUID(new Foundation.NSUuid("4CC5B0ED-08A0-C062-B1C2-B4B91D6B254A"));
            //});

            wrapper.DoPut(String.Format("{0}/Component/Leds/0", serial), body, (arg0) =>
            {
                System.Diagnostics.Debug.WriteLine("YOU SHOULD SEE THE LIGHT!!!!");
                wrapper.DisconnectPeripheralWithUUID(new Foundation.NSUuid("4CC5B0ED-08A0-C062-B1C2-B4B91D6B254A"));
            });

            //wrapper.DoGet(String.Format("{0}/Time", serial), new NSDictionary(),
            //(arg0) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("Response" + arg0.BodyDictionary.ValueForKey(new NSString("Content")).ToString());
            //    wrapper.DisconnectPeripheralWithUUID(new Foundation.NSUuid("4CC5B0ED-08A0-C062-B1C2-B4B91D6B254A"));
            //});
        }

        public void DoThing(MDSResponse arg0, MDSWrapper wrapper)
        {
            System.Diagnostics.Debug.WriteLine("I have been called: " + arg0.StatusCode);
            wrapper.ConnectPeripheralWithUUID(new Foundation.NSUuid("4CC5B0ED-08A0-C062-B1C2-B4B91D6B254A"));
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
            ConfigureView();
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }

    public class MySimpleCBCentralManagerDelegate : CoreBluetooth.CBCentralManagerDelegate
    {
        override public void UpdatedState(CBCentralManager mgr)
        {
            if (mgr.State == CBCentralManagerState.PoweredOn)
            {
                //Passing in null scans for all peripherals. Peripherals can be targeted by using CBUIIDs
                CBUUID[] cbuuids = new[] { CBUUID.FromString("61353090-8231-49cc-b57a-886370740041") };
                mgr.ScanForPeripherals(cbuuids); //Initiates async calls of DiscoveredPeripheral
                //Timeout after 30 seconds
                var timer = new Timer(30 * 1000);
                timer.Elapsed += (sender, e) => mgr.StopScan();
            }
            else
            {
                //Invalid state -- Bluetooth powered down, unavailable, etc.
                System.Diagnostics.Debug.WriteLine("Bluetooth is not available");
            }
        }

        public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral, NSDictionary advertisementData, NSNumber RSSI)
        {
            System.Diagnostics.Debug.WriteLine("Discovered {0}, data {1}, RSSI {2}", peripheral.Name, advertisementData, RSSI);
        }
    }

}

