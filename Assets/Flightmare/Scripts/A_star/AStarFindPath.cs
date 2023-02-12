using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarFindPath : MonoBehaviour {

    private AStarMAP aStarMap;

    public List<Vector3> peoplePath;
    public List<AStarNode> peopleNodePath;

    void Start(){
        aStarMap = GetComponent<AStarMAP> ();
    }

    private int GetDistance(AStarNode startNode,AStarNode endNode){
        int x = Mathf.Abs (startNode.posX - endNode.posX);
        int z = Mathf.Abs (startNode.posZ - endNode.posZ);
        if (x > z) {
            return 10 * (x - z) + 14 * z;
        } else {
            return 10 * (z - x) + 14 * x;
        }
    }


    //根据开始和结束点来查找最优路径
    private void toFindPath(Vector3 startPos,Vector3 endPos) {

        //根据位置获取到NodeItem
        AStarNode start = aStarMap.GetItem (startPos);
        AStarNode end = aStarMap.GetItem (endPos);

        List<AStarNode> openList = new List<AStarNode> ();
        List<AStarNode> closeList = new List<AStarNode> ();

        openList.Add (start);
        //从开始点开始判断
        while (openList.Count > 0) {
            AStarNode curNode = openList [0];
            for (int i = 0; i < openList.Count; i++) {
                //h是估算法的距离  f是估算法加实际格子的距离g
                if (openList [i].CostF < curNode.CostF && openList [i].costH < curNode.costH) {
                    curNode = openList [i];
                }
            }

            openList.Remove (curNode);
            closeList.Add (curNode);

            //已经找到结束点
            if (curNode == end) {
                Debug.Log (">>");
                GetPathWithPos (startPos, endPos);
                return;
            }
            // 获取当前点的周围点
            List<AStarNode> nodeItemGroup =  aStarMap.GetAroundNodes(curNode);

            //遍历当前点周围的NodeItem 对其进行
            foreach (AStarNode nodeCell in nodeItemGroup) {
                //先过滤： 墙 closeList
                if (nodeCell.isWall || closeList.Contains (nodeCell)) {
                    continue;
                }

                //计算 G H F 进行赋值
                int newCostg = curNode.costG + GetDistance (curNode, nodeCell);

                if (newCostg <= nodeCell.costG || !openList.Contains (nodeCell)) {
                    //刷新g h
                    nodeCell.costG = newCostg; //移动步数距离
                    nodeCell.costH = GetDistance (nodeCell, end);//到该结点到终点的距离
                    //设置中心点为父亲
                    nodeCell.parentNode = curNode;
                    if (!openList.Contains (nodeCell)) {
                        openList.Add (nodeCell);
                    }
                }
            }
        }
    }

    //获取路径
    private void GetPathWithPos(Vector3 startPos,Vector3 endNodePos){
        //此处可以优化GC内存
        peopleNodePath = new List<AStarNode> ();
        peoplePath = new List<Vector3> ();

        AStarNode endNode = aStarMap.GetItem (endNodePos);
        AStarNode startNode = aStarMap.GetItem (startPos);

        if (endNode != null) {

            AStarNode temp = endNode;

            while (temp != startNode) {
                peoplePath.Add (temp.pos);
                peopleNodePath.Add (temp);
                temp = temp.parentNode;
            }
            peopleNodePath.Reverse ();
            peoplePath.Reverse ();
        }
    }
    //提供给人物移动类使用
    public List<Vector3> PeopleGoTo(Vector3 startpos,Vector3 endpos){
        toFindPath (startpos, endpos);
        return peoplePath;
    }

    public List<AStarNode> PeopleGoToWithNode(Vector3 startpos,Vector3 endpos){
        toFindPath (startpos, endpos);
        return peopleNodePath;
    }
}
