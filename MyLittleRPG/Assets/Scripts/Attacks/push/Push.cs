using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Push : Attacks
{

    /*Objects à afficher*/
    public GameObject swordToInstantiate;        //image de l'épée, barre de progression du mini-jeu
    public GameObject canvas;       //canvas contenant le text pour le décompte


    /*Paramétres du jeu*/
    public float timeFadeInSword = 0.5f;
    public float alphaSword = 0.4f;
    public float gameDuration = 10.0f;
    public float reussiteCritiqueTime = 5.0f;
    public float barreSpeed = 0.1f;
    public float scoreDecrement = 0.024f;
    public float scoreIncrement = 0.3f;
    public float pushFactor = 0.25f;

    /*Boolean*/
    private bool endGame = false;

    /*sauvegarde des paramètres à recharger à la fin du jeu*/
    private Vector3 positionAttaquant;
    private Vector3 positionDefenseur;
    private Vector3 scaleAttaquant;
    private Vector3 scaleDefenseur;
    private Sens sensAttaquant;
    private Sens sensDefenseur;


    public override IEnumerator MiniJeu()
    {

        //on sauvegarde les parametres des personnages
        sensAttaquant = attaquantAnimationController.sens;
        sensDefenseur = defenseurAnimationController.sens;

        positionAttaquant = attaquant.transform.position;
        positionDefenseur = defenseur.transform.position;

        scaleAttaquant = attaquant.transform.localScale;
        scaleDefenseur = defenseur.transform.localScale;


        //on place les personnages
        Vector2 gamePositionAttaquant = new Vector2(mainCamera.transform.position.x - scaleVector.x * 1.5f, mainCamera.transform.position.y);
        Vector2 gamePositionDefensseur = new Vector2(mainCamera.transform.position.x + scaleVector.x * 1.5f, mainCamera.transform.position.y);

        attaquantAnimationController.lookAt(Sens.Right);
        defenseurAnimationController.lookAt(Sens.Left);
        attaquantInputController.moveToward(gamePositionAttaquant, scaleVector, timeFadeInSword);
        defenseurInputController.moveToward(gamePositionDefensseur, scaleVector, timeFadeInSword);

        //on instantiate sword
        GameObject sword = Instantiate(swordToInstantiate);
        sword.transform.position = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y);
        sword.transform.localScale = scaleVector;
        objectToDestroy.Add(sword);

        //on crée le mask qui cache la barre de progression rempli ou opaque
        GameObject maskObject = new GameObject();
        maskObject.transform.position = sword.transform.position;
        maskObject.transform.rotation = sword.transform.rotation;
        maskObject.transform.localScale = sword.transform.localScale;
        SpriteMask mask = maskObject.AddComponent<SpriteMask>();
        mask.sprite = sword.GetComponent<SpriteRenderer>().sprite;
        mask.alphaCutoff = 0.0f;

        objectToDestroy.Add(maskObject);

        //on crée la barre de progression opaque
        GameObject swordOnInput = Instantiate(sword);
        swordOnInput.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        swordOnInput.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1.0f);

        objectToDestroy.Add(swordOnInput);

        //fondu de l'épée
        float alpha = 0.0f;
        float timeBeforeFade = Time.time;
        while (alpha < alphaSword)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            alpha = alphaSword * (Time.time - timeBeforeFade) / timeFadeInSword;
            sword.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alpha);
        }
        sword.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alphaSword);

        //timer
        GameObject canvasInstantiated = Instantiate(canvas);
        Text textCompteur = canvasInstantiated.GetComponentInChildren<Text>();
        textCompteur.text = "3";
        yield return new WaitForSeconds(1);
        textCompteur.text = "2";
        yield return new WaitForSeconds(1);
        textCompteur.text = "1";
        yield return new WaitForSeconds(1);
        textCompteur.text = "GO!!!";
        yield return new WaitForSeconds(1);

        GameObject.DestroyImmediate(canvasInstantiated.gameObject);

        //game
        float timeBeforeGame = Time.time;
        float score = 0.0f;
        float speedAnimation;
        float step;
        Vector3 targetAnimation = new Vector3();
        float lengthSword = sword.GetComponent<SpriteRenderer>().sprite.rect.height / sword.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        while (Time.time - timeBeforeGame < gameDuration && score < 1)
        {
            yield return new WaitForFixedUpdate();

            //on diminu le score si il n'est pas inférieur à 0
            if (score > scoreDecrement * difficultyFactor)
            {
                score -= scoreDecrement * difficultyFactor;
            }
            else
            {
                score = 0.0f;
            }

            //si le joeur click on incrémente le score
            bool input = Input.GetMouseButtonDown(0);
            if (input)
            {
                attaquantAnimationController.sword(1.0f, 0.0f);
                score += scoreIncrement;
            }

            //on déplace la barre de propgression en fonction du score
            targetAnimation = new Vector3(sword.transform.position.x + score * lengthSword * sword.transform.localScale.x, maskObject.transform.position.y);
            speedAnimation = Vector3.Distance(maskObject.transform.position, targetAnimation) / barreSpeed;
            step = speedAnimation * Time.deltaTime;
            maskObject.transform.position = Vector3.MoveTowards(maskObject.transform.position, targetAnimation, step);
        }

        //on sauvegarde le temps restant
        float timeRemaining = gameDuration - Time.time + timeBeforeGame;

        //on amène la barre de progression à son état final
        while (maskObject.transform.position != targetAnimation)
        {
            yield return new WaitForFixedUpdate();
            targetAnimation = new Vector3(sword.transform.position.x + score * lengthSword * sword.transform.localScale.x, maskObject.transform.position.y);
            speedAnimation = Vector3.Distance(sword.transform.position, targetAnimation) / barreSpeed;
            step = speedAnimation * Time.deltaTime;
            maskObject.transform.position = Vector3.MoveTowards(maskObject.transform.position, targetAnimation, step);
        }

        //si le jeu est perdu on replace les personnages
        if (timeRemaining <= 0)
        {
            StartCoroutine(resetPosition(swordOnInput, sword));
            yield return new WaitUntil(() => endGame);
            endGameProcessing();
            yield break;
        }

        //si le jeu est gagné on lance l'animation d'attaque
        attaquantInputController.moveToward(defenseur.transform.position - new Vector3(0.3f * scaleVector.x, 0.0f, 0.0f), 0.5f);
        attaquantAnimationController.sword(1.0f, 0.0f);

        yield return new WaitUntil(() => attaquant.transform.position == defenseur.transform.position - new Vector3(0.3f * scaleVector.x, 0.0f, 0.0f)); //on attend que l'animation soit terminée


        Vector3 moveDefensseurHitted;
        //si le joueur a fini le jeu en moins de 5 secondes c'est une réussite critique
        if (timeRemaining > reussiteCritiqueTime)
        {
            StartCoroutine(resetPosition(swordOnInput, sword));
            yield return new WaitUntil(() => endGame);

            moveDefensseurHitted = (positionDefenseur - positionAttaquant).normalized;
            Vector3 scaler = new Vector3(2 * timeRemaining * pushFactor / difficultyFactor, 2 * timeRemaining * pushFactor / difficultyFactor, 1);
            moveDefensseurHitted.Scale(scaler);

            defenseur.hurted(2 * dgtsMax);
        }
        //sinon c'est la réussite normal
        else
        {
            StartCoroutine(resetPosition(swordOnInput, sword));
            yield return new WaitUntil(() => endGame);

            moveDefensseurHitted = (positionDefenseur - positionAttaquant).normalized;
            Vector3 scaler = new Vector3(timeRemaining * pushFactor / difficultyFactor, timeRemaining * pushFactor / difficultyFactor, 1);
            moveDefensseurHitted.Scale(scaler);


            defenseur.hurted((int)(dgtsMin + dgtsMax * timeRemaining / reussiteCritiqueTime));
        }

        defenseurInputController.moveToward(positionDefenseur + moveDefensseurHitted, 2.0f);
        yield return new WaitUntil(() => defenseur.transform.position == positionDefenseur + moveDefensseurHitted);
        endGameProcessing();

    }



    public IEnumerator resetPosition(GameObject swordOnInput, GameObject sword)
    {

        //on replace les personnages
        attaquantAnimationController.lookAt(sensAttaquant);
        defenseurAnimationController.lookAt(sensDefenseur);
        attaquantInputController.moveToward(positionAttaquant, scaleAttaquant, timeFadeInSword);
        defenseurInputController.moveToward(positionDefenseur, scaleDefenseur, timeFadeInSword);

        //FadeOut de lépée
        float alpha = alphaSword;
        float timeBeforeFade = Time.time;
        while (alpha > 0.0f)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            alpha = alphaSword - alphaSword * (Time.time - timeBeforeFade) / timeFadeInSword;
            sword.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alpha);
            swordOnInput.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, alpha / alphaSword);
        }
        sword.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.0f);
        swordOnInput.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.0f);

        yield return new WaitUntil(() => attaquant.transform.position == positionAttaquant);
        yield return new WaitUntil(() => defenseur.transform.position == positionDefenseur);
        endGame = true;
    }
}


