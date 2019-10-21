using System;
using UnityEngine;

namespace Script
{
    public class Chess : MonoBehaviour
    {
        private SpriteRenderer render;
        public ChessData data;

        private void Awake()
        {
            render = GetComponent<SpriteRenderer>();
        }

        public void Init(ChessData chessData)
        {
            if (chessData!=null)
            {
                data = chessData;
            }
            transform.position=new Vector3(GameConfig.Xlen+data.XPos,1-GameConfig.Ylen,0);
            render.sprite = SpriteMgr.instance.GetChessSpriteAtChessType(data.ChessTypeEnum);
        }

        public void MoveToTarget(int y)
        {
        }
    }
}
