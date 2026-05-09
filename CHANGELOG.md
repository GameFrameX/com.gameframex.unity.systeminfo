# 1.0.0 (2026-05-09)


### Code Refactoring

* **android:** 移除 Java/JAR，改用纯 C# JNI 实现 ([7607d19](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/7607d19e0121f21c36d98a33275f47e86ae0890a))


### Features

* **android:** add DeviceGetIMEI method ([af24955](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/af2495590e4b75ec20cdce7b41bd0f5ce3ddf533))
* **android:** add DeviceGetOAID entry point ([8b532f6](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/8b532f6b905635088c3acd9b6906638b3e0bc6ea))
* **android:** add OAID retrieval via reflection ([aa42bce](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/aa42bce098cd7f8d92d14838306047f5916cd4c7))
* **ios:** add IDFA retrieval via ASIdentifierManager ([e2e7bed](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/e2e7bed1677a233d5bbc9274dd32729e6970dcfb))
* **ios:** add IMEI device ID getter ([8af0ff4](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/8af0ff4ebb5ab3ce71c05a1e634db0ee195f66dd))
* **runtime:** integrate OAID and IDFA APIs ([27e68ea](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/27e68ea142cd53ab52f68d7d00f15ffbb684411b))
* **runtime:** integrate platform IMEI retrieval ([90e3417](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/90e3417882d6976874704e737678d43603031a62))


### BREAKING CHANGES

* **android:** 移除 com.alianhome.deviceuniqueidentifier.jar，
Android 构建不再需要此 JAR 文件。
