using UnityEngine;
using System.Collections;

public class SX_Resize : MonoBehaviour {

    private void Awake()
    {

#if UNITY_IOS
        if (UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone4 &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone4S &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone5 &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone5C &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone5S &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone6 &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone6Plus &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone6S &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone6SPlus &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone7 &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone7Plus &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone8 &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhone8Plus &&
            UnityEngine.iOS.Device.generation != UnityEngine.iOS.DeviceGeneration.iPhoneSE1Gen)
            transform.localScale = new Vector3(0.95f, 0.95f, 0);
#endif
    }
}
