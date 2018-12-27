using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Test.Sound;

namespace Test.MyScene {
    public class TitleSceneController : SceneController {
        void Awake() {
            this.SceneType = ESceneType.Title;
        }

        void Start() {
            this.IsLoading = false;
            SoundManager.Instance.StartBgm(EBgmType.TitleBgm);
        }

        public void ClickOnStartButton() {
            SoundManager.Instance.StopBgm(EBgmType.TitleBgm);
    		IsChangedNextStep = true;
    	}
    }
}
