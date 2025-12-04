using UnityEngine;
using System.Collections;

namespace AstronautPlayer
{
    public class AstronautPlayer : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        private CharacterController controller;

        public float walkSpeed = 4f;          // 걷기 속도
        public float runSpeed = 7f;           // 뛰기 속도
        private float currentSpeed;           // 현재 적용 속도

        public float jumpPower = 8f;
        private Vector3 moveDirection = Vector3.zero;
        public float gravity = 20.0f;

        public float attackRange = 2f;
        public int attackDamage = 1;
        public LayerMask monsterLayer;

        void Start()
        {
            controller = GetComponent<CharacterController>();

            if (anim == null)
                anim = GetComponentInChildren<Animator>();

            currentSpeed = walkSpeed;
        }

        void Update()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // 🔥 카메라 기준 방향 계산
            Vector3 camForward = Camera.main.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = Camera.main.transform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 moveDir = camForward * v + camRight * h;

            // 🔥 Shift로 달리기
            if (Input.GetKey(KeyCode.LeftShift))
                currentSpeed = runSpeed;
            else
                currentSpeed = walkSpeed;

            // 🔥 이동 및 회전
            if (moveDir.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
                controller.Move(moveDir * currentSpeed * Time.deltaTime);
                anim.SetInteger("AnimationPar", 1); // 걷기/달리기 애니메이션
            }
            else
            {
                anim.SetInteger("AnimationPar", 0);
            }

            // 🔥 공격 (C키)
            if (Input.GetKeyDown(KeyCode.C))
            {
                anim.SetTrigger("Attack");
                Attack();
            }

            // 🔥 점프
            if (controller.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    anim.SetTrigger("Jump");
                    moveDirection.y = jumpPower;
                }
            }

            // 중력 처리
            controller.Move(moveDirection * Time.deltaTime);
            moveDirection.y -= gravity * Time.deltaTime;
            anim.SetBool("isGrounded", controller.isGrounded);
        }


        void Attack()
        {
            // 캐릭터 중심보다 약간 위에서 앞으로 쏘기
            Vector3 origin = transform.position + Vector3.up * 1f;

            // 씬 뷰에서 빨간 선으로 보이게
            Debug.DrawRay(origin, transform.forward * attackRange, Color.red, 0.5f);

            RaycastHit hit;

            // ⛔ 일단 layerMask 빼고 전체 다 맞게
            if (Physics.Raycast(origin, transform.forward, out hit, attackRange))
            {
                Debug.Log("Ray hit: " + hit.collider.name);

                // 콜라이더가 자식이어도 부모까지 찾아보게
                MonsterScripts monster = hit.collider.GetComponentInParent<MonsterScripts>();

                if (monster != null)
                {
                    monster.TakeDamage(attackDamage);
                    Debug.Log($"C키 공격 성공 → {monster.name} 체력 감소! 남은 HP = {monster.hp}");
                }
                else
                {
                    Debug.Log("MonsterScripts 못 찾음 (콜라이더에는 붙어있지 않음).");
                }
            }
            else
            {
                Debug.Log("Raycast 아무 것도 안 맞음");
            }
        }
    }
}