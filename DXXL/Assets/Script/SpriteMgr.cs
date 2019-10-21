using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class SpriteMgr : MonoBehaviour
{
    public static SpriteMgr instance;
    public List<Sprite> chessSpriteList;
    private void Awake()
    {
        instance = this;
    }

    public Sprite GetChessSpriteAtChessType(ChessTypeEnum chessTypeEnum)
    {
        return chessSpriteList[(int)chessTypeEnum];
    }
}
