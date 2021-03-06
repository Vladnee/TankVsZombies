﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayController : MonoBehaviour
{
    public static PlayController Instance;

    [Serializable]
    private class ZTypeFactory
    {
        [SerializeField] private ZombieBehaviour _behaviour;
        [SerializeField] [Range(0, 100)] private int _chance;

        private ReusableFactory<ZombieBehaviour> _zombiesFactory;

        public int Chance
        {
            get { return _chance; }
        }

        public void CreateFactory(Transform lair)
        {
            _zombiesFactory = new ReusableFactory<ZombieBehaviour>(_behaviour, lair);
        }

        public void ProduceZombie(Vector2 position)
        {
            _zombiesFactory.Produce(position);
        }
    }

    public PlayerBehaviour Player;

    [SerializeField] private Transform _zombiesLair;
    [SerializeField] private Transform _bloodstainLair;
    [SerializeField] private int _maxZombiesCount;
    [SerializeField] private List<ZTypeFactory> _zombieFactories = new List<ZTypeFactory>();
    [SerializeField] private ReusableGameObject _bloodstainPrefab;

    private ReusableFactory<ReusableGameObject> _bloodstainFactory;

    public int CounterKilled { get; private set; }
    public float TimeStartPlay { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _zombieFactories.ForEach(x => x.CreateFactory(_zombiesLair));
        _bloodstainFactory = new ReusableFactory<ReusableGameObject>(_bloodstainPrefab, _bloodstainLair);

        for (int i = 0; i < _maxZombiesCount; i++)
        {
            _instantiateZombie();
        }
        TimeStartPlay = Time.time;
    }

    private void _instantiateZombie()
    {
        // make ray in random direction, by only layer mask (Border) and where ray hit border we are instantiate zombie
        int angle = Random.Range(0, 360);
        var dir = Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.right;
        RaycastHit2D hit = Physics2D.Raycast(Vector2.zero, dir, 200f, LayerMask.GetMask("Border"));

        // sum all chances and create random value in this range, then find zFactory which inclusive this value
        int rand = Random.Range(0, _zombieFactories.Sum(x => x.Chance));
        int cursor = 0;
        foreach (var zFactory in _zombieFactories)
        {
            if (cursor <= rand && rand <= (cursor + zFactory.Chance))
            {
                zFactory.ProduceZombie(hit.point);
                break;
            }
            else
            {
                cursor += zFactory.Chance;
            }
        }
    }

    public void ZombieKill(Vector2 position)
    {
        _bloodstainFactory.Produce(position);
        CounterKilled++;
        EventManager.CounterKillsChange.OnChangeTrigger(CounterKilled);
        _instantiateZombie();
    }
}