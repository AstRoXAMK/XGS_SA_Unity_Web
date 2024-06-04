using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Sensitivity")]
    public float mouseSensitivity = 100f;
    public float keyRotationSpeed = 1.0f;

    [Header("Player Orientation")]
    public Transform playerBody;

    float xRotation = 0f;

    [Header("Camera controls (make private later)")]
    public float screenHeight = 720f;

    private float screenDivide = Screen.width / 2;

    [Header("Rotation variables")]
    public float rotSpeed = 0.3f;
    [Range(-1, 1)]
    public int dir = -1;

    // Rotation variables
    private Touch initTouch = new Touch();
    private float rotX = 0f;
    private float rotY = 0f;
    private Vector3 origRot;

    private void Start()
    {
        origRot = playerBody.transform.eulerAngles;
        rotX = origRot.x;
        rotY = origRot.y;
    }

    private void FixedUpdate()
    {
        if (Screen.height <= screenHeight)
        {
            KeyboardControls("Mouse X", "Mouse Y", mouseSensitivity);
            KeyboardControls("Rotate X", "Rotate Y", keyRotationSpeed);
        }
        else
        {
            MobileControls(screenDivide);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xAxis"></param>
    /// <param name="yAxis"></param>
    /// <param name="sensitivity"></param>
    void KeyboardControls(string xAxis, string yAxis, float sensitivity)
    {
        float rotationX = Input.GetAxisRaw(xAxis) * sensitivity * Time.deltaTime;
        float rotationY = Input.GetAxisRaw(yAxis) * sensitivity * Time.deltaTime;

        RotationControl(rotationX, rotationY);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="axisX"></param>
    /// <param name="axisY"></param>
    void RotationControl(float axisX, float axisY)
    {
        xRotation -= axisY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * axisX);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="screenWidth"></param>
    void MobileControls(float screenWidth)
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.position.x > screenWidth)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        initTouch = touch;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {

                        float deltaX = initTouch.position.x - touch.position.x;
                        float deltaY = initTouch.position.y - touch.position.y;

                        rotX -= deltaY * Time.deltaTime * rotSpeed * dir;
                        rotY += deltaX * Time.deltaTime * rotSpeed * dir;

                        rotX = Mathf.Clamp(rotX, -90f, 90f);

                        playerBody.transform.eulerAngles = new Vector3(rotX, rotY, 0f);
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        initTouch = new Touch();
                    }
                }
            }
        }
    }
}