## [3.0.1](https://github.com/gameframex/com.gameframex.unity.systeminfo/compare/3.0.0...3.0.1) (2026-05-14)


### Bug Fixes

* **runtime:** qualify PlayerPrefs calls with UnityEngine prefix ([c4a51fc](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/c4a51fc44e436383a6be6132e062aa2dfe332035))

# [3.0.0](https://github.com/gameframex/com.gameframex.unity.systeminfo/compare/2.0.0...3.0.0) (2026-05-11)


* refactor(ios)!: prefix SSKeychain with GameFrameX ([fc34e68](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/fc34e6802c46bbbf0df517528f929ed789d1521c))


### BREAKING CHANGES

* Keychain service name changed from
com.alianhome.uuid to com.gameframex.blank.uuid.
Existing stored UUIDs will be lost and regenerated.

# [2.0.0](https://github.com/gameframex/com.gameframex.unity.systeminfo/compare/1.1.0...2.0.0) (2026-05-11)


* refactor(runtime)!: rename namespace to GameFrameX.SystemInfo.Runtime ([8889a39](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/8889a39b0d090828221bf8d31c92c12e7e2d1adb))


### BREAKING CHANGES

* namespace changed from BlankSystemInfo.Runtime
to GameFrameX.SystemInfo.Runtime, assembly definition renamed
from BlankSystemInfo.Runtime.asmdef to
GameFrameX.SystemInfo.Runtime.asmdef.
All using directives must be updated accordingly.

# [1.1.0](https://github.com/gameframex/com.gameframex.unity.systeminfo/compare/1.0.0...1.1.0) (2026-05-11)


### Features

* **ios:** 添加 AppTrackingTransparency 框架导入 ([590f541](https://github.com/gameframex/com.gameframex.unity.systeminfo/commit/590f54125e5031b5c1153081ecd14c2fce870707))

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
