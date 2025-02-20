using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float flapStrength;
    public LogicScript logic;
    public bool birdIsAlive = true;

    public SpriteRenderer WingUp;
    public SpriteRenderer WingDown;

    public AudioSource jumpSound;
    public AudioSource deathSound;

    private float timer = 0;
    private bool hasDied = false; // Track if the bird has died

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
            {
                timer = 0;
                myRigidBody.linearVelocity = Vector2.up * flapStrength; // Use velocity instead of linearVelocity
                WingUp.enabled = true;
                WingDown.enabled = false;

                if (jumpSound != null)
                {
                    jumpSound.Play();
                }
            }

            if (timer > 0.075)
            {
                if (myRigidBody.linearVelocity.y > 0) // Use velocity instead of linearVelocity
                {
                    WingUp.enabled = false;
                    WingDown.enabled = true;
                }
                else
                {
                    WingDown.enabled = false;
                    WingUp.enabled = true;
                }
            }

            // Check if the bird goes off-screen
            if ((transform.position.y > 21 || transform.position.y < -21) && birdIsAlive)
            {
                HandleDeath();
            }

            timer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (birdIsAlive) // Only handle death if the bird is still alive
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        // Mark the bird as dead
        birdIsAlive = false;

        // Play the death sound only once
        if (!hasDied && deathSound != null)
        {
            deathSound.Play();
            hasDied = true; // Ensure the sound is not played again
        }

        // Trigger game over logic
        logic.gameOver();
    }
}