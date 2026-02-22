using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class SnailMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float speed = 2f;
    public float rotationSpeed = 5f;

    private bool isMoving = false;
    private Rigidbody rb;
    private float targetYRotation = 0f;

    private Vector2 lastTouchPos;
    private bool touchActive = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // X, Z축 회전 방지 (옆으로 안 넘어지게)
    }

    void Start()
    {
        // 시작할 때 현재 달팽이의 각도를 초기값으로 설정
        targetYRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        HandleRotationInput();
    }

    void FixedUpdate()
    {
        // 1. 회전 적용
        // 짐벌락 방지를 위해 Quaternion.Euler로 목표 각도를 만들고 rb에 적용합니다.
        Quaternion targetRot = Quaternion.Euler(0f, targetYRotation, 0f);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime));

        if (isMoving)
        {
            // [범인 검거!] rb.rotation을 쓰면 회전이 끝날 때까지 방향이 안 변할 수 있습니다.
            // 현재 내가 '목표로 하는 각도(targetRot)'의 오른쪽 방향을 미리 계산해서 이동해야 
            // 회전하는 즉시 즉각적으로 방향이 바뀝니다.
            Vector3 direction = targetRot * Vector3.right;

            direction.y = 0; // 높이 변화 차단

            // normalized를 확실히 해줘야 사선 이동 시 속도가 빨라지지 않습니다.
            Vector3 moveStep = direction.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveStep);
        }
    }

    private void HandleRotationInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {
            float deltaX = Input.GetAxis("Mouse X");
            if (Mathf.Abs(deltaX) > 0.01f)
                targetYRotation += deltaX * 10f;
        }
#endif
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) { lastTouchPos = touch.position; touchActive = true; }
            else if (touch.phase == TouchPhase.Moved && touchActive)
            {
                float deltaX = touch.position.x - lastTouchPos.x;
                lastTouchPos = touch.position;
                targetYRotation += deltaX * 0.2f;
            }
            else if (touch.phase == TouchPhase.Ended) { touchActive = false; }
        }
#endif
    }

    // UI 버튼 연결용 (Event Trigger 사용)
    public void OnPointerDown(PointerEventData eventData) => isMoving = true;
    public void OnPointerUp(PointerEventData eventData) => isMoving = false;

    public void StartMoving() => isMoving = true;
    public void StopMoving() => isMoving = false;
}