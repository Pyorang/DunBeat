using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    void Start()
    {
        rb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
            gameObject.transform.position = new Vector3(rb.transform.position.x, rb.transform.position.y, -10);
    }
}
