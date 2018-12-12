using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AnimationCreator : MonoBehaviour
{

    public Texture2D texture;
    protected Sprite[] sprite = new Sprite[273];
    protected Dictionary<string, AnimationClip> clipsDict = new Dictionary<string, AnimationClip>();

    // Use this for initialization
    void Start()
    {
        var spriteTemp = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(texture));
        Debug.Log(AssetDatabase.GetAssetPath(texture));
        int j = 0;
        for (int i = 0; i < spriteTemp.Length; i++)
        {
            if (spriteTemp[i].name.IndexOf(texture.name) != -1)
            {
                Debug.Log(spriteTemp[i].name);
                sprite[j] = (Sprite)spriteTemp[i];
                j++;
            }

        }
        Directory.CreateDirectory(Application.dataPath + "/" + name);
        createAnimation();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public int getNumber(int row, int col)
    {
        return row * 13 + col;
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
        AnimationClip transition = createClipAnimation(0, -1, 0.1f, "Transition");


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
        AssetDatabase.CreateAsset(animOveride, "Assets/" + name + "/" + name + "Controller.overrideController");


        GameObject obj = new GameObject();
        Animator animObj = obj.AddComponent<Animator>();
        obj.AddComponent<SpriteRenderer>().sprite = sprite[78];
        animObj.runtimeAnimatorController = animOveride;
        Object prefab = PrefabUtility.CreatePrefab("Assets/" + name + "Object.prefab", obj);
        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
    }






    public AnimationClip createClipAnimation(int deb = 0, int fin = 0, float refreshRate = 0.1f, string nameClip = "Unnamed")
    {
        AnimationClip clip = new AnimationClip();
        EditorCurveBinding spriteBinding = new EditorCurveBinding();
        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";
        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[fin + 1 - deb];
        //on ajoute une par une chaque image dans l'animation
        for (int i = deb; i < fin + 1; i++)
        {
            spriteKeyFrames[i - deb] = new ObjectReferenceKeyframe();
            spriteKeyFrames[i - deb].time = refreshRate * (i - deb);
            spriteKeyFrames[i - deb].value = sprite[i];
        }
        clip.frameRate = 14;
        clip.name = nameClip;
        clip.wrapMode = WrapMode.Loop;

        AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeyFrames);
        clipsDict.Add(clip.name, clip);
        AssetDatabase.CreateAsset(clip, "Assets/" + name + "/" + nameClip + ".anim");


        return clip;
    }

}
