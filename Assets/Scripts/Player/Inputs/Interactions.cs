using System;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public static Action OnStoreCollision;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO
        // Temporary:
        if (collision.transform.tag.Equals("ClothesStore"))
            OnStoreCollision?.Invoke();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // TODO
    }
}
