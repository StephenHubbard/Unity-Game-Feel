using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private GameObject _hitVFX;
    [SerializeField] private float _knockBackForce = 5f;
    [SerializeField] private LayerMask _collisionLayers;
    [SerializeField] private int _damageAmount = 1;

    private bool _isInitialized = false;
    private Vector2 _fireDirection;
    private Vector2 _previousPosition;
    private Vector2 _playerPosOnFire; // getting player pos on fire in case player dies while bullet is in air
    private Rigidbody2D _rb;
    private Gun _gun;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _previousPosition = transform.position;
        _gun = FindObjectOfType<Gun>();
    }

    private void Update() {
        CheckCollision();
    }

    private void FixedUpdate()
    {
        MoveProjectile();
    }

    private void OnEnable()
    {
        _isInitialized = true;
        _playerPosOnFire = PlayerController.Instance.transform.position;
        Vector2 bulletSpawnPosition = _gun.BulletSpawnPoint.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _fireDirection = (mousePosition - bulletSpawnPosition).normalized;
        transform.position = bulletSpawnPosition;
        _previousPosition = transform.position;
    }

    private void OnDisable()
    {
        _isInitialized = false;
    }

    private void MoveProjectile()
    {
        _rb.velocity = _fireDirection * _moveSpeed;
    }

    // continous rb2d detection doesn't work on static tilemap if tilemap also has rb.  Tilemap needs it for composite collider2d for smooth walking for collider movement.
    private void CheckCollision()
    {
        if (!_isInitialized) return;

        Vector2 newPosition = _rb.position;
        Vector2 direction = (newPosition - _previousPosition).normalized;
        float distance = Vector2.Distance(newPosition, _previousPosition);

        RaycastHit2D hit = Physics2D.Raycast(_previousPosition, direction, distance, _collisionLayers);

        if (hit.collider != null)
        {
            Collide(hit);
            return;
        }

        _previousPosition = newPosition;
    }

    private void Collide(RaycastHit2D hit)
    {
        Instantiate(_hitVFX, transform.position, Quaternion.identity);

        IHitable iHitable = hit.collider.gameObject.GetComponent<IHitable>();
        iHitable?.TakeHit(hit, _playerPosOnFire, _knockBackForce, _damageAmount);

        _gun.ReleaseBulletFromPool(this);
    }
}