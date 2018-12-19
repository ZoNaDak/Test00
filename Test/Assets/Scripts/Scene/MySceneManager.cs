using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Test.MyScene {
    public class MySceneManager : Pattern.Singleton<MySceneManager> {
        private Dictionary<ESceneType, SceneController> currentSceneDictionary = new Dictionary<ESceneType, SceneController>();

        public bool IsCompleteToLoadScenes { get; private set; } = false;
        public bool IsCompleteToUnloadScenes { get; private set; } = true;

        private void SetCurrentSceneDictionary() {
            int sceneCount = SceneManager.sceneCount;
            for(int i = 0; i < sceneCount; ++i) {
                Scene tmpScene = SceneManager.GetSceneAt(i);
                if(tmpScene.name == "Main") {
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

        public IEnumerator LoadAsyncScenesForStep(EStep _step) {
            this.IsCompleteToLoadScenes = false;
            //Is Unloaded Scenes Check
            while(!this.IsCompleteToUnloadScenes) {
                yield return null;
            }

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
            this.IsCompleteToLoadScenes = true;
        }

        public IEnumerator UnloadAsyncScenesForStep(EStep _currentStep) {
            this.IsCompleteToUnloadScenes = false;
            //Is Loaded Scenes Check
            while(!this.IsCompleteToLoadScenes) {
                yield return null;
            }

            List<AsyncOperation> asyncUnLoadList = new List<AsyncOperation>();
            switch(_currentStep) {
                case EStep.Title:
                    asyncUnLoadList.Add(SceneManager.UnloadSceneAsync("Title"));
                break;
                case EStep.InGame:
                    asyncUnLoadList.Add(SceneManager.UnloadSceneAsync("InGame"));
                break;
                default:
                    throw new System.ArgumentOutOfRangeException("Argument:_step is not correct");
            }

            bool roopCheck = true;
            while(roopCheck) {
                for(int i = 0; i < asyncUnLoadList.Count; ++i) {
                    if(!asyncUnLoadList[i].isDone) {
                        break;
                    }
                    if(i == asyncUnLoadList.Count-1) {
                        roopCheck = false;
                    }
                }
                yield return null;
            }

            this.currentSceneDictionary.Clear();
            this.IsCompleteToUnloadScenes = true;
        }

        public bool CheckForChangeToNextStep(EStep _currentStep) {
            switch(_currentStep) {
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