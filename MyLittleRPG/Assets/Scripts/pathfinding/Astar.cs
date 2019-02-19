using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{

    private List<Noeud> closedList = new List<Noeud>();
    private List<Noeud> openList = new List<Noeud>();


    // Use this for initialization
    void Start()
    {
        Graphe graphe = new Graphe(new Vector2(0, 0), 5, 5, 0.1f);
    }



    public Stack<Noeud> nearestWay(Graphe graphe, GameObject start, GameObject target)
    {

        Noeud startNoeud = graphe.getNoeudAtPos(start.transform.position);
        Noeud targetNoeud = graphe.getNoeudAtPos(target.transform.position);

        //on vérifi que les noeud sont bien dans le graphe
        if (startNoeud == null || targetNoeud == null)
        {
            return null;
        }


        //   on initialise le graphe et l'openlist
        startNoeud.heuristique = 0;
        startNoeud.cout = 0;
        openList.Add(startNoeud);

        int security = 5000;
        int k = 0;

        //   tant que openList n'est pas vide
        while (openList.Count != 0 && k < security)
        {
            k++;

            //  on prend le noeud sur le dessus de l'openList
            Noeud u = openList[0];
            openList.Remove(u);

            //  si u.x == objectif.x et u.y == objectif.y
            Collider2D uCollider = Physics2D.OverlapBox(u.position + start.GetComponent<BoxCollider2D>().offset, start.GetComponent<BoxCollider2D>().size, 0);
            if (uCollider != null && uCollider.gameObject == target)
            {
                //on reconstruit le chemin
                return buildPath(graphe, u);
            }


            //  pour chaque voisin v de u dans g
            Noeud v;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector2Int index = graphe.getIndexAtPos(u.position);
                    if (index.x > 0 && index.y > 0 && (i != 0 || j != 0))
                    {

                        v = graphe[index.x + i, index.y + j];

                        float newCout = u.cout + (u.position - v.position).magnitude;

                        //            si v existe dans closedList avec un cout inférieur ou si v existe dans openList avec un cout inférieur
                        //                neRienFaire()
                        //           sinon
                        //                v.cout = u.cout +1 
                        //                v.heuristique = v.cout + distance([v.x, v.y], [objectif.x, objectif.y])
                        //                openList.ajouter(v)
                        Collider2D vCollisioner = Physics2D.OverlapBox(v.position + start.GetComponent<BoxCollider2D>().offset, start.GetComponent<BoxCollider2D>().size, 0);
                        if (newCout < v.cout && (vCollisioner == null || vCollisioner.gameObject == start || vCollisioner.gameObject == target))
                        {

                            if (openList.Contains(v))
                            {
                                openList.Remove(v);
                            }
                            if (closedList.Contains(v))
                            {
                                closedList.Remove(v);
                            }

                            v.cout = newCout;
                            v.heuristique = v.cout + (v.position - targetNoeud.position).magnitude;
                            v.previousNoeud = index;
                            openList.Add(v);

                        }

                    }
                }
            }
            openList.Sort();
            //       closedList.ajouter(u)
            closedList.Add(u);
        }
        //   terminer le programme(avec erreur)
        return null;
    }

    //renvoi la liste des noeud qui amène à target
    public Stack<Noeud> buildPath(Graphe graphe, Noeud target)
    {
        Stack<Noeud> res = new Stack<Noeud>();
        Noeud u = graphe[target.previousNoeud.x, target.previousNoeud.y];
        int limite = 10000;
        int i = 0;
        while (u.previousNoeud.x >= 0 && u.previousNoeud.y >= 0 && limite > i)
        {
            res.Push(u);
            u = graphe[u.previousNoeud.x, u.previousNoeud.y];
            i++;
        }

        if (i >= limite)
        {
            return null;
        }

        return res;
    }

}



