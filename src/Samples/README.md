# Samples

The following samples demonstrate how to use the Movesense Plugin:

## BasicDemo

This is a very simple sample. Press the 'Go' button, and the app searches for Movesense devices, connects to the first one it finds, 
and gets the Device Info and the estimated battery level and reports the output in an alert. It also turns the LED on and off again.

## GraphPlotSample

This more complex sample uses full MVVM. To use it, press the **Select** button and then tap to select one of the Movesense devices discovered.
Then tap the **Subscribe** slider to connect to the device and subscribe to accelerometer readings. Move the device to see the graph plot the readings.

## CustomServiceSample

Demonstrates how to connect to a custom resource exposed by an app running on the device. For best results, ensure your Movesense device 
has been flashed with the [Movesense mobile-device-lib hello_world_app sample](https://bitbucket.org/suunto/movesense-device-lib/src/master/samples/hello_world_app/) sample app.