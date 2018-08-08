﻿using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Transform _bulletsLair;
    [SerializeField] private Bullet _prefabBullet;
    [SerializeField] private Transform _aim;
    [SerializeField] private int _speedBullet = 100;
    [SerializeField] [Range(0, 10)] [Header("Max angle")] private float _accuracy;
    [SerializeField] [Range(1, 60)] [Header("Per second")] private int _fireRate;

    private float _lastFireTime;

    private ReusableFactory _bulletsFactory;

    private void Awake()
    {
        _bulletsFactory = new ReusableFactory(_prefabBullet, _bulletsLair);
    }

    public void Shot()
    {
        if (_lastFireTime < Time.time)
        {
            float shift = Random.Range(-_accuracy, _accuracy);
            var dir = Quaternion.AngleAxis(shift, Vector3.forward) * (_aim.position - transform.position).normalized;
            Bullet bullet = _bulletsFactory.Produce(transform.position) as Bullet;
            bullet.AddSpeed(dir, _speedBullet);
            _lastFireTime = Time.time + 1f / _fireRate;
        }
    }
}
