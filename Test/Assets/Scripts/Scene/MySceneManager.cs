using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Test.Fade;

namespace Test.MyScene {
    public class MySceneManager : Pattern.Singleton<MySceneManager> {
        private Dictionary<ESceneType, SceneController> currentSceneDictionary = new Dictionary<ESceneType, SceneController>();
        private FadeController fade;

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

        public void Initialzie(FadeController _fade) {
            this.fade = _fade;
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

        //Coroutine
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

            this.fade.StartFadeIn();
            while(this.fade.State == EFadeState.FadeIn) {
                yield return null;
            }
            this.IsCompleteToLoadScenes = true;
        }

        public IEnumerator UnloadAsyncScenesForStep(EStep _currentStep) {
            this.IsCompleteToUnloadScenes = false;
            //Is Loaded Scenes Check
            while(!this.IsCompleteToLoadScenes) {
                Debug.Log(this.IsCompleteToLoadScenes);
                yield return null;
            }
            this.fade.StartFadeOut();

            while(this.fade.State == EFadeState.FadeOut) {
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

        public IEnumerator StartScenesWhenLoadedAllScenes() {
            while(!this.IsCompleteToUnloadScenes || !this.IsCompleteToLoadScenes) {
                yield return null;
            }
            bool isLoading = true;
            while(isLoading) {
                foreach(var pair in this.currentSceneDictionary) {
                    if(pair.Value.IsLoading) {
                        break;
                    } else {
                        if(pair.Value == this.currentSceneDictionary.Last().Value) {
                            isLoading = false;
                        }
                    }
                }
                yield return null;
            }
            foreach(var pair in this.currentSceneDictionary) {
                pair.Value.StartScene();
            }
        }
    }
}