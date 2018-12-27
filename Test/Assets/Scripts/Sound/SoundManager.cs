using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Sound {
    public enum EBgmType {
        TitleBgm,
        MainGameBgm,
        End
    }

    public class SoundManager : Pattern.MonoSingleton<SoundManager> {
        private Dictionary<EBgmType, AudioSource> bgmDictionary = new Dictionary<EBgmType, AudioSource>();

        void Awake() {
            for(int i = 0; i < (int)EBgmType.End; ++i) {
                GameObject bgmPrefab = Test.Util.PrefabFactory.Instance.CreatePrefab("Main", ((EBgmType)i).ToString(), false);
                AudioSource bgm = Instantiate(bgmPrefab, this.transform).GetComponent<AudioSource>();
                bgm.gameObject.SetActive(false);
                bgmDictionary.Add(((EBgmType)i), bgm);
            }
        }

        public void StartBgm(EBgmType _bgmType) {
            this.bgmDictionary[_bgmType].gameObject.SetActive(true);
        }

        public void StopBgm(EBgmType _bgmType) {
            this.bgmDictionary[_bgmType].gameObject.SetActive(false);
        }
    }
}