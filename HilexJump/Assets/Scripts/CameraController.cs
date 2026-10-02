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

    [SerializeField]
    float cameraMoveDuration = 0.5f; // duration of smooth camera move in seconds

    Coroutine cameraMoveCoroutine;

    UnityEngine.Events.UnityAction<int> scoreChangedHandler;



    private void Start()
    {
        if (inputActions != null)
        {
            moveInput = inputActions.FindAction("Move");
            if (moveInput != null)
            {
                moveInput.Enable();
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
            }
        }

        // appel de la fonction MoveCamera() pour que la caméra suive la balle quand le score change
        GameManager.onScoreChanged.AddListener(_ => MoveCamera());

        // Disable input when finish is triggered
        if (TriggerFinish.onFinishTriggered != null)
        {
            TriggerFinish.onFinishTriggered.AddListener(OnFinishTriggered);
        }
    }


    private void OnDisable()
    {
        if (moveInput != null)
        {
            moveInput.Disable();
        }

        if (TriggerFinish.onFinishTriggered != null)
        {
            TriggerFinish.onFinishTriggered.RemoveListener(OnFinishTriggered);
        }
    }

    private void OnEnable()
    {
        if (moveInput != null)
        {
            moveInput.Enable();
        }
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

    void OnFinishTriggered()
    {
        currentMove = Vector2.zero;
        if (moveInput != null)
        {
            moveInput.Disable();
        }
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
