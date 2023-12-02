using UnityEditor.Animations;
using UnityEngine;

public class Customization : MonoBehaviour
{
    [SerializeField] private ClothesStore _clothesStore;
    [SerializeField] private Animator[] _clothesAnimators;
    [SerializeField] private AnimatorController[] _hatsAnimators;
    [SerializeField] private AnimatorController[] _hairsAnimators;
    [SerializeField] private AnimatorController[] _underwearAnimators;
    [SerializeField] private AnimatorController[] _outfitsAnimators;

    public Animator[] ClothesAnimators => _clothesAnimators;

    private void Awake()
    {
        ClothesStore.OnCustomizePlayer += CustomizePlayerVisuals;
    }

    private void OnDestroy()
    {
        ClothesStore.OnCustomizePlayer -= CustomizePlayerVisuals;
    }

    private void CustomizePlayerVisuals()
    {
        for (int i = 0; i < _clothesStore.FittingRoomAnimator.Length - 1; i++)
        {
            int j = i + 1;
            _clothesAnimators[i].runtimeAnimatorController = _clothesStore.FittingRoomAnimator[j].runtimeAnimatorController;
        }
    }
}
