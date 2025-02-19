using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMoveScript : MonoBehaviour
{

    public float moveSpeed = 10;
    public float deadZone = -50;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = Random.Range(10, 20);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Destroy(gameObject);
        }
    }
}
