using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private float regulateX;
    [SerializeField]
    private float regulateY;

    [SerializeField]
    private GameObject player;

    private Rigidbody2D rb;

    void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (rb != null)
            gameObject.transform.position = new Vector3(rb.transform.position.x + regulateX, rb.transform.position.y + regulateY, -10);
    }
}
