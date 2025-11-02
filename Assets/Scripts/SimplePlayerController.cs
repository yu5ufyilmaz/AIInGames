using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(CharacterController))]
public class SimplePlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5.0f;
    public float lookSensitivity = 0.5f;
    public float gravity = -20.0f;

    [Header("Kamera")]
    public Camera playerCamera; 

    private CharacterController controller;
    private float cameraVerticalRotation = 0f;
    private Vector3 playerVelocity;
    
    private Vector2 moveInput;
    private Vector2 lookInput;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void Update()
    {
        float mouseX = lookInput.x * lookSensitivity;
        transform.Rotate(Vector3.up * mouseX);
        
        float mouseY = lookInput.y * lookSensitivity;
        
        cameraVerticalRotation += mouseY; 
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
        
        Vector3 moveDirection = (transform.forward * moveInput.y) + (transform.right * moveInput.x);
        
        controller.Move(moveDirection * (moveSpeed * Time.deltaTime));
        
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}