using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{

    public Rigidbody2D myRigidBody;
    public float flapStrength;
    public LogicScript logic;
    public bool birdIsAlive = true;

    public SpriteRenderer WingUp;

    public SpriteRenderer WingDown;

    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive) { 
            timer = 0;
            myRigidBody.linearVelocity = Vector2.up * flapStrength;
            WingUp.enabled = true;
            WingDown.enabled = false;
        }
        if(timer > 0.075) {
            if(myRigidBody.linearVelocity.y > 0) {
                WingUp.enabled = false;
                WingDown.enabled = true;
            }
            else {
                WingDown.enabled = false;
                WingUp.enabled = true;
            }
        }
        if (transform.position.y > 21 || transform.position.y < -21)
        {
            logic.gameOver();
            birdIsAlive = false;
        }
        timer += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.gameOver();
        birdIsAlive = false;
    }
}
