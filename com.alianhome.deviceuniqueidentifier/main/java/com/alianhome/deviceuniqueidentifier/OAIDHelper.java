package com.alianhome.deviceuniqueidentifier;

import android.content.Context;
import android.os.Build;
import android.util.Log;

import java.lang.reflect.Method;

public class OAIDHelper {

    private static final String TAG = "Unity";

    public static String getOAID(Context context) {
        String oaid = tryMSA(context);
        if (!oaid.isEmpty()) return oaid;

        String manufacturer = Build.MANUFACTURER.toLowerCase();

        if (manufacturer.contains("huawei") || manufacturer.contains("honor")) {
            oaid = tryHuawei(context);
        } else if (manufacturer.contains("xiaomi") || manufacturer.contains("redmi") || manufacturer.contains("poco")) {
            oaid = tryXiaomi(context);
        } else if (manufacturer.contains("oppo") || manufacturer.contains("realme") || manufacturer.contains("oneplus")) {
            oaid = tryOPPO(context);
        } else if (manufacturer.contains("vivo") || manufacturer.contains("iqoo")) {
            oaid = tryVivo(context);
        } else if (manufacturer.contains("samsung")) {
            oaid = trySamsung(context);
        }

        if (!oaid.isEmpty()) return oaid;

        // If manufacturer-specific failed, try Huawei HMS as a generic fallback
        oaid = tryHuawei(context);
        if (!oaid.isEmpty()) return oaid;

        Log.w(TAG, "OAID: all approaches failed, returning empty");
        return "";
    }

    private static String tryMSA(Context context) {
        try {
            Class<?> clz = Class.forName("com.bun.lib.MsaIdProxy");
            Method method = clz.getMethod("getOAID", Context.class);
            String oaid = (String) method.invoke(null, context);
            if (oaid != null && !oaid.isEmpty()) {
                return oaid;
            }
        } catch (Exception e) {
            Log.d(TAG, "OAID: MSA SDK not available");
        }
        return "";
    }

    private static String tryHuawei(Context context) {
        try {
            Class<?> clz = Class.forName("com.huawei.hms.ads.identifier.AdvertisingIdClient");
            Method method = clz.getMethod("getAdvertisingIdInfo", Context.class);
            Object info = method.invoke(null, context);
            if (info != null) {
                Method getId = info.getClass().getMethod("getId");
                String id = (String) getId.invoke(info);
                if (id != null && !id.isEmpty()) {
                    return id;
                }
            }
        } catch (Exception e) {
            Log.d(TAG, "OAID: Huawei HMS not available");
        }
        return "";
    }

    private static String tryXiaomi(Context context) {
        try {
            Class<?> clz = Class.forName("com.android.id.impl.IdProviderImpl");
            Object provider = clz.newInstance();
            Method method = clz.getMethod("getOAID", Context.class);
            String oaid = (String) method.invoke(provider, context);
            if (oaid != null && !oaid.isEmpty()) {
                return oaid;
            }
        } catch (Exception e) {
            Log.d(TAG, "OAID: Xiaomi IdProvider not available");
        }
        return "";
    }

    private static String tryOPPO(Context context) {
        try {
            Class<?> clz = Class.forName("com.heytap.openid.IdProvider");
            Object provider = clz.newInstance();
            Method method = clz.getMethod("getOAID", Context.class);
            String oaid = (String) method.invoke(provider, context);
            if (oaid != null && !oaid.isEmpty()) {
                return oaid;
            }
        } catch (Exception e) {
            Log.d(TAG, "OAID: OPPO IdProvider not available");
        }
        return "";
    }

    private static String tryVivo(Context context) {
        try {
            Class<?> clz = Class.forName("com.vivo.identifier.IdProvider");
            Object provider = clz.newInstance();
            Method method = clz.getMethod("getOAID", Context.class);
            String oaid = (String) method.invoke(provider, context);
            if (oaid != null && !oaid.isEmpty()) {
                return oaid;
            }
        } catch (Exception e) {
            Log.d(TAG, "OAID: vivo IdProvider not available");
        }
        return "";
    }

    private static String trySamsung(Context context) {
        try {
            Class<?> clz = Class.forName("com.samsung.android.deviceidservice.DeviceIdService");
            Object service = clz.newInstance();
            Method method = clz.getMethod("getOAID", Context.class);
            String oaid = (String) method.invoke(service, context);
            if (oaid != null && !oaid.isEmpty()) {
                return oaid;
            }
        } catch (Exception e) {
            Log.d(TAG, "OAID: Samsung DeviceIdService not available");
        }
        return "";
    }
}
