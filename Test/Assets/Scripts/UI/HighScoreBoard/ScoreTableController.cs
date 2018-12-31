using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Test.Util;

namespace Test.UI.ScoreTable {
    public class ScoreTableController : MonoBehaviour {
        private Image medal;
        private Text ranking;
        private Text score;
        private Text date;

        void Awake() {
            this.medal = this.transform.Find("Medal").GetComponent<Image>();
            this.ranking = this.transform.Find("Ranking").GetComponent<Text>();
            this.score = this.transform.Find("Score").GetComponent<Text>();
            this.date = this.transform.Find("Date").GetComponent<Text>();
        }

        public void SetData(int _ranking, int _score, string _date) {
            switch(_ranking) {
                case 1:
                    this.ranking.gameObject.SetActive(false);
                    this.medal.sprite = SpriteFactory.Instance.GetSprite("Lobby", "ic_Medal_Gold");
                break;
                case 2:
                    this.ranking.gameObject.SetActive(false);
                    this.medal.sprite = SpriteFactory.Instance.GetSprite("Lobby", "ic_Medal_Silver");
                break;
                case 3:
                    this.ranking.gameObject.SetActive(false);
                    this.medal.sprite = SpriteFactory.Instance.GetSprite("Lobby", "ic_Medal_Bronze");
                break;
                default:
                    this.medal.gameObject.SetActive(false);
                    this.ranking.text = _ranking.ToString();
                break;
            }

            this.score.text = _score.ToString();
            this.date.text = _date;
        }
    }
}