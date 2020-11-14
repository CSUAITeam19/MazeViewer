using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FPController : MonoBehaviour
{
    public Vector2 rotScale;
    public Vector2 moveSpeed;
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
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            euler_x -= Input.GetAxis("Mouse Y") * rotScale.x;
            euler_y += Input.GetAxis("Mouse X") * rotScale.y;
            var rot = transform.rotation;
            transform.rotation = Quaternion.Euler(euler_x, euler_y, rot.z);
        }

        var forward = transform.forward;
        var right = transform.right;
        transform.position += (forward * moveSpeed.x * Input.GetAxis("Vertical") +
                                right * moveSpeed.y * Input.GetAxis("Horizontal")) * 
                            Time.deltaTime;
    }
}
