using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyController : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public float speed;
    public bool vertical;
    bool timeToTurnX = false;
    bool timeToTurnY = false;
    public int damageVal = -1;

    float xPathTimer;
    float yPathTimer;

    float pathStart = 0.0f;

    // Start is called before the first frame update
    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        if (speed == 0) {
            speed = 1.0f;
        }
        xPathTimer = pathStart;
        yPathTimer = pathStart;
    }
        // Update is called once per frame
    void Update() {
        if (xPathTimer > 3.0f) {
            timeToTurnX = true;
            vertical = true;
            if (yPathTimer > 3.0f) {
                timeToTurnY = true;
                vertical = false;
                speed = -speed;

                yPathTimer = pathStart;
                xPathTimer = pathStart;
            } else {
                yPathTimer += Time.deltaTime;
            }
        } else {
            xPathTimer += Time.deltaTime;
        }

    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate() {
        Vector2 position = rigidbody2d.position;
        if (timeToTurnX == true) {
            timeToTurnX = false;
        } 
        if (timeToTurnY == true) {
            timeToTurnY = false;
        }

        if (vertical == true) {
           position.y = position.y + speed * Time.deltaTime;
        } else {
           position.x = position.x + speed * Time.deltaTime;
        }

        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other) {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null) {
            Debug.Log(player);
           player.ChangeHealth(damageVal);
        }
   }
}
