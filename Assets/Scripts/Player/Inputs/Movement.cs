using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private InputActionReference _movementInput;
    [SerializeField] private Rigidbody2D _playerRigidBody;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;
    [SerializeField] private List<Sprite> _idleSprites;
    [SerializeField] private float _speed;

    private Vector2 _direction;

    private void Awake()
    {
        if (_speed == 0)
            _speed = 5f;

        _movementInput.action.started += GetDirection;
    }

    private void OnDestroy()
    {
        _movementInput.action.started -= GetDirection;
    }

    private void FixedUpdate()
    {
        Move();
        AnimateMovement();
    }

    private void GetDirection(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        _playerRigidBody.velocity = _movementInput.action.ReadValue<Vector2>() * _speed;
    }

    private void AnimateMovement()
    {
        if (_movementInput.action.ReadValue<Vector2>() != Vector2.zero)
        {
            if (_movementInput.action.ReadValue<Vector2>().x < 0)
                _playerAnimator.Play("WalkLeft");
            else if (_movementInput.action.ReadValue<Vector2>().x > 0)
                _playerAnimator.Play("WalkRight");
            else if (_movementInput.action.ReadValue<Vector2>().y < 0)
                _playerAnimator.Play("WalkDown");
            else if (_movementInput.action.ReadValue<Vector2>().y > 0)
                _playerAnimator.Play("WalkUp");
        }
        else
        {
            if (_direction.x < 0)
                _playerAnimator.Play("IdleLeft");
            else if (_direction.x > 0)
                _playerAnimator.Play("IdleRight");
            else if (_direction.y < 0)
                _playerAnimator.Play("IdleDown");
            else if (_direction.y > 0)
                _playerAnimator.Play("IdleUp");
        }
    }
}
