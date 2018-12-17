using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public class TitleSceneController : SceneController {
        void Awake() {
            this.sceneType = SceneType.Title;
        }

        public void ClickOnStartButton() {
    		CanChangeNextStep = true;
    	}
    }
}
