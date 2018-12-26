using UnityEngine;


public class Attacks : ScriptableObject
{
    //nom de l'attaque
    public string attackName;

    //distance minimum où on peut lancer l'attaque
    public float distanceMin;

    //points de stats nécessaire pour lancer l'attaque
    public float forceFactor;
    public float dextFactor;
    public float intFactor;

    //methode pour lancer l'attaque
    public virtual void launchAttack(CharacterStats char1, CharacterStats char2)
    {
    }

    //methode pour lancer la reussite critique
    public virtual void reussiteCritique(CharacterStats char1, CharacterStats char2)
    {
    }
}
