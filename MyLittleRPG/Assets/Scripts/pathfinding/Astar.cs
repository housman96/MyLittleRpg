using System;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{

    //Fonction cheminPlusCourt(g:Graphe, objectif:Nœud, depart:Nœud)






    private List<Noeud> closedList = new List<Noeud>();
    private List<Noeud> openList = new List<Noeud>();
    public Graphe graphe = new Graphe(new Vector2(0, 0), 1, 1, 0.1f);
    public GameObject target;
    public GameObject start;


    // Use this for initialization
    void Start()
    {
        graphe = new Graphe(new Vector2(0, 0), 5, 5, 0.1f);
        foreach (Noeud node in graphe.tabNoeud)
        {
            ////haut
            //Debug.DrawLine(new Vector3(node.position.x - node.halfSize, node.position.y + node.halfSize), new Vector3(node.position.x + node.halfSize, node.position.y + node.halfSize), Color.white, 100);
            ////bas
            //Debug.DrawLine(new Vector3(node.position.x - node.halfSize, node.position.y - node.halfSize), new Vector3(node.position.x + node.halfSize, node.position.y - node.halfSize), Color.white, 100);
            ////gauche
            //Debug.DrawLine(new Vector3(node.position.x - node.halfSize, node.position.y + node.halfSize), new Vector3(node.position.x - node.halfSize, node.position.y - node.halfSize), Color.white, 100);
            ////droite
            //Debug.DrawLine(new Vector3(node.position.x + node.halfSize, node.position.y + node.halfSize), new Vector3(node.position.x + node.halfSize, node.position.y - node.halfSize), Color.white, 100);
        }

        Debug.Log(nearestWay());
    }



    public int nearestWay()
    {
        //   closedList = File()
        //   openList = FilePrioritaire(comparateur= compare2Noeuds)

        Noeud startNoeud = graphe.getNoeudAtPos(start.transform.position);
        Noeud targetNoeud = graphe.getNoeudAtPos(target.transform.position);

        if (startNoeud == null || targetNoeud == null)
        {
            return 0;
        }


        //   openList.ajouter(depart)

        startNoeud.heuristique = 0;
        startNoeud.cout = 0;
        openList.Add(startNoeud);

        int security = 500;
        int k = 0;
        //   tant que openList n'est pas vide
        while (openList.Count != 0 && k < security)
        {
            k++;


            //       u = openList.depiler()
            Noeud u = openList[0];
            openList.Remove(u);

            //       si u.x == objectif.x et u.y == objectif.y
            if (u.position == targetNoeud.position)
            {
                //           reconstituerChemin(u)
                //           terminer le programme
                buildPath(targetNoeud);
                return 1;
            }


            //       pour chaque voisin v de u dans g
            Noeud v;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    //Vector2Int index = u.positionInGraphe;
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
                        if (newCout < v.cout && Physics2D.OverlapBox(v.position, new Vector2(v.halfSize * 2, v.halfSize * 2), 0) == null)
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
        Debug.Log("k= " + k);
        //   terminer le programme(avec erreur)
        return -1;
    }

    public Stack<Noeud> buildPath(Noeud target)
    {
        Stack<Noeud> res = new Stack<Noeud>();
        Noeud u = target;
        int limite = 10000;
        int i = 0;
        while (u.previousNoeud.x >= 0 && u.previousNoeud.y >= 0 && limite > i)
        {
            res.Push(u);
            u = graphe[u.previousNoeud.x, u.previousNoeud.y];
            i++;
        }
        res.Push(u);

        if (i >= limite)
        {
            return null;
        }

        return res;
    }

}




public class Noeud : IComparable<Noeud>
{
    public float halfSize = 1;

    public Vector2 position;
    public float cout = 0;
    public float heuristique = 0;

    public Vector2Int previousNoeud = new Vector2Int(-1, -1);


    /*CONSTRUCTEURS*/

    public Noeud()
    {

    }

    public Noeud(float x, float y)
    {
        position.x = x;
        position.y = y;

    }

    public Noeud(Noeud noeud)
    {
        halfSize = noeud.halfSize;
        position = noeud.position;
        cout = noeud.cout;
        heuristique = noeud.heuristique;
        previousNoeud = noeud.previousNoeud;
    }

    public Noeud(Vector2 position)
    {
        this.position = position;
    }

    public Noeud(float x, float y, float halfSize)
    {
        position.x = x;
        position.y = y;
        this.halfSize = halfSize;
    }

    public Noeud(Vector2 newPosition, float halfSize)
    {
        position = newPosition;
        this.halfSize = halfSize;
    }

    public Noeud(float x, float y, float halfSize, float cout, float heuristique)
    {
        position.x = x;
        position.y = y;
        this.halfSize = halfSize;
        this.cout = cout;
        this.heuristique = heuristique;
    }

    public Noeud(Vector2 newPosition, float halfSize, float cout, float heuristique)
    {
        position = newPosition;
        this.halfSize = halfSize;
        this.cout = cout;
        this.heuristique = heuristique;
    }

    /*Check si la position passée est dans le noeud*/
    public bool isInNoeud(float x, float y)
    {
        if (x < position.x + halfSize && x > position.x - halfSize && y < position.y + halfSize && y > position.y - halfSize)
        {
            return true;
        }
        return false;
    }

    public int CompareTo(Noeud other)
    {
        int res = 0;

        if (heuristique < other.heuristique)
        {
            res = -1;
        }
        else
        {
            if (heuristique > other.heuristique)
            {
                res = 1;
            }
        }

        return res;
    }
}

public class Graphe
{
    public int rows = 1;
    public int cols = 1;
    public Vector2 center = new Vector2(0, 0);
    public float sizeNoeud = 1;
    public Noeud[,] tabNoeud = new Noeud[1, 1];

    public Noeud this[int i, int j]
    {
        get { return tabNoeud[i, j]; }
        set { tabNoeud[i, j] = value; }
    }

    public Graphe(Vector2 newCenter, float sizeX, float sizeY, float newSizeNoeud)
    {
        center = newCenter;
        rows = Mathf.RoundToInt(sizeX / newSizeNoeud);
        cols = Mathf.RoundToInt(sizeY / newSizeNoeud);
        sizeNoeud = newSizeNoeud;

        Init();
    }


    public void Init()
    {
        tabNoeud = new Noeud[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector2 positionNoeud = new Vector2(center.x - ((rows / 2) - i) * sizeNoeud, center.y - ((cols / 2) - j) * sizeNoeud);
                tabNoeud[i, j] = new Noeud(positionNoeud, sizeNoeud / 2, float.MaxValue, float.MaxValue);
            }
        }
    }

    public bool isInGraphe(Vector2 pos)
    {
        bool res = false;
        float rightBound = center.x + (rows * sizeNoeud) / 2;
        float leftBound = center.x - (rows * sizeNoeud) / 2;
        float hautBound = center.y + (cols * sizeNoeud) / 2;
        float basBound = center.y - (cols * sizeNoeud) / 2;

        if (pos.x < rightBound && pos.x > leftBound && pos.y > basBound && pos.y < hautBound)
        {
            res = true;
        }
        return res;
    }

    public Noeud getNoeudAtPos(Vector2 pos)
    {
        Noeud res = null;

        if (isInGraphe(pos))
        {
            int i = Mathf.RoundToInt((rows / 2) - ((center.x - pos.x) / sizeNoeud));
            int j = Mathf.RoundToInt((cols / 2) - ((center.y - pos.y) / sizeNoeud));
            if (i < rows && j < cols)
            {
                res = tabNoeud[i, j];
            }
        }
        return res;
    }

    public Vector2Int getIndexAtPos(Vector2 pos)
    {
        Vector2Int res = new Vector2Int(-1, -1);
        if (isInGraphe(pos))
        {
            int i = Mathf.RoundToInt((rows / 2) - ((center.x - pos.x) / sizeNoeud));
            int j = Mathf.RoundToInt((cols / 2) - ((center.y - pos.y) / sizeNoeud));
            if (i < rows && j < cols)
            {
                res = new Vector2Int(i, j);
            }
        }
        return res;
    }

}