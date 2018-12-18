using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public class InGameSceneController : SceneController {
        void Awake() {
            this.SceneType = ESceneType.InGame;
        }

        public void ClickOnStartButton() {
    		IsChangedNextStep = true;
    	}
    }
}