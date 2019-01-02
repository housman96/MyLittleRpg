using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks : MonoBehaviour
{
    //mainCamera
    protected Camera mainCamera;

    //Liste des objets à détruire à la fin du jeu
    protected List<GameObject> objectToDestroy = new List<GameObject>();

    //scale des objets du fait du champ de vision de la caméra
    protected Vector2 scaleVector;

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

    protected CharacterMovementController attaquantController;
    protected CharacterMovementController defenseurController;

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
        attaquant = char1;
        defenseur = char2;

        //on verifi qu'on est à la bonne distance pour attaquer et que l'attaque ne coute pas trop cher 
        float distance = Vector3.Distance(attaquant.transform.position, defenseur.transform.position);

        //tests
        bool isAttackingTest = attaquant.isAttacking || defenseur.isAttacking;
        bool distanceTest = distance > distanceMin;
        bool costTest = attaquant.currentForce < forceCost || attaquant.currentDext < dextCost || attaquant.currentIntel < intCost;
        if (isAttackingTest || distanceTest || costTest)
        {
            GameObject.DestroyImmediate(gameObject);
            return;
        }

        //on applique le coup de l'attaque
        attaquant.isAttacking = true;
        attaquant.currentForce -= forceCost;
        attaquant.currentDext -= dextCost;
        attaquant.currentIntel -= intCost;

        //on get la camera
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        //on initialise le scale vector de la camera
        scaleVector = new Vector2(mainCamera.orthographicSize, mainCamera.orthographicSize);

        //on sauvegarde les paramètres dont on aura besoin dans la coroutine

        attaquantController = attaquant.GetComponent<CharacterMovementController>();
        defenseurController = defenseur.GetComponent<CharacterMovementController>();

        difficultyFactor = ((defenseur.resDext * dextFactor / attaquant.dext) + (defenseur.resForce * forceFactor / attaquant.force) + (defenseur.resIntel * intFactor / attaquant.intel));

        //on bloque tout ce que controle les personnages
        mainCamera.GetComponent<CameraFollowing>().enabled = false;
        attaquantController.LockMoves();
        defenseurController.LockMoves();
        attaquant.GetComponent<BoxCollider2D>().enabled = false;
        defenseur.GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine("MiniJeu");
    }

    public virtual IEnumerator MiniJeu()
    {
        return null;
    }

    public void endGameProcessing()
    {
        //on débloque les personnages
        mainCamera.GetComponent<CameraFollowing>().enabled = true;
        attaquantController.UnlockMoves();
        defenseurController.UnlockMoves();
        attaquant.GetComponent<BoxCollider2D>().enabled = true;
        defenseur.GetComponent<BoxCollider2D>().enabled = true;


        //on détruit tous les objets créé pour le jeu
        foreach (GameObject o in objectToDestroy)
        {
            Object.DestroyImmediate(o);
        }
        GameObject.DestroyImmediate(gameObject);

        attaquant.isAttacking = false;
    }

}
