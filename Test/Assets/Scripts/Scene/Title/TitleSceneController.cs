using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public class TitleSceneController : SceneController {
        void Awake() {
            this.SceneType = ESceneType.Title;
        }

        public void ClickOnStartButton() {
    		IsChangedNextStep = true;
    	}
    }
}
