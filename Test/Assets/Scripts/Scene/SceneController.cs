using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public enum ESceneType {
        Title,
        Lobby,
        InGame,
        End
    }

    public abstract class SceneController : MonoBehaviour {
        public ESceneType SceneType { get; protected set; } = ESceneType.End;
        public bool IsChangedNextStep { get; protected set;}
        public bool IsLoading { get; protected set; } = true;

        public virtual void StartScene() {}
    }
}