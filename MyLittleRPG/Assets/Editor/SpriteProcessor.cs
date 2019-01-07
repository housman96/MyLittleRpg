﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SpriteProcessor : AssetPostprocessor
{

    void OnPreprocessTexture()
    {
        //on set les paramètres du textureImporter
        if (assetPath.IndexOf("/TextureCharacter/") == -1)
            return;
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.textureType = TextureImporterType.Sprite;
        textureImporter.spriteImportMode = SpriteImportMode.Multiple;
        textureImporter.mipmapEnabled = false;
        textureImporter.filterMode = FilterMode.Point;

    }

    public void OnPostprocessTexture(Texture2D texture)
    {
        //on ne découpe que les textures qui sont dans le bon dossier
        if (assetPath.IndexOf("/TextureCharacter/") == -1)
            return;


        //on découpe la texture uniquement si elle n'est pas découpée 
        TextureImporter textureImporter = (TextureImporter)assetImporter;
        if (textureImporter.spritesheet.Length == 0)
        {
            int spriteSize = 64;
            int colCount = texture.width / spriteSize;
            int rowCount = texture.height / spriteSize;
            string nameTexture = Path.GetFileNameWithoutExtension(assetPath);
            List<SpriteMetaData> metas = new List<SpriteMetaData>();

            for (int r = 0; r < rowCount; ++r)
            {
                for (int c = 0; c < colCount; ++c)
                {
                    SpriteMetaData meta = new SpriteMetaData();
                    meta.rect = new Rect(c * spriteSize, r * spriteSize, spriteSize, spriteSize);
                    meta.name = nameTexture + "_" + ((rowCount - 1 - r) * colCount + c);
                    metas.Add(meta);
                }
            }
            textureImporter.spritesheet = metas.ToArray();
            AssetDatabase.Refresh();
        }
    }

    public void OnPostprocessSprites(Texture2D texture, Sprite[] sprite)
    {
        //on importe les sprite uniquement si on a déjà refersh
        if (sprite.Length == 0)
        {
            return;
        }

        //si les sprites n'ont pas encore été sauvegardé on les saves
        var spriteTemp = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
        if (spriteTemp.Length == 0)
        {
            assetImporter.SaveAndReimport();
        }
        else
        {
            //si la texture à la bonne taille
            if (spriteTemp.Length == 273)
            {
                //on transforme tous les objets en sprites

                Sprite[] sprites = new Sprite[273];
                for (int i = 0; i < spriteTemp.Length; i++)
                {
                    sprites[i] = (Sprite)spriteTemp[i];
                }

                //on crée tous les path utiles à la création des sprites

                string nameTexture = Path.GetFileNameWithoutExtension(assetPath);
                string pathFolderAnimation = (assetPath.Remove(assetPath.Length - Path.GetExtension(assetPath).Length) + "/").Insert("Assets".Length, "/Animation");
                string pathGameObject = assetPath.Remove(assetPath.Length - Path.GetFileName(assetPath).Length).Insert("Assets".Length, "/Prefab");
                string RelativePathFolderGameObject = Application.dataPath.Remove(Application.dataPath.Length - "Assets".Length) + pathFolderAnimation;
                string RelativePathFolderAnimation = Application.dataPath.Remove(Application.dataPath.Length - "Assets".Length) + pathGameObject;

                //on crée les folders où on va ranger les animations et les gameObjects
                Directory.CreateDirectory(RelativePathFolderGameObject);
                Directory.CreateDirectory(RelativePathFolderAnimation);

                createAnimation(sprites, pathFolderAnimation, pathGameObject, nameTexture);

                //on save les changements
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
    }


    public void createAnimation(Sprite[] sprite, string path, string pathGameObject, string name)
    {
        AnimatorOverrideController animOveride = new AnimatorOverrideController(AssetDatabase.LoadAssetAtPath<AnimatorOverrideController>("Assets/Animation/Character/overrideCharacterAnimator.overrideController"));
        Dictionary<string, AnimationClip> clipsDict = new Dictionary<string, AnimationClip>();

        //on crée tous les clips nécessaire
        //handsup
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(0, 1), getNumber(0, 6), 0.1f, "handsUpUp", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(1, 1), getNumber(1, 6), 0.1f, "handsUpLeft", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(2, 1), getNumber(2, 6), 0.1f, "handsUpDown", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(3, 1), getNumber(3, 6), 0.1f, "handsUpRight", path);

        //spear
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(4, 1), getNumber(4, 7), 0.1f, "spearUp", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(5, 1), getNumber(5, 7), 0.1f, "spearLeft", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(6, 1), getNumber(6, 7), 0.1f, "spearDown", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(7, 1), getNumber(7, 7), 0.1f, "spearRight", path);

        //walk
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(8, 1), getNumber(8, 8), 0.1f, "walkUp", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(9, 1), getNumber(9, 8), 0.1f, "walkLeft", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(10, 1), getNumber(10, 8), 0.1f, "walkDown", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(11, 1), getNumber(11, 8), 0.1f, "walkRight", path);

        //idle
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(8, 0), getNumber(8, 0), 0.1f, "IdleUp", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(9, 0), getNumber(9, 0), 0.1f, "IdleLeft", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(10, 0), getNumber(10, 0), 0.1f, "IdleDown", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(11, 0), getNumber(11, 0), 0.1f, "IdleRight", path);

        //sword
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(12, 1), getNumber(12, 5), 0.1f, "swordUp", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(13, 1), getNumber(13, 5), 0.1f, "swordLeft", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(14, 1), getNumber(14, 5), 0.1f, "swordDown", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(15, 1), getNumber(15, 5), 0.1f, "swordRight", path);

        //bow
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(16, 1), getNumber(16, 12), 0.1f, "bowUp", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(17, 1), getNumber(17, 12), 0.1f, "bowLeft", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(18, 1), getNumber(18, 12), 0.1f, "bowDown", path);
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(19, 1), getNumber(19, 12), 0.1f, "bowRight", path);

        //Dead
        clipsDict = createClipAnimation(clipsDict, sprite, getNumber(20, 1), getNumber(20, 5), 0.1f, "Dead", path);

        //Transition
        clipsDict = createClipAnimation(clipsDict, sprite, 0, -1, 0.1f, "Transition", path);


        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();

        //on modifie toutes les animations
        //WARNING: SI UNE SEULE ANIMATION N'EST PAS MODIFIE L'ANIMATOR VA BUGER ET LES ANIMATIONS MODIFIE NE VONT PAS SE JOUER
        foreach (var a in animOveride.animationClips)
        {
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, clipsDict[a.name]));
        }

        //On applique les changements d'animation et on les ranges dans l'animator
        animOveride.ApplyOverrides(anims);
        animOveride.runtimeAnimatorController.name = "newAnimation";
        AssetDatabase.CreateAsset(animOveride, path + name + "Controller.overrideController");




        //on crée le gameObject de la texture
        GameObject obj = new GameObject();
        Animator animObj = obj.AddComponent<Animator>();
        obj.AddComponent<SpriteRenderer>().sprite = sprite[78];
        animObj.runtimeAnimatorController = animOveride;

        //on save la texture du GameObject
        Object prefab = PrefabUtility.CreatePrefab(pathGameObject + name + "Object.prefab", obj);
        PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);

        //on détruit l'object créé
        Object.DestroyImmediate(obj);
    }

    public Dictionary<string, AnimationClip> createClipAnimation(Dictionary<string, AnimationClip> clipsDict, Sprite[] sprite, int deb = 0, int fin = 0, float refreshRate = 0.1f, string nameClip = "Unnamed", string path = "")
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
        AssetDatabase.CreateAsset(clip, path + nameClip + ".anim");

        return clipsDict;
    }

    public int getNumber(int row, int col)
    {
        return row * 13 + col;
    }

}
