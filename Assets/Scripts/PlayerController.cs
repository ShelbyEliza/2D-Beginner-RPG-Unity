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

    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();

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
    }

    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth (int amount, bool overTime = false)
    {
        //  Debug.Log("amount" + amount);
        if (amount < 0)
        {
            Debug.Log("isInvincible: " + isInvincible);
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
        }
        if (overTime) {
            if (isHealing) {
                return;
            }
            isHealing = true;
            healingCooldown = timeHealing; 
        }        

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        MyUIHandler.Instance.SetHealthValue(currentHealth / (float)maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
