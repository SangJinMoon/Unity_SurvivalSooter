using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //기본제공
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public Transform target; //추적할 타겟
    public float smoothing = 5f; //약간의 지연

    Vector3 offset; // 플레이어와 카메라의 오프셋 (거리)

    void Start()
    {
        offset = transform.position - target.position; //오프셋 설정
    }

    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset; //타겟위치 + 오프셋
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime); // 실제 카메라 이동 Lerp :부드럽게 이동
    }

}
