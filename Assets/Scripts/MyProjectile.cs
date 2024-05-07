using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyProjectile : MonoBehaviour {
    Rigidbody2D rigidbody2d;
    float startTimer = 2.0f; 
    bool isExisting = false;
    float inExistenceTimer;

    // Awake is called when the GameObject is initialized
    void Awake() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        isExisting = true;
        inExistenceTimer = startTimer;
    }

    void Update() {
        if (isExisting) {
            inExistenceTimer -= Time.deltaTime;
            if (inExistenceTimer < 0) {
                isExisting = false;
                Destroy(gameObject);
            }
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
