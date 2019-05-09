using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterStats : MonoBehaviour
{
    /*Variables CharacterStats*/
    private CharacterStats stats;                //stats du personnage selectionné

    /*Variables RectTransform*/
    public RectTransform rectTransform;         //RectTransform qui contient tous les objets du pannel principal
    public RectTransform rectAttackTransform;   //RectTransform qui contient la description des attaques

    /*Variables float*/
    private float defaultX;                     //offset par default de rectTransform
    private float defaultAttackX;               //offset par default de rectAttackTransform


    /*Variables Text*/
    //text attack
    public Text attackName;
    public Text attackDescription;

    //text dexctiption personnage 
    public Text name;
    public Text PV;
    public Text Force;
    public Text Dext;
    public Text Int;
    public Text ResForce;
    public Text ResDext;
    public Text ResInt;

    /*Variables Image*/
    //jauges carac
    public Image PVMask;
    public Image ForceMask;
    public Image DextMask;
    public Image IntMask;
    public Image ResForceMask;
    public Image ResDextMask;
    public Image ResIntMask;

    //image attacks
    public Image imageAttack1;
    public Image imageAttack2;
    public Image imageAttack3;
    public Image imageAttack4;

    //jauge attack
    public Image jaugeInteligence;
    public Image jaugeDext;

    //image attack par default
    public Sprite imageAttackDefault;

    /*Variables ScriptableAttacks*/

    public ScriptableAttacks currentAttackDisplay;       //stats attaque actuellement affichée

    /*Variables Bool*/
    public bool isMoving = false;


    // Use this for initialization
    void Start()
    {
        //on récupére les offset et on set la position initial des pannels
        defaultAttackX = rectAttackTransform.localPosition.x;
        rectAttackTransform.localPosition = new Vector3(0, rectAttackTransform.localPosition.y, rectAttackTransform.localPosition.z);

        defaultX = rectTransform.position.x;
        rectTransform.position = new Vector3(-rectTransform.position.x, rectTransform.position.y, rectTransform.position.z);

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void setStats(CharacterStats stats)
    {
        //si le panel est déjà en mouvement on ne set pas la variable stats
        if (isMoving)
            return;

        //si stats est différent de l'actuel stats on refresh les valeurs du panel
        if (stats != this.stats)
        {
            StartCoroutine("displayStats");

            if (stats != null)
            {
                this.stats = stats;
                name.text = stats.name;
                PV.text = stats.currentPV + "/" + stats.PV;
                Force.text = stats.currentForce + "/" + stats.force;
                Dext.text = stats.currentDext + "/" + stats.dext;
                Int.text = stats.currentIntel + "/" + stats.intel;
                ResForce.text = stats.resForce.ToString();
                ResDext.text = stats.resDext.ToString();
                ResInt.text = stats.resIntel.ToString();

                PVMask.fillAmount = (float)stats.currentPV / (float)stats.PV;
                ForceMask.fillAmount = (float)stats.currentForce / (float)stats.force;
                DextMask.fillAmount = (float)stats.currentDext / (float)stats.dext;
                IntMask.fillAmount = (float)stats.currentIntel / (float)stats.intel;

                if (stats.attack1 != null)
                {
                    imageAttack1.sprite = stats.attack1.imageAttack;
                }
                else
                {
                    imageAttack1.GetComponentInParent<Button>().interactable = false;
                    imageAttack1.sprite = imageAttackDefault;
                }
                if (stats.attack2 != null)
                    imageAttack2.sprite = stats.attack2.imageAttack;
                else
                {
                    imageAttack2.GetComponentInParent<Button>().interactable = false;
                    imageAttack2.sprite = imageAttackDefault;
                }
                if (stats.attack3 != null)
                    imageAttack3.sprite = stats.attack3.imageAttack;
                else
                {
                    imageAttack3.GetComponentInParent<Button>().interactable = false;
                    imageAttack3.sprite = imageAttackDefault;
                }
                if (stats.attack4 != null)
                    imageAttack4.sprite = stats.attack4.imageAttack;
                else
                {
                    imageAttack4.GetComponentInParent<Button>().interactable = false;
                    imageAttack4.sprite = imageAttackDefault;
                }
            }
        }
        //sinon on cache le panel
        else
        {

            StartCoroutine("hideStats");
            this.stats = null;
        }
    }

    public void onButtonClickHideStats()
    {
        if (isMoving)
            return;
        stats = null;
        StartCoroutine("hideStats");
    }

    public void onButtonClickAttack1()
    {
        if (isMoving)
            return;
        if (stats == null)
            return;
        if (currentAttackDisplay != stats.attack1)
        {
            currentAttackDisplay = stats.attack1;
            StartCoroutine("displayAttack");
        }
        else
            StartCoroutine("HideAttack");
    }

    public void onButtonClickAttack2()
    {
        if (isMoving)
            return;
        if (stats == null)
            return;
        if (currentAttackDisplay != stats.attack2)
        {
            currentAttackDisplay = stats.attack2;
            StartCoroutine("displayAttack");
        }
        else
            StartCoroutine("HideAttack");
    }

    public void onButtonClickAttack3()
    {
        if (isMoving)
            return;
        if (stats == null)
            return;
        if (currentAttackDisplay != stats.attack3)
        {
            currentAttackDisplay = stats.attack3;
            StartCoroutine("displayAttack");
        }
        else
            StartCoroutine("HideAttack");
    }

    public void onButtonClickAttack4()
    {
        if (isMoving)
            return;
        if (stats == null)
            return;
        if (currentAttackDisplay != stats.attack4)
        {
            StartCoroutine("displayAttack");
            currentAttackDisplay = stats.attack4;

        }
        else
            StartCoroutine("HideAttack");
    }

    public IEnumerator hideStats()
    {
        yield return new WaitUntil(() => !isMoving);
        isMoving = true;
        while (rectTransform.position.x > -defaultX + 0.0001f || rectAttackTransform.localPosition.x > 0.0001f)
        {
            rectTransform.position = Vector3.MoveTowards(rectTransform.position, new Vector3(-defaultX, rectTransform.position.y, rectTransform.position.z), 20);
            rectAttackTransform.localPosition = Vector3.MoveTowards(rectAttackTransform.localPosition, new Vector3(0, rectAttackTransform.localPosition.y, rectAttackTransform.localPosition.z), 30);
            yield return new WaitForSeconds(0.01f);

        }
        isMoving = false;
        currentAttackDisplay = null;
    }

    public IEnumerator displayStats()
    {
        yield return new WaitUntil(() => !isMoving);
        isMoving = true;
        while (rectTransform.transform.position.x < defaultX - 0.0001f || rectAttackTransform.localPosition.x > 0.0001f)
        {
            rectTransform.transform.position = Vector3.MoveTowards(rectTransform.transform.position, new Vector3(defaultX, rectTransform.transform.position.y, rectTransform.transform.position.z), 20);
            rectAttackTransform.localPosition = Vector3.MoveTowards(rectAttackTransform.localPosition, new Vector3(0, rectAttackTransform.localPosition.y, rectAttackTransform.localPosition.z), 30);
            yield return new WaitForSeconds(0.01f);
        }
        isMoving = false;
    }

    public IEnumerator displayAttack()
    {
        yield return new WaitUntil(() => !isMoving);
        if (currentAttackDisplay != null)
        {
            isMoving = true;

            jaugeDext.fillAmount = currentAttackDisplay.dextFactor + currentAttackDisplay.intFactor;
            jaugeInteligence.fillAmount = currentAttackDisplay.intFactor;

            attackName.text = currentAttackDisplay.name;
            attackDescription.text = currentAttackDisplay.description;

            while (rectTransform.transform.position.x < defaultX - 0.0001f || rectAttackTransform.localPosition.x < defaultAttackX - 0.0001f)
            {
                rectTransform.transform.position = Vector3.MoveTowards(rectTransform.transform.position, new Vector3(defaultX, rectTransform.transform.position.y, rectTransform.transform.position.z), 20);
                rectAttackTransform.localPosition = Vector3.MoveTowards(rectAttackTransform.localPosition, new Vector3(defaultAttackX, rectAttackTransform.localPosition.y, rectAttackTransform.localPosition.z), 30);
                yield return new WaitForSeconds(0.01f);
            }
            isMoving = false;
        }
    }

    public IEnumerator HideAttack()
    {
        yield return new WaitUntil(() => !isMoving);
        isMoving = true;
        currentAttackDisplay = null;
        while (rectTransform.transform.position.x < defaultX - 0.0001f || rectAttackTransform.localPosition.x > 0.0001f)
        {
            rectTransform.transform.position = Vector3.MoveTowards(rectTransform.transform.position, new Vector3(defaultX, rectTransform.transform.position.y, rectTransform.transform.position.z), 20);
            rectAttackTransform.localPosition = Vector3.MoveTowards(rectAttackTransform.localPosition, new Vector3(0, rectAttackTransform.localPosition.y, rectAttackTransform.localPosition.z), 30);
            yield return new WaitForSeconds(0.01f);
        }
        isMoving = false;
    }
}
