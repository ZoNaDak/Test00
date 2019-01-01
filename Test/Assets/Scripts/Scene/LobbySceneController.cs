using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Sound;

namespace Test.MyScene {
    public class LobbySceneController : SceneController {
        void Awake() {
            this.SceneType = ESceneType.Lobby;
            Util.SpriteFactory.Instance.AddAtlas("Lobby");
        }

        void Start() {
            this.IsLoading = false;
            SoundManager.Instance.StartBgm(EBgmType.LobbyBgm);
        }

        void OnDestroy() {
            Util.SpriteFactory.Instance.RemoveAtlas("Lobby");
        }

        public void ClickOnGameStartButton() {
            SoundManager.Instance.StopBgm(EBgmType.LobbyBgm);
            this.IsChangedNextStep = true;
        }
    }
}