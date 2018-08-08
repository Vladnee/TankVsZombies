using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private float _health;
    [SerializeField] [Range(0, 1)] private float _defence;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _body;
    [SerializeField] private List<PlayerWeapon> Weapons;

    private int _currentSelectWeaponIndex = 0;
    private Rigidbody2D _rigidbody2D;

    private Vector2 _currentVelocity;
    private float _currentHealth;

    private void Awake()
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning("PlayerBehaviour: Rigidbody2D not exist");
        }
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
            _selectDown();
            EventManager.SelectedWeaponChange.OnChangeTrigger(_currentSelectWeaponIndex);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            _selectUp();
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
        float newMagnitude = newVelocity.magnitude;

        if (newVelocity != Vector2.zero)
        {
            // if vectors have opposite directions, make perpendicular vector in less side
            // it's need for create smooth drift
            if (Vector2.Dot(newVelocity, _currentVelocity) < 0)
            {
                Vector2 perpendicular = _getPerpendicularVector(_currentVelocity);
                newVelocity = perpendicular * Mathf.Sign(Vector2.Dot(perpendicular, newVelocity));
            }

            _currentVelocity = Vector2.Lerp(_currentVelocity.normalized, newVelocity.normalized,
                (10 * newMagnitude) * Time.fixedDeltaTime);

            // apply _currentVelocity for rigidbody2D
            if (_rigidbody2D != null)
            {
                if (newMagnitude > 0.4f)
                {
                    _rigidbody2D.AddForce(_currentVelocity.normalized * _speed * _rigidbody2D.mass,
                        ForceMode2D.Force);
                }
                _body.transform.rotation = _currentVelocity.normalized.LookRotation2D();
            }
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

    private void _selectDown()
    {
        Weapons[_currentSelectWeaponIndex].gameObject.SetActive(false);
        if (_currentSelectWeaponIndex == 0)
        {
            _currentSelectWeaponIndex = Weapons.Count - 1;
        }
        else
        {
            _currentSelectWeaponIndex--;
        }
        Weapons[_currentSelectWeaponIndex].gameObject.SetActive(true);
    }

    private void _selectUp()
    {
        Weapons[_currentSelectWeaponIndex].gameObject.SetActive(false);
        if (_currentSelectWeaponIndex == Weapons.Count - 1)
        {
            _currentSelectWeaponIndex = 0;
        }
        else
        {
            _currentSelectWeaponIndex++;
        }
        Weapons[_currentSelectWeaponIndex].gameObject.SetActive(true);
    }

    private Vector2 _getPerpendicularVector(Vector2 vect)
    {
        return new Vector2(-vect.y, vect.x).normalized;
    }
}
