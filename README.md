<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# BlankDeviceUniqueIdentifier

[![Version](https://img.shields.io/github/v/release/AlianBlank/BlankDeviceUniqueIdentifier?label=version&color=green)](https://github.com/AlianBlank/BlankDeviceUniqueIdentifier/releases)
[![License](https://img.shields.io/badge/license-MIT+Apache%202.0-orange.svg)](LICENSE.md)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**All-in-One Solution for Indie Game Development · Empowering Indie Developers' Dreams**

[📖 Documentation](https://gameframex.doc.alianblank.com) • [🚀 Quick Start](#usage-examples)

---

🌐 **Language**: **English** | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | [한국어](README.ko.md)

---

</div>

A lightweight Unity3D plugin for retrieving device unique identifiers across Android and iOS platforms. It provides a unified C# API to access OAID, IDFA, IMEI, and a stable hardware fingerprint — with zero native dependencies on Android.

### Highlights

- **Pure C# on Android** — Uses `AndroidJavaClass` / `AndroidJavaObject` (JNI) to call system APIs and vendor SDKs directly. No Java code, no JAR files, no Gradle configuration needed.
- **Cross-platform** — Works seamlessly on Android, iOS, and the Unity Editor. Unsupported APIs gracefully fall back to `SystemInfo.deviceUniqueIdentifier`.
- **Vendor-wide OAID** — Supports MSA SDK, Huawei, Xiaomi, OPPO, vivo, and Samsung OAID retrieval on Android via reflection.
- **iOS IDFA & SSKeychain** — Retrieves IDFA via `ASIdentifierManager` with ATT support; persists device ID in the Keychain so it survives app reinstalls.
- **Zero mandatory permissions** — No permissions required. Optional permissions can be declared to improve identifier uniqueness.
- **Built-in caching** — All results are cached in `PlayerPrefs` after the first call, avoiding redundant system queries.

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

This project is licensed under the [Apache License 2.0](LICENSE.md).

```
Copyright 2023 ALianBlank (alianblank@outlook.com)

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
```
