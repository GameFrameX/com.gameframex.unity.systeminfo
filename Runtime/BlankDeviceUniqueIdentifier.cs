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

#if UNITY_IOS || UNITY_IPHONE
using System.Runtime.InteropServices;
#endif

namespace BlankSystemInfo.Runtime
{
    /// <summary>
    /// 获取Android 和 IOS 的唯一机器码
    /// </summary>
    public sealed class BlankDeviceUniqueIdentifier
    {
        /**
         *   Android 资料来源 https://stackoverflow.com/questions/2785485/is-there-a-unique-android-device-id/5626208#5626208
         *
         *   iOS  资料来源于 支付宝使用的 SSKeyChain  钥匙串存储
         *
         */

#if UNITY_IOS || UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern string __DeviceGetIMEI();
#endif

        /// <summary>
        /// 获取设备IMEI
        /// </summary>
        /// <returns></returns>
        public static string DeviceGetImei
        {
            get
            {
                var id = UnityEngine.PlayerPrefs.GetString("DeviceUniqueIdentifierIMEI");
                if (!string.IsNullOrEmpty(id) && id != "null" && id.Length >= 4)
                {
                    return id;
                }

                var sid = __DeviceGetIMEI();
                sid = sid.Replace("-", string.Empty);
                sid = sid.Substring(0, 32);
                UnityEngine.PlayerPrefs.SetString("DeviceUniqueIdentifierIMEI", sid);
                return sid;
            }
        }

        #region 获取设备机器码

#if UNITY_IOS || UNITY_IPHONE
        [DllImport("__Internal")]
        private static extern string DeviceUniqueId();
#endif

        /// <summary>
        /// 获取设备机器码
        /// </summary>
        /// <returns></returns>
        public static string DeviceUniqueIdentifier
        {
            get
            {
                string id = UnityEngine.PlayerPrefs.GetString("DeviceUniqueIdentifierID");
                if (!string.IsNullOrEmpty(id) && id != "null" && id.Length >= 4)
                {
                    return id;
                }
#if UNITY_EDITOR || UNITY_STANDALONE
                string sid = UnityEngine.SystemInfo.deviceUniqueIdentifier;
#else
#if UNITY_IOS || UNITY_IPHONE
                string sid = DeviceUniqueId();
#elif UNITY_ANDROID
                    UnityEngine.AndroidJavaObject androidJavaObject = new UnityEngine.AndroidJavaObject("com.alianhome.deviceuniqueidentifier.MainActivity");
                    string sid = androidJavaObject.CallStatic<string>("DeviceUniqueIdentifier");
#else
                    string sid = UnityEngine.SystemInfo.deviceUniqueIdentifier;
#endif

#endif
                sid = sid.Replace("-", string.Empty);
                sid = sid.Substring(0, 32);
                UnityEngine.PlayerPrefs.SetString("DeviceUniqueIdentifierID", sid);
                return sid;
            }
        }

        #endregion
    }
}