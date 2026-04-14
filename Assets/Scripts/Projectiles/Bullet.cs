using UnityEngine;

// Player bullet — attach to the player bullet prefab.
// Prefab needs: Rigidbody (Is Kinematic = true), Collider (Is Trigger = true).
public class Bullet : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 1;
    [HideInInspector] public float damageMultiplier = 1f;
    public float lifetime = 3f;

    void Start() => Destroy(gameObject, lifetime);

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>()?.TakeDamage(Mathf.RoundToInt(damage * damageMultiplier));
            Destroy(gameObject);
        }
    }
}
