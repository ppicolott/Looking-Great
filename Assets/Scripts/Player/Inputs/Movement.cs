using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
            _playerAnimator.SetInteger("WalkHorizontal", -1);

        if (Keyboard.current.dKey.wasPressedThisFrame)
            _playerAnimator.SetInteger("WalkHorizontal", 1);

        if (Keyboard.current.wKey.wasPressedThisFrame)
            _playerAnimator.SetInteger("WalkVertical", 1);

        if (Keyboard.current.sKey.wasPressedThisFrame)
            _playerAnimator.SetInteger("WalkVertical", -1);

    }
}
