using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [Header("Left bottom corner")] [SerializeField] private Vector2 _min;
    [Header("Right top corner")] [SerializeField] private Vector2 _max;
    private Transform _target;

    private float _constZ;

    private Bounds _gamePlayBox;

    // create Bounds _gamePlayBox
    void Start()
    {
        _constZ = transform.position.z;
        _gamePlayBox.SetMinMax(_min, _max);
        _target = PlayController.Instance.Player.transform;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _target.position, 7 * Time.deltaTime);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, _gamePlayBox.min.x, _gamePlayBox.max.x),
            Mathf.Clamp(transform.position.y, _gamePlayBox.min.y, _gamePlayBox.max.y), _constZ);
    }
}