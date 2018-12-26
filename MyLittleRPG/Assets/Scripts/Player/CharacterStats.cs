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
    public Attacks attack1;
    public Attacks attack2;
    public Attacks attack3;
    public Attacks attack4;

    //test
    public CharacterStats other;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //test de la methode à enlever
        if (Input.GetButtonDown("e") && name == "ElBlanco")
        {
            attack1.launchAttack(this, other);
        }
    }
}
