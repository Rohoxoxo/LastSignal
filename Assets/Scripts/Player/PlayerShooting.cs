using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;

    private float nextFireTime;
    private float damageMultiplier = 1f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        if (bulletPrefab == null || firePoint == null) return;
        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = b.GetComponent<Bullet>();
        if (bullet != null)
            bullet.damageMultiplier = damageMultiplier;
    }

    public void ApplyDamageBoost(float multiplier, float duration)
    {
        damageMultiplier = multiplier;
        CancelInvoke(nameof(ResetDamage));
        Invoke(nameof(ResetDamage), duration);
    }

    void ResetDamage() => damageMultiplier = 1f;
}
