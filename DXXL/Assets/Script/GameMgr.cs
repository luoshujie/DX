using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameMgr : MonoBehaviour
{
    public static GameMgr instance;
    private Chess[,] chessList;
    private GameObject chessObj;

    public int Xcount;
    public int Ycount;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Init();
    }

    private void Init()
    {
        chessObj = Resources.Load<GameObject>("Prefab/Chess");
        chessList = new Chess[Xcount, Ycount];

        CheckoutBoard();
    }

    private IEnumerator DeleteChess(List<Chess> seekChessList,Chess touchChess)
    {
        int x = touchChess.data.XPos;
        int y = touchChess.data.YPos;
        for (int i = 0; i < seekChessList.Count; i++)
        {
            Destroy(seekChessList[i].gameObject);
            seekChessList[i] = null;
        }

        SpawnSkill(seekChessList.Count, x,y);
        yield return new WaitForSeconds(0.2f);
        CheckoutBoard();
    }

    #region 生成技能棋子

    private void SpawnSkill(int count,int x,int y)
    {
        ChessTypeEnum chessTypeEnum = ChessTypeEnum.None;
        if (count >= 7)
        {
            //同色消除
            chessTypeEnum = ChessTypeEnum.Color;
           
        }
        else if (count>=5)
        {
            //行消除或列消除
            if (Random.Range(0,2)==0)
            {
                chessTypeEnum = ChessTypeEnum.Column;
            }
            else
            {
                chessTypeEnum = ChessTypeEnum.Line;
            }
        }
        else if (count>=3)
        {
            
            chessTypeEnum = ChessTypeEnum.Nine;
            //三宫格消除
        }

        Chess chess = SpawnSkillChess(x, y, chessTypeEnum);
        chess.transform.position=new Vector3(x+GameConfig.Xlen,y+GameConfig.Ylen,0);
    }

    private Chess SpawnSkillChess(int x, int y, ChessTypeEnum chessTypeEnum)
    {
        GameObject chessModel = Instantiate(chessObj, transform);
        Chess chess = chessModel.GetComponent<Chess>();
        ChessData chessData = new ChessData();
        chessData.id = GetID();
        chessData.ChessTypeEnum = chessTypeEnum;
        chessData.XPos = x;
        chessData.YPos = y;
        chess.InitSkill(chessData);
        chessList[x, y] = chess;
        return chess;
    }

    #endregion
    
    #region 遍历

    public void SeekChess(Chess chess)
    {
        List<Chess> seekChessList = new List<Chess>();
        seekChessList.Add(chess);
        for (int i = 0; i < seekChessList.Count; i++)
        {
            List<Chess> seekAroundList = SeekAround(seekChessList[i]);

            for (int j = 0; j < seekAroundList.Count; j++)
            {
                for (int k = 0; k < seekChessList.Count; k++)
                {
                    if (seekChessList[k].data.id == seekAroundList[j].data.id)
                    {
                        break;
                    }

                    if (k == seekChessList.Count - 1)
                    {
                        seekChessList.Add(seekAroundList[j]);
                    }
                }
            }
        }

        Debug.Log(seekChessList.Count);
        
        StartCoroutine(DeleteChess(seekChessList,chess));
    }
    
    
    /// <summary>
    /// 查找四周
    /// </summary>
    /// <param name="chessData"></param>
    private List<Chess> SeekAround(Chess chess)
    {
        List<Chess> seekList = new List<Chess>();
        for (int i = -1; i < 2; i++)
        {
            if (i != 0)
            {
                int newXpos = chess.data.XPos + i;

                if (newXpos >= 0 && newXpos < Xcount)
                {
                    if (chessList[newXpos, chess.data.YPos].data.ChessColorTypeEnum == chess.data.ChessColorTypeEnum)
                    {
                        seekList.Add(chessList[newXpos, chess.data.YPos]);
                    }
                }
            }
        }
        
        for (int j = -1; j < 2; j++)
        {
            if (j != 0)
            {
                int newYpos = chess.data.YPos + j;
                if (newYpos >= 0 && newYpos < Ycount)
                {
                    if (chessList[chess.data.XPos, newYpos].data.ChessColorTypeEnum == chess.data.ChessColorTypeEnum)
                    {
                        seekList.Add(chessList[chess.data.XPos, newYpos]);
                    }
                }
            }
        }
        
        return seekList;
    }

    #endregion

    #region 棋子的生成

    /// <summary>
    /// 检测是否存在空的棋盘并补充棋子
    /// </summary>
    private void CheckoutBoard()
    {
        for (int i = 0; i < Xcount; i++)
        {
            CheckerBoardAtXpos(i);
        }
    }

    /// <summary>
    /// 检测x轴的棋子
    /// </summary>
    /// <param name="x"></param>
    private void CheckerBoardAtXpos(int x)
    {
        List<int> nullChessYPos = new List<int>();
        for (int i = 0; i < Ycount; i++)
        {
            if (chessList[x, i] != null)
            {
                if (nullChessYPos.Count > 0)
                {
                    chessList[x, i].data.YPos=nullChessYPos[0];
                    chessList[x,nullChessYPos[0]]=chessList[x, i];
                    chessList[x, nullChessYPos[0]].UpdatePos();
                    chessList[x, i]=null;
                    nullChessYPos.Add(i);
                    nullChessYPos.RemoveAt(0);
                }
            }
            else
            {
                nullChessYPos.Add(i);
            }
        }

        SpawnListChess(x, nullChessYPos);
    }

    /// <summary>
    /// 以X轴为对照，生成一条列表棋子
    /// </summary>
    /// <param name="x"></param>
    /// <param name="yList"></param>
    private void SpawnListChess(int x, List<int> yList)
    {
        for (int i = 0; i < yList.Count; i++)
        {
            Chess chessModel = SpawnChess(x, yList[i]);
            chessModel.transform.position = new Vector3(x + GameConfig.Xlen, i+1 - GameConfig.Ylen);
            chessModel.UpdatePos();
        }
    }

    /// <summary>
    /// 生成单个棋子
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private Chess SpawnChess(int x, int y)
    {
        GameObject chessModel = Instantiate(chessObj, transform);
        Chess chess = chessModel.GetComponent<Chess>();
        ChessData chessData = new ChessData();
        chessData.id = GetID();
        chessData.ChessColorTypeEnum = (ChessColorTypeEnum) Random.Range(0, 4);
        chessData.XPos = x;
        chessData.YPos = y;
        chess.Init(chessData);
        chessList[x, y] = chess;
        return chess;
    }

    private int idCount = 0;

    private int GetID()
    {
        idCount++;
        return idCount;
    }

    #endregion
}