using UnityEngine;


public class Graphe
{
    public int rows = 1;
    public int cols = 1;
    public Vector2 center = new Vector2(0, 0);
    public float sizeNoeud = 1;
    public Noeud[,] tabNoeud = new Noeud[1, 1];

    //overload []
    public Noeud this[int i, int j]
    {
        get { return tabNoeud[i, j]; }
        set { tabNoeud[i, j] = value; }
    }

    /*CONSTRUCTEURS*/
    public Graphe(Vector2 newCenter, float sizeX, float sizeY, float newSizeNoeud)
    {
        center = newCenter;
        rows = Mathf.RoundToInt(sizeX / newSizeNoeud);
        cols = Mathf.RoundToInt(sizeY / newSizeNoeud);
        sizeNoeud = newSizeNoeud;

        Init();
    }

    /*Initialisation du graphe*/
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

    //check si la postion pos est dans le graphe
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

    //renvoi le noeud dans lequel pos est 
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

    //renvoi l'index dans le graphe du noeud à la position pos
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
