using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using UnityEngine;
using Test.Util.Xml;

namespace Test.UI.ScoreTable {
    public class HighScoreManager : Pattern.MonoSingleton<HighScoreManager> {
        private const int SAVE_SCORE_NUM_MAX = 5;

        private XPathNavigator navigator;
        
        void Start() {
            XmlNodeList scoreNodeList = XmlLoader.LoadXmlNodeList("HighScoreInfo", "ScoreInfo");
            this.navigator = XmlLoader.GetXmlNavigator("HighScoreInfo");
        }

        void Update()
        {

        }

        void OnDestroy() {
            XmlLoader.SaveXml("HighScoreInfo");
            XmlLoader.UnloadXmlNodeList("HighScoreInfo", "ScoreInfo");
        }

        public void AddHighScore(int _score) {
            XPathNodeIterator nodeIterator = this.navigator.Select("/HighScoreInfo/ScoreInfo");

            if(nodeIterator.Count == 0) {
                nodeIterator = this.navigator.Select("/HighScoreInfo");
                nodeIterator.MoveNext();
                nodeIterator.Current.AppendChildElement(this.navigator.Prefix, "ScoreInfo", "", "");
                nodeIterator.Current.MoveToFirstChild();
                nodeIterator.Current.AppendChildElement(this.navigator.Prefix, "Score", "", _score.ToString());
                nodeIterator.Current.AppendChildElement(this.navigator.Prefix, "Date", "", System.DateTime.Now.ToString("yyyy. MM. dd"));
            } else {
                while(nodeIterator.MoveNext()) {
                    nodeIterator.Current.MoveToChild("Score", "");
                    if(nodeIterator.Current.ValueAsInt < _score) {
                        nodeIterator.Current.MoveToParent();
                        if(nodeIterator.CurrentPosition == 1) {
                            nodeIterator.Current.InsertElementBefore(this.navigator.Prefix, "ScoreInfo", "", "");
                            nodeIterator.Current.MoveToPrevious();
                        } else {
                            nodeIterator.Current.MoveToPrevious();
                            nodeIterator.Current.InsertElementAfter(this.navigator.Prefix, "ScoreInfo", "", "");
                            nodeIterator.Current.MoveToNext();
                        }
                        break;
                    }
                }
                if(nodeIterator.Current.Name == "Score") {
                    nodeIterator.Current.MoveToParent();
                    nodeIterator.Current.InsertElementAfter(this.navigator.Prefix, "ScoreInfo", "", "");
                    nodeIterator.Current.MoveToNext();
                }
                
                nodeIterator.Current.AppendChildElement(this.navigator.Prefix, "Score", "", _score.ToString());
                nodeIterator.Current.AppendChildElement(this.navigator.Prefix, "Date", "", System.DateTime.Now.ToString("yyyy. MM. dd"));

                nodeIterator = this.navigator.Select("/HighScoreInfo/ScoreInfo");
                if(nodeIterator.Count > SAVE_SCORE_NUM_MAX) {
                    while(nodeIterator.MoveNext()) {}
                    nodeIterator.Current.DeleteSelf();
                }
            }
        }

        public XPathNodeIterator GetScoreNodeIterator() {
            return this.navigator.Select("/HighScoreInfo/ScoreInfo");
        }

        public void SaveHighScore() {
            XmlLoader.SaveXml("HighScoreInfo");
        }
    }
}