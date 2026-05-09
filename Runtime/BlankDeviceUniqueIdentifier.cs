// ==========================================================================================
//   GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//   GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//   均受中华人民共和国及相关国际法律法规保护。
//   are protected by the laws of the People's Republic of China and relevant international regulations.
//   使用本项目须严格遵守相应法律法规及开源许可证之规定。
//   Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//   本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//   This project is dual-licensed under the MIT License and Apache License 2.0,
//   完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//   please refer to the LICENSE file in the root directory of the source code for the full license text.
//   禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//   It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//   侵犯他人合法权益等法律法规所禁止的行为！
//   or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//   因基于本项目二次开发所产生的一切法律纠纷与责任，
//   Any legal disputes and liabilities arising from secondary development based on this project
//   本项目组织与贡献者概不承担。
//   shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//   GitHub 仓库：https://github.com/GameFrameX
//   GitHub Repository: https://github.com/GameFrameX
//   Gitee  仓库：https://gitee.com/GameFrameX
//   Gitee Repository:  https://gitee.com/GameFrameX
//   CNB  仓库：https://cnb.cool/GameFrameX
//   CNB Repository:  https://cnb.cool/GameFrameX
//   官方文档：https://gameframex.doc.alianblank.com/
//   Official Documentation: https://gameframex.doc.alianblank.com/
//  ==========================================================================================

using System.Text;
using UnityEngine;

#if UNITY_IOS || UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

namespace BlankSystemInfo.Runtime
{
    /// <summary>
    /// 获取 Android 和 iOS 的设备唯一标识符（纯 C# 实现，无需 Java/JAR）
    /// </summary>
    public sealed class BlankDeviceUniqueIdentifier
    {
#if UNITY_IOS || UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern string gameframex_device_get_imei();
        [DllImport("__Internal")]
        private static extern string gameframex_device_get_idfa();
        [DllImport("__Internal")]
        private static extern string gameframex_device_unique_id();
#endif

        /// <summary>
        /// 获取设备 OAID（Android 独有，iOS/Editor 降级为 SystemInfo.deviceUniqueIdentifier）
        /// </summary>
        public static string DeviceGetOaid
        {
            get
            {
                string id = PlayerPrefs.GetString("DeviceUniqueIdentifierOAID");
                if (IsValid(id))
                {
                    return id;
                }

#if UNITY_ANDROID
                string sid = SafeGetOaid();
#else
                string sid = SystemInfo.deviceUniqueIdentifier;
#endif
                sid = Normalize(sid);
                PlayerPrefs.SetString("DeviceUniqueIdentifierOAID", sid);
                return sid;
            }
        }

        /// <summary>
        /// 获取设备 IDFA（iOS 独有，Android/Editor 降级为 SystemInfo.deviceUniqueIdentifier）
        /// </summary>
        public static string DeviceGetIdfa
        {
            get
            {
                string id = PlayerPrefs.GetString("DeviceUniqueIdentifierIDFA");
                if (IsValid(id))
                {
                    return id;
                }

#if UNITY_IOS || UNITY_IPHONE
                string sid = gameframex_device_get_idfa();
#else
                string sid = SystemInfo.deviceUniqueIdentifier;
#endif
                sid = Normalize(sid);
                PlayerPrefs.SetString("DeviceUniqueIdentifierIDFA", sid);
                return sid;
            }
        }

        /// <summary>
        /// 获取设备 IMEI
        /// </summary>
        public static string DeviceGetImei
        {
            get
            {
                string id = PlayerPrefs.GetString("DeviceUniqueIdentifierIMEI");
                if (IsValid(id))
                {
                    return id;
                }

#if UNITY_IOS || UNITY_IPHONE
                string sid = gameframex_device_get_imei();
#elif UNITY_ANDROID
                string sid = SafeGetImei();
#else
                string sid = SystemInfo.deviceUniqueIdentifier;
#endif
                sid = Normalize(sid);
                PlayerPrefs.SetString("DeviceUniqueIdentifierIMEI", sid);
                return sid;
            }
        }

        /// <summary>
        /// 获取设备唯一标识符（多标识符 MD5 哈希）
        /// </summary>
        public static string DeviceUniqueIdentifier
        {
            get
            {
                string id = PlayerPrefs.GetString("DeviceUniqueIdentifierID");
                if (IsValid(id))
                {
                    return id;
                }

#if UNITY_IOS || UNITY_IPHONE
                string sid = gameframex_device_unique_id();
#elif UNITY_ANDROID
                string sid = ComputeDeviceUniqueId();
#else
                string sid = SystemInfo.deviceUniqueIdentifier;
#endif
                sid = Normalize(sid);
                PlayerPrefs.SetString("DeviceUniqueIdentifierID", sid);
                return sid;
            }
        }

        private static bool IsValid(string id)
        {
            return !string.IsNullOrEmpty(id) && id != "null" && id.Length >= 4;
        }

        private static string Normalize(string sid)
        {
            sid = (sid ?? "").Replace("-", "");
            if (sid.Length > 32)
            {
                sid = sid.Substring(0, 32);
            }

            return sid;
        }

        // =====================================================================================
        //  Android JNI helpers — 直接通过 AndroidJavaClass/AndroidJavaObject 调用系统 API
        // =====================================================================================
#if UNITY_ANDROID

        private static AndroidJavaObject GetActivity()
        {
            return new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                .GetStatic<AndroidJavaObject>("currentActivity");
        }

        private static AndroidJavaObject GetAppContext()
        {
            return GetActivity().Call<AndroidJavaObject>("getApplicationContext");
        }

        private static int GetSdkInt()
        {
            return new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT");
        }

        // --- IMEI ---

        private static string SafeGetImei()
        {
            try
            {
                var tm = GetActivity().Call<AndroidJavaObject>("getSystemService", "phone");
                if (tm == null) return "";
                return GetSdkInt() >= 26
                    ? (tm.Call<string>("getImei") ?? "")
                    : (tm.Call<string>("getDeviceId") ?? "");
            }
            catch { return ""; }
        }

        // --- Android ID ---

        private static string SafeGetAndroidId()
        {
            try
            {
                var resolver = GetActivity().Call<AndroidJavaObject>("getContentResolver");
                return new AndroidJavaClass("android.provider.Settings$Secure")
                    .CallStatic<string>("getString", resolver, "android_id") ?? "";
            }
            catch { return ""; }
        }

        // --- WiFi MAC ---

        private static string SafeGetWifiMac()
        {
            try
            {
                var wm = GetAppContext().Call<AndroidJavaObject>("getSystemService", "wifi");
                if (wm == null) return "";
                var info = wm.Call<AndroidJavaObject>("getConnectionInfo");
                return info != null ? (info.Call<string>("getMacAddress") ?? "") : "";
            }
            catch { return ""; }
        }

        // --- Bluetooth MAC ---

        private static string SafeGetBtMac()
        {
            try
            {
                var adapter = new AndroidJavaClass("android.bluetooth.BluetoothAdapter")
                    .CallStatic<AndroidJavaObject>("getDefaultAdapter");
                return adapter != null ? (adapter.Call<string>("getAddress") ?? "") : "";
            }
            catch { return ""; }
        }

        // --- DeviceUniqueIdentifier (composite MD5) ---

        private static string ComputeDeviceUniqueId()
        {
            try
            {
                var build = new AndroidJavaClass("android.os.Build");
                string imei = SafeGetImei();
                string devIdShort = "35"
                    + build.GetStatic<string>("BOARD").Length % 10
                    + build.GetStatic<string>("BRAND").Length % 10
                    + build.GetStatic<string>("CPU_ABI").Length % 10
                    + build.GetStatic<string>("DEVICE").Length % 10
                    + build.GetStatic<string>("DISPLAY").Length % 10
                    + build.GetStatic<string>("HOST").Length % 10
                    + build.GetStatic<string>("ID").Length % 10
                    + build.GetStatic<string>("MANUFACTURER").Length % 10
                    + build.GetStatic<string>("MODEL").Length % 10
                    + build.GetStatic<string>("PRODUCT").Length % 10
                    + build.GetStatic<string>("TAGS").Length % 10
                    + build.GetStatic<string>("TYPE").Length % 10
                    + build.GetStatic<string>("USER").Length % 10;

                string longId = imei + devIdShort + SafeGetAndroidId()
                    + SafeGetWifiMac() + SafeGetBtMac();

                byte[] hash = System.Security.Cryptography.MD5.Create()
                    .ComputeHash(Encoding.UTF8.GetBytes(longId));
                var sb = new StringBuilder(32);
                foreach (byte b in hash)
                    sb.Append(b.ToString("X2"));
                return sb.ToString();
            }
            catch { return ""; }
        }

        // --- OAID ---

        private static string SafeGetOaid()
        {
            try
            {
                var context = GetAppContext();
                if (context == null) return "";

                // 1. MSA SDK
                string oaid = TryReflectStatic("com.bun.lib.MsaIdProxy", "getOAID", context);
                if (!string.IsNullOrEmpty(oaid)) return oaid;

                // 2. Manufacturer-specific
                string mfr = new AndroidJavaClass("android.os.Build")
                    .GetStatic<string>("MANUFACTURER").ToLower();

                if (mfr.Contains("huawei") || mfr.Contains("honor"))
                {
                    oaid = TryHuaweiOaid(context);
                    if (!string.IsNullOrEmpty(oaid)) return oaid;
                }
                else if (mfr.Contains("xiaomi") || mfr.Contains("redmi") || mfr.Contains("poco"))
                {
                    oaid = TryReflectInstance("com.android.id.impl.IdProviderImpl", "getOAID", context);
                    if (!string.IsNullOrEmpty(oaid)) return oaid;
                }
                else if (mfr.Contains("oppo") || mfr.Contains("realme") || mfr.Contains("oneplus"))
                {
                    oaid = TryReflectInstance("com.heytap.openid.IdProvider", "getOAID", context);
                    if (!string.IsNullOrEmpty(oaid)) return oaid;
                }
                else if (mfr.Contains("vivo") || mfr.Contains("iqoo"))
                {
                    oaid = TryReflectInstance("com.vivo.identifier.IdProvider", "getOAID", context);
                    if (!string.IsNullOrEmpty(oaid)) return oaid;
                }
                else if (mfr.Contains("samsung"))
                {
                    oaid = TryReflectInstance("com.samsung.android.deviceidservice.DeviceIdService", "getOAID", context);
                    if (!string.IsNullOrEmpty(oaid)) return oaid;
                }

                // 3. Fallback: try Huawei HMS
                oaid = TryHuaweiOaid(context);
                if (!string.IsNullOrEmpty(oaid)) return oaid;
            }
            catch { }
            return "";
        }

        private static string TryHuaweiOaid(AndroidJavaObject context)
        {
            try
            {
                var info = new AndroidJavaClass(
                    "com.huawei.hms.ads.identifier.AdvertisingIdClient")
                    .CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", context);
                return info != null ? (info.Call<string>("getId") ?? "") : "";
            }
            catch { return ""; }
        }

        private static string TryReflectStatic(string className, string method, AndroidJavaObject arg)
        {
            try
            {
                return new AndroidJavaClass(className).CallStatic<string>(method, arg) ?? "";
            }
            catch { return ""; }
        }

        private static string TryReflectInstance(string className, string method, AndroidJavaObject arg)
        {
            try
            {
                return new AndroidJavaObject(className).Call<string>(method, arg) ?? "";
            }
            catch { return ""; }
        }
#endif
    }
}
