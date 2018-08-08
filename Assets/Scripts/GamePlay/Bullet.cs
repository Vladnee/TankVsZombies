using System.Collections;
using UnityEngine;

public class Bullet : ReusableGameObject
{
    [SerializeField] private float _damage;

    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    private void Awake()
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning("Bullet: Rigidbody2D not exist");
        }

        if (GetComponent<Collider2D>() != null)
        {
            _collider2D = GetComponent<Collider2D>();
        }
        else
        {
            Debug.LogWarning("Bullet: Collider2D not exist");
        }
    }

    public override void Enable(Vector2 position)
    {
        base.Enable(position);
        _collider2D.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Zombie"))
        {
            coll.gameObject.GetComponent<ZombieBehaviour>().MakeDamage(_damage);
        }
        StartCoroutine(_disable());
    }

    public void AddSpeed(Vector2 dir, float speed)
    {
        _rigidbody2D.AddForce(dir * speed, ForceMode2D.Impulse);
    }

    // wait 0.1 second for trail visualize
    private IEnumerator _disable()
    {
        _collider2D.enabled = false;
        _rigidbody2D.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);
        Disable();
    }
}
