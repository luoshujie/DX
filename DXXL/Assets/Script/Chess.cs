using System;
using DG.Tweening;
using UnityEngine;

namespace Script
{
    public class Chess : MonoBehaviour
    {
        public SpriteRenderer render;
        
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
            render.sprite = SpriteMgr.instance.GetChessSpriteAtChessColorType(data.ChessColorTypeEnum);
        }

        public void InitSkill(ChessData chessData)
        {
            if (chessData!=null)
            {
                data = chessData;
            }

            render.sprite = SpriteMgr.instance.GetChessSpriteAtChessType(data.ChessTypeEnum);
        }

        public void UpdatePos()
        {
            MoveToTarget();
        }

        public void MoveToTarget()
        {
            float distance = transform.position.y - (data.YPos+GameConfig.Ylen);
            float time = distance / GameConfig.ChessMoveSpeed;
            transform.DOMove(new Vector3(data.XPos+GameConfig.Xlen, data.YPos+GameConfig.Ylen, 0), time).SetEase(Ease.Linear);
        }

        private void OnMouseDown()
        {
            GameMgr.instance.SeekChess(this);
        }
    }
}
