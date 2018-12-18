using UnityEngine;

public abstract class Attacks : MonoBehaviour
{
    public string attackName;

    public float forceFactor;
    public float dextFactor;
    public float intFactor;

    public abstract void launchAttack(CharacterStats char1, CharacterStats char2);
    public abstract void reussiteCritique(CharacterStats char1, CharacterStats char2);
}
