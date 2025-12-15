using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Transform[] _points;

    private int _currentPointIndex = 0;

    private void Start()
    {
        if (_points.Length == 0)
        {
            Debug.LogError("MovingPlatform has no points assigned!");
            enabled = false;
            return;
        }

        transform.position = _points[0].position;
    }

    private void Update()
    {
        MovePlatform();
    }

    // ------------------------- Movement -------------------------

    private void MovePlatform()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            _points[_currentPointIndex].position,
            _speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, _points[_currentPointIndex].position) < 0.05f)
        {
            _currentPointIndex++;

            if (_currentPointIndex >= _points.Length)
                _currentPointIndex = 0;
        }
    }

    // ------------------------- Player Parenting -------------------------

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
