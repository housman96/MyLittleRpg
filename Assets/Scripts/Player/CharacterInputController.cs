using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    Vector2 input;

    //animation variable
    public bool isPlayer = false;
    public float speed;
    private bool isInAnimation = false;
    private Stack<Noeud> stack = new Stack<Noeud>();
    private float animationDuration;
    private Vector3 targetAnimation;
    private Vector3 scaleAnimation;
    private float speedAnimation;
    private float speedScale;
    private float lastTime;

    private CharacterAnimationController animationController;
    private bool isLocked;

    // Use this for initialization
    void Start()
    {
        lastTime = Time.time;
        input = new Vector2(0, 0);
        animationController = GetComponent<CharacterAnimationController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float lastUpdateDeltaTime = (Time.time - lastTime);

        if (Input.GetButton("e"))
        {
            animationController.sword(1, 1);
        }


        //si on est dans une animation
        if (isInAnimation)
        {
            //on déplace le personnage vers targetAnimation à la vitesse de speedAnimation

            float step = speedAnimation * lastUpdateDeltaTime;
            float stepScale = speedScale * lastUpdateDeltaTime;

            Vector3 newPositon = Vector3.MoveTowards(transform.position, targetAnimation, step);
            Vector2 translation = newPositon - transform.position;


            if (Mathf.Abs(translation.x) > Mathf.Abs(translation.y))
            {
                input.y = 0;
                if (translation.x > 0)
                {
                    input.x = 1;
                }
                else
                {
                    input.x = -1;
                }
            }
            else
            {
                input.x = 0;
                if (translation.y > 0)
                {
                    input.y = 1;
                }
                else
                {
                    input.y = -1;
                }
            }


            transform.localScale = Vector3.MoveTowards(transform.localScale, scaleAnimation, stepScale);
            transform.position = newPositon;


            if (transform.position == targetAnimation && transform.localScale == scaleAnimation)
            {
                if (stack.Count == 0)
                {
                    isInAnimation = false;
                }
                else
                {
                    targetAnimation = stack.Pop().position;
                    speedAnimation = Vector3.Distance(transform.position, targetAnimation) / animationDuration;
                }

            }




        }
        else
        {
            //si on est un joueur
            if (isPlayer && !isLocked)
            {
                //on récupére les inputs
                input.x = Input.GetAxis("Horizontal");
                input.y = Input.GetAxis("Vertical");

                Vector2 translation = new Vector3(input.x * lastUpdateDeltaTime * speed, input.y * lastUpdateDeltaTime * speed);

                //on bouge le personnage
                transform.Translate(translation);
            }
        }

        if (animationController != null)
        {
            animationController.input = input;
        }
        lastTime = Time.time;
    }

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }

    //animation de marche jusqu'au point target en time temps 
    public void moveToward(Stack<Noeud> stack)
    {
        if (stack != null && stack.Count != 0)
        {
            isInAnimation = true;
            targetAnimation = stack.Pop().position;
            this.stack = stack;
            scaleAnimation = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            speedAnimation = speed;
            animationDuration = Vector3.Distance(transform.position, targetAnimation) / speedAnimation;
            speedScale = Vector3.Distance(transform.localScale, scaleAnimation) / animationDuration;
            lastTime = Time.time;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (isInAnimation)
        {

            stack.Clear();
            targetAnimation = transform.position;
            isInAnimation = false;
        }
    }
}
