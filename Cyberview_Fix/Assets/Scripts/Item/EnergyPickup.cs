using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : AbstractLvlItem
{
    public int energyValue;
    //(for some reason there's a chance the code will execute twice before destroying, that's why there's the bool)
    bool collected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "_Player" && !collected)
        {
            collision.gameObject.GetComponent<PlayerManager>().Recharge(energyValue);

            //save state
            PlayerPrefs.SetInt(objectID, 1);

            collected = true;
            Destroy(gameObject);
        }
    }
}
