using UnityEngine;

public class CharacterEnabler : MonoBehaviour
{

    public Character character;

    public string name;

    public CharacterStats stats;

    //part body
    public GameObject body;
    public GameObject eyes;
    public GameObject chest;
    public GameObject arms;
    public GameObject pant;
    public GameObject shoes;
    public GameObject hands;
    public GameObject hair;
    public GameObject weapon;
    public GameObject hat;
    public GameObject nose;

    // Use this for initialization
    void Start()
    {
        name = character.name;
        gameObject.name = name;
        stats.name = character.name;

        body.GetComponent<Animator>().runtimeAnimatorController = character.body.GetComponent<Animator>().runtimeAnimatorController;
        eyes.GetComponent<Animator>().runtimeAnimatorController = character.eyes.GetComponent<Animator>().runtimeAnimatorController;
        chest.GetComponent<Animator>().runtimeAnimatorController = character.chest.GetComponent<Animator>().runtimeAnimatorController;
        arms.GetComponent<Animator>().runtimeAnimatorController = character.arms.GetComponent<Animator>().runtimeAnimatorController;
        pant.GetComponent<Animator>().runtimeAnimatorController = character.pant.GetComponent<Animator>().runtimeAnimatorController;
        shoes.GetComponent<Animator>().runtimeAnimatorController = character.shoes.GetComponent<Animator>().runtimeAnimatorController;
        hands.GetComponent<Animator>().runtimeAnimatorController = character.hands.GetComponent<Animator>().runtimeAnimatorController;
        hair.GetComponent<Animator>().runtimeAnimatorController = character.hair.GetComponent<Animator>().runtimeAnimatorController;
        weapon.GetComponent<Animator>().runtimeAnimatorController = character.weapon.GetComponent<Animator>().runtimeAnimatorController;
        hat.GetComponent<Animator>().runtimeAnimatorController = character.hat.GetComponent<Animator>().runtimeAnimatorController;
        nose.GetComponent<Animator>().runtimeAnimatorController = character.nose.GetComponent<Animator>().runtimeAnimatorController;


        body.GetComponent<SpriteRenderer>().sortingOrder = 0;
        eyes.GetComponent<SpriteRenderer>().sortingOrder = 1;
        chest.GetComponent<SpriteRenderer>().sortingOrder = 2;
        arms.GetComponent<SpriteRenderer>().sortingOrder = 3;
        pant.GetComponent<SpriteRenderer>().sortingOrder = 1;
        shoes.GetComponent<SpriteRenderer>().sortingOrder = 1;
        hands.GetComponent<SpriteRenderer>().sortingOrder = 1;
        hair.GetComponent<SpriteRenderer>().sortingOrder = 1;
        weapon.GetComponent<SpriteRenderer>().sortingOrder = 3;
        hat.GetComponent<SpriteRenderer>().sortingOrder = 4;
        nose.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
