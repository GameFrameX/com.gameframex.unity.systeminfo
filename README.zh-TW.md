<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# BlankDeviceUniqueIdentifier

[![Version](https://img.shields.io/github/v/release/AlianBlank/BlankDeviceUniqueIdentifier?label=version&color=green)](https://github.com/AlianBlank/BlankDeviceUniqueIdentifier/releases)
[![License](https://img.shields.io/badge/license-MIT+Apache%202.0-orange.svg)](LICENSE.md)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使**

[📖 文檔](https://gameframex.doc.alianblank.com) • [🚀 快速開始](#使用範例)

---

🌐 **語言**: [English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

---

</div>

輕量級 Unity3D 設備唯一標識符獲取插件，支援 Android 和 iOS 平台。提供統一的 C# API 存取 OAID、IDFA、IMEI 和穩定的硬體指紋，Android 端零原生依賴。

### 核心特性

- **Android 純 C# 實作** — 透過 `AndroidJavaClass` / `AndroidJavaObject`（JNI）直接呼叫系統 API 和廠商 SDK，無需 Java 程式碼、JAR 檔案或 Gradle 配置。
- **跨平台** — Android、iOS、Unity Editor 統一支援。不支援的平台 API 會自動降級為 `SystemInfo.deviceUniqueIdentifier`。
- **多廠商 OAID** — 透過反射支援 MSA SDK、華為、小米、OPPO、vivo、三星等主流廠商的 OAID 獲取。
- **iOS IDFA 與 SSKeychain** — 透過 `ASIdentifierManager` 獲取 IDFA 並支援 ATT 授權；設備 ID 持久化到 Keychain，應用重裝後依然有效。
- **零強制權限** — 無需任何權限即可運行。聲明可選權限可提升標識符唯一性。
- **內建快取** — 所有 API 首次呼叫後透過 `PlayerPrefs` 快取結果，避免重複查詢系統介面。

## 功能

| API | 說明 | 返回值 |
|-----|------|--------|
| `DeviceGetOaid` | 獲取設備 OAID（Android 獨有） | OAID 原始值（去 `-`，最長 32 位） |
| `DeviceGetIdfa` | 獲取設備 IDFA（iOS 獨有） | IDFA 原始值（去 `-`，最長 32 位） |
| `DeviceGetImei` | 獲取設備 IMEI | IMEI 原始值（去 `-`，最長 32 位） |
| `DeviceUniqueIdentifier` | 獲取設備唯一機器碼 | MD5 雜湊值（32 位十六進制字串） |

所有 API 均通過 `PlayerPrefs` 快取結果，首次獲取後不再重複呼叫系統介面。

## 平台實現

| 平台 | `DeviceGetOaid` | `DeviceGetIdfa` | `DeviceGetImei` | `DeviceUniqueIdentifier` |
|------|-----------------|-----------------|-----------------|--------------------------|
| Android | JNI 反射獲取（MSA / 華為 / 小米 / OPPO / vivo / 三星） | `SystemInfo.deviceUniqueIdentifier`（降級） | JNI 呼叫 `TelephonyManager` | IMEI + 硬體資訊 + Android ID + WLAN MAC + BT MAC 的 MD5 |
| iOS | `SystemInfo.deviceUniqueIdentifier`（降級） | `ASIdentifierManager.advertisingIdentifier` | native `__DeviceGetIMEI()` (IDFV) | native `DeviceUniqueId()` (SSKeychain) |
| Editor / 其他 | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` |

## 權限（可選）

本插件**不強制要求任何權限**，無權限時降級運行不會崩潰。插件不自帶 `AndroidManifest.xml`，以下權限需由**使用方專案**在自己的 `AndroidManifest.xml` 中按需聲明。

### Android

| 權限 | 提升 `DeviceUniqueIdentifier` 唯一性 | 提升 `DeviceGetImei` 唯一性 |
|------|------|------|
| `READ_PHONE_STATE` | IMEI 參與雜湊計算 | 可獲取真實 IMEI |
| `ACCESS_WIFI_STATE` | WLAN MAC 參與雜湊計算 | - |
| `BLUETOOTH` | 藍牙 MAC 參與雜湊計算 | - |

```xml
<!-- 在使用方專案的 AndroidManifest.xml 中按需添加 -->
<uses-permission android:name="android.permission.READ_PHONE_STATE" />
<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
<uses-permission android:name="android.permission.BLUETOOTH" />
```

### iOS

| 配置 | 用途 |
|------|------|
| `NSUserTrackingUsageDescription` | IDFA 需要的 ATT 授權描述 |

```xml
<!-- 在使用方專案的 Info.plist 中添加 -->
<key>NSUserTrackingUsageDescription</key>
<string>您的廣告標識符將用於提供更好的服務</string>
```

## 使用範例

```csharp
using GameFrameX.SystemInfo.Runtime;

// 獲取設備 OAID（Android 獨有，iOS/Editor 降級為 SystemInfo.deviceUniqueIdentifier）
string oaid = BlankDeviceUniqueIdentifier.DeviceGetOaid;

// 獲取設備 IDFA（iOS 獨有，Android/Editor 降級為 SystemInfo.deviceUniqueIdentifier）
string idfa = BlankDeviceUniqueIdentifier.DeviceGetIdfa;

// 獲取設備 IMEI
string imei = BlankDeviceUniqueIdentifier.DeviceGetImei;

// 獲取設備唯一標識符
string deviceId = BlankDeviceUniqueIdentifier.DeviceUniqueIdentifier;
```

### iOS IDFA 注意事項

IDFA 需要使用者授權 ATT（App Tracking Transparency）。使用前需在 `Info.plist` 中添加：

```xml
<key>NSUserTrackingUsageDescription</key>
<string>您的廣告標識符將用於提供更好的服務</string>
```

並在呼叫 `DeviceGetIdfa` 前請求授權：

```csharp
#if UNITY_IOS || UNITY_IPHONE
// iOS 14+ 需要先請求 ATT 授權
if (UnityEngine.iOS.Device.systemVersion.CompareTo("14") >= 0)
{
    UnityEngine.iOS.Device.RequestUserAuthorization(UnityEngine.iOS.UserTracking.Authorization);
}
#endif
string idfa = BlankDeviceUniqueIdentifier.DeviceGetIdfa;
```

未授權時 `DeviceGetIdfa` 返回空串，不會崩潰。

## 目錄結構

```
Plugins/
  iOS/
    BlankDeviceUniqueIdentifier/
      AHDeviceUniqueIdentifier.h               # iOS native 標頭檔
      AHDeviceUniqueIdentifier.mm              # iOS native 實作
      SSKeychain.h                              # SSKeychain 鑰匙串工具
      SSKeychain.m
Runtime/
  BlankDeviceUniqueIdentifier.cs               # C# 統一介面（Android 透過 JNI 直接呼叫系統 API，無需 Java/JAR）
```

> Android 端透過 `AndroidJavaClass` / `AndroidJavaObject` 直接呼叫系統 API 和廠商 SDK，無需編譯和引入任何 Java 程式碼或 JAR 檔案。

## 開源協議

本專案基於 [Apache License 2.0](LICENSE.md) 開源。

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
