using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Util.Coroutine;

namespace Test.UI {
    public class UICanvasController : Pattern.MonoSingleton<UICanvasController> {
        private const float ON_START_TEXT_TIME = 1.0f;

        public ScoreController Score { get; private set; }
        public TimerController Timer { get; private set; }
        public GameStateTextController GameStateText { get; private set; }

        void Awake() {
            this.Score = this.transform.Find("Score").GetComponent<ScoreController>();
            this.Timer = this.transform.Find("Timer").GetComponent<TimerController>();
            this.GameStateText = this.transform.Find("GameStateText").GetComponent<GameStateTextController>();
        }

        public void ReadyGame(float _startTime) {
            this.Timer.Initialize(_startTime);
            this.GameStateText.OnReadyText();
        }

        public void StartGame() {
            UI.UICanvasController.Instance.Timer.StartCountDown();
            this.GameStateText.OnStartText();
            this.StartCoroutine(UseableCoroutine.WaitThenCallback(ON_START_TEXT_TIME, () => this.GameStateText.OffStartText()));
        }

        public void TimeUpGame() {
            this.GameStateText.OnTimeUpText();
        }
    }
}