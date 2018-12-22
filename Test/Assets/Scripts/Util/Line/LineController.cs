using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Util {
    public class LineController : MonoBehaviour {
        const float MY_DEPTH = -2.0f;
        const float OUTCIRCLE_DEPTH = -1.0f;
        const float INCIRCLE_DEPTH = -3.0f;

        private LineRenderer myRenderer;

        private List<GameObject> selectedObjects = new List<GameObject>();
        private List<GameObject> outCircles = new List<GameObject>();
        private List<GameObject> inCircles = new List<GameObject>();

        public int PointNum { get { return this.selectedObjects.Count; } }

        void Awake() {
            this.myRenderer = this.gameObject.GetComponent<LineRenderer>();
        }

        public void Initialize(int _candyNum) {
            GameObject outCirclePrefab = PrefabFactory.Instance.FindPrefab("OutCircle");
            if(outCirclePrefab == null) {
                outCirclePrefab = PrefabFactory.Instance.CreatePrefab("InGame", "OutCircle", true);
            }
            GameObject inCirclePrefab = PrefabFactory.Instance.FindPrefab("InCircle");
            if(inCirclePrefab == null) {
                inCirclePrefab = PrefabFactory.Instance.CreatePrefab("InGame", "InCircle", true);
            }
            
            for(int i = 0; i < _candyNum; ++i) {
                this.outCircles.Add(Instantiate(outCirclePrefab, this.transform));
                this.outCircles[i].SetActive(false);
                this.inCircles.Add(Instantiate(inCirclePrefab, this.transform));
                this.inCircles[i].SetActive(false);
            }

            this.gameObject.SetActive(false);
        }

        void LateUpdate() {
            //Update Pos
            for(int i = 0; i < this.selectedObjects.Count; ++i) {
                Vector3 candyPos = this.selectedObjects[i].transform.localPosition;
                this.myRenderer.SetPosition(i, new Vector3(candyPos.x, candyPos.y, MY_DEPTH));

                this.outCircles[i].transform.localPosition = new Vector3(candyPos.x, candyPos.y, OUTCIRCLE_DEPTH);
                this.inCircles[i].transform.localPosition = new Vector3(candyPos.x, candyPos.y, INCIRCLE_DEPTH);
            }
        }
        
        public void AddPoint(GameObject _obj) {
            if(this.selectedObjects.Count == 0) {
                this.gameObject.SetActive(true);
            }
            this.selectedObjects.Add(_obj);

            this.outCircles[this.selectedObjects.Count-1].SetActive(true);
            this.inCircles[this.selectedObjects.Count-1].SetActive(true);

            this.myRenderer.positionCount = this.selectedObjects.Count;
        }

        public void RemoveLastPoint() {
            this.outCircles[this.selectedObjects.Count-1].SetActive(false);
            this.inCircles[this.selectedObjects.Count-1].SetActive(false);

            this.selectedObjects.RemoveAt(this.selectedObjects.Count-1);

            this.myRenderer.positionCount = this.selectedObjects.Count;
        }

        public void Clear() {
            for(int i = 0; i < this.selectedObjects.Count; ++i) {
                this.outCircles[i].SetActive(false);
                this.inCircles[i].SetActive(false);
            }
            this.selectedObjects.Clear();
            this.myRenderer.positionCount = 0;
            this.gameObject.SetActive(false);
        }
    }
}