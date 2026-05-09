//
//  AHDeviceUniqueIdentifier.mm
//
//  Created by GameframeX(AlianBlank) on 16/8/16.
//  https://github.com/gameframex
//  https://github.com/alianblank
//

#import <Foundation/Foundation.h>

@interface AHDeviceUniqueIdentifier : NSObject

#ifdef __cplusplus
extern "C" {
#endif
    char * DeviceUniqueId();
    char * __DeviceGetIMEI();
    char * __DeviceGetIDFA();
#ifdef __cplusplus
}
#endif


@end
