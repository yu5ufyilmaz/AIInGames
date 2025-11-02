using UnityEngine;
using UnityEngine.AI; 

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class FollowAgent : MonoBehaviour
{
    [Header("Hedef")]
    public Transform playerTransform;

    [Header("Animasyon Ayarları")]
    [Tooltip("Animasyon geçişlerinin ne kadar yumuşak olacağı.")]
    [SerializeField] float animationDampTime = 0.1f;
    
    private string paramSpeed = "Speed";
    private string paramGrounded = "Grounded";
    private string paramMotionSpeed = "MotionSpeed";
    
    private NavMeshAgent agent;
    private Animator animator;
    
    private int animIDSpeed;
    private int animIDGrounded;
    private int animIDMotionSpeed; 
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        animIDSpeed = Animator.StringToHash(paramSpeed);
        animIDGrounded = Animator.StringToHash(paramGrounded);
        animIDMotionSpeed = Animator.StringToHash(paramMotionSpeed);
        
        if (playerTransform == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                playerTransform = playerObject.transform;
            }
            else
            {
                Debug.LogError("FollowAgent: Player Transform'u bulunamadı! 'Player' objenizin Tag'ini 'Player' olarak ayarladığınızdan emin olun.", this);
            }
        }
    }

    void Update()
    {
        if (playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
        }
        
        float currentPhysicalSpeed = agent.velocity.magnitude;

     
        animator.SetBool(animIDGrounded, true);
        animator.SetFloat(animIDSpeed, currentPhysicalSpeed, animationDampTime, Time.deltaTime);
        
        float motionSpeedValue = currentPhysicalSpeed > 0.1f ? 1.0f : 0.0f;
        animator.SetFloat(animIDMotionSpeed, motionSpeedValue);
    }
}