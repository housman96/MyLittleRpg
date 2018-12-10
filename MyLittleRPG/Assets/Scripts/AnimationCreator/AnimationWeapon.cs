using UnityEngine;

public class AnimationWeapon : AnimationCreator
{

    public bool isRightHand;
    public bool isLeftHand;
    public bool isMale;
    public bool isFemale;


    // Use this for initialization
    void Start()
    {
        getTextures(textureName);
        setFrame(0);
        createAnimation();
    }


    public override void getTextures(string nameTexture)
    {
        Texture2D textureImport;
        string path = "CharactersTextures/weapons/";
        if (isRightHand && isLeftHand)
        {
            path = path + "both hand/";
        }
        else
        {
            if (isRightHand)
            {
                path = path + "right hand/";
            }
            else
            {
                path = path + "left hand/";
            }
        }
        if (isMale && isFemale)
        {
            path = path + "either/";
        }
        else
        {
            if (isMale)
            {
                path = path + "male/";
            }
            else
            {
                path = path + "female/";
            }
        }
        textureImport = Resources.Load<Texture2D>(path + nameTexture);
        textureImport.filterMode = FilterMode.Point;
        for (int i = 0; i < 21; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                sprite[i * 13 + j] = Sprite.Create(textureImport, new Rect(j * 64, i * 64, 64, 64), new Vector3(0.5f, 0.5f));
            }
        }
    }

}
