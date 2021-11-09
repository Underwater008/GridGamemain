using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject cameraon;//��ʼ�������λ��
    public GameObject camerto;//��һ�����λ��
    private float speed = 0.1f;//�����ʱ��  ʱ��Խ�󻺳��ٶ�Խ��
    private Vector2 velocity;//�����3D��������Vector3,2D��Vector2
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
