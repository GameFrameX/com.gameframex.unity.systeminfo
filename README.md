 # BlankDeviceUniqueIdentifier

用于在 Unity3D 中获取 Android 和 iOS 平台设备唯一标识符的插件。

## 功能

| API | 说明 | 返回值 |
|-----|------|--------|
| `DeviceGetOaid` | 获取设备 OAID（Android 独有） | OAID 原始值（去 `-`，最长 32 位） |
| `DeviceGetIdfa` | 获取设备 IDFA（iOS 独有） | IDFA 原始值（去 `-`，最长 32 位） |
| `DeviceGetImei` | 获取设备 IMEI | IMEI 原始值（去 `-`，最长 32 位） |
| `DeviceUniqueIdentifier` | 获取设备唯一机器码 | MD5 哈希值（32 位十六进制字符串） |

所有 API 均通过 `PlayerPrefs` 缓存结果，首次获取后不再重复调用系统接口。

## 平台实现

| 平台 | `DeviceGetOaid` | `DeviceGetIdfa` | `DeviceGetImei` | `DeviceUniqueIdentifier` |
|------|-----------------|-----------------|-----------------|--------------------------|
| Android | JNI 反射获取（MSA / 华为 / 小米 / OPPO / vivo / 三星） | `SystemInfo.deviceUniqueIdentifier`（降级） | JNI 调用 `TelephonyManager` | IMEI + 硬件信息 + Android ID + WLAN MAC + BT MAC 的 MD5 |
| iOS | `SystemInfo.deviceUniqueIdentifier`（降级） | `ASIdentifierManager.advertisingIdentifier` | native `__DeviceGetIMEI()` (IDFV) | native `DeviceUniqueId()` (SSKeychain) |
| Editor / 其他 | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` |

## 权限（可选）

本插件**不强制要求任何权限**，无权限时降级运行不会崩溃。插件不自带 `AndroidManifest.xml`，以下权限需由**使用方项目**在自己的 `AndroidManifest.xml` 中按需声明。

### Android

| 权限 | 提升 `DeviceUniqueIdentifier` 唯一性 | 提升 `DeviceGetImei` 唯一性 |
|------|------|------|
| `READ_PHONE_STATE` | IMEI 参与哈希计算 | 可获取真实 IMEI |
| `ACCESS_WIFI_STATE` | WLAN MAC 参与哈希计算 | - |
| `BLUETOOTH` | 蓝牙 MAC 参与哈希计算 | - |

```xml
<!-- 在使用方项目的 AndroidManifest.xml 中按需添加 -->
<uses-permission android:name="android.permission.READ_PHONE_STATE" />
<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
<uses-permission android:name="android.permission.BLUETOOTH" />
```

### iOS

| 配置 | 用途 |
|------|------|
| `NSUserTrackingUsageDescription` | IDFA 需要的 ATT 授权描述 |

```xml
<!-- 在使用方项目的 Info.plist 中添加 -->
<key>NSUserTrackingUsageDescription</key>
<string>您的广告标识符将用于提供更好的服务</string>
```

## 使用示例

```csharp
using BlankSystemInfo.Runtime;

// 获取设备 OAID（Android 独有，iOS/Editor 降级为 SystemInfo.deviceUniqueIdentifier）
string oaid = BlankDeviceUniqueIdentifier.DeviceGetOaid;

// 获取设备 IDFA（iOS 独有，Android/Editor 降级为 SystemInfo.deviceUniqueIdentifier）
string idfa = BlankDeviceUniqueIdentifier.DeviceGetIdfa;

// 获取设备 IMEI
string imei = BlankDeviceUniqueIdentifier.DeviceGetImei;

// 获取设备唯一标识符
string deviceId = BlankDeviceUniqueIdentifier.DeviceUniqueIdentifier;
```

### iOS IDFA 注意事项

IDFA 需要用户授权 ATT（App Tracking Transparency）。使用前需在 `Info.plist` 中添加：

```xml
<key>NSUserTrackingUsageDescription</key>
<string>您的广告标识符将用于提供更好的服务</string>
```

并在调用 `DeviceGetIdfa` 前请求授权：

```csharp
#if UNITY_IOS || UNITY_IPHONE
// iOS 14+ 需要先请求 ATT 授权
if (UnityEngine.iOS.Device.systemVersion.CompareTo("14") >= 0)
{
    UnityEngine.iOS.Device.RequestUserAuthorization(UnityEngine.iOS.UserTracking.Authorization);
}
#endif
string idfa = BlankDeviceUniqueIdentifier.DeviceGetIdfa;
```

未授权时 `DeviceGetIdfa` 返回空串，不会崩溃。

## 目录结构

```
Plugins/
  iOS/
    BlankDeviceUniqueIdentifier/
      AHDeviceUniqueIdentifier.h               # iOS native 头文件
      AHDeviceUniqueIdentifier.mm              # iOS native 实现
      SSKeychain.h                              # SSKeychain 钥匙串工具
      SSKeychain.m
Runtime/
  BlankDeviceUniqueIdentifier.cs               # C# 统一接口（Android 通过 JNI 直接调用系统 API，无需 Java/JAR）
```

> Android 端通过 `AndroidJavaClass` / `AndroidJavaObject` 直接调用系统 API 和厂商 SDK，无需编译和引入任何 Java 代码或 JAR 文件。
