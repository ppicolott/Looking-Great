using System.Collections.Generic;
using System.IO;
using UnityEditor.Animations;
using UnityEngine;

public class Customization : MonoBehaviour
{
    [SerializeField] private ClothesStore _clothesStore;
    [SerializeField] private Animator[] _clothesAnimators;
    [SerializeField] private List<RuntimeAnimatorController> _animatorControllers;
    [SerializeField] private AnimatorController[] _hatsAnimators;
    [SerializeField] private AnimatorController[] _hairsAnimators;
    [SerializeField] private AnimatorController[] _underwearAnimators;
    [SerializeField] private AnimatorController[] _outfitsAnimators;
    private string _customizationFilePath;
    private Dictionary<string, RuntimeAnimatorController> _currentVisuals;
    private List<string> _currentAnimatorControllers;

    public Animator[] ClothesAnimators => _clothesAnimators;

    private void Awake()
    {
        _currentAnimatorControllers = new List<string>();
        _currentVisuals = new Dictionary<string, RuntimeAnimatorController>();

        for (int i = 0; i < _animatorControllers.Count; i++)
            _currentVisuals.Add(_animatorControllers[i].name, _animatorControllers[i]);

        Directory.CreateDirectory(Application.persistentDataPath + "/Player");
        _customizationFilePath = Application.persistentDataPath + "/Player/CustomizationData.txt";

        if (!File.Exists(_customizationFilePath))
            File.WriteAllText(_customizationFilePath, "");

        LoadCurrentClothes();

        ClothesStore.OnCustomizePlayer += CustomizePlayerVisuals;
    }

    private void OnDestroy()
    {
        ClothesStore.OnCustomizePlayer -= CustomizePlayerVisuals;
    }

    private void CustomizePlayerVisuals()
    {
        _currentAnimatorControllers.Clear();

        for (int i = 0; i < _clothesStore.FittingRoomAnimator.Length - 1; i++)
        {
            int j = i + 1;
            _clothesAnimators[i].runtimeAnimatorController = _clothesStore.FittingRoomAnimator[j].runtimeAnimatorController;
            _currentAnimatorControllers.Add(_clothesAnimators[i].runtimeAnimatorController.name);
        }

        SaveCurrentClothes();
    }

    private void SaveCurrentClothes()
    {
        File.WriteAllText(_customizationFilePath, "");

        for (int i = 0; i < _currentAnimatorControllers.Count; i++)
            File.AppendAllText(_customizationFilePath, _currentAnimatorControllers[i] + "\n");

    }

    private void LoadCurrentClothes()
    {
        string[] lines = File.ReadAllLines(_customizationFilePath);

        for (int i = 0; i < lines.Length; i++)
        {
            _clothesAnimators[i].runtimeAnimatorController = _currentVisuals[lines[i]];
            _clothesAnimators[i].Play("IdleDown");
            _clothesStore.FittingRoomAnimator[i + 1].runtimeAnimatorController = _clothesAnimators[i].runtimeAnimatorController;
            _clothesStore.FittingRoomAnimator[i + 1].Play("IdleDown");
        }
    }
}
