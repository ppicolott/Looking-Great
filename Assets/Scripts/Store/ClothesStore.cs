using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClothesStore : MonoBehaviour
{
    [Header("Clothes Data")]
    [SerializeField] private SaveLoadSystem _saveLoadSystem;
    private Clothes[] _clothesData;

    [Header("Buy/Sell Tabs")]
    [SerializeField] private Button _buyTabButton;
    [SerializeField] private Button _sellTabButton;
    [SerializeField] private Button _equipTabButton;
    private Tabs _currentTab;

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
    [SerializeField] private RuntimeAnimatorController[] _hatsAnimators;
    [SerializeField] private RuntimeAnimatorController[] _hairsAnimators;
    [SerializeField] private RuntimeAnimatorController[] _underwearAnimators;
    [SerializeField] private RuntimeAnimatorController[] _outfitsAnimators;
    [SerializeField] private RuntimeAnimatorController _nakedAnimator;

    [Space(5)]
    [Header("Cash and Price Tags")]
    [SerializeField] private Button _HUDInvetory;
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

    private RuntimeAnimatorController[][] _animatorsReferences;
    private string _currentDirection;

    [Space(5)]
    [Header("Store Purchase Button")]
    [SerializeField] private Button _purchaseClothesButton;
    [SerializeField] private Button _sellClothesButton;
    [SerializeField] private Button _equipClothesButton;
    [SerializeField] private Button _exitClothesButton;

    public Animator[] FittingRoomAnimator => _fittingRoomAnimator;

    public static Action OnInventory;
    public static Action OnEquipped;

    private enum Tabs
    {
        BuyTab = 0,
        SellTab = 1,
        EquipTab = 2
    }

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
            _hatsToggles[i].onValueChanged.AddListener(delegate { SetPreviewAnimation(_hatsToggles[j].isOn, j, _animatorsReferences[0], 1); });
        }

        for (int i = 0; i < _hairsToggles.Length; i++)
        {
            int j = i;
            _hairsToggles[i].onValueChanged.AddListener(delegate { SetPreviewAnimation(_hairsToggles[j].isOn, j, _animatorsReferences[1], 2); });
        }

        for (int i = 0; i < _underwearsToggles.Length; i++)
        {
            int j = i;
            _underwearsToggles[i].onValueChanged.AddListener(delegate { SetPreviewAnimation(_underwearsToggles[j].isOn, j, _animatorsReferences[2], 3); });
        }

        for (int i = 0; i < _outfitsToggles.Length; i++)
        {
            int j = i;
            _outfitsToggles[i].onValueChanged.AddListener(delegate { SetPreviewAnimation(_outfitsToggles[j].isOn, j, _animatorsReferences[3], 4); });
        }

        _cashText.text = "$ " + string.Format("{0:0.00}", _cash);
        _HUDCashText.text = _cashText.text;

        _leftArrow.onClick.AddListener(() => SetPreviewAnimationDirection("IdleLeft"));
        _rightArrow.onClick.AddListener(() => SetPreviewAnimationDirection("IdleRight"));
        _downArrow.onClick.AddListener(() => SetPreviewAnimationDirection("IdleDown"));
        _upArrow.onClick.AddListener(() => SetPreviewAnimationDirection("IdleUp"));

        _animatorsReferences = new RuntimeAnimatorController[][] { _hatsAnimators, _hairsAnimators, _underwearAnimators, _outfitsAnimators };
        _currentDirection = "IdleDown";

        _buyTabButton.onClick.AddListener(() => ChangeTab((int)Tabs.BuyTab));
        _sellTabButton.onClick.AddListener(() => ChangeTab((int)Tabs.SellTab));
        _equipTabButton.onClick.AddListener(() => ChangeTab((int)Tabs.EquipTab));

        _purchaseClothesButton.onClick.AddListener(PurchaseClothes);
        _sellClothesButton.onClick.AddListener(SellClothes);
        _equipClothesButton.onClick.AddListener(EquipClothes);
        _exitClothesButton.onClick.AddListener( delegate {
            ChangeTab((int)Tabs.BuyTab);
            _buyTabButton.gameObject.SetActive(true);
            _sellTabButton.gameObject.SetActive(true);
            _equipTabButton.gameObject.SetActive(true);
        } );

        _HUDInvetory.onClick.AddListener( delegate {
            ChangeTab((int)Tabs.EquipTab);
            OnInventory?.Invoke();
            _buyTabButton.gameObject.SetActive(false);
            _sellTabButton.gameObject.SetActive(false);
            _equipTabButton.gameObject.SetActive(false);

        });

        ChangeTab((int)Tabs.BuyTab);
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

        _buyTabButton.onClick.RemoveAllListeners();
        _sellTabButton.onClick.RemoveAllListeners();
        _purchaseClothesButton.onClick.RemoveAllListeners();
        _sellClothesButton.onClick.RemoveAllListeners();
        _equipClothesButton.onClick.RemoveAllListeners();
        _exitClothesButton.onClick.RemoveAllListeners();

        _HUDInvetory.onClick.RemoveAllListeners();
    }

    private void SetPreviewAnimation(bool toggleOn, int animatorIndex, RuntimeAnimatorController[] animatorReference, int currentAnimatorIndex)
    {
        if (!toggleOn)
            _fittingRoomAnimator[currentAnimatorIndex].runtimeAnimatorController = _nakedAnimator;
        else
            _fittingRoomAnimator[currentAnimatorIndex].runtimeAnimatorController = animatorReference[animatorIndex];

        for (int i = 0; i < _clothesData.Length; i++)
        {
            bool equiped = _clothesToggles[i].isOn && _clothesData[i].Purchased;
            _saveLoadSystem.UpdateData(_clothesData[i].FilePath, _clothesData[i], _clothesData[i].Purchased, equiped);
        }

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

        if (_currentTab == 0)
        {
            for (int i = 0; i < _clothesToggles.Length; i++)
                if (_clothesToggles[i].isOn && !_clothesData[i].Purchased)
                    _cartAddedPrices.Add(_clothesData[i].Price);
        }
        else
        {
            for (int i = 0; i < _clothesToggles.Length; i++)
                if (_clothesToggles[i].isOn && _clothesData[i].Purchased)
                    _cartAddedPrices.Add(_clothesData[i].Price);
        }

        foreach (double price in _cartAddedPrices)
            _cartValue += price;

        if (_currentTab == Tabs.BuyTab)
            _cartTotalText.text = "Cart: -$ " + string.Format("{0:0.00}", _cartValue);
        else
            _cartTotalText.text = "Cart: +$ " + string.Format("{0:0.00}", _cartValue);
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
    }

    private void SellClothes()
    {
        for (int i = 0; i < _clothesToggles.Length; i++)
        {
            if (_clothesToggles[i].isOn && _clothesData[i].Purchased)
            {
                _clothesToggles[i].isOn = false;
                _clothesData[i].Purchased = false;
                _saveLoadSystem.UpdateData(_clothesData[i].FilePath, _clothesData[i], false, false);

                _cash += _clothesData[i].Price;
                _cashText.text = "$ " + string.Format("{0:0.00}", _cash);
                _HUDCashText.text = _cashText.text;
                PlayerPrefs.SetString("Cash", _cash.ToString());

                LoadPriceTags();
                CalculateShopCart();
            }
        }

        for (int i = 0; i < _clothesToggles.Length; i++)
            if (!_clothesData[i].Purchased)
                _clothesToggles[i].interactable = false;

        OnEquipped?.Invoke();
    }

    private void EquipClothes()
    {
        OnEquipped?.Invoke();
    }

    private void ChangeTab(int tabIndex)
    {
        if (tabIndex == 0)
        {
            _currentTab = Tabs.BuyTab;

            _buyTabButton.interactable = false;
            _sellTabButton.interactable = true;
            _equipTabButton.interactable = true;

            _purchaseClothesButton.gameObject.SetActive(true);
            _sellClothesButton.gameObject.SetActive(false);
            _equipClothesButton.gameObject.SetActive(false);

            _cashText.gameObject.SetActive(true);
            _cartTotalText.gameObject.SetActive(true);

            for (int i = 0; i < _clothesToggles.Length; i++)
            {
                if (!_clothesData[i].Purchased)
                    _clothesToggles[i].interactable = true;
            }
        }
        else if (tabIndex == 1)
        {
            _currentTab = Tabs.SellTab;

            _buyTabButton.interactable = true;
            _sellTabButton.interactable = false;
            _equipTabButton.interactable = true;

            _purchaseClothesButton.gameObject.SetActive(false);
            _sellClothesButton.gameObject.SetActive(true);
            _equipClothesButton.gameObject.SetActive(false);

            _cashText.gameObject.SetActive(true);
            _cartTotalText.gameObject.SetActive(true);

            for (int i = 0; i < _clothesToggles.Length; i++)
            {
                if (!_clothesData[i].Purchased)
                    _clothesToggles[i].interactable = false;
            }
        }
        else if (tabIndex == 2)
        {
            _currentTab = Tabs.EquipTab;

            _buyTabButton.interactable = true;
            _sellTabButton.interactable = true;
            _equipTabButton.interactable = false;

            _purchaseClothesButton.gameObject.SetActive(false);
            _sellClothesButton.gameObject.SetActive(false);
            _equipClothesButton.gameObject.SetActive(true);

            _cashText.gameObject.SetActive(false);
            _cartTotalText.gameObject.SetActive(false);

            for (int i = 0; i < _clothesToggles.Length; i++)
            {
                if (!_clothesData[i].Purchased)
                    _clothesToggles[i].interactable = false;
            }
        }

        CalculateShopCart();
        LoadEquippedClothes();
    }
}
