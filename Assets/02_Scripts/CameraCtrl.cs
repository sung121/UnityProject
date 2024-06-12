using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] Transform targetTr;
    [SerializeField] float distance = 10.0f;
    [SerializeField] float zoomSpd = 200.0f;

    [SerializeField] float xSpeed = 120.0f;
    [SerializeField] float ySpeed = 120.0f;

    [SerializeField] float yMinLimit = -20f; // 카메라의 최소 Y축 각도
    [SerializeField] float yMaxLimit = 80f;  // 카메라의 최대 Y축 각도

    private float rotX = 0.0f; // X축 회전 각도
    private float rotY = 30.0f; // Y축 회전 각도

    private Transform camTr;

    // Start is called before the first frame update
    void Start()
    {
        camTr = GetComponent<Transform>();

        Vector3 angles = transform.eulerAngles;
        rotX = angles.y;
        rotY = angles.x;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        rotX += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        rotY -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel != 0.0f)
        {
            distance -= wheel * zoomSpd * Time.deltaTime;
            Debug.Log(distance);
            if (distance > 8)
            {
                distance = 8;
            }
            else if (distance < 1)
            {
                distance = 1;
            }
        }


        // Y축 각도 제한
        rotY = ClampAngle(rotY, yMinLimit, yMaxLimit);

        // 회전 각도를 Quaternion으로 변환
        Quaternion rotation = Quaternion.Euler(rotY, rotX, 0);

        // 새로운 위치 계산
        Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + targetTr.position;

        // 카메라 위치와 회전 적용
        camTr.rotation = rotation;
        camTr.position = position;


    }
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}
