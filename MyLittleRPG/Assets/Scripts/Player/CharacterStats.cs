using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    //Force
    public int force;
    public int currentForce;
    public int resForce;

    //Dexterite
    public int dext;
    public int currentDext;
    public int resDext;

    //Inteligence
    public int intel;
    public int currentIntel;
    public int resIntel;

    //PV
    public int PV;
    public int currentPV;

    //Attacks
    public ScriptableAttacks attack1;
    public ScriptableAttacks attack2;
    public ScriptableAttacks attack3;
    public ScriptableAttacks attack4;

    //bool
    public bool isAttacking = false;

    //test
    public CharacterStats other;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentPV <= 0)
        {
            GetComponent<CharacterAnimationController>().dead();
        }

        //test de la methode à enlever
        if (Input.GetButtonDown("e") && name == "ElBlanco")
        {
            attack1.launchAttack(this, other);
        }
    }


    public void hurted(int dgts)
    {
        currentPV -= dgts;
        StartCoroutine(GetComponent<CharacterAnimationController>().hurted());
    }
}
