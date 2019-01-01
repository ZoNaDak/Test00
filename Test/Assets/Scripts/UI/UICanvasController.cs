using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Test.Util.Coroutine;

namespace Test.UI {
    public class UICanvasController : Pattern.MonoSingleton<UICanvasController> {
        private const float ON_TIME_OF_START_TEXT = 1.0f;
        private const float ON_TIME_OF_TIME_UP_TEXT = 2.0f;

        private Image blackPanel;
        private Button titleButton;
        private Button retryButton;

        public ScoreController Score { get; private set; }
        public TimerController Timer { get; private set; }
        public GameStateTextController GameStateText { get; private set; }
        public ResultUIController ResultUI { get; private set; }

        void Awake() {
            this.blackPanel = this.transform.Find("BlackPanel").GetComponent<Image>();
            this.Score = this.transform.Find("Score").GetComponent<ScoreController>();
            this.Timer = this.transform.Find("Timer").GetComponent<TimerController>();
            this.GameStateText = this.transform.Find("GameStateText").GetComponent<GameStateTextController>();
            this.ResultUI = this.transform.Find("ResultUI").GetComponent<ResultUIController>();
            this.titleButton = this.transform.Find("TitleButton").GetComponent<Button>();
            this.retryButton = this.transform.Find("RetryButton").GetComponent<Button>();
        }

        public void ReadyGame(float _startTime) {
            this.Timer.Initialize(_startTime);
            this.Score.ClearScore();
            this.GameStateText.OnReadyText();
        }

        public void StartGame() {
            UI.UICanvasController.Instance.Timer.StartCountDown();
            this.GameStateText.OnStartText();
            this.StartCoroutine(UseableCoroutine.WaitThenCallback(ON_TIME_OF_START_TEXT, () => this.GameStateText.OffStartText()));
        }

        public void TimeUpGame() {
            this.GameStateText.OnTimeUpText();
        }

        public void OnResult() {
            this.GameStateText.OffTimeUpText();
            this.blackPanel.gameObject.SetActive(true);
            this.ResultUI.gameObject.SetActive(true);
            this.ResultUI.SetScorePoint(this.Score.Score);
            this.Score.SaveScore();
            this.titleButton.gameObject.SetActive(true);
            this.retryButton.gameObject.SetActive(true);
        }

        public void OffResult() {
            this.blackPanel.gameObject.SetActive(false);
            this.ResultUI.gameObject.SetActive(false);
            this.titleButton.gameObject.SetActive(false);
            this.retryButton.gameObject.SetActive(false);
        }
    }
}