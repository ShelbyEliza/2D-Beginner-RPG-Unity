using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleHealth : MonoBehaviour {
    public int HealthCollectibleVal = 2;
    public AudioClip collectedClip;

    void OnTriggerEnter2D(Collider2D other) {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null && controller.health < controller.maxHealth) {
            controller.PlaySound(collectedClip);
            controller.ChangeHealth(HealthCollectibleVal);
            Destroy(gameObject);
        }
    }
}
