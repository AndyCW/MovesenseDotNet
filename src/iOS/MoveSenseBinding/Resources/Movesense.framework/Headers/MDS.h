//
//  MDS.h
//
//  Copyright (c) 2017 Suunto Oy. All rights reserved.
//
//

#pragma once
#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

extern NSString *const MovesenseServiceUUID;

typedef NS_ENUM(NSInteger, MDSResponseMethod)
{
    MDSResponseMethod_UNKNOWN,
    MDSResponseMethod_GET,
    MDSResponseMethod_POST,
    MDSResponseMethod_PUT,
    MDSResponseMethod_DELETE,
    MDSResponseMethod_SUBSCRIBE,
    MDSResponseMethod_UNSUBSCRIBE,
    MDSResponseMethod_REGISTER,
    MDSResponseMethod_UNREGISTER
};

/**
 A conveniency wrapper for easier handling of the responses received from the device through the MDS.
 */
@interface MDSResponse : NSObject
@property (nonatomic, readonly) NSDictionary *header;   //!< Response header defining the content
@property (nonatomic, readonly) NSData *bodyData;   //!< Response body, mainly JSON
@property (nonatomic, readonly, nullable) NSDictionary *bodyDictionary; //!< Response body converted as a dictionary
@property (nonatomic, readonly) NSString *contentType;  //!< Response content type
@property (nonatomic, readonly) NSInteger statusCode;   //!< HTTP like response status code
@property (nonatomic, readonly) MDSResponseMethod method;   //!< Response method

- (NSData *)getStreamData; // Returns base64 decoded "Content"-field if available
@end

typedef void(^MDSResponseBlock)(MDSResponse *response);

/**
 A conveniency wrapper for easier handling of the events received from the device through the MDS.
 */
@interface MDSEvent : NSObject
@property (nonatomic, readonly) NSDictionary *header;   //!< Response header defining the content
@property (nonatomic, readonly) NSData *bodyData;   //!< Response body, mainly JSON
@property (nonatomic, readonly, nullable) NSDictionary *bodyDictionary; //!< Response body converted as a dictionary
@property (nonatomic, readonly) NSString *contentType;  //!< Response content type
@end

typedef void(^MDSEventBlock)(MDSEvent *event);

/**
 A conveniency wrapper for easier handling of the responses to inetgw tunnel requests.
 */
@interface MDSTunnelResponse : NSObject
@property (nonatomic, readonly) NSInteger status;   //!< HTTP like response status code
@property (nonatomic, readonly) NSString *response; //!< Response string defining the content

- (instancetype)initWithStatus:(NSInteger)status response:(nonnull NSString *)response;
@end

/**
 A conveniency wrapper for easier handling of the requests received from the device through the MDS.
 */
@interface MDSTunnelRequest : NSObject
@property (nonatomic, readonly) NSDictionary *header;   //!< Request header defining the content
@property (nonatomic, readonly) NSData *bodyData;   //!< Request body, mainly JSON
@property (nonatomic, readonly, nullable) NSDictionary *bodyDictionary; //!< Request body converted as a dictionary
@property (nonatomic, readonly) NSString *contentType;  //!< Request content type
@end

typedef MDSTunnelResponse * _Nullable(^MDSTunnelRequestBlock)(MDSTunnelRequest *request);


typedef NS_ENUM(NSUInteger, MDSRequestMethod)
{
    MDSRequestMethod_UNKNOWN,
    MDSRequestMethod_GET,
    MDSRequestMethod_POST,
    MDSRequestMethod_PUT,
    MDSRequestMethod_DELETE,
    MDSRequestMethod_SUBSCRIBE,
    MDSRequestMethod_UNSUBSCRIBE,
};

/**
 A conveniency wrapper for easier handling of the responses to registered resource requests.
 */
@interface MDSResourceRequestResponse : NSObject
@property (nonatomic, readonly) NSInteger status;  //!< HTTP like response status code
@property (nonatomic, readonly) BOOL isStream;     //!< If true the response is a stream data buffer
@property (nonatomic, readonly) NSData *response;  //!< Response (text message in case of an error or if successful either JSON string or just data buffer if stream is true)

- (instancetype)initWithStatus:(NSInteger)status message:(nonnull NSString *)response;
- (instancetype)initWithStatus:(NSInteger)status json:(nonnull NSString *)response;
- (instancetype)initWithStatus:(NSInteger)status streamData:(nonnull NSData *)response;
@end

@interface MDSResourceRequest : NSObject
@property (nonatomic, readonly) MDSRequestMethod method; //!< Request method
@property (nonatomic, readonly) NSDictionary *header;    //!< Request header defining the content
@property (nonatomic, readonly) NSString *contentType;   //!< Request content type
@property (nonatomic, readonly) NSData *bodyData;        //!< Request content/body, mainly JSON
@property (nonatomic, readonly) NSData *streamData;      //!< in PUT/POST stream request the data is here (bodyData JSON contains total length and transmitted information)
@property (nonatomic, readonly, nullable) NSDictionary *bodyDictionary; //!< Request body (if content type is "application/json") converted as a dictionary
@end

typedef MDSResourceRequestResponse * _Nullable(^MDSResourceRequestBlock)(MDSResourceRequest *request);


/**
 MDSConnectivityServiceDelegate for notifying the delegate
 */
@protocol MDSConnectivityServiceDelegate <NSObject>
@optional
// Function that is called when connectivity errors are generated from WBCentral due to BLE disconnect.
// The reason for disconnection is part of the error.
- (void)didFailToConnectWithError:(const NSError *)error;
@end

/**
 MDS API class
 */
@interface MDSWrapper : NSObject
{
}

@property(nonatomic, weak) id<MDSConnectivityServiceDelegate> delegate;

- (instancetype)init;

/*
 * Provide external configuration bundle in initiliser if needed.
 */
- (instancetype)init:(NSBundle *)configBundle;

/*
 * BleService tries to the connect asynchronously, and then discovers device's services and characteristics.
 * Result is aggregate of the whole sequence (i.e. internally any phase may return an error and thus end the sequence).
 * Shortly after that the device will become visible through MDS::ConnectedDevices event (if subscribed).
 */

/**
 Starts connection attempt to a peripheral with given @c uuid
 Though there is a resource (MDS/ConnectedDevices) in MDS that can be subscribed for notifying (MDSEvent) the succesfull connection and disconnection.
 Note that there is no timeout - connection may be created after long period of time (even hours) after it has been requested.
 @param uuid UUID of the peripheral
 */
- (void)connectPeripheralWithUUID:(NSUUID *)uuid;

/**
 Starts disconnect attempt from a peripheral with given @c uuid
 If peripheral was connected when this method was called and MDS resource (MDS/ConnectedDevices) was subscribed MDS will send an MDSEvent when the disconnect either succeeds or fails.
 This method can also be used to cancel a connection attempt previously started with @c connectPeripheralWithUUID: even if
 connection has not yet been established
 @param uuid UUID of the peripheral
 @return @c YES, if peripheral was found with given @c uuid and the request to cancel/close the connection to that peripheral was sent successfully. @c NO if the peripheral was not found, or it was found but it was already disconnected.
 */
- (BOOL)disconnectPeripheralWithUUID:(NSUUID *)uuid;

/**
 Prevent receiver from automatically reconnecting to device with given @c uuid after it has been disconnected.
 If this method is not called, the default behaviour is to reconnect to device, also when the app is suspended in the background.
 @param uuid UUID of the device for which auto-reconnect should be disabled. 
 */
- (void)disableAutoReconnectForDeviceWithUUID:(NSUUID *)uuid;

/**
 Prevent receiver from automatically reconnecting to device with given @c serial after it has been disconnected.
 If this method is not called, the default behaviour is to reconnect to device, also when the app is suspended in the background.
 @param serial Serial number of the device for which auto-reconnect should be disabled. Duplicate serials will be ignored. Cannot be nil.
 */
- (void)disableAutoReconnectForDeviceWithSerial:(NSString *)serial;

/*
 Subscriptions are way to receive callback calls for desired events in the device.
 */

/**
 Initiates a subscriptions request to a connected peripheral with given @c resource path, the device identification is part of the path.
 Calls @c onEvent callback when a new event for the subscription arrives from the device.
 Multiple subscriptions for the same path (paths includes device identification part) with different contract is not supported thus doUpdateSubsciption
 should be used to change subscription parameters, but note that it will change the parameters of ALL existing subscriptions for that path.
 @param resourcePath Subscription resource path of the peripheral device (including device identification e.g. "<serial>/Dev/Time")
 @param contract Subscription parameters (i.e. contract) related to the subscribed resource
 @param response The callback function for the response of this request
 @param onEvent The callback function for the received event
 */
- (void)doSubscribe:(NSString *)resourcePath contract:(NSDictionary*)contract response:(MDSResponseBlock)response onEvent:(MDSEventBlock)onEvent;

/**
 Updates parameters for ALL existing subscriptions of the given @c resource path, the device identification is part of the path.
 @param resourcePath Subscription resource path of the peripheral device (including device identification e.g. "<serial>/Dev/Time")
 @param contract Subscription parameters (i.e. contract) related to the subscribed resource
 @param completion The callback function for the response of this request (error if there is no existing subscription for the given path)
 */
- (void)doUpdateSubscription:(NSString *)resourcePath contract:(NSDictionary*)contract completion:(MDSResponseBlock)completion;

/**
 Unsubscribes an existing subscription to a connected peripheral with given @c resource path, the device identification is part of the path.
 @param resourcePath Subscription resource path of the peripheral device (including device identification e.g. "<serial>/Dev/Time")
 */
- (void)doUnsubscribe:(NSString *)resourcePath;

/**
 Registers a client side resource with given @c resource path and provides a handler for it.
 Calls @c onRequest callback when a new device initiated request arrives through MDS.
 Multiple registrations for the same path are not supported.
 @param resourcePath Resource path of the registered resource, i.e. name of the resource
 @param contract Resource parameters (i.e. contract) related to the registered resource
 @param response The callback function for the response of this request
 @param onRequest The callback function for the received device side request
 */
- (void)doRegisterResource:(NSString *)resourcePath contract:(NSDictionary *)contract response:(MDSResponseBlock)response onRequest:(MDSTunnelRequestBlock)onRequest;

/**
 Registers a client side Whiteboard resource with given @c metadata binary file and provides a handler for it.
 Calls @c onRequest callback when a new request arrives through MDS.
 Multiple registrations for the same path are not supported.
 @param resourceMetadataBinary Url for the Whiteboard metadata binary file (.wbr) which specifies the resource and its properties
 @param response The callback function for the response of this request
 @param onRequest The callback function for the received request
 */
- (void)doRegisterResource:(NSURL *)resourceMetadataBinary response:(MDSResponseBlock)response onRequest:(MDSResourceRequestBlock)onRequest;

/**
 Unregisters a client side resource with given @c resource path.
 @param resourceMetadataBinary Url of the previously registered resource
 @param response The callback function for the response of this request
 */
- (void)doUnregisterResource:(NSURL *)resourceMetadataBinary response:(MDSResponseBlock)response;

/**
 Sends a notification for the subscribers of the @c resourcePath.
 @param resourcePath Resource path of the registered resource
 @param contract Resource parameters (i.e. contract) related to the registered resource subscription
 */
- (void)doSendResourceNotification:(NSString *)resourcePath contract:(NSDictionary *)contract;

/**
 REST like commands to access the resource on the device, each of them have the similar parameters.
 @param resourcePath Subscription resource path of the peripheral device
 @param contract Subscription parameters (i.e. contract) related to the subscribed resource
 @param completion The callback handler for response
 */
- (void)doGet:(NSString *)resourcePath contract:(NSDictionary *)contract completion:(MDSResponseBlock)completion;
- (void)doPut:(NSString *)resourcePath contract:(NSDictionary *)contract completion:(MDSResponseBlock)completion;
- (void)doPost:(NSString *)resourcePath contract:(NSDictionary *)contract completion:(MDSResponseBlock)completion;
- (void)doDelete:(NSString *)resourcePath contract:(NSDictionary *)contract completion:(MDSResponseBlock)completion;

/**
 Controlled deactivation of the MDS service
 */
- (void)deactivate;

@end

NS_ASSUME_NONNULL_END
