using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private Rigidbody2D _bulletRigidbody;
    private Vector2 _direction;
    private float _speed;
    private float _lifeTime = 5f;
    private float _currentTime = 0f;
    private float _damage;

    public void InitialMoveInfo(Vector2 direction, float speed)
    {
        _direction = direction.normalized;
        _speed = speed;
    }

    private void Throw() {
        _bulletRigidbody.velocity = _direction * _speed;
    }

    private void Start() {
        _bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        Throw();
        _currentTime += Time.deltaTime;

        if (_currentTime > _lifeTime) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "tag_Player") {
            collision.GetComponent<Unit>().GetDamage(_damage);
        }
    }
}
