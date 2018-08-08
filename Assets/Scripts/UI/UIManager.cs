using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider _playerHealthBar;
    [SerializeField] private TextMeshProUGUI _killsCounter;
    [SerializeField] private List<Toggle> _weapons;

    private void Awake()
    {
        _playerHealthBar.maxValue = 0;
        EventManager.PlayerHealthChange.OnChange += _setPlayerHealthBar;
        EventManager.CounterKillsChange.OnChange += _setKillsCounter;
        EventManager.SelectedWeaponChange.OnChange += _setSelectedWeapon;
    }

    private void OnDestroy()
    {
        EventManager.PlayerHealthChange.OnChange -= _setPlayerHealthBar;
        EventManager.CounterKillsChange.OnChange -= _setKillsCounter;
        EventManager.SelectedWeaponChange.OnChange -= _setSelectedWeapon;
    }

    private void _setPlayerHealthBar(float health)
    {
        if (health > _playerHealthBar.maxValue)
        {
            _playerHealthBar.maxValue = health;
        }
        _playerHealthBar.value = health;
    }

    private void _setKillsCounter(int kills)
    {
        _killsCounter.text = string.Format("{0}", kills);
    }

    private void _setSelectedWeapon(int index)
    {
        _weapons[index].isOn = true;
    }

}
