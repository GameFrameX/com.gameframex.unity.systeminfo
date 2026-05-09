//
//  AHDeviceUniqueIdentifier.mm
//
//  Created by GameframeX(AlianBlank) on 16/8/16.
//  https://github.com/gameframex
//  https://github.com/alianblank
//

#import "AHDeviceUniqueIdentifier.h"
#import "SSKeychain.h"
#import <AdSupport/AdSupport.h>
#import <AppTrackingTransparency/AppTrackingTransparency.h>

// 设备唯一标识符工具类实现 / Device unique identifier utility implementation
@implementation AHDeviceUniqueIdentifier


// 生成 UUID / Generate UUID
+ (NSString *) GenUUID
{

    CFUUIDRef uuid_ref = CFUUIDCreate(NULL);

    CFStringRef uuid_string_ref= CFUUIDCreateString(NULL, uuid_ref);

    CFRelease(uuid_ref);

    NSString *uuid = [NSString stringWithString:(__bridge NSString*)uuid_string_ref];

    CFRelease(uuid_string_ref);

    return uuid;
}、

// 获取或创建 UUID，通过 Keychain 持久化存储 / Get or create UUID, persisted via Keychain
+ (NSString *)getuuid{

    NSString * serviceName =@"com.alianhome.uuid";

    NSString * accountName =@"blank";
        // 读取已存储的值 / Read stored value
    NSString * value = [SSKeychain passwordForService:serviceName account:accountName];


    if (value==nil) {

            // 生成新 UUID / Generate new UUID
        NSString * uuid =[self GenUUID];

            // 保存到 Keychain / Save to Keychain
        [SSKeychain setPassword:uuid forService:serviceName account:accountName];
        return uuid;
    }else{
        return value;
    }
}

// 获取设备 IMEI（使用 identifierForVendor）/ Get device IMEI (uses identifierForVendor)
+ (NSString *) deviceGetIMEI
    {
        NSString *const DEVICE_ID_KEY = @"device_id";

        NSString *bundleID = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleIdentifier"];

        NSString *deviceIdKey = [NSString stringWithFormat:@"%@_%@",bundleID,DEVICE_ID_KEY];

        NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
        NSString * currentDeviceUUIDStr = [userDefaults objectForKey:deviceIdKey];

        if(currentDeviceUUIDStr == nil || [currentDeviceUUIDStr isEqualToString:@""])
        {

            NSUUID * currentDeviceUUID = [UIDevice currentDevice].identifierForVendor;
            currentDeviceUUIDStr = currentDeviceUUID.UUIDString;
            // 移除连字符并转为小写 / Remove dashes and convert to lowercase
            currentDeviceUUIDStr = [currentDeviceUUIDStr stringByReplacingOccurrencesOfString:@"-" withString:@""];
            currentDeviceUUIDStr = [currentDeviceUUIDStr lowercaseString];

            [userDefaults setObject:currentDeviceUUIDStr forKey:deviceIdKey];
        }

        [userDefaults synchronize];

        return currentDeviceUUIDStr;
    }


// C 函数：获取设备 IMEI，供 Unity P/Invoke 调用 / C function: get device IMEI for Unity P/Invoke
char * gameframex_device_get_imei(){

        // 获取 identifierForVendor / Get identifierForVendor
    const char *uuid = [[AHDeviceUniqueIdentifier deviceGetIMEI] UTF8String];
        // 分配内存 / Allocate memory
    char *result = (char*)malloc(strlen(uuid)+1);
        // 复制字符串 / Copy string
    strcpy(result, uuid);
        // 返回结果 / Return result
    return result;
}

// C 函数：获取设备唯一标识符（UUID），供 Unity P/Invoke 调用 / C function: get device unique identifier (UUID) for Unity P/Invoke
char * gameframex_device_unique_id(){

        // 获取 Keychain UUID / Get Keychain UUID
    const char *uuid = [[AHDeviceUniqueIdentifier getuuid] UTF8String];
        // 分配内存 / Allocate memory
    char *result = (char*)malloc(strlen(uuid)+1);
        // 复制字符串 / Copy string
    strcpy(result, uuid);
        // 返回结果 / Return result
    return result;
}

// C 函数：获取设备 IDFA，供 Unity P/Invoke 调用 / C function: get device IDFA for Unity P/Invoke
char * gameframex_device_get_idfa(){
    NSString *idfa = @"";
    // iOS 14+：检查 ATT 授权状态 / iOS 14+: check ATT authorization status
    if (@available(iOS 14, *)) {
        if ([ATTrackingManager trackingAuthorizationStatus] == ATTrackingManagerAuthorizationStatusAuthorized) {
            idfa = [[ASIdentifierManager sharedManager].advertisingIdentifier UUIDString];
        }
    } else {
        // iOS 14 以下：检查广告跟踪开关 / Below iOS 14: check advertising tracking enabled
        if ([ASIdentifierManager sharedManager].isAdvertisingTrackingEnabled) {
            idfa = [[ASIdentifierManager sharedManager].advertisingIdentifier UUIDString];
        }
    }
    const char *cstr = [idfa UTF8String];
    char *result = (char*)malloc(strlen(cstr)+1);
    strcpy(result, cstr);
    return result;
}

@end
