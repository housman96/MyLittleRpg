using UnityEngine;

public class playerSound : MonoBehaviour
{

    public AudioSource source;

    public AudioClip[] stepClip;

    public AudioClip[] swordClip;

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
            source.Stop();
            int clipIndex = Random.Range(0, stepClip.Length);
            while (source.clip == stepClip[clipIndex])
            {
                clipIndex = Random.Range(0, stepClip.Length);
            }
            source.clip = stepClip[clipIndex];
            source.Play();
        }
    }

    public void swordSound()
    {
        if (swordClip.Length > 0)
        {
            source.Stop();
            int clipIndex = Random.Range(0, swordClip.Length);
            while (source.clip == swordClip[clipIndex])
            {
                clipIndex = Random.Range(0, swordClip.Length);
            }
            source.clip = swordClip[clipIndex];
            source.Play();
        }
    }
}
