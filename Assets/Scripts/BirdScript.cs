using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

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

    public TextMeshProUGUI slowMotionCooldownText;
    private float cooldownTimer = 0f;
    private float timer = 0;
    private bool hasDied = false;
    private bool isSlowMotion = false;
    private Coroutine slowMotionCoroutine;
    private bool canUseSlowMotion = true;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        slowMotionCooldownText.text = "Slow Motion Ready!";
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && birdIsAlive)
            {
                timer = 0;
                myRigidBody.linearVelocity = Vector2.up * flapStrength;
                WingUp.enabled = true;
                WingDown.enabled = false;

                if (jumpSound != null)
                {
                    jumpSound.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.Mouse1) && canUseSlowMotion)
            {
                ToggleSlowMotion();
            }

            if (timer > 0.075)
            {
                if (myRigidBody.linearVelocity.y > 0)
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

            if ((transform.position.y > 21 || transform.position.y < -21) && birdIsAlive)
            {
                HandleDeath();
            }

            timer += Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (birdIsAlive)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        birdIsAlive = false;

        if (!hasDied && deathSound != null)
        {
            deathSound.Play();
            hasDied = true;
        }

        logic.gameOver();
    }

    private void ToggleSlowMotion()
    {
        if (isSlowMotion)
        {
            StopSlowMotion();
        }
        else
        {
            StartSlowMotion();
        }
    }

    private void StartSlowMotion()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 
        isSlowMotion = true;
        canUseSlowMotion = false;
        
        if (slowMotionCoroutine != null)
        {
            StopCoroutine(slowMotionCoroutine);
        }
        slowMotionCoroutine = StartCoroutine(SlowMotionTimer());
    }

    private void StopSlowMotion()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f; 
        isSlowMotion = false;
        
        if (slowMotionCoroutine != null)
        {
            StopCoroutine(slowMotionCoroutine);
            slowMotionCoroutine = null;
        }
        
        StartCoroutine(SlowMotionCooldown());
    }

    private IEnumerator SlowMotionTimer()
    {
        yield return new WaitForSecondsRealtime(3f);
        StopSlowMotion();
    }

    private IEnumerator SlowMotionCooldown()
    {
        cooldownTimer = 10f;
        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.unscaledDeltaTime;
            slowMotionCooldownText.text = $"Slow Motion: {Mathf.Ceil(cooldownTimer)}s";
            yield return null;
        }
        
        slowMotionCooldownText.text = "Slow Motion Ready!";
        canUseSlowMotion = true;
    }
}