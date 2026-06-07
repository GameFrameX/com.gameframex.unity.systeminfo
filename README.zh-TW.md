<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# BlankDeviceUniqueIdentifier

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.systeminfo)](https://github.com/GameFrameX/com.gameframex.unity.systeminfo/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.systeminfo)](https://github.com/GameFrameX/com.gameframex.unity.systeminfo/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

獨立遊戲前後端一體化解決方案 · 獨立遊戲開發者的圓夢大使

<br />

[文檔](https://gameframex.doc.alianblank.com) · [快速開始](#quick-start) · QQ群: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | **繁體中文** | [日本語](README.ja.md) | [한국어](README.ko.md)

</div>

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

詳見 [LICENSE.md](LICENSE.md) 檔案。
