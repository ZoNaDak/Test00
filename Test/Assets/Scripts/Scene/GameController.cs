using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Test.MyScene;

namespace Test {
    public enum EStep {
        Title,
        Lobby,
        InGame,
        Result,
        End
    }

    public class GameController : Pattern.MonoSingleton<GameController> {
        private Dictionary<ESceneType, SceneController> currentSceneDictionary = new Dictionary<ESceneType, SceneController>();
        private bool isSceneLoaded;

        public EStep CurrentStep { get; private set; }

        void Awake() {
            #if !UNITY_EDITOR
				Debug.unityLogger.logEnabled = false;
			#endif

			//SetResoultion
			Screen.SetResolution(360, 640, false);
        }

        void Start() {
            StartCoroutine(LoadAsyncScenesForStep(EStep.Title));
        }

        void Update() {
            if(!this.isSceneLoaded) {
                return;
            }
        }

        void LateUpdate() {
            if(!this.isSceneLoaded) {
                return;
            }

            try {
                if(CheckGoToNextStep()) {
                    GoToNextStepInternal();
                }  
            } catch(System.NullReferenceException) {
                Debug.LogError("currentScene is Null!!");
            }      
        }

        private void GoToNextStepInternal() {
            switch(this.CurrentStep) {
                case EStep.Title:
                    SceneManager.UnloadSceneAsync("Title");
                    StartCoroutine(LoadAsyncScenesForStep(EStep.InGame));
                break;
                case EStep.InGame:
                    SceneManager.UnloadSceneAsync("InGame");
                break;
                default:
                    throw new System.ArgumentOutOfRangeException("CurrentStep is not correct");
            }
        }

        private IEnumerator LoadAsyncScenesForStep(EStep _step) {
            this.isSceneLoaded = false;
            this.CurrentStep = _step;

            List<AsyncOperation> asyncLoadList = new List<AsyncOperation>();
            switch(_step) {
                case EStep.Title:
                    asyncLoadList.Add(SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive));
                break;
                case EStep.InGame:
                    asyncLoadList.Add(SceneManager.LoadSceneAsync("InGame", LoadSceneMode.Additive));
                break;
                default:
                    throw new System.ArgumentOutOfRangeException("Argument:_step is not correct");
            }

            bool roopCheck = true;
            while(roopCheck) {
                for(int i = 0; i < asyncLoadList.Count; ++i) {
                    if(!asyncLoadList[i].isDone) {
                        break;
                    }
                    if(i == asyncLoadList.Count-1) {
                        roopCheck = false;
                    }
                }
                yield return null;
            }
            SetCurrentSceneDictionary();
            this.isSceneLoaded = true;
        }

        private void SetCurrentSceneDictionary() {
            int sceneCount = SceneManager.sceneCount;
            for(int i = 0; i < sceneCount; ++i) {
                Scene tmpScene = SceneManager.GetSceneAt(i);
                if(tmpScene == this.gameObject.scene) {
                    continue;
                }

                var objects = tmpScene.GetRootGameObjects();
                foreach(var obj in objects) {
                    SceneController sceneController = obj.GetComponent<SceneController>();
                    if(sceneController != null) {
                        this.currentSceneDictionary.Add(sceneController.SceneType, sceneController);
                    }
                }
            }
        }

        private bool CheckGoToNextStep() {
            switch(this.CurrentStep) {
                case EStep.Title:
                    if(this.currentSceneDictionary[ESceneType.Title].IsChangedNextStep) {
                        return true;
                    }
                break;
                case EStep.InGame:
                    if(this.currentSceneDictionary[ESceneType.InGame].IsChangedNextStep) {
                        return true;
                    }
                break;
                default:
                    throw new System.ArgumentOutOfRangeException("CurrentStep is not correct");
            }
            return false;
        }
    }
}