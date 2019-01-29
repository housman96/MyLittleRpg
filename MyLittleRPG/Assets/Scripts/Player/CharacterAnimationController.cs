using System.Collections;
using UnityEngine;

public enum Sens { Down, Up, Left, Right }

public class CharacterAnimationController : MonoBehaviour
{


    public GameObject Layers;
    public Sens sens = Sens.Down;


    private Animator[] animators;
    public Vector2 input = new Vector2();


    //boolean utilisé pour savoir si on doit lancer une animation
    public bool isLocked = false;
    private bool isMoving = false;
    private bool isDisplayed = true;
    private bool isDead = false;

    void Awake()
    {
        //on récupére tous les animators des enfants dans l'objet Layers
        int i = 0;
        animators = new Animator[Layers.transform.childCount];
        foreach (Transform child in Layers.transform)
        {
            animators[i] = child.GetComponent<Animator>();
            i++;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //si on a des animators dans les enfants
        if (animators.Length > 0)
        {

            if (isLocked)
            {
                input = new Vector2(0, 0);
                isMoving = false;
            }
            //sinon on set les paramètres de l'animator à 0
            else
            {
                if (input.x == 0 && input.y == 0)
                {
                    isMoving = false;
                }
                else
                {
                    isMoving = true;

                    if (input.x * input.x > input.y * input.y)
                    {
                        if (input.x > 0)
                        {

                            sens = Sens.Right;
                        }
                        else
                        {
                            sens = Sens.Left;
                        }
                    }
                    else
                    {
                        if (input.y > 0)
                        {
                            sens = Sens.Up;
                        }
                        else
                        {
                            sens = Sens.Down;
                        }
                    }
                }
            }


            //pour tous les animators
            foreach (Animator animator in animators)
            {
                //si on est mort on lance l'animation de mort
                if (isDead)
                {
                    animator.SetBool("Dead", true);
                    return;
                }

                //si on doit afficher le personnage
                if (isDisplayed)
                {
                    animator.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    animator.GetComponent<SpriteRenderer>().enabled = false;
                }

                //movement

                animator.SetBool("isMoving", isMoving);
                animator.SetFloat("XSpeed", input.x);
                animator.SetFloat("YSpeed", input.y);

                //si on est dans une animation d'attaque on désactive le boolean qui declenche l'animation d'attaque
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag("handsUp"))
                {
                    animator.SetBool("HandsUp", false);
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag("spear"))
                {
                    animator.SetBool("spear", false);
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag("sword"))
                {
                    animator.SetBool("sword", false);
                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsTag("bow"))
                {
                    animator.SetBool("bow", false);
                }
            }
        }
    }



    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }

    //animation d'attaque de lance
    public void spear(float Xspeed, float Yspeed)
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("spear", true);
            animator.SetFloat("XSpeed", Xspeed);
            animator.SetFloat("YSpeed", Yspeed);

        }
    }

    //animation d'attaque mains en l'air
    public void handsUp(float Xspeed, float Yspeed)
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("HandsUp", true);
            animator.SetFloat("XSpeed", Xspeed);
            animator.SetFloat("YSpeed", Yspeed);
        }
    }

    //animation d'attaque à l'épée
    public void sword(float Xspeed, float Yspeed)
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("sword", true);
            animator.SetFloat("XSpeed", Xspeed);
            animator.SetFloat("YSpeed", Yspeed);
        }
    }

    //animation d'attaque de l'arc
    public void bow(float Xspeed, float Yspeed)
    {
        foreach (Animator animator in animators)
        {
            animator.SetBool("sword", true);
            animator.SetFloat("XSpeed", Xspeed);
            animator.SetFloat("YSpeed", Yspeed);
        }
    }



    //on fait regarder le personnage d'un côté
    public void lookAt(Sens sens)
    {
        foreach (Animator animator in animators)
        {
            float Xspeed = 0.0f;
            float Yspeed = 0.0f;

            if (sens == Sens.Right)
            {
                Xspeed = 1.0f;
            }
            if (sens == Sens.Left)
            {
                Xspeed = -1.0f;
            }
            if (sens == Sens.Up)
            {
                Yspeed = 1.0f;
            }
            if (sens == Sens.Down)
            {
                Yspeed = -1.0f;
            }


            animator.SetBool("isMoving", false);
            animator.SetFloat("XSpeed", Xspeed);
            animator.SetFloat("YSpeed", Yspeed);
        }
    }

    //lance l'animation de blessure
    public IEnumerator hurted()
    {
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            isDisplayed = false;
            yield return new WaitForSeconds(0.1f);
            isDisplayed = true;
        }
    }

    //lance l'animation de mort
    public void dead()
    {
        isDead = true;
        foreach (Animator animator in animators)
        {
            animator.SetFloat("XSpeed", 0.0f);
            animator.SetFloat("YSpeed", 0.0f);
        }
    }

}
