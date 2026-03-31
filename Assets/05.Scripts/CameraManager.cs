using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera setupCam;
    public CinemachineCamera playCam;

    public void EnterSetupMode()
    {
        setupCam.Priority = 10;
        playCam.Priority = 0;
    }

    public void EnterPlayMode()
    {
        setupCam.Priority = 0;
        playCam.Priority = 10;
    }

}