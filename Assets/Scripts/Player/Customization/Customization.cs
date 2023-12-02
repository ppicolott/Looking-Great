using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

public class Customization : MonoBehaviour
{
    [SerializeField] private Animator[] _clothesAnimators;

    [SerializeField] private AnimatorController[] _hatsAnimators;
    [SerializeField] private AnimatorController[] _hairsAnimators;
    [SerializeField] private AnimatorController[] _underwearAnimators;
    [SerializeField] private AnimatorController[] _outfitsAnimators;

    public Animator[] ClothesAnimators => _clothesAnimators;

    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
            ChangeOutfit();
    }

    private void ChangeOutfit()
    {
        _clothesAnimators[5].runtimeAnimatorController = _outfitsAnimators[0];
    }
}
