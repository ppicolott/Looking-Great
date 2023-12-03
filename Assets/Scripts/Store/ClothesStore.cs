using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class ClothesStore : MonoBehaviour
{
    [Header("Clothes Data")]
    [SerializeField] private SaveLoadSystem _saveLoadSystem;
    private Clothes[] _clothesData;

    [Space(5)]
    [Header("Clothes Toggles")]
    [SerializeField] private Toggle[] _clothesToggles;
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
    [Header("Animations")]
    [SerializeField] private Animator[] _fittingRoomAnimator;
    [SerializeField] private AnimatorController[] _hatsAnimators;
    [SerializeField] private AnimatorController[] _hairsAnimators;
    [SerializeField] private AnimatorController[] _underwearAnimators;
    [SerializeField] private AnimatorController[] _outfitsAnimators;
    [SerializeField] private AnimatorController _nakedAnimator;

    [Space(5)]
    [Header("Cash and Price Tags")]
    [SerializeField] private TMP_Text _HUDCashText;
    [SerializeField] private TMP_Text _cashText;
    [SerializeField] private TMP_Text[] _priceTags;
    [SerializeField] private TMP_Text _cartTotalText;
#if UNITY_EDITOR
    [ReadOnly][SerializeField] private List<double> _cartAddedPrices;
#else
    private List<double> _cartAddedPrices;
#endif
    private double _cartValue;
    private double _cash;

    private AnimatorController[][] _animatorsReferences;
    private string _currentDirection;

    [Space(5)]
    [Header("Store Purchase Button")]
    [SerializeField] private Button _purchaseClothesButton;

    public Animator[] FittingRoomAnimator => _fittingRoomAnimator;

    public static Action OnCustomizePlayer;

    private void Start()
    {
        if (_cash == 0)
            _cash = 100d;

        if (PlayerPrefs.HasKey("Cash"))
            _cash = double.Parse(PlayerPrefs.GetString("Cash"));
        else
            PlayerPrefs.SetString("Cash", _cash.ToString());

        _clothesData = new Clothes[] { _saveLoadSystem.HatOne, _saveLoadSystem.HatTwo, _saveLoadSystem.FemaleHair , _saveLoadSystem.MaleHair ,
            _saveLoadSystem.FemaleUnderwear, _saveLoadSystem.MaleUnderwear , _saveLoadSystem.OutfitOne , _saveLoadSystem.OutfitTwo };

        _cartAddedPrices = new List<double>();
        _cartValue = 0;

        LoadPriceTags();
        CalculateShopCart();
        LoadEquippedClothes();
        

        for (int i = 0; i < _hatsToggles.Length; i++)
        {
            int j = i;
            _hatsToggles[i].onValueChanged.AddListener(delegate { SetPreviewAnimation(_hatsToggles[j].isOn, j, _animatorsReferences[0], 1); OnCustomizePlayer?.Invoke(); });
        }

        for (int i = 0; i < _hairsToggles.Length; i++)
        {
            int j = i;
            _hairsToggles[i].onValueChanged.AddListener(delegate { SetPreviewAnimation(_hairsToggles[j].isOn, j, _animatorsReferences[1], 2); OnCustomizePlayer?.Invoke(); });
        }

        for (int i = 0; i < _underwearsToggles.Length; i++)
        {
            int j = i;
            _underwearsToggles[i].onValueChanged.AddListener(delegate { SetPreviewAnimation(_underwearsToggles[j].isOn, j, _animatorsReferences[2], 3); OnCustomizePlayer?.Invoke(); });
        }

        for (int i = 0; i < _outfitsToggles.Length; i++)
        {
            int j = i;
            _outfitsToggles[i].onValueChanged.AddListener(delegate { SetPreviewAnimation(_outfitsToggles[j].isOn, j, _animatorsReferences[3], 4); OnCustomizePlayer?.Invoke(); });
        }

        _cashText.text = "$ " + string.Format("{0:0.00}", _cash);
        _HUDCashText.text = _cashText.text;

        _leftArrow.onClick.AddListener(() => SetPreviewAnimationDirection("IdleLeft"));
        _rightArrow.onClick.AddListener(() => SetPreviewAnimationDirection("IdleRight"));
        _downArrow.onClick.AddListener(() => SetPreviewAnimationDirection("IdleDown"));
        _upArrow.onClick.AddListener(() => SetPreviewAnimationDirection("IdleUp"));

        _animatorsReferences = new AnimatorController[][] { _hatsAnimators, _hairsAnimators, _underwearAnimators, _outfitsAnimators };
        _currentDirection = "IdleDown";

        _purchaseClothesButton.onClick.AddListener(PurchaseClothes);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _hatsToggles.Length; i++)
            _hatsToggles[i].onValueChanged.RemoveAllListeners();

        for (int i = 0; i < _hatsToggles.Length; i++)
            _hairsToggles[i].onValueChanged.RemoveAllListeners();

        for (int i = 0; i < _hatsToggles.Length; i++)
            _underwearsToggles[i].onValueChanged.RemoveAllListeners();

        for (int i = 0; i < _hatsToggles.Length; i++)
            _outfitsToggles[i].onValueChanged.RemoveAllListeners();

        _leftArrow.onClick.RemoveAllListeners();
        _rightArrow.onClick.RemoveAllListeners();
        _downArrow.onClick.RemoveAllListeners();
        _upArrow.onClick.RemoveAllListeners();

        _purchaseClothesButton.onClick.RemoveAllListeners();
    }

    private void SetPreviewAnimation(bool toggleOn, int animatorIndex, AnimatorController[] animatorReference, int currentAnimatorIndex)
    {
        // Set animations on preview
        if (!toggleOn)
            _fittingRoomAnimator[currentAnimatorIndex].runtimeAnimatorController = _nakedAnimator;
        else
            _fittingRoomAnimator[currentAnimatorIndex].runtimeAnimatorController = animatorReference[animatorIndex];

        // Updates clothes equipped data
        for (int i = 0; i < _clothesData.Length; i++)
        {
            bool equiped = _clothesToggles[i].isOn && _clothesData[i].Purchased;
            _saveLoadSystem.UpdateData(_clothesData[i].FilePath, _clothesData[i], _clothesData[i].Purchased, equiped);
        }

        OnCustomizePlayer?.Invoke();
        SetPreviewAnimationDirection(_currentDirection);
        CalculateShopCart();
    }

    private void SetPreviewAnimationDirection(string animationName)
    {
        for (int i = 0; i < _fittingRoomAnimator.Length; i++)
            _fittingRoomAnimator[i].Play(animationName);

        _currentDirection = animationName;
    }

    private void LoadEquippedClothes()
    {
        for (int i = 0; i < _clothesToggles.Length; i++)
        {
            if (_clothesData[i].Equipped)
                _clothesToggles[i].isOn = true;
            else
                _clothesToggles[i].isOn = false;
        }
    }

    private void LoadPriceTags()
    {
        for (int i = 0; i < _clothesData.Length; i++)
        {
            if (!_clothesData[i].Purchased)
                _priceTags[i].text = "$ " + string.Format("{0:0.00}", _clothesData[i].Price);
            else
                _priceTags[i].text = "Purchased";
        }
    }

    private void CalculateShopCart()
    {
        _cartAddedPrices.Clear();
        _cartValue = 0;

        for (int i = 0; i < _clothesToggles.Length; i++)
            if (_clothesToggles[i].isOn && !_clothesData[i].Purchased)
                _cartAddedPrices.Add(_clothesData[i].Price);

        foreach (double price in _cartAddedPrices)
            _cartValue += price;

        _cartTotalText.text = "Cart: $ " + string.Format("{0:0.00}", _cartValue);
    }

    private void PurchaseClothes()
    {
        if (_cash < _cartValue)
            return;

        _cash -= _cartValue;
        _cashText.text = "$ " + string.Format("{0:0.00}", _cash);
        _HUDCashText.text = _cashText.text;

        PlayerPrefs.SetString("Cash", _cash.ToString());

        _cartAddedPrices.Clear();
        _cartValue = 0;

        for (int i = 0; i < _clothesToggles.Length; i++)
        {
            if (_clothesToggles[i].isOn && !_clothesData[i].Purchased)
            {
                _clothesData[i].Purchased = true;
                _saveLoadSystem.UpdateData(_clothesData[i].FilePath, _clothesData[i], true, true);
            }
        }

        LoadPriceTags();
        CalculateShopCart();
        OnCustomizePlayer?.Invoke();
    }
}
