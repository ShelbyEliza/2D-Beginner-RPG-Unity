using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;

    // Variable related to player animation
    Animator animator;
    Vector2 moveDirection = new Vector2(1,0);

    // Variables related to temporary invincibility
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float damageCooldown;

    // Variables related to temporary healing
    public float timeHealing = 4.0f;
    public bool isHealing = false;
    float healingCooldown;

    // Variables related to the health system
    public int maxHealth = 5;
    public int health { get { return currentHealth; }}
    int currentHealth;

    // Variables related to projectile
    public GameObject projectilePrefab;
    public InputAction launchAction;

    // Variables related to NPC
    public InputAction talkAction;

    // Variables related to audio
    AudioSource audioSource;
    public AudioClip damagedClip;
    public AudioClip shootClip;

    // VFX Effects
    public ParticleSystem dizzyEffect;
    public ParticleSystem healthEffect;

    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

        talkAction.Enable();
        talkAction.performed += FindFriend;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();

            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else {
            if (audioSource.isPlaying) {
                audioSource.Pause();
            }
        }

        if (healthEffect.IsAlive()) {
            healthEffect.transform.position = rigidbody2d.transform.position;
        }

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;
            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }
        if (isHealing)
        {
            healingCooldown -= Time.deltaTime;
            if (healingCooldown < 0)
            {
                isHealing = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Space)) {
           Launch();
        }
    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate() {
        Vector2 position = (Vector2)rigidbody2d.position + speed * Time.deltaTime * move;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth (int amount, bool overTime = false) {
        if (amount < 0) {
                Debug.Log(dizzyEffect);              

            if (isInvincible) {
                return;
            }
            if (!dizzyEffect.IsAlive()) {
                ParticleSystem dizzyInstance = Instantiate(dizzyEffect, rigidbody2d.transform.position, Quaternion.identity);
                dizzyInstance.Play();
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
            
            animator.SetTrigger("Hit");
            PlaySound(damagedClip);
        }
        if (overTime) {
            if (isHealing) {
                return;
            }
            isHealing = true;
            healingCooldown = timeHealing; 
        }        

        if (!healthEffect.IsAlive() && amount > 0) {
            ParticleSystem healthBurstInstance = Instantiate(healthEffect, rigidbody2d.transform.position, Quaternion.identity);
            healthBurstInstance.Play();
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        MyUIHandler.Instance.SetHealthValue(currentHealth / (float)maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    void Launch() {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        MyProjectile projectile = projectileObject.GetComponent<MyProjectile>();

        projectile.Launch(moveDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(shootClip);
    }

    void FindFriend(InputAction.CallbackContext context) {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));

        if (hit.collider != null) {
            MyNonPlayerCharacter character = hit.collider.GetComponent<MyNonPlayerCharacter>();
            
            if (character != null) {
                MyUIHandler.Instance.DisplayDialogue(character.dialogOption);
            }
        }
    }

    public void PlaySound(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }
}
