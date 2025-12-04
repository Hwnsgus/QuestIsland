using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float distance = 6f;
    public float height = 3f;

    public float mouseSensitivity = 3f;
    public float cameraSmoothness = 8f;

    float yaw;
    float pitch;



    void LateUpdate()
    {
        if (target == null) return;

        // 마우스 입력으로 카메라 회전
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, 10f, 70f); // 너무 위/아래로 안 가도록 제한

        // 회전값으로 카메라 위치 계산
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 desiredPos = target.position +
                             rotation * new Vector3(0, height, -distance);

        transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * cameraSmoothness);

        // 카메라가 플레이어 바라보기
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
