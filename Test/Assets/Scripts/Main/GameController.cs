using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.MyScene;
using Test.Fade;

namespace Test {
    public enum EStep {
        Title,
        Lobby,
        InGame,
        Result,
        End
    }

    public class GameController : Pattern.MonoSingleton<GameController> {
        private MySceneManager sceneManager;
        private FadeController fade;

        public EStep CurrentStep { get; private set; }

        void Awake() {
            #if !UNITY_EDITOR
				Debug.unityLogger.logEnabled = false;
			#endif

			//SetResoultion
			Screen.SetResolution(360, 640, false);

            this.fade = this.transform.Find("Fade").GetComponent<FadeController>();
        }

        void Start() {
            this.sceneManager = MySceneManager.Instance;
            this.sceneManager.Initialzie(this.fade);
            this.CurrentStep = EStep.Title;
            StartCoroutine(this.sceneManager.LoadAsyncScenesForStep(EStep.Title));
            StartCoroutine(this.sceneManager.StartScenesWhenLoadedAllScenes());
        }

        void Update() {
            if(!this.sceneManager.IsCompleteToLoadScenes) {
                return;
            }
        }

        void LateUpdate() {
            if(!this.sceneManager.IsCompleteToLoadScenes) {
                return;
            }

            try {
                if(this.sceneManager.CheckForChangeToNextStep(this.CurrentStep)) {
                    GoToNextStep();
                }  
            } catch(System.NullReferenceException) {
                Debug.LogError("currentScene is Null!!");
            }      
        }

        private void GoToNextStep() {
            StartCoroutine(this.sceneManager.UnloadAsyncScenesForStep(this.CurrentStep));
            switch(this.CurrentStep) {
                case EStep.Title:
                    this.CurrentStep = EStep.Lobby;
                break;
                case EStep.Lobby:
                    this.CurrentStep = EStep.InGame;
                break;
                case EStep.InGame:
                    this.CurrentStep = EStep.Lobby;
                break;
                default:
                    throw new System.ArgumentOutOfRangeException("CurrentStep is not correct");
            }
            StartCoroutine(this.sceneManager.LoadAsyncScenesForStep(this.CurrentStep));
            StartCoroutine(this.sceneManager.StartScenesWhenLoadedAllScenes());
        }
    }
}