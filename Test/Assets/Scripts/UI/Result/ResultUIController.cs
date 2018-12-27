using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Test.UI {
    public class ResultUIController : MonoBehaviour {
        private Text scorePointText;

        void Awake() {
            this.scorePointText = this.transform.Find("ScorePointText").GetComponent<Text>();
        }

        public void SetScorePoint(int _scorePoint) {
            this.scorePointText.text = _scorePoint.ToString();
        }
    }
}