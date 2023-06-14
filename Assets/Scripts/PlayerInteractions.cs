using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private LayerMask _collectableLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        Debug.Log("apple layer " + collision.gameObject.layer + " collectable layer " + _collectableLayer.value);
        if (((1 << collision.gameObject.layer) & _collectableLayer.value) != 0)
        {
            Destroy(collision.gameObject);
        }
    }
}
