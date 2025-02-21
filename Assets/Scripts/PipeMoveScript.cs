using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public double moveSpeed = 8;
    public float deadZone = -50;
    public double initialMoveSpeed;

    private LogicScript logic;

    // Start is called before the first frame update
    void Start()
    {
        logic = FindObjectOfType<LogicScript>();
        logic.RegisterPipe(this);
        initialMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.left * (float)moveSpeed) * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}