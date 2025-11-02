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
    [Tooltip("Animator'deki hız parametresinin adı.")]
    [SerializeField] string speedParameterName = "Speed"; 
    
    private NavMeshAgent agent;
    private Animator animator;
    
    private int speedParamID;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        speedParamID = Animator.StringToHash(speedParameterName);
        
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
        
        float currentSpeed = agent.velocity.magnitude;
        animator.SetFloat(speedParamID, currentSpeed, animationDampTime,Time.deltaTime);
    }
}