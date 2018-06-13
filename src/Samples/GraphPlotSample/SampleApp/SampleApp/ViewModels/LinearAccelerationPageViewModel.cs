using GalaSoft.MvvmLight;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using MdsLibrary.Api;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Movesense;

namespace SampleApp.ViewModels
{
    public class LinearAccelerationPageViewModel : ViewModelBase
    {
        private IMdsSubscription subscription;

        public ICommand ToggleSubscribeSwitchCommand { get; set; }

        // Property to reflect the current state of the ToggleSwitch in the UI
        private bool isSubscribeSwitchOn = false;
        public bool IsSubscribeSwitchOn { get { return isSubscribeSwitchOn; } set { Set(() => IsSubscribeSwitchOn, ref isSubscribeSwitchOn, value); } }

        private string connectionStatusText = string.Empty;
        public string ConnectionStatusText { get { return connectionStatusText; } set { Set(() => ConnectionStatusText, ref connectionStatusText, value); } }

        public LinearAccelerationPageViewModel()
        {
            // Command to start the subscription
            ToggleSubscribeSwitchCommand = new Xamarin.Forms.Command(
                async () =>
                {
                    if (IsSubscribeSwitchOn)
                    {
                        ConnectionStatusText = "Connecting...";
                        await MovesenseDevice.Connect();
                        ConnectionStatusText = "Subscribing...";

                        subscription = await CrossMovesense.Current.SubscribeAccelerometerAsync(
                            MovesenseDevice.Name, 
                            (d) =>
                            {
                                PlotData(d.Data.Timestamp, d.Data.AccData[0].X, d.Data.AccData[0].Y, d.Data.AccData[0].Z);
                            },
                            26);
                        ConnectionStatusText = "Subscribed";

                    }
                    else
                    {
                        // Unsubscribe
                        subscription.Unsubscribe();
                        ConnectionStatusText = "Unsubscribed";
                        await MovesenseDevice.Disconnect();
                        ConnectionStatusText = "Disconnected";
                    }
                }
                , () => (MovesenseDevice != null) // Enable command only if we've got a device
            );

            InitPlotModel();
        }


        private MovesenseDeviceViewModel _movesenseDeviceViewModel;
        public MovesenseDeviceViewModel MovesenseDevice
        {
            get { return _movesenseDeviceViewModel;
            }
            set
            {
                Set(() => MovesenseDevice, ref _movesenseDeviceViewModel, value);
                ((Xamarin.Forms.Command)ToggleSubscribeSwitchCommand).ChangeCanExecute();
            }
        }

        public void Init()
        {
            ConnectionStatusText = string.Empty;
            IsSubscribeSwitchOn = false;
            gotInitialReading = false;
        }

        public void OnExit()
        {
            subscription?.Unsubscribe();
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
                Maximum = 15,
                MajorStep = 5,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineThickness = 1,
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                MinimumRange = 25000,
                MaximumRange = 25000,
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
