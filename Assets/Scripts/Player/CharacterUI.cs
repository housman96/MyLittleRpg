using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    public GameObject halo;
    private bool isMouseOver = false;
    // Use this for initialization
    void Start()
    {
        halo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseEnter()
    {
        isMouseOver = true;
        halo.SetActive(true);
    }

    public void OnMouseExit()
    {
        isMouseOver = false;
        halo.SetActive(false);
        halo.GetComponent<Behaviour>();
    }


    public void OnMouseDown()
    {
        if (isMouseOver)
        {
            FindObjectOfType<UICharacterStats>().setStats(gameObject.GetComponent<CharacterStats>());
        }
    }
}
