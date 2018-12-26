using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public class TitleSceneController : SceneController {
        void Awake() {
            this.SceneType = ESceneType.Title;
        }

        void Start() {
            this.IsLoading = false;
        }

        public void ClickOnStartButton() {
    		IsChangedNextStep = true;
    	}
    }
}
