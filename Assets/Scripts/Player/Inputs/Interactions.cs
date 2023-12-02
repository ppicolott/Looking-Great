using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag.Equals("ClothesStore"))
        {
            Debug.Log("!");
        }
    }
}
