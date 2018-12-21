using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.Util {
    public class LineController : MonoBehaviour {
        const float MY_DEPTH = -2.0f;
        const float OUTCIRCLE_DEPTH = -1.0f;
        const float INCIRCLE_DEPTH = -3.0f;

        private LineRenderer myRenderer;

        private Stack<Vector3> points = new Stack<Vector3>();
        private List<GameObject> outCircles = new List<GameObject>();
        private List<GameObject> inCircles = new List<GameObject>();

        public int PointNum { get { return this.points.Count; } }

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
        
        public void AddPoint(Vector2 _addPoint) {
            if(this.points.Count == 0) {
                this.gameObject.SetActive(true);
            }
            this.points.Push(new Vector3(_addPoint.x, _addPoint.y, MY_DEPTH));

            this.outCircles[this.points.Count-1].SetActive(true);
            this.outCircles[this.points.Count-1].transform.localPosition = new Vector3(_addPoint.x, _addPoint.y, OUTCIRCLE_DEPTH);
            this.inCircles[this.points.Count-1].SetActive(true);
            this.inCircles[this.points.Count-1].transform.localPosition = new Vector3(_addPoint.x, _addPoint.y, INCIRCLE_DEPTH);

            this.myRenderer.positionCount = this.points.Count;
            this.myRenderer.SetPositions(this.points.ToArray());
        }

        public void RemoveLastPoint() {
            this.outCircles[this.points.Count-1].SetActive(false);
            this.inCircles[this.points.Count-1].SetActive(false);

            this.points.Pop();

            this.myRenderer.positionCount = this.points.Count;
            this.myRenderer.SetPositions(this.points.ToArray());
        }

        public void Clear() {
            for(int i = 0; i < this.points.Count; ++i) {
                this.outCircles[i].SetActive(false);
                this.inCircles[i].SetActive(false);
            }
            this.points.Clear();
            this.myRenderer.positionCount = 0;
            this.gameObject.SetActive(false);
        }
    }
}