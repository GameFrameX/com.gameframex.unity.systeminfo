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

@implementation AHDeviceUniqueIdentifier


+ (NSString *) GenUUID
{
    
    CFUUIDRef uuid_ref = CFUUIDCreate(NULL);
    
    CFStringRef uuid_string_ref= CFUUIDCreateString(NULL, uuid_ref);
    
    CFRelease(uuid_ref);
    
    NSString *uuid = [NSString stringWithString:(__bridge NSString*)uuid_string_ref];
    
    CFRelease(uuid_string_ref);
    
    return uuid;
}

+ (NSString *)getuuid{
    
    NSString * serviceName =@"com.alianhome.uuid";
    
    NSString * accountName =@"blank";
        // get value
    NSString * value = [SSKeychain passwordForService:serviceName account:accountName];
    
    
    if (value==nil) {
        
            // gen uuid
        NSString * uuid =[self GenUUID];

            // save uuid
        [SSKeychain setPassword:uuid forService:serviceName account:accountName];
        return uuid;
    }else{
        return value;
    }    
}
    
+ (NSString *) deviceGetIMEI
    {
        //获取设备唯一ID
        NSString *const DEVICE_ID_KEY = @"device_id";
       
        NSString *bundleID = [[[NSBundle mainBundle] infoDictionary] objectForKey:@"CFBundleIdentifier"];
        
        NSString *deviceIdKey = [NSString stringWithFormat:@"%@_%@",bundleID,DEVICE_ID_KEY];
        
        NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
        NSString * currentDeviceUUIDStr = [userDefaults objectForKey:deviceIdKey];
       
        if(currentDeviceUUIDStr == nil || [currentDeviceUUIDStr isEqualToString:@""])
        {
           
            NSUUID * currentDeviceUUID = [UIDevice currentDevice].identifierForVendor;
            currentDeviceUUIDStr = currentDeviceUUID.UUIDString;
            currentDeviceUUIDStr = [currentDeviceUUIDStr stringByReplacingOccurrencesOfString:@"-" withString:@""];
            currentDeviceUUIDStr = [currentDeviceUUIDStr lowercaseString];
            
            [userDefaults setObject:currentDeviceUUIDStr forKey:deviceIdKey];
        }
        
        [userDefaults synchronize];
        
        return currentDeviceUUIDStr;
    }


char * gameframex_device_get_imei(){
    
        // get uuid
    const char *uuid = [[AHDeviceUniqueIdentifier deviceGetIMEI] UTF8String];
        // alloc
    char *result = (char*)malloc(strlen(uuid)+1);
        // copy
    strcpy(result, uuid);
        // return
    return result;
}

char * gameframex_device_unique_id(){

        // get uuid
    const char *uuid = [[AHDeviceUniqueIdentifier getuuid] UTF8String];
        // alloc
    char *result = (char*)malloc(strlen(uuid)+1);
        // copy
    strcpy(result, uuid);
        // return
    return result;
}

char * gameframex_device_get_idfa(){
    NSString *idfa = @"";
    if (@available(iOS 14, *)) {
        if ([ATTrackingManager trackingAuthorizationStatus] == ATTrackingManagerAuthorizationStatusAuthorized) {
            idfa = [[ASIdentifierManager sharedManager].advertisingIdentifier UUIDString];
        }
    } else {
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
