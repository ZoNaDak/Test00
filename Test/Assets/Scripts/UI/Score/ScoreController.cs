using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using UnityEngine;
using UnityEngine.UI;
using Test.Util.Xml;
using Test.UI.ScoreTable;

namespace Test.UI {
    public class ScoreController : MonoBehaviour {
        private Text scoreText;

        public int Score { get; private set; }
        public bool IsSaved { get; private set; }

        void Awake() {
            this.scoreText = this.transform.Find("ScoreText").GetComponent<Text>();
        }
        
        void Start() {
            this.scoreText.text = this.Score.ToString();
        }

        public void AddScore(int _addScore) {
            this.Score += _addScore;
            this.scoreText.text = this.Score.ToString();
        }

        public void ClearScore() {
            this.Score = 0;
            this.scoreText.text = this.Score.ToString();
        }

        public void SaveScore() {
            this.IsSaved = false;
            HighScoreManager.Instance.AddHighScore(this.Score);
            this.IsSaved = true;
        }
    }
}