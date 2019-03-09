using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterStats : MonoBehaviour
{

    public CharacterStats stats;
    public RectTransform rectTransform;
    private float defaultX;
    public Text name;
    public Text PV;
    public Text Force;
    public Text Dext;
    public Text Int;
    public Text ResForce;
    public Text ResDext;
    public Text ResInt;

    public Image PVMask;
    public Image ForceMask;
    public Image DextMask;
    public Image IntMask;
    public Image ResForceMask;
    public Image ResDextMask;
    public Image ResIntMask;

    public Image imageAttack1;
    public Image imageAttack2;
    public Image imageAttack3;
    public Image imageAttack4;

    public Sprite imageAttackDefault;


    // Use this for initialization
    void Start()
    {
        defaultX = rectTransform.transform.position.x;
        rectTransform.transform.position = new Vector3(-rectTransform.transform.position.x, rectTransform.transform.position.y, rectTransform.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        if (stats == null)
            return;
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
            imageAttack1.sprite = stats.attack1.imageAttack;
        else
            imageAttack1.sprite = imageAttackDefault;

        if (stats.attack2 != null)
            imageAttack2.sprite = stats.attack2.imageAttack;
        else
            imageAttack2.sprite = imageAttackDefault;

        if (stats.attack3 != null)
            imageAttack3.sprite = stats.attack3.imageAttack;
        else
            imageAttack3.sprite = imageAttackDefault;

        if (stats.attack4 != null)
            imageAttack4.sprite = stats.attack4.imageAttack;
        else
            imageAttack4.sprite = imageAttackDefault;
    }

    public void setStats(CharacterStats stats)
    {
        this.stats = stats;
        StartCoroutine("displayStats");
    }

    public void onButtonClickHideStats()
    {
        StartCoroutine("hideStats");
    }

    public IEnumerator hideStats()
    {
        while (rectTransform.transform.position.x > -defaultX)
        {
            rectTransform.transform.position = Vector3.MoveTowards(rectTransform.transform.position, new Vector3(-defaultX, rectTransform.transform.position.y, rectTransform.transform.position.z), 10);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator displayStats()
    {
        while (rectTransform.transform.position.x < defaultX)
        {
            rectTransform.transform.position = Vector3.MoveTowards(rectTransform.transform.position, new Vector3(defaultX, rectTransform.transform.position.y, rectTransform.transform.position.z), 10);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
