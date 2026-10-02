using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform Balle;

    [SerializeField]
    GameObject Plateform;

    [SerializeField]
    InputActionAsset inputActions;
    InputAction moveInput;


    Vector2 currentMove = Vector2.zero;

    [SerializeField]
    float rotationSpeed = 90f; // degrees per second when rotating

    float offsetCameraY = 6.5f; // Offset for the camera's Y position

    [SerializeField]
    float cameraMoveDuration = 0.5f; // duration of smooth camera move in seconds

    Coroutine cameraMoveCoroutine;

    UnityEngine.Events.UnityAction<int> scoreChangedHandler;



    private void Start()
    {
        moveInput = inputActions.FindAction("Move");
        moveInput.Enable();

        // Update currentMove when the action is performed. Supports both Vector2 and single-axis (float) bindings.
        moveInput.performed += ctx =>
        {
            if (ctx.control != null && ctx.control.valueType == typeof(float))
            {
                currentMove = new Vector2(ctx.ReadValue<float>(), 0f);
            }
            else
            {
                currentMove = ctx.ReadValue<Vector2>();
            }
        };
        moveInput.canceled += _ => currentMove = Vector2.zero;

        //appel de la fonction MoveCamera() pour que la caméra suive la balle quand le score change
        GameManager.onScoreChanged.AddListener(_ => MoveCamera());
    }


    private void OnDisable()
    {
        moveInput.Disable();
    }

    private void OnEnable()
    {
        moveInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // If there's input, rotate the platform while the key is held
        if (currentMove != Vector2.zero)
        {
            // Rotate around Y using the X component of the input (horizontal)
            float yRot = currentMove.x * rotationSpeed * Time.deltaTime;
            Plateform.transform.Rotate(Vector3.up, yRot, Space.Self);

            // Optional: rotate around X using the Y component (vertical) if needed
            // float xRot = currentMove.y * rotationSpeed * Time.deltaTime;
            // Plateform.transform.Rotate(Vector3.right, xRot, Space.Self);
        }
    }

    void MoveCamera()
    {
        if (cameraMoveCoroutine != null)
        {
            StopCoroutine(cameraMoveCoroutine);
        }
        cameraMoveCoroutine = StartCoroutine(SmoothMoveCamera());
    }

    System.Collections.IEnumerator SmoothMoveCamera()
    {
        if (Camera.main == null || Balle == null)
        {
            yield break;
        }

        Transform cam = Camera.main.transform;
        Vector3 start = cam.position;
        Vector3 target = new Vector3(start.x, start.y - 2, start.z);

        float elapsed = 0f;
        if (cameraMoveDuration <= 0f)
        {
            cam.position = target;
            yield break;
        }

        while (elapsed < cameraMoveDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / cameraMoveDuration);
            cam.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        cam.position = target;
        cameraMoveCoroutine = null;
    }
}
