using UnityEngine;

public class LayerOrderer : MonoBehaviour
{

    const int numberOfIndex = 10;    //nombre de layer dans un personnage
    private int[] indexOrder;        //index du layerOrderer
    SpriteRenderer[] renderers;      //les spriteRenderers

    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        indexOrder = new int[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            indexOrder[i] = renderers[i].sortingOrder;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1 - numberOfIndex + indexOrder[i];
        }
    }
}
