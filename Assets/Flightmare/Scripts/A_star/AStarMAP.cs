using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarMAP : MonoBehaviour {

    public AStarNode [,] AllNodeGroup; 

    public LayerMask wallLayer;
    public int xWidth;
    public int zHeight;
    private float nodeRange = 0.4f;

    public GameObject nodeWallPrefabs;

    void Awake(){
        Init();
    }

    void Init(){

        zHeight = 40;
        xWidth = 40;
        AllNodeGroup = new AStarNode[zHeight, xWidth];
        for (int i = 0; i < zHeight; i++) {
            for (int j = 0; j < xWidth; j++) {

                Vector3 nodePos =new Vector3 (j,0,i);
                nodePos += new Vector3 (0.5f, 0, 0.5f); 
                bool isWall = Physics.CheckSphere (nodePos, nodeRange, wallLayer);
                AStarNode nd = new AStarNode (isWall, nodePos,i,j);

                if (isWall) {
                    GameObject obj = GameObject.Instantiate (nodeWallPrefabs, nodePos, Quaternion.identity) as GameObject;

                }
                AllNodeGroup[i,j] = nd;

            }
        }
    }

    public List<AStarNode> GetAroundNodes(AStarNode curNode){

        List<AStarNode> retGroup = new List<AStarNode> ();
        for (int i = -1; i <= 1; i++) {
            for (int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0) {
                    continue;
                }
                int z = curNode.posZ + i;
                int x = curNode.posX + j;

                if (x >= 0 && x < xWidth && z >= 0 && z < zHeight) {
                    retGroup.Add (AllNodeGroup [z, x]);
                }
            }
        }
        return retGroup;

    }

    public AStarNode GetItem(Vector3 pos){
        int x = Mathf.RoundToInt (pos.x - 0.5f);
        int z = Mathf.RoundToInt (pos.z - 0.5f);
        x = Mathf.Clamp (x, 0, xWidth - 1);
        z = Mathf.Clamp (z, 0, zHeight - 1);
        return AllNodeGroup [z, x];
    }
}
