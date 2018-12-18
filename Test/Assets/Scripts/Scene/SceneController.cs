using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.MyScene {
    public enum ESceneType {
        Title,
        InGame,
        End
    }

    public abstract class SceneController : MonoBehaviour {
        public ESceneType SceneType { get; protected set; } = ESceneType.End;
        public bool IsChangedNextStep { get; protected set;} = false;
    }
}