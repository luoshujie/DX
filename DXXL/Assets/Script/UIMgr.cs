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
            //Screen Height/2/Pixel To Units=Main Camera.Size
            
            //iPhone4的屏幕像素为640*960，宽高比为2:3，假设Pixels To Units值为100，
            //那么如果设摄像机高度size值为4.8，那么摄像机实际宽度按照公式算出6.4，刚好就是屏幕的单位宽度。
            //Camera.main.orthographicSize = (Screen.height / 100f) / ((Screen.height * 1.0f)/Screen.width);
//            Camera.main.orthographicSize = (Screen.height / 100.0f) / 2.0f;
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
