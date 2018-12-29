using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public class LobbySceneController : SceneController {
        void Awake() {
            this.SceneType = ESceneType.Lobby;
        }

        void Start() {
            this.IsLoading = false;
        }

        public void ClickOnGameStartButton() {
            this.IsChangedNextStep = true;
        }
    }
}