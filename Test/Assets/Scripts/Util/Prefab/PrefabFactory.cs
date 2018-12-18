using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Test.Util {
    public class PrefabFactory : Pattern.Singleton<PrefabFactory> {
        private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();
        
        public GameObject CreatePrefab(string _sceneName, string _prefabName, bool _save) {
            if(this.prefabDictionary.ContainsKey(_prefabName)) {
                Debug.Log(string.Format("Already Saved Prefab : {0}", _prefabName));
                return this.prefabDictionary[_prefabName];
            }

            GameObject prefab = Resources.Load(Path.Combine("Prefabs", _sceneName, _prefabName)) as GameObject;
            if(prefab == null) {
                throw new System.NullReferenceException(string.Format("Can't Load Prefab. Scene : {0}, Prefab : {1}", _sceneName, _prefabName));
            }

            if(_save) {
                this.prefabDictionary.Add(_prefabName, prefab);
            }

            return prefab;
        }

        public GameObject FindPrefab(string _prefabName) {
			if(!this.prefabDictionary.ContainsKey(_prefabName)) {
				Debug.Log(string.Format("Can't Find Prefab. : {0}",  _prefabName));
                return null;
			}
			return this.prefabDictionary[_prefabName];
		}

		public void AllClearPrefabs() {
			this.prefabDictionary.Clear();
		}
    }
}

