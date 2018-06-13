# Plugin.Movesense #

## Type CrossMovesense

 Cross Movesense 



---
#### Property CrossMovesense.IsSupported

 Gets if the plugin is supported on the current platform. 



---
#### Property CrossMovesense.Current

 Current plugin implementation to use 



---
## Type IMovesense

 Movesense Plugin API 



---
#### Property IMovesense.MdsInstance

 Gets the native MdsLib object 



---
#### Property IMovesense.Activity

 On Android, this must be set to the current Activity before first access of the library. 



---
#### Property IMovesense.SCHEME_PREFIX

 Root of all paths to Movesense resources. 



---
#### Method IMovesense.CreateLogEntryAsync(System.String)

 Create a new logbook entry resource (increment log Id). Returns the new log Id. 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
**Returns**: new Log Id



---
#### Method IMovesense.DeleteLogEntriesAsync(System.String)

 Delete all the Logbook entries 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.GetAccInfoAsync(System.String)

 Get Accelerometer configuration 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.GetBatteryLevelAsync(System.String)

 Get Battery level, CallAsync returns BatteryResult 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.GetDeviceInfoAsync(System.String)

 Get device info 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.GetGyroInfoAsync(System.String)

 Get Gyrometer configuration 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.GetIMUInfoAsync(System.String)

 Get IMU configuration 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.GetLedsStateAsync(System.String)

 Get state of all Leds in the system 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.GetLedStateAsync(System.String,System.Int32)

 Get LedState for an LED 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|ledIndex: |Number of the Led|


---
#### Method IMovesense.GetLogbookDataAsync(System.String,System.Int32)

 Get data from a Logbook entry 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|logId: |Number of the entry to get|


---
#### Method IMovesense.GetLogbookDescriptorsAsync(System.String,System.Int32)

 Get Descriptors for a Logbook entry 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|logId: |Logbook entry to get|


---
#### Method IMovesense.GetLogEntriesAsync(System.String)

 Get details of Logbook entries 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.GetLoggerStatusAsync(System.String)

 Get Logger status, CallAsync returns LogStatusResult object 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.GetMagInfoAsync(System.String)

 Get Magnetometer configuration 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.GetTimeAsync(System.String)

 Gets current time in number of microseconds since epoch 1.1.1970 (UTC). If not explicitly set, contains number of seconds since reset. 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.SetLedStateAsync(System.String,System.Int32,System.Boolean,Plugin.Movesense.Api.LedColor)

 Sets state of an LED 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|ledIndex: |Index of the Led - use 0 for standard Movesense sensor|
|ledOn: |Set on or off|
|ledColor: |[optional]value from LedColor enumeration - default is LedColor.Red|


---
#### Method IMovesense.SetLoggerStatusAsync(System.String,System.Boolean)

 Set state of the Datalogger 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|start: |Set true to start the datalogger, false to stop|


---
#### Method IMovesense.SetTimeAsync(System.String)

 Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC). 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method IMovesense.SetupLoggerAsync(System.String,System.Int32)

 Set configuration for the Datalogger - ONLY sets IMU9 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|freq: |Sampling rate, e.g. 26 for 26Hz|


---
#### Method IMovesense.SubscribeAccelerometerAsync(System.String,System.Action{MdsLibrary.Model.AccData},System.Int32)

 Subscribe to periodic linear acceleration measurements. 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|notificationCallback: |Callback function to receive the AccData|
|sampleRate: |Sampling rate, e.g. 26 for 26Hz|


---
#### Method IMovesense.SubscribeGyrometerAsync(System.String,System.Action{MdsLibrary.Model.GyroData},System.Int32)

 Subscribe to periodic Gyrometer data 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|notificationCallback: |Callback function to receive the GyroData|
|sampleRate: |Sampling rate, e.g. 26 for 26Hz|


---
#### Method IMovesense.SubscribeIMU6Async(System.String,System.Action{MdsLibrary.Model.IMU6Data},System.Int32)

 Subscribe to periodic 6-axis IMU measurements (Acc + Gyro). 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|notificationCallback: |Callback function to receive the IMU6Data|
|sampleRate: |Sampling rate, e.g. 26 for 26Hz|


---
#### Method IMovesense.SubscribeIMU9Async(System.String,System.Action{MdsLibrary.Model.IMU9Data},System.Int32)

 Subscribe to periodic 9-axis IMU measurements. 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|notificationCallback: |Callback function to receive the IMU9Data|
|sampleRate: |Sampling rate, e.g. 26 for 26Hz|


---
#### Method IMovesense.SubscribeMagnetometerAsync(System.String,System.Action{MdsLibrary.Model.MagnData},System.Int32)

 Subscribe to periodic Magnetometer data measurements 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|notificationCallback: |Callback function to receive the MagnData|
|sampleRate: |Sampling rate, e.g. 26 for 26Hz|


---

## Type MdsLibrary.Api.ApiCallAsync

 Makes an APICall to MdsLib for those MdsLib methods that do not return data 



---
#### Method MdsLibrary.Api.ApiCallAsync.#ctor(System.String,Plugin.Movesense.Api.MdsOp,System.String,System.String)

 Create an ApiCall instance 

|Name | Description |
|-----|------|
|deviceName: |The name of the device e.g. Movesense 908637721113|
|restOp: |The type of REST call to make to MdsLib|
|path: |The path of the MdsLib resource|
|body: |JSON body if any|


---
#### Field MdsLibrary.Api.ApiCallAsync.RetryFunction

 Retry function, called after the function call fails. The built-in implementation retries 2 times, regardless of the exception thrown. Override the built-in implementation by setting this to your own implementation of the Retry function 



---
#### Method MdsLibrary.Api.ApiCallAsync.CallWithRetryAsync

 Make the API call (async) 

**Returns**: Response object of type T



---
#### Method MdsLibrary.Api.ApiCallAsync.CallAsync

 Make the API call (async) 

**Returns**: Response object of type T



---
## Type MdsLibrary.Api.ApiCallAsync.MdsResponseListener

 MdsResponseListener called by MdsLib with result of the call (Internal) 



---
#### Method MdsLibrary.Api.ApiCallAsync`1.#ctor(System.String,Plugin.Movesense.Api.MdsOp,System.String,System.String)

 Base class for all Mds API calls 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|restOp: |The type of REST call to make to MdsLib|
|path: |The path of the MdsLib resource|
|body: |JSON body if any|


---
#### Field MdsLibrary.Api.ApiCallAsync`1.RetryFunction

 Retry function, called after the function call fails. The built-in implementation retries 2 times, regardless of the exception thrown. Override the built-in implementation by setting this to your own implementation of the Retry function 



---
#### Method MdsLibrary.Api.ApiCallAsync`1.CallWithRetryAsync

 Make the API call (async) with up to two retries if the call throws an exception. Set RetryFunction property to override the builtin retry logic. 

**Returns**: Response object of type T



---
#### Method MdsLibrary.Api.ApiCallAsync`1.CallAsync

 Make the API call (async) 

**Returns**: Response object of type T



---
## Type MdsLibrary.Api.ApiSubscription`1

 Makes a subscription to an MdsLib resource 

|Name | Description |
|-----|------|
|T: ||


---
#### Property MdsLibrary.Api.ApiSubscription`1.Subscription

 The context for the subscription 



---
#### Method MdsLibrary.Api.ApiSubscription`1.#ctor(System.String,System.String,System.Int32)

 Utility class for API subscriptions 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|path: |The path of the MdsLib resource|
|body: |JSON body if any|


---
#### Field MdsLibrary.Api.ApiSubscription`1.RetryFunction

 Retry function, called after the function call fails. The built-in implementation retries 2 times, regardless of the exception thrown. Override the built-in implementation by setting this to your own implementation of the Retry function 



---
#### Method MdsLibrary.Api.ApiSubscription`1.SubscribeWithRetryAsync(System.Action{`0})

 Subscribe to the resource 

|Name | Description |
|-----|------|
|notificationCallback: |Callback function that will receive periodic notifications with data from the subscription resource|
**Returns**: The subscription context



---
#### Method MdsLibrary.Api.ApiSubscription`1.SubscribeAsync(System.Action{`0})

 Subscribe to the resource 

|Name | Description |
|-----|------|
|notificationCallback: |Callback function that will receive periodic notifications with data from the subscription resource|
**Returns**: The subscription context



---
#### Method MdsLibrary.Api.ApiSubscription`1.UnSubscribe

 Unsubscribe from the MdsLib resource 



---
## Type MdsLibrary.Api.IApiSubscription`1

 Interface for class that allows subscription to a Movesense subscription resource 

|Name | Description |
|-----|------|
|T: |Type of the data object the subscription reports in notifications|


---
#### Property MdsLibrary.Api.IApiSubscription`1.Subscription

 The Subscription to the resource 



---
#### Method MdsLibrary.Api.IApiSubscription`1.SubscribeAsync(System.Action{`0})

 Subscribe to the resource 

|Name | Description |
|-----|------|
|notificationCallback: |Callback function that the MdsLib calls with periodic notifications|
**Returns**: 



---
#### Method MdsLibrary.Api.IApiSubscription`1.SubscribeWithRetryAsync(System.Action{`0})

 Subscribe to the resource. If the initial subscription throws an exception, retry up to two times. 

|Name | Description |
|-----|------|
|notificationCallback: |Callback function that the MdsLib calls with periodic notifications|
**Returns**: 



---
#### Method MdsLibrary.Helpers.Util.GetVisibleSerial(System.String)

 Utility method to extract the device serial number from the full device name 

|Name | Description |
|-----|------|
|name: ||
**Returns**: 



---
## Type MdsLibrary.Model.AccData.Body

 Data for Accelerometer readings 



---
#### Field MdsLibrary.Model.AccData.Body.Timestamp

 Local timestamp of first measurement in the array of readings 



---
#### Field MdsLibrary.Model.AccData.Body.AccData

 Array of Accelerometer readings 



---
#### Field MdsLibrary.Model.AccData.Body.Header

 Headers 



---
#### Method MdsLibrary.Model.AccData.Body.#ctor(System.Int64,MdsLibrary.Model.AccData.AccDataArray[],MdsLibrary.Model.AccData.Headers)

 Constructs an Accelerometer Readings object 

|Name | Description |
|-----|------|
|timestamp: |Local timestamp of first measurement in the array of readings|
|array: |Array of Accelerometer readings|
|header: |Headers|


---
## Type MdsLibrary.Model.AccData.AccDataArray

 Array of Accelerometer readings 



---
#### Field MdsLibrary.Model.AccData.AccDataArray.X

 X value 



---
#### Field MdsLibrary.Model.AccData.AccDataArray.Y

 Y value 



---
#### Field MdsLibrary.Model.AccData.AccDataArray.Z

 Z value 



---
#### Method MdsLibrary.Model.AccData.AccDataArray.#ctor(System.Double,System.Double,System.Double)

 Constructs an AccDataArray 

|Name | Description |
|-----|------|
|x: |X value|
|y: |Y value|
|z: |Z value|


---
#### Field MdsLibrary.Model.AccInfo.Data

 Container for the Accelerometer Configuration information 



---
## Type MdsLibrary.Model.AccInfo.Content

 Accelerometer Configuration information 



---
#### Field MdsLibrary.Model.AccInfo.Content.SampleRates

 Available sample rates for acceleration measurement. 



---
#### Field MdsLibrary.Model.AccInfo.Content.Ranges

 Available ranges for acceleration measurement. For example range value 2 means the range is -2...+2 G. 



---
## Type MdsLibrary.Model.BaseResult

 Generic response class for retrieving non-strongly typed responses from MdsAPI 



---
#### Field MdsLibrary.Model.BaseResult.Content

 Data response from MdsAPI 



---
## Type MdsLibrary.Model.BatteryResult

 Energy level response 



---
#### Field MdsLibrary.Model.BatteryResult.ChargePercent

 Estimated battery charge percent 



---
## Type MdsLibrary.Model.CreateLogResult

 Response from command to create a new Log entry resource 



---
#### Field MdsLibrary.Model.CreateLogResult.LogId

 ID of the new Logbook entry resource 



---
## Type MdsLibrary.Model.DataLoggerConfig

 DataLogger configuration 



---
#### Field MdsLibrary.Model.DataLoggerConfig.Configuration

 Contains the configuration of the Datalogger 



---
#### Method MdsLibrary.Model.DataLoggerConfig.#ctor(MdsLibrary.Model.DataLoggerConfig.Config)

 Creates a DataLoggerConfig 

|Name | Description |
|-----|------|
|config: |Configuration data|


---
## Type MdsLibrary.Model.DataLoggerConfig.Config

 Configuration of the Datalogger 



---
#### Field MdsLibrary.Model.DataLoggerConfig.Config.DataEntries

 DataEntries object contining an array of structs containing wb-paths to the subscription of data to log. 



---
#### Method MdsLibrary.Model.DataLoggerConfig.Config.#ctor(MdsLibrary.Model.DataLoggerConfig.DataEntries)

 Creates the DataLogger Config object 

|Name | Description |
|-----|------|
|dataEntries: |DataEntries object containing an array of structs containing wb-paths to the subscription of data to log.|


---
#### Field MdsLibrary.Model.DataLoggerConfig.DataEntries.DataEntry

 Class container for an array of DataEntry objects. 



---
#### Method MdsLibrary.Model.DataLoggerConfig.DataEntries.#ctor(MdsLibrary.Model.DataLoggerConfig.DataEntry[])

 Creates a DataEntries object 

|Name | Description |
|-----|------|
|dataEntry: |Array of DataEntry objects|


---
#### Field MdsLibrary.Model.DataLoggerConfig.DataEntry.Path

 WB-path to the subscription of data to log. 



---
#### Method MdsLibrary.Model.DataLoggerConfig.DataEntry.#ctor(System.String)

 Creates a DataEntry 

|Name | Description |
|-----|------|
|path: |Wb-path to the subscription of data to log.|


---
## Type MdsLibrary.Model.DeviceInfoResult

 Contains response to a device information query 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfo

 Device Information query response 



---
## Type MdsLibrary.Model.DeviceInfoResult.DeviceInfoData

 Device Information 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.ManufacturerName

 Original Equipment Manufacturer (OEM) 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.BrandName

 Brand name of the device 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.ProductName

 Official product name visible to the consumers 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.Variant

 Internal SW variant name 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.Design

 Product design name 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.HwCompatibilityId

 Identifier defining which firmware can be installed on the device. This ID together with the variant name is used to check/fetch the latest FW.Different PCBAs which can run the same Firmware, have the same HwCompatibilityId. 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.Serial

 Globally unique device serial number (12 digit) 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.PcbaSerial

 Globally unique PCBA assemply identifier (this can be OEM specific). 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.Sw

 Version number of the firmware. 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.Hw

 Identifier for the HW configuration. Everytime there are changes in components/PCB, this ID is updated. 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.VersionInfoArray

 Array of VersionInfo 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.AddressInfo

 Array of structs containing pairs of address type identifier and address. 



---
#### Field MdsLibrary.Model.DeviceInfoResult.DeviceInfoData.ApiLevel

 Movesense-api level. This is defined by single number. 



---
## Type MdsLibrary.Model.DeviceInfoResult.VersionInfoArray

 Version Info array 



---
#### Field MdsLibrary.Model.DeviceInfoResult.VersionInfoArray.VersionInfo

 Version Info array 



---
## Type MdsLibrary.Model.DeviceInfoResult.VersionInfo

 Version info 



---
#### Field MdsLibrary.Model.DeviceInfoResult.VersionInfo.Name

 Version name 



---
#### Field MdsLibrary.Model.DeviceInfoResult.VersionInfo.Version

 Version 



---
## Type MdsLibrary.Model.DeviceInfoResult.AddressInfo

 Network address info 



---
#### Field MdsLibrary.Model.DeviceInfoResult.AddressInfo.Name

 Address type (BLE, WIFI) 



---
#### Field MdsLibrary.Model.DeviceInfoResult.AddressInfo.Address

 Address value, for example 80-1F-02-4E-F1-70 



---
## Type MdsLibrary.Model.GyroData

 Response object containing gyrometer data readings 



---
#### Field MdsLibrary.Model.GyroData.Data

 Response object containing gyrometer data readings 



---
#### Method MdsLibrary.Model.GyroData.#ctor(MdsLibrary.Model.GyroData.Body)

 Constructor 

|Name | Description |
|-----|------|
|body: |Body object containing gryometer data readings|


---
## Type MdsLibrary.Model.GyroData.Body

 Gyrometer data readings 



---
#### Field MdsLibrary.Model.GyroData.Body.Timestamp

 Local timestamp of first measurement. 



---
#### Field MdsLibrary.Model.GyroData.Body.ArrayGyro

 Measured angular velocity values (3D) in array. 



---
#### Field MdsLibrary.Model.GyroData.Body.Header

 Headers 



---
#### Method MdsLibrary.Model.GyroData.Body.#ctor(System.Int64,MdsLibrary.Model.GyroData.Values3D[],MdsLibrary.Model.GyroData.Headers)

 Creates a Body that contains Gyrometer readings 

|Name | Description |
|-----|------|
|timestamp: |Local timestamp of first measurement|
|array: |Measured angular velocity values (3D) in array|
|header: |Header|


---
## Type MdsLibrary.Model.GyroData.Values3D

 Measured angular velocity values (3D). 



---
#### Field MdsLibrary.Model.GyroData.Values3D.X

 X value 



---
#### Field MdsLibrary.Model.GyroData.Values3D.Y

 Y value 



---
#### Field MdsLibrary.Model.GyroData.Values3D.Z

 Z value 



---
#### Method MdsLibrary.Model.GyroData.Values3D.#ctor(System.Double,System.Double,System.Double)

 Creates a GyroArray 

|Name | Description |
|-----|------|
|x: ||
|y: ||
|z: ||


---
## Type MdsLibrary.Model.GyroData.Headers

 Headers 



---
## Type MdsLibrary.Model.GyroInfo

 Gyroscope measurement configuration 



---
#### Field MdsLibrary.Model.GyroInfo.Info

 Container for Gyroscope measurement configuration 



---
## Type MdsLibrary.Model.GyroInfo.Content

 Gyroscope measurement configuration 



---
#### Field MdsLibrary.Model.GyroInfo.Content.SampleRates

 Available sample rates for gyroscope measurement. 



---
#### Field MdsLibrary.Model.GyroInfo.Content.Ranges

 Available ranges for angular range measurement rate. For example range value 500 means the range is -500...+500 dps. 



---
## Type MdsLibrary.Model.IMU6Data

 Response object for subscription to periodic 6-axis (with magnetometer) IMU measurements. 



---
#### Field MdsLibrary.Model.IMU6Data.body

 Response object for subscription to periodic 6-axis (with magnetometer) IMU measurements. 



---
#### Method MdsLibrary.Model.IMU6Data.#ctor(MdsLibrary.Model.IMU6Data.Data)

 Creates an IMU6Data object 

|Name | Description |
|-----|------|
|body: ||


---
## Type MdsLibrary.Model.IMU6Data.Data

 Container for IMU6 readings 



---
#### Field MdsLibrary.Model.IMU6Data.Data.Timestamp

 Local timestamp of first measurement. 



---
#### Field MdsLibrary.Model.IMU6Data.Data.ArrayAcc

 Measured acceleration values (3D) in array. 



---
#### Field MdsLibrary.Model.IMU6Data.Data.ArrayGyro

 Measured angular velocity values (3D) in array. 



---
#### Field MdsLibrary.Model.IMU6Data.Data.Header

 Headers 



---
## Type MdsLibrary.Model.IMU6Data.Values3D

 3D values 



---
#### Field MdsLibrary.Model.IMU6Data.Values3D.X

 X reading 



---
#### Field MdsLibrary.Model.IMU6Data.Values3D.Y

 Y reading 



---
#### Field MdsLibrary.Model.IMU6Data.Values3D.Z

 Z reading 



---
#### Method MdsLibrary.Model.IMU6Data.Values3D.#ctor(System.Double,System.Double,System.Double)

 Constructs Values3D 

|Name | Description |
|-----|------|
|x: |X value|
|y: |Y value|
|z: |Z value|


---
## Type MdsLibrary.Model.IMU9Data

 Response object for subscription to periodic 9-axis IMU measurements. 



---
#### Field MdsLibrary.Model.IMU9Data.body

 Response object for subscription to periodic 9-axis IMU measurements. 



---
#### Method MdsLibrary.Model.IMU9Data.#ctor(MdsLibrary.Model.IMU9Data.Data)

 Creates an IMU9Data object 

|Name | Description |
|-----|------|
|body: ||


---
## Type MdsLibrary.Model.IMU9Data.Data

 Container for IMU9 data readings 



---
#### Field MdsLibrary.Model.IMU9Data.Data.Timestamp

 Local timestamp of first measurement. 



---
#### Field MdsLibrary.Model.IMU9Data.Data.ArrayAcc

 Measured acceleration values (3D) in array. 



---
#### Field MdsLibrary.Model.IMU9Data.Data.ArrayGyro

 Measured angular velocity values (3D) in array. 



---
#### Field MdsLibrary.Model.IMU9Data.Data.ArrayMagn

 Measured magnetic field values (3D) in array. 



---
#### Field MdsLibrary.Model.IMU9Data.Data.Header

 Headers 



---
## Type MdsLibrary.Model.IMU9Data.Values3D

 3D values 



---
#### Field MdsLibrary.Model.IMU9Data.Values3D.X

 X reading 



---
#### Field MdsLibrary.Model.IMU9Data.Values3D.Y

 Y reading 



---
#### Field MdsLibrary.Model.IMU9Data.Values3D.Z

 Z reading 



---
#### Method MdsLibrary.Model.IMU9Data.Values3D.#ctor(System.Double,System.Double,System.Double)

 Constructs Values3D 

|Name | Description |
|-----|------|
|x: |X value|
|y: |Y value|
|z: |Z value|


---
## Type MdsLibrary.Model.IMUInfo

 Supported IMU sample rates and ranges. 



---
#### Field MdsLibrary.Model.IMUInfo.Info

 Supported IMU sample rates and ranges. 



---
## Type MdsLibrary.Model.IMUInfo.Content

 Supported sample rates and ranges. 



---
#### Field MdsLibrary.Model.IMUInfo.Content.SampleRates

 Available sample rates for IMU measurement. 



---
#### Field MdsLibrary.Model.IMUInfo.Content.AccRanges

 Available ranges for acceleration measurement. For example range value 2 means the range is -2...+2 G. 



---
#### Field MdsLibrary.Model.IMUInfo.Content.GyroRanges

 Available ranges for angular range measurement rate. For example range value 500 means the range is -500...+500 dps. 



---
#### Field MdsLibrary.Model.IMUInfo.Content.MagnRanges

 Available scales for magnetometer measurement. For example scale value 400 means the range is -400...+400 µT(microtesla). 



---
## Type MdsLibrary.Model.LedsResult

 Response for Get leds in the system and their state (on/off and possible color). 



---
#### Property MdsLibrary.Model.LedsResult.Content

 Response for Get leds in the system and their state (on/off and possible color). 



---
## Type MdsLibrary.Model.Content

 Response for Get leds in the system and their state (on/off and possible color). 



---
#### Property MdsLibrary.Model.Content.LedStates

 Array of LedState objects describing state of the Leds in the system 



---
## Type MdsLibrary.Model.LedState

 State of an Led 



---
#### Property MdsLibrary.Model.LedState.IsOn

 Led is on when true, otherwise off 



---
#### Property MdsLibrary.Model.LedState.LedColor

 Color of the Led: 'Red' : 0, 'Green' : 1, 'Blue' : 2 



---
## Type MdsLibrary.Model.LogEntriesResult

 Result from query for GetLogEntries 



---
#### Field MdsLibrary.Model.LogEntriesResult.LogEntries

 Result from query for GetLogEntries 



---
## Type MdsLibrary.Model.LogEntriesResult.Content

 Result content from query for GetLogEntries 



---
#### Field MdsLibrary.Model.LogEntriesResult.Content.Elements

 Array of Zero or more log entries describing log contents. Zero entries are received if log iteration has completed. 



---
## Type MdsLibrary.Model.LogEntriesResult.LogEntry

 Log Entry data 



---
#### Field MdsLibrary.Model.LogEntriesResult.LogEntry.Id

 Id of the log entry. 



---
#### Field MdsLibrary.Model.LogEntriesResult.LogEntry.ModificationTimestamp

 Timestamp of last modification to entry in seconds after 0:00 Jan 1st 1970 (UTC) without leap seconds. 



---
## Type MdsLibrary.Model.LogStatusResult

 State of the DataLogger component 



---
#### Field MdsLibrary.Model.LogStatusResult.LogStatus

 Current DataLogger status: 1: name: 'DATALOGGER_INVALID' description: Unknown or Invalid state prior to initialization or due to comm error. 2: name: 'DATALOGGER_READY' description: If we are ready for logging. 3: name: 'DATALOGGER_LOGGING' description: If we are logging data. 



---
## Type MdsLibrary.Model.MagnData

 Periodic magnetometer measurements 



---
#### Field MdsLibrary.Model.MagnData.body

 Periodic magnetometer measurements 



---
#### Method MdsLibrary.Model.MagnData.#ctor(MdsLibrary.Model.MagnData.Body)

 Builds a MagnData 

|Name | Description |
|-----|------|
|body: ||


---
## Type MdsLibrary.Model.MagnData.Body

 Contains data for Periodic magnetometer measurements 



---
#### Field MdsLibrary.Model.MagnData.Body.Timestamp

 Local timestamp of first measurement. 



---
#### Field MdsLibrary.Model.MagnData.Body.ArrayMagn

 Measured magnetic field values (3D) in array. 



---
#### Field MdsLibrary.Model.MagnData.Body.Header

 Headers 



---
## Type MdsLibrary.Model.MagnData.Values3D

 Measured magnetometer values (3D). 



---
#### Field MdsLibrary.Model.MagnData.Values3D.X

 X value 



---
#### Field MdsLibrary.Model.MagnData.Values3D.Y

 Y value 



---
#### Field MdsLibrary.Model.MagnData.Values3D.Z

 Z value 



---
#### Method MdsLibrary.Model.MagnData.Values3D.#ctor(System.Double,System.Double,System.Double)

 Creates a Values3D 

|Name | Description |
|-----|------|
|x: ||
|y: ||
|z: ||


---
## Type MdsLibrary.Model.MagnData.Headers

 Headers 



---
## Type MdsLibrary.Model.MagnInfo

 Available sample rates and scales for magnetometer measurement. 



---
#### Field MdsLibrary.Model.MagnInfo.Info

 Info on available sample rates and scales for magnetometer measurement. 



---
## Type MdsLibrary.Model.MagnInfo.Content

 Available sample rates and scales for magnetometer measurement. 



---
#### Field MdsLibrary.Model.MagnInfo.Content.SampleRates

 Available sample rates for magnetometer measurement. 



---
#### Field MdsLibrary.Model.MagnInfo.Content.Scale

 Available scales for magnetometer measurement. For example scale value 400 means the range is -400...+400 µT(microtesla). 



---
## Type MdsLibrary.Model.TimeResult

 Current time in number of microseconds since epoch 1.1.1970 (UTC). 



---
#### Field MdsLibrary.Model.TimeResult.Time

 Current time in number of microseconds since epoch 1.1.1970 (UTC). If not explicitly set, contains number of seconds since reset. 



---
## Type Api.MdsSubscription

 Contains context for a subscription to an MdsLib subscription resource 



---
#### Method Api.MdsSubscription.#ctor(System.Object)

 Creates a context for a subscription to an MdsLib subscription resource 

|Name | Description |
|-----|------|
|nativeSubscription: |Reference to the native MdsLib IMdsSubscription|


---
#### Method Api.MdsSubscription.Unsubscribe

 Unsubscribe from the resource 



---
## Type MovesenseImplementation

 Interface for Plugin.Movesense 



---
#### Method MovesenseImplementation.CreateLogEntryAsync(System.String)

 Create a new logbook entry resource (increment log Id). Returns the new log Id. 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.GetLogEntriesAsync(System.String)

 Get details of Logbook entries 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.DeleteLogEntriesAsync(System.String)

 Delete all the Logbook entries 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.GetAccInfoAsync(System.String)

 Get Accelerometer configuration 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.GetMagInfoAsync(System.String)

 Get Magnetometer configuration 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.GetBatteryLevelAsync(System.String)

 Get Battery level, CallAsync returns BatteryResult 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.GetLogbookDataAsync(System.String,System.Int32)

 Get data from a Logbook entry 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|logId: |Number of the entry to get|


---
#### Method MovesenseImplementation.GetLogbookDescriptorsAsync(System.String,System.Int32)

 Get Descriptors for a Logbook entry 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|logId: |Logbook entry to get|


---
#### Method MovesenseImplementation.GetDeviceInfoAsync(System.String)

 Get device info 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.GetGyroInfoAsync(System.String)

 Get Gyrometer configuration 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.GetIMUInfoAsync(System.String)

 Get IMU configuration 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.GetLedStateAsync(System.String,System.Int32)

 Get LedState for an LED 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|ledIndex: |Number of the Led|


---
#### Method MovesenseImplementation.SetLedStateAsync(System.String,System.Int32,System.Boolean,Plugin.Movesense.Api.LedColor)

 Sets state of an LED 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|ledIndex: |Index of the Led - use 0 for standard Movesense sensor|
|ledOn: |Set on or off|
|ledColor: |[optional]value from LedColor enumeration - default is LedColor.Red|


---
#### Method MovesenseImplementation.GetLedsStateAsync(System.String)

 Get state of all Leds in the system 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.GetLoggerStatusAsync(System.String)

 Get Logger status, CallAsync returns LogStatusResult object 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.SetLoggerStatusAsync(System.String,System.Boolean)

 Set state of the Datalogger 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|start: |Set true to start the datalogger, false to stop|


---
#### Method MovesenseImplementation.SetupLoggerAsync(System.String,System.Int32)

 Set configuration for the Datalogger - ONLY sets IMU9 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|freq: |Sampling rate, e.g. 26 for 26Hz|


---
#### Method MovesenseImplementation.GetTimeAsync(System.String)

 Gets current time in number of microseconds since epoch 1.1.1970 (UTC). If not explicitly set, contains number of seconds since reset. 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.SetTimeAsync(System.String)

 Set clock time on the device to sync with the time on the phone, as number of microseconds since epoch 1.1.1970 (UTC). 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|


---
#### Method MovesenseImplementation.SubscribeAccelerometerAsync(System.String,System.Action{MdsLibrary.Model.AccData},System.Int32)

 Subscribe to periodic linear acceleration measurements. 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|notificationCallback: |Callback function to receive the AccData|
|sampleRate: |Sampling rate, e.g. 26 for 26Hz|


---
#### Method MovesenseImplementation.SubscribeGyrometerAsync(System.String,System.Action{MdsLibrary.Model.GyroData},System.Int32)

 Subscribe to periodic Gyrometer data 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|notificationCallback: |Callback function to receive the GyroData|
|sampleRate: |Sampling rate, e.g. 26 for 26Hz|


---
#### Method MovesenseImplementation.SubscribeMagnetometerAsync(System.String,System.Action{MdsLibrary.Model.MagnData},System.Int32)

 Subscribe to periodic Magnetometer data measurements 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|notificationCallback: |Callback function to receive the MagnData|
|sampleRate: |Sampling rate, e.g. 26 for 26Hz|


---
#### Method MovesenseImplementation.SubscribeIMU6Async(System.String,System.Action{MdsLibrary.Model.IMU6Data},System.Int32)

 Subscribe to periodic 6-axis IMU measurements (Acc + Gyro). 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|notificationCallback: |Callback function to receive the IMU6Data|
|sampleRate: |Sampling rate, e.g. 26 for 26Hz|


---
#### Method MovesenseImplementation.SubscribeIMU9Async(System.String,System.Action{MdsLibrary.Model.IMU9Data},System.Int32)

 Subscribe to periodic 9-axis IMU measurements. 

|Name | Description |
|-----|------|
|deviceName: |Name of the device, e.g. Movesense 174430000051|
|notificationCallback: |Callback function to receive the IMU9Data|
|sampleRate: |Sampling rate, e.g. 26 for 26Hz|


---


