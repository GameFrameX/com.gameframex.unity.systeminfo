<div align="center">

<img src="https://download.alianblank.com/gameframex/gameframex_logo_320.png" alt="Game Frame X Logo" width="160" />

# BlankDeviceUniqueIdentifier

[![License](https://img.shields.io/github/license/GameFrameX/com.gameframex.unity.systeminfo)](https://github.com/GameFrameX/com.gameframex.unity.systeminfo/blob/main/LICENSE.md)
[![Version](https://img.shields.io/github/v/release/GameFrameX/com.gameframex.unity.systeminfo)](https://github.com/GameFrameX/com.gameframex.unity.systeminfo/releases)
[![Unity Version](https://img.shields.io/badge/Unity-2019.4-black?logo=unity)](https://unity.com/)
[![Documentation](https://img.shields.io/badge/Documentation-docs-blue)](https://gameframex.doc.alianblank.com)

インディゲーム開発者向けオールインワンソリューション · インディ開発者の夢を支援

<br />

[ドキュメント](https://gameframex.doc.alianblank.com) · [クイックスタート](#quick-start) · QQグループ: 467608841 / 233840761

<br />

[English](README.md) | [简体中文](README.zh-CN.md) | [繁體中文](README.zh-TW.md) | **日本語** | [한국어](README.ko.md)

</div>

## 機能

| API | 説明 | 戻り値 |
|-----|------|--------|
| `DeviceGetOaid` | デバイス OAID を取得（Android のみ） | OAID 元の値（`-` 除去、最大 32 文字） |
| `DeviceGetIdfa` | デバイス IDFA を取得（iOS のみ） | IDFA 元の値（`-` 除去、最大 32 文字） |
| `DeviceGetImei` | デバイス IMEI を取得 | IMEI 元の値（`-` 除去、最大 32 文字） |
| `DeviceUniqueIdentifier` | デバイス固有マシン ID を取得 | MD5 ハッシュ（32 文字の 16 進数文字列） |

すべての API は `PlayerPrefs` を通じて結果をキャッシュし、初回取得後はシステムインターフェースを再呼び出ししません。





## クイックスタート

### インストール

以下のいずれかの方法を選択してください：

1. Unity プロジェクトの `Packages/manifest.json` を編集し、`scopedRegistries` セクションを追加してください：
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

   `scopes` は、どのパッケージをこのレジストリから解決するかを制御します。`com.gameframex` で始まるパッケージのみがこのレジストリから取得されます。

2. `manifest.json` の `dependencies` に直接追加：
   ```json
   {
      "com.gameframex.unity.systeminfo": "https://github.com/gameframex/com.gameframex.unity.systeminfo.git"
   }
   ```
3. Unity の **Package Manager** で **Git URL** を使用して追加：`https://github.com/gameframex/com.gameframex.unity.systeminfo.git`
4. リポジトリを Unity プロジェクトの `Packages` ディレクトリにクローンしてください。自動的に読み込まれます.

## クイックスタート

### インストール

以下のいずれかの方法を選択してください：

1. Unity プロジェクトの `Packages/manifest.json` を編集し、`scopedRegistries` セクションを追加してください：
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

   `scopes` は、どのパッケージをこのレジストリから解決するかを制御します。`com.gameframex` で始まるパッケージのみがこのレジストリから取得されます。

2. `manifest.json` の `dependencies` に直接追加：
   ```json
   {
      "com.gameframex.unity.systeminfo": "https://github.com/gameframex/com.gameframex.unity.systeminfo.git"
   }
   ```
3. Unity の **Package Manager** で **Git URL** を使用して追加：`https://github.com/gameframex/com.gameframex.unity.systeminfo.git`
4. リポジトリを Unity プロジェクトの `Packages` ディレクトリにクローンしてください。自動的に読み込まれます.

## プラットフォーム実装

| プラットフォーム | `DeviceGetOaid` | `DeviceGetIdfa` | `DeviceGetImei` | `DeviceUniqueIdentifier` |
|----------|-----------------|-----------------|-----------------|--------------------------|
| Android | JNI リフレクション取得（MSA / Huawei / Xiaomi / OPPO / vivo / Samsung） | `SystemInfo.deviceUniqueIdentifier`（フォールバック） | JNI による `TelephonyManager` 呼び出し | IMEI + ハードウェア情報 + Android ID + WLAN MAC + BT MAC の MD5 |
| iOS | `SystemInfo.deviceUniqueIdentifier`（フォールバック） | `ASIdentifierManager.advertisingIdentifier` | native `__DeviceGetIMEI()` (IDFV) | native `DeviceUniqueId()` (SSKeychain) |
| Editor / その他 | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` |

## 権限（オプション）

本プラグインは**権限を一切要求しません**。権限がない場合はグレースフルにフォールバックします。プラグインは `AndroidManifest.xml` を同梱していません。以下の権限は**使用側プロジェクト**の `AndroidManifest.xml` で必要に応じて宣言してください。

### Android

| 権限 | `DeviceUniqueIdentifier` の一意性向上 | `DeviceGetImei` の一意性向上 |
|------------|------|------|
| `READ_PHONE_STATE` | IMEI がハッシュ計算に含まれる | 実際の IMEI を取得可能 |
| `ACCESS_WIFI_STATE` | WLAN MAC がハッシュ計算に含まれる | - |
| `BLUETOOTH` | Bluetooth MAC がハッシュ計算に含まれる | - |

```xml
<!-- 使用側プロジェクトの AndroidManifest.xml に必要に応じて追加 -->
<uses-permission android:name="android.permission.READ_PHONE_STATE" />
<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
<uses-permission android:name="android.permission.BLUETOOTH" />
```

### iOS

| 設定 | 目的 |
|------|------|
| `NSUserTrackingUsageDescription` | IDFA に必要な ATT 認証の説明 |

```xml
<!-- 使用側プロジェクトの Info.plist に追加 -->
<key>NSUserTrackingUsageDescription</key>
<string>広告識別子はより良いサービスの提供に使用されます</string>
```

## 使用例

```csharp
using GameFrameX.SystemInfo.Runtime;

// デバイス OAID を取得（Android のみ、iOS/Editor では SystemInfo.deviceUniqueIdentifier にフォールバック）
string oaid = BlankDeviceUniqueIdentifier.DeviceGetOaid;

// デバイス IDFA を取得（iOS のみ、Android/Editor では SystemInfo.deviceUniqueIdentifier にフォールバック）
string idfa = BlankDeviceUniqueIdentifier.DeviceGetIdfa;

// デバイス IMEI を取得
string imei = BlankDeviceUniqueIdentifier.DeviceGetImei;

// デバイス一意識別子を取得
string deviceId = BlankDeviceUniqueIdentifier.DeviceUniqueIdentifier;
```

### iOS IDFA の注意事項

IDFA は ATT（App Tracking Transparency）によるユーザー認証が必要です。使用前に `Info.plist` に追加：

```xml
<key>NSUserTrackingUsageDescription</key>
<string>広告識別子はより良いサービスの提供に使用されます</string>
```

`DeviceGetIdfa` を呼び出す前に認証を要求：

```csharp
#if UNITY_IOS || UNITY_IPHONE
// iOS 14+ では先に ATT 認証を要求
if (UnityEngine.iOS.Device.systemVersion.CompareTo("14") >= 0)
{
    UnityEngine.iOS.Device.RequestUserAuthorization(UnityEngine.iOS.UserTracking.Authorization);
}
#endif
string idfa = BlankDeviceUniqueIdentifier.DeviceGetIdfa;
```

未認証の場合、`DeviceGetIdfa` は空文字列を返し、クラッシュしません。

## ディレクトリ構造

```
Plugins/
  iOS/
    BlankDeviceUniqueIdentifier/
      AHDeviceUniqueIdentifier.h               # iOS native ヘッダ
      AHDeviceUniqueIdentifier.mm              # iOS native 実装
      SSKeychain.h                              # SSKeychain キーチェーンユーティリティ
      SSKeychain.m
Runtime/
  BlankDeviceUniqueIdentifier.cs               # C# 統一インターフェース（Android は JNI でシステム API を直接呼び出し、Java/JAR 不要）
```

> Android では `AndroidJavaClass` / `AndroidJavaObject` を使用してシステム API とベンダー SDK を直接呼び出します。Java コードのコンパイルや JAR ファイルは不要です。


## 依存関係

| パッケージ | 説明 |
|----------|------|
| (无) | - |


## ドキュメントとリソース

- [ドキュメント](https://gameframex.doc.alianblank.com)

## コミュニティとサポート

- QQグループ: 467608841 / 233840761

## 変更履歴

[Releases](https://github.com/GameFrameX/gameframex/com.gameframex.unity.systeminfo/releases) で変更履歴を確認してください。
## ライセンス

詳しくは [LICENSE.md](LICENSE.md) をご参照ください。
