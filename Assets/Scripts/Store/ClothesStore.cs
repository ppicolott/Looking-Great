using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class ClothesStore : MonoBehaviour
{
    [Header("Clothes Toggles")]
    [SerializeField] private Toggle[] _hatsToggles;
    [SerializeField] private Toggle[] _hairsToggles;
    [SerializeField] private Toggle[] _underwearsToggles;
    [SerializeField] private Toggle[] _outfitsToggles;

    [Space(5)]
    [Header("Arrow Buttons")]
    [SerializeField] private Button _leftArrow;
    [SerializeField] private Button _rightArrow;
    [SerializeField] private Button _downArrow;
    [SerializeField] private Button _upArrow;

    [Space(5)]
    [Header("Animation")]
    [SerializeField] private Animator[] _fittingRoomAnimator;
    [SerializeField] private AnimatorController[] _hatsAnimators;
    [SerializeField] private AnimatorController[] _hairsAnimators;
    [SerializeField] private AnimatorController[] _underwearAnimators;
    [SerializeField] private AnimatorController[] _outfitsAnimators;


    public Animator[] FittingRoomAnimator => _fittingRoomAnimator;

    private void Awake()
    {
        _leftArrow.onClick.AddListener(() => SetPreviewAnimation("IdleLeft"));
        _rightArrow.onClick.AddListener(() => SetPreviewAnimation("IdleRight"));
        _downArrow.onClick.AddListener(() => SetPreviewAnimation("IdleDown"));
        _upArrow.onClick.AddListener(() => SetPreviewAnimation("IdleUp"));
    }

    private void OnDestroy()
    {
        _leftArrow.onClick.RemoveAllListeners();
        _rightArrow.onClick.RemoveAllListeners();
        _downArrow.onClick.RemoveAllListeners();
        _upArrow.onClick.RemoveAllListeners();
    }

    private void Update()
    {
        Debug.Log(_hatsToggles[0].isOn);
    }

    private void SetPreviewAnimation(string animationName)
    {
        for (int i = 0; i < _fittingRoomAnimator.Length; i++)
            _fittingRoomAnimator[i].Play(animationName);
    }
}
