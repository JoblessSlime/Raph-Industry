using UnityEngine;
using Unity.Cinemachine;

public class FollowCinemachine : MonoBehaviour
{
    public CinemachineCamera virtualCamera;
    public Vector3 offset = new Vector3(0, 0, 5); // Adjust as needed

    private Transform cameraTransform;

    void Start()
    {
        if (virtualCamera != null)
        {
            cameraTransform = virtualCamera.gameObject.transform;
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera is not assigned!");
        }
    }

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            transform.position = cameraTransform.position + cameraTransform.forward * offset.z + cameraTransform.up * offset.y + cameraTransform.right * offset.x;
            transform.rotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
        }
    }
}
