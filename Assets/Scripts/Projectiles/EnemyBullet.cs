using UnityEngine;

// Enemy bullet — attach to the enemy bullet prefab.
// Prefab needs: Rigidbody (Is Kinematic = true), Collider (Is Trigger = true).
public class EnemyBullet : MonoBehaviour
{
    public float speed = 2.7f;
    public int damage = 1;
    public float lifetime = 4f;

    void Start() => Destroy(gameObject, lifetime);

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
