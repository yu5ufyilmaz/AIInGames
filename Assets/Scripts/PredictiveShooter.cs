using UnityEngine;

public class PredictiveShooter : MonoBehaviour
{
    [Header("Hedefleme")]
    public Transform playerTransform; 

    [Header("Atış Ayarları")]
    public GameObject bulletPrefab;
    public Transform firePoint;     
    public float bulletSpeed = 50f; 

    [Header("Zamanlama")]
    public float fireRate = 1.5f; 
    private float fireTimer = 0f;

    [Header("Tahminleme (Prediction)")]
    private Vector3 lastPlayerPosition;
    private Vector3 playerVelocity; 

    void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }
        
        if(playerTransform != null)
        {
            lastPlayerPosition = playerTransform.position;
        }

        fireTimer = fireRate;
    }

    void Update()
    {
        if (playerTransform == null) return; 

 
        lastPlayerPosition = playerTransform.position; 
        
        Vector3 predictedPosition = CalculatePredictedPosition();
        
        Vector3 aimDirection = predictedPosition - transform.position;
        aimDirection.y = 0; 
        transform.rotation = Quaternion.LookRotation(aimDirection);
        
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            Shoot();
            fireTimer = fireRate;
        }
    }

    Vector3 CalculatePredictedPosition()
    {
        float distance = Vector3.Distance(firePoint.position, playerTransform.position);
        float timeToHit = distance / bulletSpeed;
        Vector3 futurePosition = playerTransform.position + (playerVelocity * timeToHit);
        return futurePosition;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Bullet>().bulletSpeed = this.bulletSpeed;
    }
}