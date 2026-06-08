<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# BlankDeviceUniqueIdentifier

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.systeminfo)](https://github.com/GameFrameX/com.gameframex.unity.systeminfo/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.systeminfo)](https://github.com/GameFrameX/com.gameframex.unity.systeminfo/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

인디 게임 개발자를 위한 올인원 솔루션 · 인디 개발자의 꿈을 실현

<br />

[문서](https://gameframex.doc.alianblank.com) · [빠른 시작](#quick-start) · QQ 그룹: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | [日本語](README.ja.md) | **한국어**

</div>

## 기능

| API | 설명 | 반환값 |
|-----|------|--------|
| `DeviceGetOaid` | 기기 OAID 가져오기 (Android 전용) | OAID 원본 값 (`-` 제거, 최대 32자) |
| `DeviceGetIdfa` | 기기 IDFA 가져오기 (iOS 전용) | IDFA 원본 값 (`-` 제거, 최대 32자) |
| `DeviceGetImei` | 기기 IMEI 가져오기 | IMEI 원본 값 (`-` 제거, 최대 32자) |
| `DeviceUniqueIdentifier` | 기기 고유 머신 ID 가져오기 | MD5 해시 (32자 16진수 문자열) |

모든 API는 `PlayerPrefs`를 통해 결과를 캐시하며, 최초 가져오기 후에는 시스템 인터페이스를 다시 호출하지 않습니다.





## 빠른 시작

### 설치

다음 방법 중 하나를 선택하세요:

1. Unity 프로젝트의 `Packages/manifest.json`을 편집하여 `scopedRegistries` 섹션을 추가하세요:
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.systeminfo": "3.0.1"
     }
   }
   ```

   `scopes`는 이 레지스트리를 통해 어떤 패키지를 해석할지 제어합니다. `com.gameframex`로 시작하는 패키지만 이 레지스트리에서 가져옵니다.

2. `manifest.json`의 `dependencies`에 직접 추가:
   ```json
   {
      "com.gameframex.unity.systeminfo": "https://github.com/gameframex/com.gameframex.unity.systeminfo.git"
   }
   ```
3. Unity의 **Package Manager**에서 **Git URL**을 사용하여 추가: `https://github.com/gameframex/com.gameframex.unity.systeminfo.git`
4. 리포지토리를 Unity 프로젝트의 `Packages` 디렉토리에 클론하세요. 자동으로 로드됩니다.

## 빠른 시작

### 설치

다음 방법 중 하나를 선택하세요:

1. Unity 프로젝트의 `Packages/manifest.json`을 편집하여 `scopedRegistries` 섹션을 추가하세요:
   ```json
   {
     "scopedRegistries": [
       {
         "name": "GameFrameX",
         "url": "https://gameframex.upm.alianblank.uk",
         "scopes": [
           "com.gameframex"
         ]
       }
     ],
     "dependencies": {
       "com.gameframex.unity.systeminfo": "3.0.1"
     }
   }
   ```

   `scopes`는 이 레지스트리를 통해 어떤 패키지를 해석할지 제어합니다. `com.gameframex`로 시작하는 패키지만 이 레지스트리에서 가져옵니다.

2. `manifest.json`의 `dependencies`에 직접 추가:
   ```json
   {
      "com.gameframex.unity.systeminfo": "https://github.com/gameframex/com.gameframex.unity.systeminfo.git"
   }
   ```
3. Unity의 **Package Manager**에서 **Git URL**을 사용하여 추가: `https://github.com/gameframex/com.gameframex.unity.systeminfo.git`
4. 리포지토리를 Unity 프로젝트의 `Packages` 디렉토리에 클론하세요. 자동으로 로드됩니다.

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


## 의존성

| 패키지 | 설명 |
|--------|------|
| (无) | - |


## 문서 및 자료

- [문서](https://gameframex.doc.alianblank.com)

## 커뮤니티 및 지원

- QQ 그룹: 467608841 / 233840761

## 변경 로그

[Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.systeminfo/releases)에서 변경 로그를 확인하세요.
## 라이선스

자세한 내용은 [LICENSE.md](LICENSE.md) 파일을 참조하세요.
