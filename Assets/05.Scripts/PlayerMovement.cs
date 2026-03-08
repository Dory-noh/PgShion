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

    public Transform cameraPoint;
    public Animator animator;
    public Transform startPoint;

    public float cameraRotateSpeed = 120f;
    public float cameraResetSpeed = 5f;

    private bool isCameraMode = false;

    private float baseX = 25.4f;
    private float baseY = -90f;
    private float currentY;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        // 시작할 때 현재 달팽이의 Y축 각도를 초기값으로 설정
        targetYRotation = transform.eulerAngles.y;
        currentY = baseY;
        GoStartPoint();
    }

    public void GoStartPoint()
    {
        gameObject.transform.position = startPoint.position;
    }

    void Update()
    {
        // 입력 방식(마우스/터치)에 상관없이 회전 로직을 실행
        HandleRotationInput();

        if (isCameraMode)
        {
            float delta = Input.GetAxis("Mouse X") * cameraRotateSpeed * Time.deltaTime;
            currentY += delta;

            // 앞모습까지 허용 (뒤 기준 ±180)
            currentY = Mathf.Clamp(currentY, baseY - 180f, baseY + 180f);

            cameraPoint.localRotation = Quaternion.Euler(baseX, currentY, 0f);
        }
        else
        {
            // 자연스럽게 기본값(-90)으로 복귀
            currentY = Mathf.LerpAngle(currentY, baseY, Time.deltaTime * cameraResetSpeed);
            cameraPoint.localRotation = Quaternion.Euler(baseX, currentY, 0f);
        }
    }

    void FixedUpdate()
    {
        //이동시에만 꿈틀 애니메이션 적용
        animator.SetBool("IsMove",isMoving);

        // 1. 부드러운 회전 적용
        Quaternion targetRot = Quaternion.Euler(0f, targetYRotation, 0f);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));

        // 2. 이동 적용
        if (isMoving)
        {
            // 현재 바라보는 방향(targetRot) 기준으로 오른쪽(Vector3.right)으로 이동
            Vector3 direction = targetRot * Vector3.left;
            direction.y = 0; // 중력 외의 Y축 이동 차단

            Vector3 moveStep = direction.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveStep);
        }
    }

    private void HandleRotationInput()
    {
        //마우스 입력을 이용한 회전
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

    public void StartCameraMode()
    {
        isCameraMode = true;
        isMoving = false;
    }

    public void StopCameraMode()
    {
        isCameraMode = false;
    }
}