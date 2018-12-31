using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;
using Test.MyScene;

namespace Test.Util {
    public class SpriteFactory : Pattern.Singleton<SpriteFactory> {
        private Dictionary<string, SpriteAtlas> atlasDictionary = new Dictionary<string, SpriteAtlas>();
        
        public void RemoveAllAtlas() {
            atlasDictionary.Clear();
            Resources.UnloadUnusedAssets();
        }

        public bool AddAtlas(string _sceneName) {
            if(this.atlasDictionary.ContainsKey(_sceneName)) {
                Debug.Log(string.Format("Already Have Atlas : {0}", _sceneName));
                return false;
            }
            
            SpriteAtlas atlas = Resources.Load<SpriteAtlas>(
                    Path.Combine("Sprites", _sceneName, string.Format("Atlas_{0}", _sceneName)));
            if(atlas == null) {
                throw new System.NullReferenceException(string.Format("Atlas is Null. Scene : {0}", _sceneName));
            }
            this.atlasDictionary.Add(_sceneName, atlas);
            return true;
        }

        public bool RemoveAtlas(string _sceneName) {
            if(this.atlasDictionary.ContainsKey(_sceneName)) {
                Resources.UnloadAsset(this.atlasDictionary[_sceneName]);
                this.atlasDictionary.Remove(_sceneName);
                return true;
            } else {
                Debug.Log(string.Format("Not Have Atlas : {0}", _sceneName));
                return false;
            }
        }
        
        public Sprite GetSprite(string _sceneName, string _spriteName) {
            Sprite sprite = this.atlasDictionary[_sceneName].GetSprite(_spriteName);
            if(sprite == null) {
                throw new System.NullReferenceException(
                    string.Format("Sprite is Null! Scene : {0}, Sprite : {1}", _sceneName, _spriteName));
            }

            return sprite;
        }
    }
}