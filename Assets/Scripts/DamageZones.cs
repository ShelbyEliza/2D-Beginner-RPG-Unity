using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZones : MonoBehaviour
{
    public int DamageVal = -1;

    void OnTriggerStay2D(Collider2D other) 
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null && controller.health != 0)
        {

            controller.ChangeHealth(DamageVal);
        }
    }
}
