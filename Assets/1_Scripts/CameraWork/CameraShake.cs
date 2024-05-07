using UnityEngine;
using Cinemachine;
public class CameraShake : MonoSingleton<CameraShake>, ISelfInstantiatingMonoSingleton
{
    private CinemachineImpulseSource cinemachineImpulseSource;

    protected override void OnAwake()
    {
        base.OnAwake();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            cinemachineImpulseSource.GenerateImpulse();
        }


    }

    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
