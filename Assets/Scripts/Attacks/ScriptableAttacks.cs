using UnityEngine;

[CreateAssetMenu(fileName = "attack", menuName = "Attack")]

public class ScriptableAttacks : ScriptableObject
{
    public Sprite imageAttack;

    public string name;
    public string description;

    //nom de l'attaque
    public string attackName;

    //distance minimum où on peut lancer l'attaque
    public float distanceMin;

    //points de stats nécessaire pour lancer l'attaque
    public float forceFactor;
    public float dextFactor;
    public float intFactor;

    //couts de l'attaque
    public int forceCost;
    public int dextCost;
    public int intCost;

    //dégats
    public int dgtsMax;
    public int dgtsMin;

    //attack en elle même
    public Attacks attack;


    //methode pour lancer l'attaque
    public virtual void launchAttack(CharacterStats char1, CharacterStats char2)
    {
        Attacks objectTemp = Instantiate(attack);


        objectTemp.AttackName = attackName;
        objectTemp.DistanceMin = distanceMin;
        objectTemp.ForceFactor = forceFactor;
        objectTemp.DextFactor = dextFactor;
        objectTemp.IntFactor = intFactor;
        objectTemp.ForceCost = forceCost;
        objectTemp.DextCost = dextCost;
        objectTemp.IntCost = intCost;
        objectTemp.DgtsMax = dgtsMax;
        objectTemp.DgtsMin = dgtsMin;
        objectTemp.launchAttack(char1, char2);
    }
}
