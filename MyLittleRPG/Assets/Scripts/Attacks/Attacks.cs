using UnityEngine;

public class Attacks : MonoBehaviour
{

    //nom de l'attaque
    protected string attackName;

    //distance minimum où on peut lancer l'attaque
    protected float distanceMin;

    //points de stats nécessaire pour lancer l'attaque
    protected float forceFactor;
    protected float dextFactor;
    protected float intFactor;

    //couts de l'attaque
    protected int forceCost;
    protected int dextCost;
    protected int intCost;

    protected float difficultyFactor;

    protected CharacterStats attaquant;
    protected CharacterStats defenseur;

    //dégats
    protected int dgtsMax;
    protected int dgtsMin;

    public string AttackName
    {
        get
        {
            return attackName;
        }

        set
        {
            attackName = value;
        }
    }

    public float DistanceMin
    {
        get
        {
            return distanceMin;
        }

        set
        {
            distanceMin = value;
        }
    }

    public float ForceFactor
    {
        get
        {
            return forceFactor;
        }

        set
        {
            forceFactor = value;
        }
    }

    public float DextFactor
    {
        get
        {
            return dextFactor;
        }

        set
        {
            dextFactor = value;
        }
    }

    public float IntFactor
    {
        get
        {
            return intFactor;
        }

        set
        {
            intFactor = value;
        }
    }

    public int ForceCost
    {
        get
        {
            return forceCost;
        }

        set
        {
            forceCost = value;
        }
    }

    public int DextCost
    {
        get
        {
            return dextCost;
        }

        set
        {
            dextCost = value;
        }
    }

    public int IntCost
    {
        get
        {
            return intCost;
        }

        set
        {
            intCost = value;
        }
    }

    public CharacterStats Attaquant
    {
        get
        {
            return attaquant;
        }

        set
        {
            attaquant = value;
        }
    }

    public CharacterStats Defenseur
    {
        get
        {
            return defenseur;
        }

        set
        {
            defenseur = value;
        }
    }

    public int DgtsMax
    {
        get
        {
            return dgtsMax;
        }

        set
        {
            dgtsMax = value;
        }
    }

    public int DgtsMin
    {
        get
        {
            return dgtsMin;
        }

        set
        {
            dgtsMin = value;
        }
    }




    //methode pour lancer l'attaque
    public virtual void launchAttack(CharacterStats char1, CharacterStats char2)
    {
        Debug.Log(dextFactor);

        //on sauvegarde les paramètres dont on aura besoin dans la coroutine
        attaquant = char1;
        defenseur = char2;
        difficultyFactor = ((defenseur.resDext * dextFactor / attaquant.dext) + (defenseur.resForce * forceFactor / attaquant.force) + (defenseur.resIntel * intFactor / attaquant.intel));



        //on verifi qu'on est à la bonne distance pour attaquer et que l'attaque ne coute pas trop cher 
        float distance = Vector3.Distance(char1.transform.position, char2.transform.position);

        CharacterStats statChar1 = char1.GetComponent<CharacterStats>();
        if (distance > distanceMin && statChar1.currentForce < forceCost && statChar1.currentDext < dextCost && statChar1.currentIntel < intCost)
        {
            return;
        }
        else
        {
            statChar1.currentForce -= forceCost;
            statChar1.currentDext -= dextCost;
            statChar1.currentIntel -= intCost;
        }
    }

}
