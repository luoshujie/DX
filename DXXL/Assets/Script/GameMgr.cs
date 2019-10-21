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

    /// <summary>
    /// 点击棋子
    /// </summary>
    /// <param name="chess"></param>
    public void SeekChess(Chess chess)
    {
        List<Chess> seekChessList = new List<Chess>();

        if (chess.data.ChessTypeEnum == ChessTypeEnum.None)
        {
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
        }
        else
        {
            List<Chess> seekList = new List<Chess>();
            switch (chess.data.ChessTypeEnum)
            {
                case ChessTypeEnum.Color:
                    seekList = SeekColor(chess.data.ChessColorTypeEnum);
                    break;
                case ChessTypeEnum.Column:
                    seekList = SeekColumn(chess.data.XPos);
                    break;
                case ChessTypeEnum.Line:
                    seekList = SeekLine(chess.data.YPos);
                    break;
                case ChessTypeEnum.Nine:
                    seekList = SeekNine(chess);
                    break;
            }

            for (int i = 0; i < seekList.Count; i++)
            {
                seekChessList.Add(seekList[i]);
            }
        }


        Debug.Log(seekChessList.Count);

        StartCoroutine(DeleteChess(seekChessList, chess));
    }

    private IEnumerator DeleteChess(List<Chess> seekChessList, Chess touchChess)
    {
        int x = touchChess.data.XPos;
        int y = touchChess.data.YPos;
        int count = seekChessList.Count;
        for (int i = 0; i < seekChessList.Count; i++)
        {
            Destroy(seekChessList[i].gameObject);
            seekChessList[i] = null;
        }

        if (touchChess.data.ChessTypeEnum == ChessTypeEnum.None && count >= 3)
        {
            ChessData chessData = new ChessData();
            chessData.XPos = x;
            chessData.YPos = y;
            chessData.id = GetID();
            chessData.ChessColorTypeEnum = touchChess.data.ChessColorTypeEnum;
            SpawnSkill(chessData, count);
        }

        yield return new WaitForSeconds(0.2f);
        CheckoutBoard();
    }

    #region 生成技能棋子

    private void SpawnSkill(ChessData chessData, int count)
    {
        ChessTypeEnum chessTypeEnum = ChessTypeEnum.None;
        if (count >= 7)
        {
            //同色消除
            chessTypeEnum = ChessTypeEnum.Color;
        }
        else if (count >= 5)
        {
            chessData.ChessColorTypeEnum = ChessColorTypeEnum.None;
            //行消除或列消除
            if (Random.Range(0, 2) == 0)
            {
                chessTypeEnum = ChessTypeEnum.Column;
            }
            else
            {
                chessTypeEnum = ChessTypeEnum.Line;
            }
        }
        else if (count >= 3)
        {
            chessData.ChessColorTypeEnum = ChessColorTypeEnum.None;
            chessTypeEnum = ChessTypeEnum.Nine;
            //三宫格消除
        }

        chessData.ChessTypeEnum = chessTypeEnum;
        Chess chess = SpawnSkillChess(chessData);
        chess.transform.position = new Vector3(chessData.XPos + GameConfig.Xlen, chessData.YPos + GameConfig.Ylen, 0);
    }

    private Chess SpawnSkillChess(ChessData chessData)
    {
        GameObject chessModel = Instantiate(chessObj, transform);
        Chess chess = chessModel.GetComponent<Chess>();
        chess.InitSkill(chessData);
        chessList[chessData.XPos, chessData.YPos] = chess;
        return chess;
    }

    #endregion

    #region 遍历

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

    /// <summary>
    /// 行遍历
    /// </summary>
    /// <param name="y"></param>
    /// <returns></returns>
    private List<Chess> SeekLine(int y)
    {
        List<Chess> seekList = new List<Chess>();
        for (int i = 0; i < Xcount; i++)
        {
            seekList.Add(chessList[i, y]);
        }

        Debug.Log("行遍历:" + y);
        return seekList;
    }

    /// <summary>
    /// 列遍历
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    private List<Chess> SeekColumn(int x)
    {
        List<Chess> seekList = new List<Chess>();
        for (int i = 0; i < Ycount; i++)
        {
            seekList.Add(chessList[x, i]);
        }

        Debug.Log("列遍历:" + x);
        return seekList;
    }

    /// <summary>
    /// 同色遍历
    /// </summary>
    /// <param name="chessColorTypeEnum"></param>
    /// <returns></returns>
    private List<Chess> SeekColor(ChessColorTypeEnum chessColorTypeEnum)
    {
        List<Chess> seekList = new List<Chess>();
        for (int i = 0; i < Xcount; i++)
        {
            for (int j = 0; j < Ycount; j++)
            {
                if (chessList[i, j].data.ChessColorTypeEnum == chessColorTypeEnum)
                {
                    seekList.Add(chessList[i, j]);
                }
            }
        }

        Debug.Log("同色遍历:" + chessColorTypeEnum);
        return seekList;
    }

    /// <summary>
    /// 九宫格遍历
    /// </summary>
    /// <param name="chess"></param>
    /// <returns></returns>
    private List<Chess> SeekNine(Chess chess)
    {
        List<Chess> seekList = new List<Chess>();

        for (int i = -1; i < 2; i++)
        {
            int newX = chess.data.XPos + i;
            for (int j = -1; j < 2; j++)
            {
                int newY = chess.data.YPos+j;
                if (newX >= 0 && newX < Xcount && newY >= 0 && newY < Ycount)
                {
                    seekList.Add(chessList[newX, newY]);
                }
            }
        }

        Debug.LogFormat("九宫格遍历:{0},{1}", chess.data.XPos, chess.data.YPos);
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
                    chessList[x, i].data.YPos = nullChessYPos[0];
                    chessList[x, nullChessYPos[0]] = chessList[x, i];
                    chessList[x, nullChessYPos[0]].UpdatePos();
                    chessList[x, i] = null;
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
            chessModel.transform.position = new Vector3(x + GameConfig.Xlen, i + 1 - GameConfig.Ylen);
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