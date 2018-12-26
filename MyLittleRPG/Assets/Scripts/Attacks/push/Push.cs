using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "push", menuName = "PushAttack")]



public class Push : Attacks
{
    private GameObject swordInstantiated = null;
    public GameObject sword;

    public override void launchAttack(CharacterStats ourChar, CharacterStats otherChar)
    {
        float distance = Vector3.Distance(ourChar.transform.position, otherChar.transform.position);
        //on verifi qu'on est à la bonne distance pour attaquer
        if (distance < distanceMin)
        {
            //on bloque tout ce que controle le joueur
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.GetComponent<CameraFollowing>().enabled = false;
            ourChar.GetComponent<CharacterController>().LockMoves();

            //on place les personnages
            ourChar.GetComponent<CharacterController>().lookAt(Sens.Right);
            otherChar.GetComponent<CharacterController>().lookAt(Sens.Left);
            ourChar.GetComponent<CharacterController>().moveToward(new Vector2(camera.transform.position.x - 1f, camera.transform.position.y));
            otherChar.GetComponent<CharacterController>().moveToward(new Vector2(camera.transform.position.x + 1f, camera.transform.position.y));

            //mini-jeu

            swordInstantiated = Instantiate(sword);
            swordInstantiated.transform.position = new Vector2(camera.transform.position.x, camera.transform.position.y);

            StartCoroutine("MiniJeu");

            //on replace les personnages
            //on réactive les personnages

            Debug.Log("launchAttack  distance=" + distance);
        }

    }


    public IEnumerator MiniJeu()
    {
        float alpha = 0.0f;
        while (alpha < 1)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            alpha += 0.02f;
            swordInstantiated.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, alpha);
        }
        swordInstantiated.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1.0f);
    }

    public override void reussiteCritique(CharacterStats char1, CharacterStats char2)
    {
        Debug.Log("reussiteCritique");
    }
}
