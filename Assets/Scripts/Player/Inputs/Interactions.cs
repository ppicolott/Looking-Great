using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactions : MonoBehaviour
{
    [SerializeField] private InputActionReference _interactionKeyInput;
    [SerializeField] private GameObject _interactionKey;

    private bool _isDetectingStore;

    public static Action OnStoreCollision;

    private void Awake()
    {
        _isDetectingStore = false;
        _interactionKeyInput.action.started += _ => OpenStore();
    }

    private void OnDestroy()
    {
        _interactionKeyInput.action.started -= _ => OpenStore();
    }

    private void OpenStore()
    {
        if (_isDetectingStore)
        {
            OnStoreCollision?.Invoke();
            _interactionKey.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag.Equals("ClothesStore"))
        {
            _interactionKey.SetActive(true);
            _isDetectingStore = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collisio)
    {
        _interactionKey.SetActive(false);
        _isDetectingStore = false;
    }
}
