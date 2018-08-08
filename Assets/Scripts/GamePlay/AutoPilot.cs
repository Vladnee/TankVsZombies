using UnityEngine;

public class AutoPilot : MonoBehaviour
{
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private float _speed;
    private Transform _target;
    [SerializeField] private float _dimensionsRadius = 0.5f;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _toward;
    private Vector2 _currentVelocity;

    void Start()
    {
        if (GetComponent<Rigidbody2D>() != null)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogWarning("AutoPilot: Rigidbody2D not exist");
        }
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            _findWay(transform.position, _target.position);

            Vector2 newVelocity = _toward - (Vector2)transform.position;

            _currentVelocity = Vector2.Lerp(_currentVelocity.normalized, newVelocity.normalized,
                5 * Time.fixedDeltaTime);

            _rigidbody2D.AddForce(_currentVelocity.normalized * _speed * _rigidbody2D.mass, ForceMode2D.Force);
        }
    }

    public void SetParameters(Transform target, float speed)
    {
        _target = target;
        _speed = speed;
    }

    public Vector2 GetDirection()
    {
        return _currentVelocity;
    }

    // return bool if any ray find obstacles in front, and out this ray
    private bool _getInterestedDimensionsRay(Vector2 startPosition, Vector2 direction, out RaycastHit2D ray)
    {
        ray = new RaycastHit2D();

        RaycastHit2D dimensionsRight =
            Physics2D.Raycast(startPosition + _getPerpendicularVector(direction) * _dimensionsRadius,
                direction.normalized, direction.magnitude, _obstacleLayers);

        RaycastHit2D dimensionsLeft =
            Physics2D.Raycast(startPosition - _getPerpendicularVector(direction).normalized * _dimensionsRadius,
                direction.normalized, direction.magnitude,
                _obstacleLayers);

        if (dimensionsLeft.transform == null && dimensionsRight.transform == null)
        {
            RaycastHit2D main =
                Physics2D.Raycast(startPosition, direction.normalized, direction.magnitude,
                    _obstacleLayers);
            if (main.transform == null)
            {
                return false;
            }
            ray = main;
        }
        else
        {
            if (dimensionsRight.transform != null && dimensionsLeft.transform != null)
            {
                if (dimensionsLeft.distance < dimensionsRight.distance)
                {
                    ray = dimensionsLeft;
                }
                else
                {
                    ray = dimensionsRight;
                }
            }
            else if (dimensionsLeft.transform != null)
            {
                ray = dimensionsLeft;
            }
            else if (dimensionsRight.transform != null)
            {
                ray = dimensionsRight;
            }
        }
        return true;
    }

    // make dimensions rays, find obstacles in front and find way for bypass, then set it into _target
    private void _findWay(Vector2 startPosition, Vector2 finishPosition)
    {
        Vector2 direction = finishPosition - startPosition;

        RaycastHit2D interestedRay;

        if (!_getInterestedDimensionsRay(startPosition, direction, out interestedRay))
        {
            _toward = finishPosition;
            return;
        }

        Transform obstacleTransform = interestedRay.transform;
        Vector2 pointHit = interestedRay.point;

        Vector2 shift = _getPerpendicularVector(direction) * _dimensionsRadius * 2;

        shift *= Mathf.Sign(Vector2.Dot(shift, finishPosition - (Vector2)obstacleTransform.position));

        Vector2 pointRay = pointHit - (Vector2)obstacleTransform.position;

        Vector2 dir = (Vector2)obstacleTransform.position +
                      (shift + pointRay).normalized * pointRay.magnitude + shift;

        _toward = dir;
    }

    private Vector2 _getPerpendicularVector(Vector2 vect)
    {
        return new Vector2(-vect.y, vect.x).normalized;
    }
}