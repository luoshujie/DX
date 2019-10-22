using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class UIMgr : MonoBehaviour
    {
        public static UIMgr instance;
        public GameObject mask;
        public Button exitBtn;
        public Button startBtn;
        public Text scoreText;

        private void Awake()
        {
            instance = this;
            AddEvent();
        }

        private void ExitBtnClick()
        {
            Application.Quit();
        }
        
        private void StartBtnClick()
        {
            GameMgr.instance.SetGameState(true);
            mask.SetActive(false);
        }
        
        public void UpdateScore(int score)
        {
            scoreText.text = score.ToString();
        }

        private void AddEvent()
        {
            startBtn.onClick.AddListener(StartBtnClick);
            exitBtn.onClick.AddListener(ExitBtnClick);
        }
    }
}
