using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Test.Util.Xml;

namespace Test.UI.ScoreTable {
    public class HighScoreBoardController : MonoBehaviour {
        private const float SCORE_SPACE_Y = 130.0f; 

        private List<ScoreTableController> scoreTableList = new List<ScoreTableController>();
        private GameObject dotLines;
        private GameObject scoreTables;

        void Awake() {
            this.scoreTables = this.transform.Find("ScoreTables").gameObject;
            this.dotLines = this.scoreTables.transform.Find("DotLines").gameObject;
        }

        void Start() {
            CreateScoreTables();
        }

        private void CreateScoreTables() {
            GameObject scoreTablePrefab = Util.PrefabFactory.Instance.FindPrefab("ScoreTable");
            if(scoreTablePrefab == null) {
                scoreTablePrefab = Util.PrefabFactory.Instance.CreatePrefab("Lobby", "ScoreTable", true);
            }
            
            XmlNodeList scoreNodeList = XmlLoader.LoadXmlNodeList("HighScoreInfo", "ScoreInfo");
            for(int i = 0; i < scoreNodeList.Count; ++i) {
                ScoreTableController scoreTable = Instantiate(scoreTablePrefab, this.scoreTables.transform).GetComponent<ScoreTableController>();
                scoreTable.SetData(i + 1
                    , System.Convert.ToInt32(scoreNodeList[i].SelectSingleNode("Score").InnerText)
                    , scoreNodeList[i].SelectSingleNode("Date").InnerText);
                scoreTable.transform.localPosition = new Vector3(0.0f, -SCORE_SPACE_Y * i, 0.0f);
                this.scoreTableList.Add(scoreTable);
            }

            XmlLoader.UnloadXmlNodeList("HighScoreInfo", "ScoreInfo");

            GameObject linePrefab = Util.PrefabFactory.Instance.FindPrefab("Line_Dot");
            if(linePrefab == null) {
                linePrefab = Util.PrefabFactory.Instance.CreatePrefab("Lobby", "Line_Dot", true);
            }

            for(int i = 0; i < scoreNodeList.Count; ++i) {
                GameObject line = Instantiate(linePrefab, this.dotLines.transform);
                line.transform.localPosition = new Vector3(0.0f, -SCORE_SPACE_Y * i - SCORE_SPACE_Y * 0.5f, 0.0f);
            }
        }
    }
}