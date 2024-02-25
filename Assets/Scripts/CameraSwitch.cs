using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();
    public static CinemachineVirtualCamera activeCamera = null;

    public static void Register(CinemachineVirtualCamera cam)
    {
        cameras.Add(cam);
        Debug.Log("camera registered" + cam);
    }

    public static void Unregister(CinemachineVirtualCamera cam)
    {
        cameras.Remove(cam);
        Debug.Log("camera unregistered" + cam);
    }

    public static void SwitchCamera(CinemachineVirtualCamera cam)
    {
        cam.Priority = 10;
        activeCamera = cam;

        foreach (CinemachineVirtualCamera c in cameras)
        {
            if (c != cam && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    public static bool IsActiveCamera(CinemachineVirtualCamera cam)
    {
        return cam == activeCamera;
    }
}
