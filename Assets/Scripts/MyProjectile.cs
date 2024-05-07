using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        MyEnemyController enemy = other.collider.GetComponent<MyEnemyController>();
        MyRandomEnemyController randomEnemy = other.collider.GetComponent<MyRandomEnemyController>();


        if (enemy != null) {
            enemy.Fix();
        }

        if (randomEnemy != null) {
            randomEnemy.Fix();
        }
        Destroy(gameObject);
    }
}
