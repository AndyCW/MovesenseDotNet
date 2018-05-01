using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SampleApp.Services;
using MdsLibrary.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SampleApp.ViewModels
{
    public class LinearAccelerationPageViewModel : ViewModelBase
    {
        private IDeviceScanService _deviceScanService;
        private AccelerometerSubscription subscription;

        public ICommand ToggleSubscribeSwitchCommand { get; set; }

        // Property to reflect the current state of the ToggleSwitch in the UI
        private bool isSubscribeSwitchOn = false;
        public bool IsSubscribeSwitchOn { get { return isSubscribeSwitchOn; } set { Set(() => IsSubscribeSwitchOn, ref isSubscribeSwitchOn, value); } }

        private string connectionStatusText = string.Empty;
        public string ConnectionStatusText { get { return connectionStatusText; } set { Set(() => ConnectionStatusText, ref connectionStatusText, value); } }

        public LinearAccelerationPageViewModel(IDeviceScanService deviceScanService)
        {
            _deviceScanService = deviceScanService;

            // Command to start the subscription
            ToggleSubscribeSwitchCommand = new Xamarin.Forms.Command(
                async () =>
                {
                    if (IsSubscribeSwitchOn)
                    {
                        ConnectionStatusText = "Connecting...";
                        await Sensor.Connect();
                        ConnectionStatusText = "Subscribed";
                        subscription = new AccelerometerSubscription(false, Sensor.Name);
                        await subscription.SubscribeAsync((d) =>
                        {
                            PlotData(d.body.timestamp, d.body.array[0].x, d.body.array[0].y, d.body.array[0].z);
                        });
                    }
                    else
                    {
                        // Unsubscribe
                        subscription.UnSubscribe();
                        ConnectionStatusText = "Unsubscribed";
                        await Sensor.Disconnect();
                        ConnectionStatusText = "Disconnected";
                    }
                }
                , () => (Sensor != null) // Enable command only if we've got a device
            );

            InitPlotModel();
        }


        private MovesenseDeviceViewModel _movesenseDeviceViewModel;
        public MovesenseDeviceViewModel Sensor
        {
            get { return _movesenseDeviceViewModel;
            }
            set
            {
                Set(() => Sensor, ref _movesenseDeviceViewModel, value);
                ((Xamarin.Forms.Command)ToggleSubscribeSwitchCommand).ChangeCanExecute();
            }
        }

        public void Init()
        {
            ConnectionStatusText = string.Empty;
            IsSubscribeSwitchOn = false;

            Sensor = null;
            if (Application.Current.Properties.ContainsKey("SelectedSensorId"))
            {
                // Discover the device the user has selected so that we can start subscription
                // TODO - connect directly to the device without scanning again??
                StartScanning();
            }

            gotInitialReading = false;
        }

        public void OnExit()
        {
            StopScanning();
            subscription?.UnSubscribe();
        }

        public void StartScanning()
        {
            _deviceScanService.MovesenseDeviceFound += _deviceScanService_MovesenseDeviceFound;
            _deviceScanService.StartScanning();
        }

        public void StopScanning()
        {
            _deviceScanService.MovesenseDeviceFound -= _deviceScanService_MovesenseDeviceFound;
            _deviceScanService.StopScanning();
        }

        private void _deviceScanService_MovesenseDeviceFound(object sender, MovesenseDeviceFoundArgs e)
        {
            // Is this the one?
            string Id = Application.Current.Properties["SelectedSensorId"] as string;
            if (e.Device.Id.ToString() == Id)
            {
                Sensor = new MovesenseDeviceViewModel(e.Device);
                StopScanning();
            }
        }

        public PlotModel Model { get; private set; }

        private LineSeries xlineSeries;
        private LineSeries ylineSeries;
        private LineSeries zlineSeries;

        private bool gotInitialReading = false;
        private long initialTimestamp = 0;

        public void InitPlotModel()
        {
            var model = new PlotModel { Title = "Linear Acceleration" };
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = -10,
                Maximum = 10,
                MajorStep = 5,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineThickness = 1,
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                MinimumRange = 25000,
                MajorStep = 5000,
                MinorStep = 5000,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineThickness = 1,
                TextColor = OxyColors.Transparent
            });

            xlineSeries = new LineSeries { Title = "X", MarkerType = MarkerType.None, Color = OxyColors.Red };
            ylineSeries = new LineSeries { Title = "Y", MarkerType = MarkerType.None, Color = OxyColors.Green };
            zlineSeries = new LineSeries { Title = "Z", MarkerType = MarkerType.None, Color = OxyColors.CadetBlue };
            model.Series.Add(xlineSeries);
            model.Series.Add(ylineSeries);
            model.Series.Add(zlineSeries);
            this.Model = model;
        }

        private void PlotData(long timestamp, double x, double y, double z)
        {
            if (!gotInitialReading)
            {
                gotInitialReading = true;
                initialTimestamp = timestamp;
            }

            var normalizedTimestamp = timestamp - initialTimestamp;
            xlineSeries.Points.Add(new DataPoint(normalizedTimestamp, x));
            ylineSeries.Points.Add(new DataPoint(normalizedTimestamp, y));
            zlineSeries.Points.Add(new DataPoint(normalizedTimestamp, z));

            Debug.WriteLine($"Updating chart at timestamp {normalizedTimestamp} ");
            Model.InvalidatePlot(true);

        }
    }
}
