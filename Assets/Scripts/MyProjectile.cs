using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Numerics;
// using UnityEngine.InputSystem;

public class MyProjectile : MonoBehaviour {
    Rigidbody2D rigidbody2d;

    // Awake is called when the GameObject is initialized
    void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Update() {
       if (transform.position.magnitude > 100.0f) {
           Destroy(gameObject);
       }
    }

    public void Launch(UnityEngine.Vector2 direction, float force) {
        rigidbody2d.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other) {
        // Debug.Log("Projectile collision with " + other.gameObject);
        MyEnemyController enemy = other.collider.GetComponent<MyEnemyController>();

        if (enemy != null) {
            enemy.Fix();
        }
        Destroy(gameObject);
    }
}
