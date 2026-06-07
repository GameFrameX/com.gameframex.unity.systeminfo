<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# BlankDeviceUniqueIdentifier

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.systeminfo)](https://github.com/GameFrameX/com.gameframex.unity.systeminfo/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.systeminfo)](https://github.com/GameFrameX/com.gameframex.unity.systeminfo/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams

<br />

[Documentation](https://gameframex.doc.alianblank.com) · [Quick Start](#quick-start) · QQ Group: 467608841 / 233840761

<br />

**English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

## Features

| API | Description | Return Value |
|-----|-------------|--------------|
| `DeviceGetOaid` | Get device OAID (Android only) | Raw OAID (stripped `-`, max 32 chars) |
| `DeviceGetIdfa` | Get device IDFA (iOS only) | Raw IDFA (stripped `-`, max 32 chars) |
| `DeviceGetImei` | Get device IMEI | Raw IMEI (stripped `-`, max 32 chars) |
| `DeviceUniqueIdentifier` | Get device unique machine ID | MD5 hash (32-char hex string) |

All APIs cache results via `PlayerPrefs`. System interfaces are not called again after the first retrieval.

## Platform Implementation

| Platform | `DeviceGetOaid` | `DeviceGetIdfa` | `DeviceGetImei` | `DeviceUniqueIdentifier` |
|----------|-----------------|-----------------|-----------------|--------------------------|
| Android | JNI reflection (MSA / Huawei / Xiaomi / OPPO / vivo / Samsung) | `SystemInfo.deviceUniqueIdentifier` (fallback) | JNI call to `TelephonyManager` | MD5 of IMEI + hardware info + Android ID + WLAN MAC + BT MAC |
| iOS | `SystemInfo.deviceUniqueIdentifier` (fallback) | `ASIdentifierManager.advertisingIdentifier` | native `__DeviceGetIMEI()` (IDFV) | native `DeviceUniqueId()` (SSKeychain) |
| Editor / Other | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` |

## Permissions (Optional)

This plugin **does not require any permissions**. It gracefully degrades without them. The plugin does not ship with an `AndroidManifest.xml`. The following permissions should be declared in the **consuming project's** `AndroidManifest.xml` as needed.

### Android

| Permission | Improves `DeviceUniqueIdentifier` uniqueness | Improves `DeviceGetImei` uniqueness |
|------------|------|------|
| `READ_PHONE_STATE` | IMEI included in hash | Can retrieve real IMEI |
| `ACCESS_WIFI_STATE` | WLAN MAC included in hash | - |
| `BLUETOOTH` | Bluetooth MAC included in hash | - |

```xml
<!-- Add to the consuming project's AndroidManifest.xml as needed -->
<uses-permission android:name="android.permission.READ_PHONE_STATE" />
<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
<uses-permission android:name="android.permission.BLUETOOTH" />
```

### iOS

| Config | Purpose |
|--------|---------|
| `NSUserTrackingUsageDescription` | ATT authorization description required for IDFA |

```xml
<!-- Add to the consuming project's Info.plist -->
<key>NSUserTrackingUsageDescription</key>
<string>Your advertising identifier will be used to provide better services</string>
```

## Usage Examples

```csharp
using GameFrameX.SystemInfo.Runtime;

// Get device OAID (Android only, falls back to SystemInfo.deviceUniqueIdentifier on iOS/Editor)
string oaid = BlankDeviceUniqueIdentifier.DeviceGetOaid;

// Get device IDFA (iOS only, falls back to SystemInfo.deviceUniqueIdentifier on Android/Editor)
string idfa = BlankDeviceUniqueIdentifier.DeviceGetIdfa;

// Get device IMEI
string imei = BlankDeviceUniqueIdentifier.DeviceGetImei;

// Get device unique identifier
string deviceId = BlankDeviceUniqueIdentifier.DeviceUniqueIdentifier;
```

### iOS IDFA Notes

IDFA requires user authorization via ATT (App Tracking Transparency). Add to `Info.plist` before use:

```xml
<key>NSUserTrackingUsageDescription</key>
<string>Your advertising identifier will be used to provide better services</string>
```

And request authorization before calling `DeviceGetIdfa`:

```csharp
#if UNITY_IOS || UNITY_IPHONE
// iOS 14+ requires ATT authorization first
if (UnityEngine.iOS.Device.systemVersion.CompareTo("14") >= 0)
{
    UnityEngine.iOS.Device.RequestUserAuthorization(UnityEngine.iOS.UserTracking.Authorization);
}
#endif
string idfa = BlankDeviceUniqueIdentifier.DeviceGetIdfa;
```

When unauthorized, `DeviceGetIdfa` returns an empty string without crashing.

## Directory Structure

```
Plugins/
  iOS/
    BlankDeviceUniqueIdentifier/
      AHDeviceUniqueIdentifier.h               # iOS native header
      AHDeviceUniqueIdentifier.mm              # iOS native implementation
      SSKeychain.h                              # SSKeychain keychain utility
      SSKeychain.m
Runtime/
  BlankDeviceUniqueIdentifier.cs               # C# unified interface (Android uses JNI to call system APIs directly, no Java/JAR needed)
```

> On Android, `AndroidJavaClass` / `AndroidJavaObject` is used to call system APIs and vendor SDKs directly — no Java code compilation or JAR files required.

## License

See [LICENSE.md](LICENSE.md) for license information.
