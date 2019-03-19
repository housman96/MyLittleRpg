using UnityEngine;

public class playerSound : MonoBehaviour
{

    public AudioSource source;

    public AudioClip[] stepClip;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void stepSound()
    {
        if (stepClip.Length > 0)
        {
            int clipIndex = Random.Range(0, stepClip.Length);
            while (source.clip == stepClip[clipIndex])
            {
                clipIndex = Random.Range(0, stepClip.Length);
            }
            source.clip = stepClip[clipIndex];
            source.Play();
        }
    }
}
