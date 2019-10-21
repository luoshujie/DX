using System;
using UnityEngine.Serialization;

namespace Script
{
    [Serializable]
    public class ChessData
    {
        public int id;
        public ChessColorTypeEnum ChessColorTypeEnum;
        public ChessTypeEnum ChessTypeEnum = ChessTypeEnum.None;
        public int XPos;
        public int YPos;
    }
}