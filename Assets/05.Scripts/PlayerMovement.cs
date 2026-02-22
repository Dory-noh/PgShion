using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class SnailMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("이동 및 회전 설정")]
    public float speed = 2f;
    public float rotationSpeed = 5f;

    private bool isMoving = false;
    private Rigidbody rb;
    private float targetYRotation = 0f;

    // 모바일 터치 대응 변수
    private Vector2 lastTouchPos;
    private bool touchActive = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        // 시작할 때 현재 달팽이의 Y축 각도를 초기값으로 설정
        targetYRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        // 입력 방식(마우스/터치)에 상관없이 회전 로직을 실행
        HandleRotationInput();
    }

    void FixedUpdate()
    {
        // 1. 부드러운 회전 적용
        Quaternion targetRot = Quaternion.Euler(0f, targetYRotation, 0f);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));

        // 2. 이동 적용
        if (isMoving)
        {
            // 현재 바라보는 방향(targetRot) 기준으로 오른쪽(Vector3.right)으로 이동
            // (모델의 앞방향이 다를 경우 Vector3.forward 등으로 수정 가능)
            Vector3 direction = targetRot * Vector3.right;
            direction.y = 0; // 중력 외의 Y축 이동 차단

            Vector3 moveStep = direction.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveStep);
        }
    }

    private void HandleRotationInput()
    {
        // --- 방법 1: 마우스 입력을 이용한 회전
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPos = Input.mousePosition;
            touchActive = true;
        }
        else if (Input.GetMouseButton(0) && touchActive)
        {
            // 드래그한 거리만큼 각도 변경
            float deltaX = Input.mousePosition.x - lastTouchPos.x;
            lastTouchPos = Input.mousePosition;

            // 0.2f는 감도
            targetYRotation += deltaX * 0.2f;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            touchActive = false;
        }

      
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                // 터치 델타값을 이용해 각도 변경
                targetYRotation += touch.deltaPosition.x * 0.2f;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) => isMoving = true;
    public void OnPointerUp(PointerEventData eventData) => isMoving = false;

    // 외부 호출용 함수
    public void StartMoving() => isMoving = true;
    public void StopMoving() => isMoving = false;
}