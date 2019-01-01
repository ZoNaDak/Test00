using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Test.Sound {
    public enum EBgmType {
        TitleBgm,
        LobbyBgm,
        MainGameBgm,
        End
    }

    public enum EEffectSoundType {
        LinkCandy,
        DelinkCandy,
        Pang,
        OnResult,
        TimeUp,
        Ready,
        End
    }

    public class SoundManager : Pattern.MonoSingleton<SoundManager> {
        private const int EFFECT_SOUND_NUM = 10;

        private Dictionary<EBgmType, AudioSource> bgmDictionary = new Dictionary<EBgmType, AudioSource>();
        private Dictionary<EEffectSoundType, AudioClip> effectSoundClipDictionary = new Dictionary<EEffectSoundType, AudioClip>();
        private Queue<AudioSource> waitEffectSoundQueue = new Queue<AudioSource>();
        private List<AudioSource> playEffectSoundList = new List<AudioSource>();

        void Awake() {
            for(int i = 0; i < (int)EBgmType.End; ++i) {
                GameObject bgmPrefab = Test.Util.PrefabFactory.Instance.CreatePrefab("Main", ((EBgmType)i).ToString(), false);
                AudioSource bgm = Instantiate(bgmPrefab, this.transform).GetComponent<AudioSource>();
                bgm.gameObject.SetActive(false);
                bgmDictionary.Add(((EBgmType)i), bgm);
            }

            GameObject effectSoundPrefab = Test.Util.PrefabFactory.Instance.CreatePrefab("Main", "EffectSound", false);
            for(int i = 0; i < EFFECT_SOUND_NUM; ++i) {
                AudioSource effectSound = Instantiate(effectSoundPrefab, this.transform).GetComponent<AudioSource>();
                this.waitEffectSoundQueue.Enqueue(effectSound);
            }

            for(int i = 0; i < (int)EEffectSoundType.End; ++i) {
                effectSoundClipDictionary.Add(((EEffectSoundType)i), Resources.Load<AudioClip>(Path.Combine("Audio/FX", ((EEffectSoundType)i).ToString())));
            }
        }

        void Update() {
            for(int i = 0; i < this.playEffectSoundList.Count; ++i) {
                if(!this.playEffectSoundList[i].isPlaying) {
                    this.waitEffectSoundQueue.Enqueue(this.playEffectSoundList[i]);
                    this.playEffectSoundList.RemoveAt(i);
                }
            }
        }

        public void StartBgm(EBgmType _bgmType) {
            this.bgmDictionary[_bgmType].gameObject.SetActive(true);
        }

        public void StopBgm(EBgmType _bgmType) {
            this.bgmDictionary[_bgmType].gameObject.SetActive(false);
        }

        public void PlayEffectSound(EEffectSoundType _effectSound) {
            if(this.waitEffectSoundQueue.Count == 0) {
                GameObject effectSoundPrefab = Test.Util.PrefabFactory.Instance.CreatePrefab("Main", "EffectSound", false);
                AudioSource effectSound = Instantiate(effectSoundPrefab, this.transform).GetComponent<AudioSource>();
                this.waitEffectSoundQueue.Enqueue(effectSound);
            }
            AudioSource playSound = this.waitEffectSoundQueue.Dequeue();
            playSound.clip = this.effectSoundClipDictionary[_effectSound];
            playSound.Play();
            this.playEffectSoundList.Add(playSound);
            
        }
    }
}