#ifdef __OBJC__
#import <UIKit/UIKit.h>
#else
#ifndef FOUNDATION_EXPORT
#if defined(__cplusplus)
#define FOUNDATION_EXPORT extern "C"
#else
#define FOUNDATION_EXPORT extern
#endif
#endif
#endif

#import "MDS.h"
#import "Movesense-Bridging-Header.h"
#import "Movesense.h"

FOUNDATION_EXPORT double MovesenseVersionNumber;
FOUNDATION_EXPORT const unsigned char MovesenseVersionString[];

