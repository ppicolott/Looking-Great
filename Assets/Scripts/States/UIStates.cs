using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIStates : MonoBehaviour
{
    [Header("Screens")]
    [SerializeField] private GameObject[] _screens;

    [Space(10)]
    [Header("Buttons")]
    [SerializeField] private Button[] _buttons;

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
    public States CurrentState;
#endif

    public enum GameStates
    {
        MainMenuScreen = 0,
        GameplayScreen = 1,
        VictoryScreen = 2,
        GameOverScreen = 3,
        FakeMainMenuScreen = 4
    }

    private void Awake()
    {
        if (_transitionXEndPos == 0)
            _transitionXEndPos = -2000f;

        for (int i = 0; i < _buttons.Length; i++)
            _buttons[i].onClick.AddListener(NextState);

        _gameStateMaxIndex = Enum.GetValues(typeof(GameStates)).Length - 1;
        _gameStateIndex = 0;
        CurrentState = GameStates.MainMenuScreen;
        StartCoroutine(SetState());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        for (int i = 0; i < _buttons.Length; i++)
            _buttons[i].onClick.RemoveAllListeners();
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
                SetScreens(GameStates.MainMenuScreen);
                break;

            case GameStates.GameplayScreen:
                StartCoroutine(AnimateScreens(_screens[_gameStateIndex - 1], _screens[_gameStateIndex]));
                break;

            case GameStates.VictoryScreen:
                StartCoroutine(AnimateScreens(_screens[_gameStateIndex - 1], _screens[_gameStateIndex]));
                break;

            case GameStates.GameOverScreen:
                StartCoroutine(AnimateScreens(_screens[_gameStateIndex - 1], _screens[_gameStateIndex]));
                break;

            case GameStates.FakeMainMenuScreen:
                StartCoroutine(AnimateScreens(_screens[_gameStateIndex - 1], _screens[_gameStateIndex]));
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
}
