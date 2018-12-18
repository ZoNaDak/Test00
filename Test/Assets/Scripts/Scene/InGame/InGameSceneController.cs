using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public class InGameSceneController : SceneController {
        void Awake() {
            this.SceneType = ESceneType.InGame;
            Physics2D.gravity = new Vector2(0.0f, -98.1f);
        }

        public void ClickOnStartButton() {
    		IsChangedNextStep = true;
    	}
    }
}