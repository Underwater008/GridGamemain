using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject cameraon;//初始摄像机的位置
    public GameObject camerto;//另一个点的位置
    private float speed = 0.1f;//缓冲的时间  时间越大缓冲速度越慢
    private Vector2 velocity;//如果是3D场景就用Vector3,2D用Vector2
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        cameraon.transform.position = new Vector3(Mathf.SmoothDamp(cameraon.transform.position.x, camerto.transform.position.x,
            ref velocity.x, speed), Mathf.SmoothDamp(cameraon.transform.position.y, camerto.transform.position.y,
            ref velocity.y, speed), -1);

    }
}
