using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBehaviour : MonoBehaviour
{
    [Header("Data")] [SerializeField] private float _health;
    [SerializeField] [Range(0, 1)] private float _defence;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _body;
    [SerializeField] private List<PlayerWeapon> Weapons;

    private int _currentSelectWeaponIndex;

    private Rigidbody2D _rigidbody2D;

    private Vector2 _currentVelocity;
    private float _currentHealth;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _currentHealth = _health;
        EventManager.PlayerHealthChange.OnChangeTrigger(_currentHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _nextWeapon();
            EventManager.SelectedWeaponChange.OnChangeTrigger(_currentSelectWeaponIndex);
        }

        if (Input.GetKey(KeyCode.X))
        {
            Weapons[_currentSelectWeaponIndex].Shot();
        }
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 newVelocity = new Vector2(x, y);

        if (newVelocity != Vector2.zero)
        {
            float newMagnitude = newVelocity.magnitude;
            // if vectors have opposite directions, make perpendicular vector in less side
            // it's need for create smooth drift
            if (Vector2.Dot(newVelocity, _currentVelocity) < 0)
            {
                Vector2 perpendicular = _currentVelocity.PerpendicularClockwise();
                newVelocity = perpendicular * Mathf.Sign(Vector2.Dot(perpendicular, newVelocity));
            }

            _currentVelocity = Vector2.Lerp(_currentVelocity.normalized, newVelocity,
                (10 * newMagnitude) * Time.fixedDeltaTime);

            // apply _currentVelocity for rigidbody2D
            if (newMagnitude > 0.4f)
            {
                _rigidbody2D.AddForce(_currentVelocity.normalized * _speed * _rigidbody2D.mass,
                    ForceMode2D.Force);
            }
            _body.transform.rotation = _currentVelocity.normalized.LookRotation2D();
        }
    }

    public void MakeDamage(float damage)
    {
        _currentHealth -= damage * (1 - _defence);
        EventManager.PlayerHealthChange.OnChangeTrigger(_currentHealth);
        if (_currentHealth <= 0)
        {
            EventManager.GameOver.Trigger();
        }
    }

    private void _nextWeapon()
    {
        Weapons[_currentSelectWeaponIndex].gameObject.SetActive(false);

        _currentSelectWeaponIndex++;

        if (_currentSelectWeaponIndex > Weapons.Count - 1)
        {
            _currentSelectWeaponIndex = 0;
        }

        Weapons[_currentSelectWeaponIndex].gameObject.SetActive(true);
    }
}
