using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRandomEnemyController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2d;
    public float speed;
    public bool vertical;
    public float timer;
    bool timeToTurn = false;
    public int damageVal = -1;
    float pathStart = 0.0f;
    float randomTime;

    // Start is called before the first frame update
    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        if (speed == 0) {
            speed = 1.0f;
        }
        timer = pathStart;
        randomTime = Random.Range(1, 3);
        animator = GetComponent<Animator>();
    }
        // Update is called once per frame
    void Update() {
        Debug.Log(timer + "," + randomTime);
        if (timer > randomTime) {
            timeToTurn = true;
            if (randomTime%2 == 0) {
              vertical = true;
            } else {
              vertical = false;
            }
        } else {
            timer += Time.deltaTime;
        }
    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate() {
        Vector2 position = rigidbody2d.position;
        if (timeToTurn == true) {
            randomTime = Random.Range(1, 3);
            timeToTurn = false;
            timer = 0;
        } 

        if (vertical == true) {
            position.y = position.y + speed * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", speed);
        } else {
            position.x = position.x + speed * Time.deltaTime;
            animator.SetFloat("Move X", speed);
            animator.SetFloat("Move Y", 0);
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
