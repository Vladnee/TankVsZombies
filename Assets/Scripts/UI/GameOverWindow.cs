using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverWindow : BaseUIWindow
{
    [SerializeField] private TextMeshProUGUI _timeInGame;
    [SerializeField] private TextMeshProUGUI _counterKills;

    private void Start()
    {
        EventManager.GameOver.OnTrigger += Open;
    }

    private void OnDestroy()
    {
        EventManager.GameOver.OnTrigger -= Open;
    }

    public override void Open()
    {
        base.Open();
        _setWindowData();
        Time.timeScale = 0;
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void _setWindowData()
    {
        _counterKills.text = string.Format("Zombies\nkilled: {0}", PlayController.Instance.CounterKilled);
        _timeInGame.text = string.Format("Time: {0}", TimeSpan.FromSeconds((int)(Time.time - PlayController.Instance.TimeStartPlay)));
    }
}
