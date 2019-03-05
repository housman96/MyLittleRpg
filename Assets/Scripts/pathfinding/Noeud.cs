using System;
using UnityEngine;


public class Noeud : IComparable<Noeud>
{
    public float halfSize = 1;  //demitaille d'un noeud
    public float cout = 0;
    public float heuristique = 0;
    public Vector2 position;
    public Vector2Int previousNoeud = new Vector2Int(-1, -1);   //noeud précédent pour le Astar


    /*CONSTRUCTEURS*/

    public Noeud() { }

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

    /*CompareTo pour le rangement des noeud dans l'openList de l'Astar*/
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