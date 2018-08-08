using UnityEngine;

public class PauseWindow : BaseUIWindow
{
    public override void Open()
    {
        base.Open();
        Time.timeScale = 0;
    }

    public override void Close()
    {
        base.Close();
        Time.timeScale = 1;
    }
}