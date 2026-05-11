<div align="center">

![GameFrameX Logo](https://download.alianblank.com/gameframex/gameframex_logo_320.png)

# BlankDeviceUniqueIdentifier

[![Version](https://img.shields.io/github/v/release/AlianBlank/BlankDeviceUniqueIdentifier?label=version&color=green)](https://github.com/AlianBlank/BlankDeviceUniqueIdentifier/releases)
[![License](https://img.shields.io/badge/license-MIT+Apache%202.0-orange.svg)](LICENSE.md)
[![Documentation](https://img.shields.io/badge/docs-gameframex-brightgreen.svg)](https://gameframex.doc.alianblank.com)

**인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현**

[📖 문서](https://gameframex.doc.alianblank.com) • [🚀 빠른 시작](#사용-예시)

---

🌐 **언어**: [English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

---

</div>

경량 Unity3D 기기 고유 식별자 플러그인. Android 및 iOS 플랫폼에서 OAID, IDFA, IMEI 및 안정적인 하드웨어 핑거프린트에 접근하는 통합 C# API를 제공합니다. Android 측은 네이티브 종속성이 없습니다.

### 특징

- **Android 순수 C# 구현** — `AndroidJavaClass` / `AndroidJavaObject`(JNI)를 사용하여 시스템 API와 벤더 SDK를 직접 호출합니다. Java 코드, JAR 파일, Gradle 설정이 필요하지 않습니다.
- **크로스 플랫폼** — Android, iOS, Unity Editor를 통합 지원합니다. 미지원 플랫폼 API는 자동으로 `SystemInfo.deviceUniqueIdentifier`로 폴백됩니다.
- **멀티 벤더 OAID** — 리플렉션을 통해 MSA SDK, Huawei, Xiaomi, OPPO, vivo, Samsung의 OAID 가져오기를 지원합니다.
- **iOS IDFA 및 SSKeychain** — `ASIdentifierManager`를 통해 IDFA를 가져오고 ATT 인증을 지원합니다. 기기 ID를 Keychain에 영구 저장하여 앱 재설치 후에도 유지됩니다.
- **권한 불필요** — 권한 없이 동작합니다. 선택적 권한을 선언하면 식별자의 고유성을 향상할 수 있습니다.
- **내장 캐시** — 모든 API는 첫 번째 호출 후 `PlayerPrefs`에 결과를 캐시하여 중복 시스템 쿼리를 방지합니다.

## 기능

| API | 설명 | 반환값 |
|-----|------|--------|
| `DeviceGetOaid` | 기기 OAID 가져오기 (Android 전용) | OAID 원본 값 (`-` 제거, 최대 32자) |
| `DeviceGetIdfa` | 기기 IDFA 가져오기 (iOS 전용) | IDFA 원본 값 (`-` 제거, 최대 32자) |
| `DeviceGetImei` | 기기 IMEI 가져오기 | IMEI 원본 값 (`-` 제거, 최대 32자) |
| `DeviceUniqueIdentifier` | 기기 고유 머신 ID 가져오기 | MD5 해시 (32자 16진수 문자열) |

모든 API는 `PlayerPrefs`를 통해 결과를 캐시하며, 최초 가져오기 후에는 시스템 인터페이스를 다시 호출하지 않습니다.

## 플랫폼 구현

| 플랫폼 | `DeviceGetOaid` | `DeviceGetIdfa` | `DeviceGetImei` | `DeviceUniqueIdentifier` |
|----------|-----------------|-----------------|-----------------|--------------------------|
| Android | JNI 리플렉션 가져오기 (MSA / Huawei / Xiaomi / OPPO / vivo / Samsung) | `SystemInfo.deviceUniqueIdentifier` (폴백) | JNI로 `TelephonyManager` 호출 | IMEI + 하드웨어 정보 + Android ID + WLAN MAC + BT MAC의 MD5 |
| iOS | `SystemInfo.deviceUniqueIdentifier` (폴백) | `ASIdentifierManager.advertisingIdentifier` | native `__DeviceGetIMEI()` (IDFV) | native `DeviceUniqueId()` (SSKeychain) |
| Editor / 기타 | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` |

## 권한 (선택 사항)

이 플러그인은 **어떠한 권한도 요구하지 않습니다**. 권한이 없을 경우 정상적으로 폴백됩니다. 플러그인은 `AndroidManifest.xml`을 포함하지 않으며, 다음 권한은 **사용 측 프로젝트**의 `AndroidManifest.xml`에서 필요에 따라 선언해야 합니다.

### Android

| 권한 | `DeviceUniqueIdentifier` 고유성 향상 | `DeviceGetImei` 고유성 향상 |
|------------|------|------|
| `READ_PHONE_STATE` | IMEI가 해시 계산에 포함됨 | 실제 IMEI를 가져올 수 있음 |
| `ACCESS_WIFI_STATE` | WLAN MAC이 해시 계산에 포함됨 | - |
| `BLUETOOTH` | Bluetooth MAC이 해시 계산에 포함됨 | - |

```xml
<!-- 사용 측 프로젝트의 AndroidManifest.xml에 필요에 따라 추가 -->
<uses-permission android:name="android.permission.READ_PHONE_STATE" />
<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
<uses-permission android:name="android.permission.BLUETOOTH" />
```

### iOS

| 설정 | 목적 |
|------|------|
| `NSUserTrackingUsageDescription` | IDFA에 필요한 ATT 인증 설명 |

```xml
<!-- 사용 측 프로젝트의 Info.plist에 추가 -->
<key>NSUserTrackingUsageDescription</key>
<string>광고 식별자는 더 나은 서비스 제공을 위해 사용됩니다</string>
```

## 사용 예시

```csharp
using GameFrameX.SystemInfo.Runtime;

// 기기 OAID 가져오기 (Android 전용, iOS/Editor에서는 SystemInfo.deviceUniqueIdentifier로 폴백)
string oaid = BlankDeviceUniqueIdentifier.DeviceGetOaid;

// 기기 IDFA 가져오기 (iOS 전용, Android/Editor에서는 SystemInfo.deviceUniqueIdentifier로 폴백)
string idfa = BlankDeviceUniqueIdentifier.DeviceGetIdfa;

// 기기 IMEI 가져오기
string imei = BlankDeviceUniqueIdentifier.DeviceGetImei;

// 기기 고유 식별자 가져오기
string deviceId = BlankDeviceUniqueIdentifier.DeviceUniqueIdentifier;
```

### iOS IDFA 참고 사항

IDFA는 ATT (App Tracking Transparency)를 통한 사용자 인증이 필요합니다. 사용 전 `Info.plist`에 추가:

```xml
<key>NSUserTrackingUsageDescription</key>
<string>광고 식별자는 더 나은 서비스 제공을 위해 사용됩니다</string>
```

`DeviceGetIdfa`를 호출하기 전에 인증을 요청:

```csharp
#if UNITY_IOS || UNITY_IPHONE
// iOS 14+에서는 먼저 ATT 인증을 요청해야 합니다
if (UnityEngine.iOS.Device.systemVersion.CompareTo("14") >= 0)
{
    UnityEngine.iOS.Device.RequestUserAuthorization(UnityEngine.iOS.UserTracking.Authorization);
}
#endif
string idfa = BlankDeviceUniqueIdentifier.DeviceGetIdfa;
```

인증되지 않은 경우 `DeviceGetIdfa`는 빈 문자열을 반환하며, 충돌이 발생하지 않습니다.

## 디렉토리 구조

```
Plugins/
  iOS/
    BlankDeviceUniqueIdentifier/
      AHDeviceUniqueIdentifier.h               # iOS native 헤더
      AHDeviceUniqueIdentifier.mm              # iOS native 구현
      SSKeychain.h                              # SSKeychain 키체인 유틸리티
      SSKeychain.m
Runtime/
  BlankDeviceUniqueIdentifier.cs               # C# 통합 인터페이스 (Android는 JNI로 시스템 API를 직접 호출, Java/JAR 불필요)
```

> Android에서는 `AndroidJavaClass` / `AndroidJavaObject`를 사용하여 시스템 API와 벤더 SDK를 직접 호출합니다. Java 코드 컴파일이나 JAR 파일이 필요하지 않습니다.

## 라이선스

이 프로젝트는 [Apache License 2.0](LICENSE.md)에 따라 라이선스가 부여됩니다.

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
