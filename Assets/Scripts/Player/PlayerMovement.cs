using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //상단 public
    public float speed = 6f; // 속도


    //하단 private
    Vector3 movement; //동작 저장
    Animator anim; //애니메이션
    Rigidbody playerRigidbody; 
    int floorMask; // 플로어쿼드 -->(레이캐스트)
    float camRayLength = 100f; // 카메라에서 발사하는 광선의 길이

    private void Awake()
    {
        //참조설정
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // ahems Physics 를 업데이트
        // 일반적인 업데이트시스템은 렌더링과 함께 실행되지만 이건 물리효과 함께 실행
        float h = Input.GetAxisRaw("Horizontal"); // Input으로 -1, 0 ,1 만 가지는 Raw
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    private void Move(float h, float v)
    {
        movement.Set(h, 0f, v);

        // 벡터의경우 (대각선) 1.4가 되므로 동시에 누를경우 더빨라지는것을 방지
        movement = movement.normalized * speed * Time.deltaTime; //deltaTime 업데이트 호출 시간 간격 1/50초

        playerRigidbody.MovePosition(transform.position + movement);
    }

    
    private void Turning() // 마우스 기준이므로 파라미터 안받은것
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 포지션 찾기

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask)) //floorHit에 저장
        {
            Vector3 playerToMouse = floorHit.point - transform.position; // 선택한지점 - 현재플레이어 위치
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse); // Quaternion : 회전 저장, Z축이 기본 전진축
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    private void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f; // h나 v가 0이 아닐경우 true 세팅
        anim.SetBool("IsWalking", walking);
    }
}
