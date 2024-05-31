using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRandomEnemyController : MonoBehaviour {
    Animator animator;
    Rigidbody2D rigidbody2d;
    public float speed;
    public bool vertical;
    public float timer;
    bool timeToTurn = false;
    public int damageVal = -1;
    float pathStart = 0.0f;
    float randomTime;

    // Related to taking damage from player
    bool aggressive = true;

    // Related to audio
    AudioSource audioSource;

    public ParticleSystem smokeEffect;

    // Start is called before the first frame update
    void Start() {
        rigidbody2d = GetComponent<Rigidbody2D>();
        if (speed == 0) {
            speed = 1.0f;
        }
        timer = pathStart;
        randomTime = Random.Range(1, 3);
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
    }
        // Update is called once per frame
    void Update() {
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
        if (!aggressive) {
           return;
        }
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
           player.ChangeHealth(damageVal);
        }
   }

      public void Fix() {
        aggressive = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        audioSource.Stop();

        smokeEffect.Stop();
   }
}
