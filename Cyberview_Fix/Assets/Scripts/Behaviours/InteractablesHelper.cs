using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesHelper : MonoBehaviour
{
    private PlayerManager owner;

    private void Start()
    {
        owner = GameObject.Find("_Player").GetComponent<PlayerManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        owner.AddInteractable(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        owner.RemoveInteractable(other.gameObject);
    }
}
