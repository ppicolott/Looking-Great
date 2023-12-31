using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIStates : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] private PlayerInput _playerInput;

    [Space(10)]
    [Header("Screens")]
    [SerializeField] private GameObject[] _screens;
    [SerializeField] private Button _quitGameButton;

    [Space(10)]
    [Header("Next State Buttons")]
    [SerializeField] private Button[] _nextStateButtons;

    [Space(10)]
    [Header("Store Canvas")]
    [SerializeField] private GameObject _storeCanvas;
    [SerializeField] private Button _exitStoreButton;

    [Space(10)]
    [Header("HUD")]
    [SerializeField] private GameObject _HUDInvetory;
    [SerializeField] private GameObject _HUDCashText;
    [SerializeField] private GameObject _backToMainMenu;

    [Space(10)]
    [Header("UI Animation")]
    [SerializeField] private float _transitionXEndPos;

    private int _gameStateMaxIndex;
    private int _gameStateIndex;

    [Space(10)]
    [Header("Debug")]
#if UNITY_EDITOR
    [ReadOnly] public GameStates CurrentState;
#else
    public GameStates CurrentState;
#endif

    public enum GameStates
    {
        MainMenuScreen = 0,
        GameplayScreen = 1,
        FakeMainMenuScreen = 2
    }

    private void Awake()
    {
        if (_transitionXEndPos == 0)
            _transitionXEndPos = -2000f;

        for (int i = 0; i < _nextStateButtons.Length; i++)
            _nextStateButtons[i].onClick.AddListener(NextState);

        _gameStateMaxIndex = Enum.GetValues(typeof(GameStates)).Length - 1;
        _gameStateIndex = 0;
        CurrentState = GameStates.MainMenuScreen;
        StartCoroutine(SetState());

        _quitGameButton.onClick.AddListener(() => Application.Quit());
        _exitStoreButton.onClick.AddListener(CloseStore);

        Interactions.OnStoreCollision += OpeStore;
        ClothesStore.OnInventory += OpeStore;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        for (int i = 0; i < _nextStateButtons.Length; i++)
            _nextStateButtons[i].onClick.RemoveAllListeners();

        _quitGameButton.onClick.RemoveAllListeners();
        _exitStoreButton.onClick.RemoveAllListeners();

        Interactions.OnStoreCollision -= OpeStore;
        ClothesStore.OnInventory -= OpeStore;
    }

    private void NextState()
    {
        if (_gameStateIndex < _gameStateMaxIndex)
            _gameStateIndex++;
        else
            _gameStateIndex = 0;

        CurrentState = (GameStates)_gameStateIndex;
        StartCoroutine(SetState());
    }

    private IEnumerator SetState()
    {
        switch (CurrentState)
        {
            case GameStates.MainMenuScreen:
                _playerInput.actions.Disable();
                _storeCanvas.SetActive(false);
                SetScreens(GameStates.MainMenuScreen);
                break;

            case GameStates.GameplayScreen:
                _playerInput.actions.Enable();
                StartCoroutine(AnimateScreens(_screens[_gameStateIndex - 1], _screens[_gameStateIndex]));
                break;

            case GameStates.FakeMainMenuScreen:
                _playerInput.actions.Disable();
                StartCoroutine(AnimateScreens(_screens[_gameStateIndex - 1], _screens[_gameStateIndex]));
                yield return new WaitForSeconds(1f);
                NextState();
                break;
        }
        yield break;
    }

    private void SetScreens(GameStates _currentState)
    {
        for (int i = 0; i < _screens.Length; i++)
            _screens[i].SetActive(false);
        _screens[(int)_currentState].SetActive(true);
    }

    private IEnumerator AnimateScreens(GameObject previousScreen, GameObject currentScreen)
    {
        currentScreen.SetActive(true);

        previousScreen.transform.DOLocalMoveX(_transitionXEndPos, 1f);
        yield return new WaitForSeconds(1.15f);

        previousScreen.SetActive(false);
        previousScreen.transform.localPosition = Vector3.zero;

        yield break;
    }

    private void OpeStore()
    {
        _playerInput.actions.Disable();
        _storeCanvas.SetActive(true);
        _HUDInvetory.gameObject.SetActive(false);
        _HUDCashText.SetActive(false);
        _backToMainMenu.SetActive(false);
    }

    private void CloseStore()
    {
        _storeCanvas.SetActive(false);
        _playerInput.actions.Enable();
        _HUDInvetory.gameObject.SetActive(true);
        _HUDCashText.SetActive(true);
        _backToMainMenu.SetActive(true);
    }
}
