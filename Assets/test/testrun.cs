using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testrun : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up"))
        {
            animator.SetBool("is_running", true);
        }
        else
        {
            animator.SetBool("is_running", false);
        }
        if (Input.GetKey("right"))
        {
            transform.Rotate(0, 10, 0);
        }
        if (Input.GetKey("left"))
        {
            transform.Rotate(0, -10, 0);
        }
    }
}
