using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode {

    public bool isWall;
    public int posX;
    public int posY;
    public int posZ;

    public Vector3 pos;
    public AStarNode parentNode;

    public int costG;
    public int costH;

    public int CostF{
        get{ return costG + costH; }
    }

    public AStarNode(bool _isWall,Vector3 _pos,int _z,int _x){
        this.isWall = _isWall;
        this.pos = _pos;
        this.posX = _x;
        this.posZ = _z;
    }
}
