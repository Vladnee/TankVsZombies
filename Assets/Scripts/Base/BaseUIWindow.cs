using UnityEngine;

public class BaseUIWindow : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private BlurRenderer _blurRenderer;

    public virtual void Open()
    {
        _blurRenderer.SetBlur();
        _content.SetActive(true);
    }

    public virtual void Close()
    {
        _content.SetActive(false);
    }
}
