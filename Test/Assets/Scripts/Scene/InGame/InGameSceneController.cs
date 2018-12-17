using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public class InGameSceneController : SceneController {
        void Awake() {
            this.sceneType = SceneType.InGame;
        }

        public void ClickOnStartButton() {
    		CanChangeNextStep = true;
    	}
    }
}