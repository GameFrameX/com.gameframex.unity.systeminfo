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
    char * gameframex_device_unique_id();
    char * gameframex_device_get_imei();
    char * gameframex_device_get_idfa();
#ifdef __cplusplus
}
#endif


@end
