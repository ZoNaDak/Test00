using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Candy;
using Test.Sound;
using Test.Util.Coroutine;

namespace Test.MyScene {
    public class InGameSceneController : SceneController {
        public const int CANDY_NUM = 42;    
        public const float GRAVITY_SCALE = 50.0f;
        public const float START_REMAIN_TIME = 60.0f;
        public const float READY_TIME = 2.0f;

        private CandyManager candyManager;
        private bool isTimeUp;

        void Awake() {
            this.SceneType = ESceneType.InGame;
            Physics2D.gravity = new Vector2(0.0f, -9.81f) * GRAVITY_SCALE;
            this.candyManager = CandyManager.Instance;
            Util.SpriteFactory.Instance.AddAtlas("InGame");
        }

        void Start() {
            this.candyManager.Initialize(CANDY_NUM);
            UI.UICanvasController.Instance.ReadyGame(START_REMAIN_TIME);
            this.IsLoading = false;
        }

        void Update() {
            if(UI.UICanvasController.Instance.Timer.TimeOver && !this.isTimeUp) {
                this.StartCoroutine(TimeUpGame());
                this.isTimeUp = true;
            }
        }

        void OnDestroy() {
            Util.SpriteFactory.Instance.RemoveAtlas("InGame");
        }

        public void ClickOnTitleButton() {
            this.StartCoroutine(UseableCoroutine.WaitUnitlTrueThenCallback(
                () => { return UI.UICanvasController.Instance.Score.IsSaved; }
                , () => this.IsChangedNextStep = true));
        }

        public void ClickOnRetryButton() {
            UI.UICanvasController.Instance.OffResult();
            this.candyManager.RetryGame();
            this.StartCoroutine(StartGame());
        }

        public override void StartScene() {
            this.StartCoroutine(StartGame());
        }

        //Coroutine
        private IEnumerator StartGame() {
            SoundManager.Instance.PlayEffectSound(EEffectSoundType.Ready);
            this.isTimeUp = false;
            yield return new WaitForSecondsRealtime(READY_TIME);
            SoundManager.Instance.StartBgm(EBgmType.MainGameBgm);
            UI.UICanvasController.Instance.StartGame();
            this.candyManager.StartGame();
        }

        private IEnumerator TimeUpGame() {
            SoundManager.Instance.StopBgm(EBgmType.MainGameBgm);
            this.candyManager.TimeUpGame();
            UI.UICanvasController.Instance.TimeUpGame();
            SoundManager.Instance.PlayEffectSound(EEffectSoundType.TimeUp);
            yield return new WaitForSecondsRealtime(2.0f);
            UI.UICanvasController.Instance.OnResult();
            SoundManager.Instance.PlayEffectSound(EEffectSoundType.OnResult);
        }
    }
}