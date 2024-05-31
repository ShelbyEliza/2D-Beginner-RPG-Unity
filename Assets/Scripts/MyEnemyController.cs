using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEnemyController : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rigidbody2d;

    public int damageVal = -1;

    // Related to movement 
    public float speed;
    public bool vertical;
    bool timeToTurnX = false;
    bool timeToTurnY = false;
    float xPathTimer;
    float yPathTimer;

    float pathStart = 0.0f;

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
        xPathTimer = pathStart;
        yPathTimer = pathStart;

        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();
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
        if (!aggressive) {
           return;
        }

        Vector2 position = rigidbody2d.position;
        if (timeToTurnX == true) {
            timeToTurnX = false;
        } 
        if (timeToTurnY == true) {
            timeToTurnY = false;
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
