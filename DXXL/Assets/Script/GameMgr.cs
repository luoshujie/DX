using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class GameMgr : MonoBehaviour
{
    private Chess[,] chessList;
    private GameObject chessObj;

    public int Xcount;
    public int Ycount;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        chessObj = Resources.Load<GameObject>("Prefab/Chess");
        chessList = new Chess[Xcount, Ycount];

        for (int i = 0; i < Xcount; i++)
        {
            for (int j = 0; j < Ycount; j++)
            {
                SpawnChessAtPos(i,j);
            }
        }
    }

    private void SpawnChessAtPos(int x, int y)
    {
        GameObject chessModel = Instantiate(chessObj, transform);
        Chess chess = chessModel.GetComponent<Chess>();
        ChessData chessData = new ChessData();
        chessData.ChessTypeEnum = (ChessTypeEnum) Random.Range(0, 4);
        chessData.XPos = x;
        chessData.YPos = y;
        chess.Init(chessData);
    }
}