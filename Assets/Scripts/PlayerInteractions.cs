using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private LayerMask _collectableLayer;
    [SerializeField] private LayerMask _trapLayer;
    [SerializeField] private LayerMask _checkpointLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & _collectableLayer.value) != 0)
        {
            GameManager.Instance.OnPlayerCollectedAnItem();
            Destroy(collision.gameObject);
        }
        else if (((1 << collision.gameObject.layer) & _trapLayer.value) != 0)
        {
            GameManager.Instance.GameOver(false);
        }
        else if (((1 << collision.gameObject.layer) & _checkpointLayer.value) != 0)
        {
            GameManager.Instance.GameOver(true);
        }
    }
}
