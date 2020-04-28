using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardSelector : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            animator.SetBool("Zpress", true);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            animator.SetBool("Xpress", true);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            animator.SetBool("Cpress", true);
        }
        else if (Input.GetKey(KeyCode.V))
        {
            animator.SetBool("Vpress", true);
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            animator.SetBool("Bpress", true);
        }
        else if (Input.GetKey(KeyCode.None))
        {
            animator.SetBool("Zpress", false);
            animator.SetBool("Xpress", false);
            animator.SetBool("Cpress", false);
            animator.SetBool("Vpress", false);
            animator.SetBool("Bpress", false);
        }
        

    }
}
