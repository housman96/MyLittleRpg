using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationCreator : MonoBehaviour
{

    public string textureName;
    protected Sprite[] sprite = new Sprite[273];
    protected Dictionary<string, AnimationClip> clipsDict = new Dictionary<string, AnimationClip>();

    // Use this for initialization
    void Start()
    {
        getTextures(textureName);
        setFrame(getNumber(6, 0));
        createAnimation();
    }


    public abstract void getTextures(string nameTexture);

    public int getNumber(int row, int col)
    {
        return (20 - row) * 13 + col;
    }


    public void createAnimation()
    {
        Animator anim = GetComponent<Animator>();
        AnimatorOverrideController animOveride;

        //on crée tous les clips nécessaire
        //handsup
        AnimationClip handsUpUp = createClipAnimation(getNumber(0, 1), getNumber(0, 6), 0.1f, "handsUpUp");
        AnimationClip handsUpLeft = createClipAnimation(getNumber(1, 1), getNumber(1, 6), 0.1f, "handsUpLeft");
        AnimationClip handsUpDown = createClipAnimation(getNumber(2, 1), getNumber(2, 6), 0.1f, "handsUpDown");
        AnimationClip handsUpRight = createClipAnimation(getNumber(3, 1), getNumber(3, 6), 0.1f, "handsUpRight");

        //spear
        AnimationClip spearUp = createClipAnimation(getNumber(4, 1), getNumber(4, 7), 0.1f, "spearUp");
        AnimationClip spearLeft = createClipAnimation(getNumber(5, 1), getNumber(5, 7), 0.1f, "spearLeft");
        AnimationClip spearDown = createClipAnimation(getNumber(6, 1), getNumber(6, 7), 0.1f, "spearDown");
        AnimationClip spearRight = createClipAnimation(getNumber(7, 1), getNumber(7, 7), 0.1f, "spearRight");

        //walk
        AnimationClip walkUp = createClipAnimation(getNumber(8, 1), getNumber(8, 8), 0.1f, "walkUp");
        AnimationClip walkLeft = createClipAnimation(getNumber(9, 1), getNumber(9, 8), 0.1f, "walkLeft");
        AnimationClip walkDown = createClipAnimation(getNumber(10, 1), getNumber(10, 8), 0.1f, "walkDown");
        AnimationClip walkRight = createClipAnimation(getNumber(11, 1), getNumber(11, 8), 0.1f, "walkRight");

        //idle
        AnimationClip IdleUp = createClipAnimation(getNumber(8, 0), getNumber(8, 0), 0.1f, "IdleUp");
        AnimationClip IdleLeft = createClipAnimation(getNumber(9, 0), getNumber(9, 0), 0.1f, "IdleLeft");
        AnimationClip IdleDown = createClipAnimation(getNumber(10, 0), getNumber(10, 0), 0.1f, "IdleDown");
        AnimationClip IdleRight = createClipAnimation(getNumber(11, 0), getNumber(11, 0), 0.1f, "IdleRight");

        //sword
        AnimationClip swordDown = createClipAnimation(getNumber(12, 1), getNumber(12, 5), 0.1f, "swordDown");
        AnimationClip swordUp = createClipAnimation(getNumber(13, 1), getNumber(13, 5), 0.1f, "swordUp");
        AnimationClip swordLeft = createClipAnimation(getNumber(14, 1), getNumber(14, 5), 0.1f, "swordLeft");
        AnimationClip swordRight = createClipAnimation(getNumber(15, 1), getNumber(15, 5), 0.1f, "swordRight");

        //bow
        AnimationClip bowDown = createClipAnimation(getNumber(16, 1), getNumber(16, 12), 0.1f, "bowDown");
        AnimationClip bowUp = createClipAnimation(getNumber(17, 1), getNumber(17, 12), 0.1f, "bowUp");
        AnimationClip bowLeft = createClipAnimation(getNumber(18, 1), getNumber(18, 12), 0.1f, "bowLeft");
        AnimationClip bowRight = createClipAnimation(getNumber(19, 1), getNumber(19, 12), 0.1f, "bowRight");

        //Dead
        AnimationClip Dead = createClipAnimation(getNumber(20, 1), getNumber(20, 5), 0.1f, "Dead");

        //Transition
        AnimationClip transition = createClipAnimation(-1, -1, 0.1f, "Transition");


        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        animOveride = new AnimatorOverrideController(anim.runtimeAnimatorController);

        //on modifie toutes les animations
        //WARNING: SI UNE SEULE ANIMATION N'EST PAS MODIFIE L'ANIMATOR VA BUGER ET LES ANIMATIONS MODIFIE NE VONT PAS SE JOUER
        foreach (var a in animOveride.animationClips)
        {
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
        }

        clip.frameRate = 14;
        clip.name = nameClip;
        clip.wrapMode = WrapMode.Loop;
        clipsDict.Add(clip.name, clip);

        return clip;
    }

    public void setFrame(int i = 0)
    {
        GetComponent<SpriteRenderer>().sprite = sprite[i];
    }
}
