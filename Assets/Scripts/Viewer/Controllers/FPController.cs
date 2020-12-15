using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MazeViewer.Viewer.Control
{
    public class FPController : MonoBehaviour
    {
        public Vector2 rotScale;
        public Vector3 moveSpeed;
        private float euler_x;
        private float euler_y;
        [SerializeField] private Transform rotateTarget = default;
        [SerializeField] private Rigidbody rigid;

        private void Awake()
        {
            if (rigid == null) rigid = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            var rot = rotateTarget.rotation.eulerAngles;
            euler_x = rot.x;
            euler_y = rot.y;
        }

        private void Update()
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
                {
                    euler_x -= Input.GetAxis("Mouse Y") * rotScale.x;
                    // 设定一个上下限防止往上或往下越界导致倒立
                    euler_x = Mathf.Clamp(euler_x, -89.9f, 89.9f);
                    euler_y += Input.GetAxis("Mouse X") * rotScale.y;
                    var rot = rotateTarget.rotation;
                    rotateTarget.rotation = Quaternion.Euler(euler_x, euler_y, rot.z);
                }

                var forward = Vector3.Scale(rotateTarget.forward, new Vector3(1, 0, 1)).normalized;
                var right = rotateTarget.right;
                // 防止垂直往下看时不动
                if (forward.sqrMagnitude < 0.01) forward = Vector3.Cross(right, Vector3.up);
                var up = Vector3.up * (Input.GetAxisRaw("Jump") - Input.GetAxisRaw("Dive"));
                rigid.AddForce((forward * (moveSpeed.z * Input.GetAxisRaw("Vertical")) +
                                       right * (moveSpeed.x * Input.GetAxisRaw("Horizontal")) +
                                       up * moveSpeed.y)); 
            }
        }
    }
}
