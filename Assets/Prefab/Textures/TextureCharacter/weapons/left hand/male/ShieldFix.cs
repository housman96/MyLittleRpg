using UnityEngine;

public class ShieldFix : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Animator anim = GetComponent<Animator>();
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Down"))
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("Transition"))
                GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
