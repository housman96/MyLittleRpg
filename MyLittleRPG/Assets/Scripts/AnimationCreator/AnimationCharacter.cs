using UnityEngine;

public class AnimationCharacter : AnimationCreator
{
    public bool isMale;


    public override void getTextures(string nameTexture)
    {
        Texture2D textureImport;

        if (isMale)
        {
            textureImport = Resources.Load<Texture2D>("CharactersTextures/body/male/" + nameTexture);
        }
        else
        {
            textureImport = Resources.Load<Texture2D>("CharactersTextures/body/female/" + nameTexture);
        }
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
