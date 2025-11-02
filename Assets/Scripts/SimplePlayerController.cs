using UnityEngine;

// CharacterController component'ini bu objeye zorunlu kıl
[RequireComponent(typeof(CharacterController))]
public class SimplePlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;

    [Header("Kamera")]
    public Camera playerCamera; // Buraya Main Camera'yı sürükleyin
    public float gravity = -20.0f; // Yerçekimi

    private CharacterController controller;
    private float cameraVerticalRotation = 0f; 
    private Vector3 playerVelocity; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
        
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        cameraVerticalRotation -= mouseY;
        
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);

        
        float horizontal = Input.GetAxis("Horizontal"); // A, D tuşları
        float vertical = Input.GetAxis("Vertical");     // W, S tuşları

        Vector3 moveDirection = (transform.forward * vertical) + (transform.right * horizontal);
        
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        
        playerVelocity.y += gravity * Time.deltaTime;
        
        controller.Move(playerVelocity * Time.deltaTime);
    }
}