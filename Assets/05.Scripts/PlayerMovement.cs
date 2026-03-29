using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class SnailMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float speed = 2f;
    public float rotationSpeed = 5f;

    private bool isMoving = false;
    private Rigidbody rb;

    public JoyStick joystick;

    public Transform cameraPoint;
    public Animator animator;
    public Transform startPoint;

    public float cameraRotateSpeed = 120f;

    private float baseX = 25.4f;
    private float baseY = -90f;
    private float currentY;

    private int joystickFingerId = -1;
    private bool isMouseOnJoystick = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        currentY = baseY;
        GoStartPoint();
    }

    public void GoStartPoint()
    {
        transform.position = startPoint.position;
    }

    void Update()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (joystickFingerId == -1)
            {
                if (IsTouchOnJoystick(touch))
                {
                    joystickFingerId = touch.fingerId;
                    continue;
                }
            }

            if (touch.fingerId == joystickFingerId)
                continue;

            if (touch.phase == TouchPhase.Moved)
            {
                currentY += touch.deltaPosition.x * 0.2f;
            }
        }

        if (Input.touchCount == 0)
            joystickFingerId = -1;

        if (Input.GetMouseButtonDown(0))
            isMouseOnJoystick = IsMouseOnJoystick();

        if (Input.GetMouseButtonUp(0))
            isMouseOnJoystick = false;

        if (Input.touchCount == 0 && Input.GetMouseButton(0))
        {
            if (!isMouseOnJoystick)
            {
                float delta = Input.GetAxis("Mouse X") * cameraRotateSpeed * Time.deltaTime;
                currentY += delta;
            }
        }

        currentY = Mathf.Clamp(currentY, baseY - 180f, baseY + 180f);
        cameraPoint.localRotation = Quaternion.Euler(baseX, currentY, 0f);
    }

    void FixedUpdate()
    {
        if (joystick == null)
            return;

        Vector2 input = joystick.InputDir;

        if (Mathf.Abs(input.x) > 0.1f)
        {
            float turn = input.x * rotationSpeed * Time.fixedDeltaTime * 10f;
            Quaternion deltaRot = Quaternion.Euler(0f, turn, 0f);
            rb.MoveRotation(rb.rotation * deltaRot);
        }

        if (input.y > 0.1f)
        {
            Vector3 direction = transform.rotation * Vector3.left;
            direction.y = 0;

            Vector3 moveStep = direction.normalized * input.y * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveStep);

            animator.SetBool("IsMove", true);
        }
        else
        {
            animator.SetBool("IsMove", false);
            rb.linearVelocity = Vector3.zero;
        }
    }

    bool IsTouchOnJoystick(Touch touch)
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = touch.position;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        foreach (var r in results)
        {
            if (r.gameObject.transform.IsChildOf(joystick.transform))
                return true;
        }

        return false;
    }

    bool IsMouseOnJoystick()
    {
        PointerEventData data = new PointerEventData(EventSystem.current);
        data.position = Input.mousePosition;

        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);

        foreach (var r in results)
        {
            if (r.gameObject.transform.IsChildOf(joystick.transform))
                return true;
        }

        return false;
    }

    public void OnPointerDown(PointerEventData eventData) => isMoving = true;
    public void OnPointerUp(PointerEventData eventData) => isMoving = false;
}