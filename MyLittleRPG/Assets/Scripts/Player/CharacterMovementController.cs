using System.Collections;
using UnityEngine;

public enum Sens { Down, Up, Left, Right }

public class CharacterMovementController : MonoBehaviour
{

    public float speed;
    public GameObject Layers;
    public Sens sens = Sens.Down;

    private Animator[] animators;
    private SpriteRenderer spriteRenderer;
    private float animationDuration;
    private Vector3 targetAnimation;
    private Vector3 scaleAnimation;
    private float speedAnimation;
    private float speedScale;
    private float lastTime;

    //boolean utilisé pour savoir si on doit lancer une animation
    public bool isPlayer = false;
    private bool isInAnimation = false;
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
            //pour tous les animators
            foreach (Animator animator in animators)
            {
                //si on est mort on lance l'animation de mort
                if (isDead)
                {
                    animator.SetBool("Dead", true);
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

                //si on est dans une animation
                if (isInAnimation)
                {
                    //on déplace le personnage vers targetAnimation à la vitesse de speedAnimation
                    float step = speedAnimation * (Time.time - lastTime);
                    float stepScale = speedScale * (Time.time - lastTime);

                    if (!isLocked)
                    {
                        animator.SetBool("isMoving", isMoving);
                        animator.SetFloat("XSpeed", Vector3.MoveTowards(transform.position, targetAnimation, step).x);
                        animator.SetFloat("YSpeed", Vector3.MoveTowards(transform.position, targetAnimation, step).y);
                    }


                    transform.localScale = Vector3.MoveTowards(transform.localScale, scaleAnimation, stepScale);
                    transform.position = Vector3.MoveTowards(transform.position, targetAnimation, step);
                    if (transform.position == targetAnimation && transform.localScale == scaleAnimation)
                    {
                        isInAnimation = false;
                    }
                    lastTime = Time.time;

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
                    //si les mouvements du personnage ne sont pas lock on lance les animations de marches en fonctions des inputs
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
                        if (horizontalInput != 0 || verticalInput != 0)
                        {
                            if (horizontalInput * horizontalInput > verticalInput * verticalInput)
                            {
                                if (horizontalInput > 0)
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
                                if (verticalInput > 0)
                                {
                                    sens = Sens.Up;
                                }
                                else
                                {
                                    sens = Sens.Down;
                                }
                            }

                        }

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

    //animation de marche jusqu'au point target en time temps 
    public void moveToward(Vector3 target, float time)
    {
        animationDuration = time;
        isInAnimation = true;
        targetAnimation = target;
        scaleAnimation = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);

        speedAnimation = Vector3.Distance(transform.position, targetAnimation) / animationDuration;
        speedScale = Vector3.Distance(transform.localScale, scaleAnimation) / animationDuration;
        lastTime = Time.time;
    }

    //animation de marche jusqu'au point target à la taille scale en time temps 
    public void moveToward(Vector3 target, Vector3 scale, float time)
    {
        animationDuration = time;
        isInAnimation = true;
        targetAnimation = target;
        scaleAnimation = scale;

        speedAnimation = Vector3.Distance(transform.position, targetAnimation) / animationDuration;
        speedScale = Vector3.Distance(transform.localScale, scaleAnimation) / animationDuration;
        lastTime = Time.time;
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
