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

        private void Start()
        {
            var rot = transform.rotation.eulerAngles;
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
                    euler_y += Input.GetAxis("Mouse X") * rotScale.y;
                    var rot = transform.rotation;
                    transform.rotation = Quaternion.Euler(euler_x, euler_y, rot.z);
                }

                var forward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
                var right = transform.right;
                // 防止垂直往下看时不动
                if (forward.sqrMagnitude < 0.01) forward = Vector3.Cross(right, Vector3.up);
                var up = Vector3.up * (Input.GetAxis("Jump") - Input.GetAxis("Dive"));
                transform.position += (forward * (moveSpeed.z * Input.GetAxis("Vertical")) +
                                       right * (moveSpeed.x * Input.GetAxis("Horizontal")) +
                                       up * moveSpeed.y) *
                                      Time.deltaTime; 
            }
        }
    }
}
