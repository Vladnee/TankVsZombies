using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ZombieMovement))]
public class ZombieBehaviour : ReusableGameObject
{
    [Header("Data")] [SerializeField] private float _health;
    [SerializeField] private float _damage;
    [SerializeField] [Range(0, 1)] private float _defence;
    [SerializeField] private float _speed;

    [SerializeField] private Slider _healthBar;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _body;

    private ZombieMovement _zombieMovement;

    private float _currentHealth;

    private void Awake()
    {
        _zombieMovement = GetComponent<ZombieMovement>();
    }

    public override void Enable(Vector2 position)
    {
        base.Enable(position);
        _healthBar.maxValue = _health;
        _currentHealth = _health;
        _healthBar.value = _currentHealth;
        _zombieMovement.SetParameters(PlayController.Instance.Player.transform, _speed);
    }

    public void MakeDamage(float damage)
    {
        _currentHealth -= damage * (1 - _defence);
        _healthBar.value = _currentHealth;
        _animator.SetTrigger("hit");
        if (_currentHealth <= 0)
        {
            PlayController.Instance.ZombieKill(transform.position);
            Disable();
        }
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            coll.gameObject.GetComponent<PlayerBehaviour>().MakeDamage(_damage);
        }
    }

    private void FixedUpdate()
    {
        _body.transform.rotation = _zombieMovement.GetDirection().LookRotation2D(0);
    }
}