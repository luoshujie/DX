using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.Serialization;

public class SpriteMgr : MonoBehaviour
{
    public static SpriteMgr instance;
    public List<Sprite> chessColorSpriteList;
    public List<Sprite> chessTypeSpriteList;
    private void Awake()
    {
        instance = this;
    }

    public Sprite GetChessSpriteAtChessColorType(ChessColorTypeEnum chessTypeEnum)
    {
        return chessColorSpriteList[(int)chessTypeEnum];
    }
    public Sprite GetChessSpriteAtChessType(ChessTypeEnum chessTypeEnum)
    {
        return chessTypeSpriteList[(int)chessTypeEnum];
    }
}
