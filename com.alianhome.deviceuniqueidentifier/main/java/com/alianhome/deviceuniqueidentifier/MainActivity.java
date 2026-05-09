package com.alianhome.deviceuniqueidentifier;

import android.annotation.SuppressLint;
import android.bluetooth.BluetoothAdapter;
import android.content.Context;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.provider.Settings;
import android.telephony.TelephonyManager;
import android.util.Log;

import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;

import com.unity3d.player.*;

public class MainActivity extends UnityPlayerActivity {

	@SuppressLint("HardwareIds")
	public static String DeviceUniqueIdentifier() {

		// 1 compute IMEI
		TelephonyManager TelephonyMgr = (TelephonyManager) UnityPlayer.currentActivity
				.getSystemService(TELEPHONY_SERVICE);
		String m_szImei = "";
		if (TelephonyMgr != null) {
			try {
				m_szImei = TelephonyMgr.getDeviceId(); // Requires
														// READ_PHONE_STATE
			} catch (Exception e) {
				Log.e("Unity", "READ_PHONE_STATE permission failed");
			}
		}

		// 2 compute DEVICE ID
		String m_szDevIDShort = "35"
				+ // we make this look like a valid IMEI
				Build.BOARD.length() % 10 + Build.BRAND.length() % 10
				+ Build.CPU_ABI.length() % 10 + Build.DEVICE.length() % 10
				+ Build.DISPLAY.length() % 10 + Build.HOST.length() % 10
				+ Build.ID.length() % 10 + Build.MANUFACTURER.length() % 10
				+ Build.MODEL.length() % 10 + Build.PRODUCT.length() % 10
				+ Build.TAGS.length() % 10 + Build.TYPE.length() % 10
				+ Build.USER.length() % 10; // 13 digits
		// 3 android ID - unreliable
		String m_szAndroidID = Settings.Secure.getString(
				UnityPlayer.currentActivity.getContentResolver(),
				Settings.Secure.ANDROID_ID);
		if (m_szAndroidID == null) {
			m_szAndroidID = "";
		}
		String m_szWLANMAC = "";
		try {
			WifiManager wm = (WifiManager) UnityPlayer.currentActivity
					.getApplicationContext().getSystemService(
							Context.WIFI_SERVICE);
			if (wm != null) {
				m_szWLANMAC = wm.getConnectionInfo().getMacAddress();
			}
		} catch (Exception e) {
			Log.e("Unity", "ACCESS_WIFI_STATE permission failed");
		}
		if (m_szWLANMAC == null) {
			m_szWLANMAC = "";
		}

		String m_szBTMAC = "";
		try {
			BluetoothAdapter m_BluetoothAdapter = BluetoothAdapter
					.getDefaultAdapter();
			if (m_BluetoothAdapter != null) {
				m_szBTMAC = m_BluetoothAdapter.getAddress();
			}
		} catch (Exception e) {
			Log.e("Unity", "BLUETOOTH permission failed");
		}
		if (m_szBTMAC == null) {
			m_szBTMAC = "";
		}

		// 6 SUM THE IDs
		String m_szLongID = m_szImei + m_szDevIDShort + m_szAndroidID
				+ m_szWLANMAC + m_szBTMAC;
		MessageDigest m = null;
		try {
			m = MessageDigest.getInstance("MD5");
		} catch (NoSuchAlgorithmException e) {
			Log.e("Unity", "MD5 not available", e);
		}
		if (m == null) {
			return m_szLongID;
		}
		m.update(m_szLongID.getBytes(), 0, m_szLongID.length());
		byte p_md5Data[] = m.digest();

		StringBuilder sb = new StringBuilder();
		for (byte b : p_md5Data) {
			sb.append(String.format("%02X", b));
		}
		return sb.toString();
	}
}
