 # BlankDeviceUniqueIdentifier

用于在 Unity3D 中获取 Android 和 iOS 平台设备唯一标识符的插件。

## 功能

| API | 说明 | 返回值 |
|-----|------|--------|
| `DeviceGetImei` | 获取设备 IMEI | IMEI 原始值（去 `-`，最长 32 位） |
| `DeviceUniqueIdentifier` | 获取设备唯一机器码 | MD5 哈希值（32 位十六进制字符串） |

两个 API 均通过 `PlayerPrefs` 缓存结果，首次获取后不再重复调用系统接口。

## 平台实现

| 平台 | `DeviceGetImei` | `DeviceUniqueIdentifier` |
|------|-----------------|--------------------------|
| Android | Java `getImei()` / `getDeviceId()` | IMEI + 硬件信息 + Android ID + WLAN MAC + BT MAC 的 MD5 |
| iOS | native `__DeviceGetIMEI()` (SSKeychain) | native `DeviceUniqueId()` (SSKeychain) |
| Editor / 其他 | `SystemInfo.deviceUniqueIdentifier` | `SystemInfo.deviceUniqueIdentifier` |

## 权限（可选）

本插件**不强制要求任何权限**，无权限时降级运行不会崩溃。声明以下权限可提高标识符的唯一性：

| 权限 | 提升 `DeviceUniqueIdentifier` 唯一性 | 提升 `DeviceGetImei` 唯一性 |
|------|------|------|
| `READ_PHONE_STATE` | IMEI 参与哈希计算 | 可获取真实 IMEI |
| `ACCESS_WIFI_STATE` | WLAN MAC 参与哈希计算 | - |
| `BLUETOOTH` | 蓝牙 MAC 参与哈希计算 | - |

如需使用，在 Android 的 `AndroidManifest.xml` 中添加：

```xml
<uses-permission android:name="android.permission.READ_PHONE_STATE" />
<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
<uses-permission android:name="android.permission.BLUETOOTH" />
```

## 使用示例

```csharp
using BlankSystemInfo.Runtime;

// 获取设备 IMEI
string imei = BlankDeviceUniqueIdentifier.DeviceGetImei;

// 获取设备唯一标识符
string deviceId = BlankDeviceUniqueIdentifier.DeviceUniqueIdentifier;
```

## 目录结构

```
Plugins/
  Android/
    com.alianhome.deviceuniqueidentifier.jar   # Android Java 插件
  iOS/
    BlankDeviceUniqueIdentifier/
      AHDeviceUniqueIdentifier.h               # iOS native 头文件
      AHDeviceUniqueIdentifier.mm              # iOS native 实现
      SSKeychain.h                              # SSKeychain 钥匙串工具
      SSKeychain.m
Runtime/
  BlankDeviceUniqueIdentifier.cs               # C# 统一接口
```
