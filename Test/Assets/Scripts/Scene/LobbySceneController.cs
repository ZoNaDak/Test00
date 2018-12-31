using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public class LobbySceneController : SceneController {
        void Awake() {
            this.SceneType = ESceneType.Lobby;
            Util.SpriteFactory.Instance.AddAtlas("Lobby");
        }

        void Start() {
            this.IsLoading = false;
        }

        void OnDestroy() {
            Util.SpriteFactory.Instance.RemoveAtlas("Lobby");
        }

        public void ClickOnGameStartButton() {
            this.IsChangedNextStep = true;
        }
    }
}