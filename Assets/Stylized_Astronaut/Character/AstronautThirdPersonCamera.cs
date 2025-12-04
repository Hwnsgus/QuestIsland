using UnityEngine;

namespace AstronautThirdPersonCamera
{
    public class AstronautThirdPersonCamera : MonoBehaviour
    {
        public Transform lookAt;
        public float height = 4f;
        public float distance = 6f;
        public float followSpeed = 5f;

        public float angle = 20f;   // 위에서 내려다보는 각도
        public float yaw = 0f;      // 고정된 방향(0이면 북쪽 방향에서 바라봄)

        private void LateUpdate()
        {
            if (lookAt == null) return;

            // 🔥 카메라 회전 = 플레이어 Yaw + angle
            Quaternion rotation = Quaternion.Euler(angle, yaw, 0f);

            // 🔥 플레이어 뒤쪽 방향(offset)
            Vector3 offset = rotation * new Vector3(0, 0, -distance);
            offset.y += height;

            // 🔥 카메라 목표 위치 계산
            Vector3 desiredPos = lookAt.position + offset;

            // 🔥 자연스럽게 따라가기
            transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

            // 🔥 플레이어 바라보기
            transform.LookAt(lookAt.position + Vector3.up * 1.5f);
        }
    }
}
