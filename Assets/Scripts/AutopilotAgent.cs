using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AutopilotAgent : MonoBehaviour
{
    [Header("Hedefler")]
    public Transform playerTransform;
    public Transform baseLocation; 

    [Header("Durum")]
    [SerializeField] private bool isAutopilotActive = false;
    
    [Header("Animasyon Ayarları")]
    [SerializeField] float animationDampTime = 0.1f;
    private string paramSpeed = "Speed";
    private string paramGrounded = "Grounded";
    private string paramMotionSpeed = "MotionSpeed";
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDMotionSpeed;

    private NavMeshAgent agent;
    private Animator animator;
    private PlayerInput playerInput; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        animIDSpeed = Animator.StringToHash(paramSpeed);
        animIDGrounded = Animator.StringToHash(paramGrounded);
        animIDMotionSpeed = Animator.StringToHash(paramMotionSpeed);
        
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }

        if (baseLocation == null)
        {
            Debug.LogError("AutopilotAgent: 'Base Location' atanmamış! Lütfen Inspector'dan sürükleyin.", this);
        }
        
        if (playerTransform != null)
        {
            playerInput = playerTransform.GetComponent<PlayerInput>();
            if (playerInput != null)
            {
                playerInput.actions["Autopilot"].performed += ToggleAutopilot;
            }
            else
            {
                Debug.LogError("AutopilotAgent: Player objesinde 'PlayerInput' bileşeni bulunamadı!", this);
            }
        }
    }
    
    private void ToggleAutopilot(InputAction.CallbackContext context)
    {
        isAutopilotActive = !isAutopilotActive;
    }

    void Update()
    {
        Transform currentTarget;

        if (isAutopilotActive)
        {
            currentTarget = baseLocation;
        }
        else
        {
            currentTarget = playerTransform;
        }
        
        if (currentTarget != null)
        {
            agent.SetDestination(currentTarget.position);
        }
        float currentPhysicalSpeed = agent.velocity.magnitude;
        
        animator.SetBool(animIDGrounded, true); 
        animator.SetFloat(animIDSpeed, currentPhysicalSpeed, animationDampTime, Time.deltaTime);
        
        float motionSpeedValue = currentPhysicalSpeed > 0.1f ? 1.0f : 0.0f;
        animator.SetFloat(animIDMotionSpeed, motionSpeedValue);
    }
    
    private void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.actions["Autopilot"].performed -= ToggleAutopilot;
        }
    }
}