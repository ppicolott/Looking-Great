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
    [SerializeField] private Customization _customization;

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
            {
                _playerAnimator.Play("WalkLeft");
                for (int i = 0; i < _customization.ClothesAnimators.Length; i++)
                    _customization.ClothesAnimators[i].Play("WalkLeft");
            }
            else if (_movementInput.action.ReadValue<Vector2>().x > 0)
            {
                _playerAnimator.Play("WalkRight");
                for (int i = 0; i < _customization.ClothesAnimators.Length; i++)
                    _customization.ClothesAnimators[i].Play("WalkRight");
            }
            else if (_movementInput.action.ReadValue<Vector2>().y < 0)
            {
                _playerAnimator.Play("WalkDown");
                for (int i = 0; i < _customization.ClothesAnimators.Length; i++)
                    _customization.ClothesAnimators[i].Play("WalkDown");
            }
            else if (_movementInput.action.ReadValue<Vector2>().y > 0)
            {
                _playerAnimator.Play("WalkUp");
                for (int i = 0; i < _customization.ClothesAnimators.Length; i++)
                    _customization.ClothesAnimators[i].Play("WalkUp");
            }
        }
        else
        {
            if (_direction.x < 0)
            {
                _playerAnimator.Play("IdleLeft");
                for (int i = 0; i < _customization.ClothesAnimators.Length; i++)
                    _customization.ClothesAnimators[i].Play("IdleLeft");
            }
            else if (_direction.x > 0)
            {
                _playerAnimator.Play("IdleRight");
                for (int i = 0; i < _customization.ClothesAnimators.Length; i++)
                    _customization.ClothesAnimators[i].Play("IdleRight");
            }
            else if (_direction.y < 0)
            {
                _playerAnimator.Play("IdleDown");
                for (int i = 0; i < _customization.ClothesAnimators.Length; i++)
                    _customization.ClothesAnimators[i].Play("IdleDown");
            }
            else if (_direction.y > 0)
            {
                _playerAnimator.Play("IdleUp");
                for (int i = 0; i < _customization.ClothesAnimators.Length; i++)
                    _customization.ClothesAnimators[i].Play("IdleUp");
            }
        }
    }
}
