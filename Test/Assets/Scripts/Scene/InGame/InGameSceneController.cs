using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Bubble;

namespace Test.MyScene {
    public class InGameSceneController : SceneController {
        public const int BUBBLE_NUM = 30;

        private BubbleManager bubbleManager;

        void Awake() {
            this.SceneType = ESceneType.InGame;
            Physics2D.gravity = new Vector2(0.0f, -98.1f);
            this.bubbleManager = BubbleManager.Instance;
        }

        void Start() {
            this.bubbleManager.Initialize(BUBBLE_NUM);
        }

        public void ClickOnStartButton() {
    		IsChangedNextStep = true;
    	}
    }
}