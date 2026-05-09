//
//  AHDeviceUniqueIdentifier.mm
//
//  Created by GameframeX(AlianBlank) on 16/8/16.
//  https://github.com/gameframex
//  https://github.com/alianblank
//

#import <Foundation/Foundation.h>

// 设备唯一标识符工具类 / Device unique identifier utility class
@interface AHDeviceUniqueIdentifier : NSObject

#ifdef __cplusplus
extern "C" {
#endif
    // 获取设备唯一标识符（UUID，通过 Keychain 持久化）/ Get device unique identifier (UUID, persisted via Keychain)
    char * gameframex_device_unique_id();
    // 获取设备 IMEI（iOS 使用 identifierForVendor）/ Get device IMEI (iOS uses identifierForVendor)
    char * gameframex_device_get_imei();
    // 获取设备 IDFA（需用户授权 ATT）/ Get device IDFA (requires user ATT authorization)
    char * gameframex_device_get_idfa();
#ifdef __cplusplus
}
#endif


@end
