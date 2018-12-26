using UnityEngine;

public enum Sens { Up, Down, Left, Right }

public class CharacterController : MonoBehaviour
{



    public bool isPlayer = false;
    public float speed;

    private Animator[] animators;
    private SpriteRenderer spriteRenderer;
    private Vector3 targetAnimation;
    private bool isInAnimation = false;
    private bool isLocked = false;
    private bool isMoving = false;

    void Awake()
    {
        //on récupére tous les animators des enfants
        int i = 0;
        animators = new Animator[transform.childCount];
        foreach (Transform child in transform)
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
            //pour tous les animators
            foreach (Animator animator in animators)
            {
                //si on est dans une animation
                if (isInAnimation)
                {
                    //on déplace le personnage vers targetAnimation en 2 secondes
                    float speedAnimation = Vector3.Distance(transform.position, targetAnimation) / 2;
                    float step = speedAnimation * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, targetAnimation, step);
                    if (transform.position == targetAnimation)
                    {
                        isInAnimation = false;
                    }
                }

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

                //si on est un joueur
                if (isPlayer)
                {
                    //si les mouvements du personnage sont pas lock on lance les animations de marches en fonctions des inputs
                    if (!isLocked)
                    {
                        //on récupére les inputs
                        float horizontalInput = Input.GetAxis("Horizontal");
                        float verticalInput = Input.GetAxis("Vertical");

                        if (Mathf.Abs(horizontalInput) > 0.01 || Mathf.Abs(verticalInput) > 0.01)
                        {
                            isMoving = true;
                        }
                        else
                        {
                            isMoving = false;
                        }

                        float horizontalTranslation = horizontalInput * Time.deltaTime * speed;
                        float verticalTranslation = verticalInput * Time.deltaTime * speed;

                        Vector2 translation = new Vector3(horizontalTranslation, verticalTranslation);

                        //on set les paramètres de l'animator
                        animator.SetBool("isMoving", isMoving);
                        animator.SetFloat("XSpeed", horizontalInput);
                        animator.SetFloat("YSpeed", verticalInput);

                        //on bouge le personnage
                        transform.Translate(translation);
                    }
                    //sinon on set les paramètres de l'animator à 0
                    else
                    {
                        animator.SetFloat("XSpeed", 0); // Stop les animations quand on est lock
                        animator.SetFloat("YSpeed", 0);
                    }
                }
            }
        }
    }

    public void LockMoves()
    {
        isLocked = true;
    }

    public void UnlockMoves()
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

    //animation de marche jusqu'au point v
    public void moveToward(Vector3 v)
    {
        isInAnimation = true;
        targetAnimation = v;
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

}
