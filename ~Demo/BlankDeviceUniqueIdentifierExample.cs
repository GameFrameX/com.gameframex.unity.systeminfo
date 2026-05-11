using UnityEngine;
using System.Collections;
using GameFrameX.SystemInfo.Runtime;

public class BlankDeviceUniqueIdentifierExample : MonoBehaviour {

	private string id;
	void OnGUI () {
		if (GUILayout.Button (" GET DeviceUniqueIdentifier ", GUILayout.Width (200), GUILayout.Height (200))) {
			id = BlankDeviceUniqueIdentifier.DeviceUniqueIdentifier;
		}
		GUILayout.Label (" DeviceUniqueIdentifier : "+ id);
	}
}
