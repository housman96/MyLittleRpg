using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public int speed;
    private bool isLocked = false;
    private bool isMoving = false;

    void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isLocked)
        {
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

            animator.SetBool("isMoving", isMoving);
            animator.SetFloat("XSpeed", horizontalInput);
            animator.SetFloat("YSpeed", verticalInput);

            transform.Translate(translation);
        }
        else
        {
            animator.SetFloat("XSpeed", 0); // Stop les animations quand on est lock
            animator.SetFloat("YSpeed", 0);
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
}
