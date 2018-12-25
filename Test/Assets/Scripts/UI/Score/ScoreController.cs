using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test.UI {
    public class ScoreController : Pattern.MonoSingleton<ScoreController> {
        [SerializeField] private Text scoreText;

        public int Score { get; private set; }
        
        void Start() {
            this.scoreText.text = this.Score.ToString();
        }

        public void AddScore(int _addScore) {
            this.Score += _addScore;
            this.scoreText.text = this.Score.ToString();
        }
    }
}