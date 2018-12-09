using System.Collections.Generic;
using UnityEngine;

public class AnimationCharacter : MonoBehaviour
{
    public string bodyType;
    private Sprite[] sprite = new Sprite[178];
    private Dictionary<string, AnimationClip> clipsDict = new Dictionary<string, AnimationClip>();

    // Use this for initialization
    void Start()
    {
        getTextures(bodyType);
        createAnimation();
    }


    public void getTextures(string nameTexture)
    {
        for (int i = 0; i < 178; i++)
        {
            var texture = Resources.LoadAll<Sprite>("CharactersTextures/" + nameTexture);
            sprite[i] = texture[i];
        }
    }


    public void createAnimation()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorOverrideController animOveride;

        //on crée tous les clips nécessaire
        AnimationClip Dead = createClipAnimation(172, 177, 0.1f, "Dead");
        AnimationClip IdleRight = createClipAnimation(87, 87, 0.1f, "IdleRight");
        AnimationClip walkRight = createClipAnimation(88, 95, 0.1f, "walkRight");
        AnimationClip IdleLeft = createClipAnimation(69, 69, 0.1f, "IdleLeft");
        AnimationClip walkLeft = createClipAnimation(70, 77, 0.1f, "walkLeft");
        AnimationClip IdleUp = createClipAnimation(60, 60, 0.1f, "IdleUp");
        AnimationClip walkUp = createClipAnimation(61, 68, 0.1f, "walkUp");
        AnimationClip IdleDown = createClipAnimation(78, 78, 0.1f, "IdleDown");
        AnimationClip walkDown = createClipAnimation(79, 86, 0.1f, "walkDown");
        AnimationClip transition = createClipAnimation(0, -1, 0.1f, "Transition");
        AnimationClip handsUpDown = createClipAnimation(15, 20, 0.1f, "handsUpDown");
        AnimationClip handsUpUp = createClipAnimation(1, 6, 0.1f, "handsUpUp");
        AnimationClip handsUpLeft = createClipAnimation(8, 13, 0.1f, "handsUpLeft");
        AnimationClip handsUpRight = createClipAnimation(22, 27, 0.1f, "handsUpRight");
        AnimationClip spearDown = createClipAnimation(45, 51, 0.1f, "spearDown");
        AnimationClip spearUp = createClipAnimation(29, 35, 0.1f, "spearUp");
        AnimationClip spearLeft = createClipAnimation(37, 43, 0.1f, "spearLeft");
        AnimationClip spearRight = createClipAnimation(53, 59, 0.1f, "spearRight");
        AnimationClip swordDown = createClipAnimation(109, 113, 0.1f, "swordDown");
        AnimationClip swordUp = createClipAnimation(97, 101, 0.1f, "swordUp");
        AnimationClip swordLeft = createClipAnimation(103, 107, 0.1f, "swordLeft");
        AnimationClip swordRight = createClipAnimation(115, 119, 0.1f, "swordRight");
        AnimationClip bowDown = createClipAnimation(147, 158, 0.1f, "bowDown");
        AnimationClip bowUp = createClipAnimation(121, 132, 0.1f, "bowUp");
        AnimationClip bowLeft = createClipAnimation(134, 145, 0.1f, "bowLeft");
        AnimationClip bowRight = createClipAnimation(160, 171, 0.1f, "bowRight");

        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        animOveride = new AnimatorOverrideController(anim.runtimeAnimatorController);

        //on modifie toutes les animations
        //WARNING: SI UNE SEULE ANIMATION N'EST PAS MODIFIE L'ANIMATOR VA BUGER ET LES ANIMATIONS MODIFIE NE VONT PAS SE JOUER
        foreach (var a in animOveride.animationClips)
        {
            Debug.Log(a.name);
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, clipsDict[a.name]));
        }

        //On applique les changements d'animation et on les ranges dans l'animator
        animOveride.ApplyOverrides(anims);
        anim.runtimeAnimatorController = animOveride;
        anim.runtimeAnimatorController.name = "newAnimation";

    }






    public AnimationClip createClipAnimation(int deb = 0, int fin = 0, float refreshRate = 0.1f, string nameClip = "Unnamed")
    {
        AnimationClip clip = new AnimationClip();
        //on ajoute une par une chaque image dans l'animation
        for (int i = deb; i < fin + 1; i++)
        {
            AnimationEvent eventAnim = new AnimationEvent();
            eventAnim.intParameter = i;
            eventAnim.time = refreshRate * (i - deb);
            eventAnim.functionName = "setFrame";
            clip.AddEvent(eventAnim);
            clip.frameRate = 14;
            clip.name = nameClip;
            clip.wrapMode = WrapMode.Loop;
        }
        clipsDict.Add(clip.name, clip);

        return clip;
    }

    public void setFrame(int i = 0)
    {
        GetComponent<SpriteRenderer>().sprite = sprite[i];
    }
}
