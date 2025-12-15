using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Transform[] _points;

    private int _currentPointIndex = 0;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        if (_points.Length == 0)
        {
            Debug.LogError("Enemy has no points assigned!");
            enabled = false;
            return;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = _points[0].position;
    }

    private void Update()
    {
        MoveEnemy();
        UpdateDirectionSprite();
    }

    // ------------------------- Movement -------------------------

    private void MoveEnemy()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            _points[_currentPointIndex].position,
            _speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, _points[_currentPointIndex].position) < 0.25f)
        {
            _currentPointIndex++;

            if (_currentPointIndex >= _points.Length)
                _currentPointIndex = 0;
        }

    }

    // ------------------------- Sprite Direction -------------------------

    private void UpdateDirectionSprite()
    {
        Vector3 target = _points[_currentPointIndex].position;

        // Flip depending on which direction the enemy is moving
        _spriteRenderer.flipX = target.x > transform.position.x;
    }
}
