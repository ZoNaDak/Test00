using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using UnityEngine;

namespace Test.Util.Xml {
    public static class XmlLoader {
        private struct XmlInfo {
            public XmlDocument document { get; private set;}
            public int refNum { get; private set; }

            public XmlInfo(XmlDocument _document) {
                this.document = _document;
                this.refNum = 0;
            }

            public int AddRef() {
                this.refNum++;
                return this.refNum;
            }

            public int SubRef() {
                this.refNum--;
                return this.refNum;
            }
        }

        private static Dictionary<string, XmlInfo> xmlDocumentList = new Dictionary<string, XmlInfo>();

        public static XmlNodeList LoadXmlNodeList(string _xmlName, string _nodeName) {
            XmlDocument document;
            if(xmlDocumentList.ContainsKey(_xmlName)) {
                document = xmlDocumentList[_xmlName].document;
                xmlDocumentList[_xmlName].AddRef();
            } else {
                TextAsset textAsset = Resources.Load(Path.Combine("XmlFiles", _xmlName)) as TextAsset;
                if(textAsset == null) {
                    throw new System.NullReferenceException(string.Format("XmlFile is None : {0}", _xmlName));
                }
                document = new XmlDocument();
                document.LoadXml(textAsset.text);
                XmlInfo xmlInfo = new XmlInfo(document);
                xmlDocumentList.Add(_xmlName, xmlInfo);
                xmlInfo.AddRef();

                Resources.UnloadAsset(textAsset);
            }

            return document.SelectNodes(string.Format("{0}/{1}", _xmlName, _nodeName));
        }

        public static bool UnloadXmlNodeList(string _xmlName, string _nodeName) {
            if(!xmlDocumentList.ContainsKey(_xmlName)) {
                Debug.LogError(string.Format("Can't Unload XmlDocument : XmlDocument is None. : {0}", _xmlName));
                return false;
            }

            xmlDocumentList[_xmlName].SubRef();
            if(xmlDocumentList[_xmlName].refNum == 0) {
                
                xmlDocumentList[_xmlName].document.RemoveAll();
                xmlDocumentList.Remove(_xmlName);
            }
            return true;
        }

        public static bool SaveXml(string _xmlName) {
            if(!xmlDocumentList.ContainsKey(_xmlName)) {
                Debug.LogError(string.Format("Can't Unload XmlDocument : XmlDocument is None. : {0}", _xmlName));
                return false;
            }
            
            xmlDocumentList[_xmlName].document.Save(Path.Combine("./Assets/Resources/XmlFiles", string.Format("{0}.xml", _xmlName)));
            return true;
        }

        public static XPathNavigator GetXmlNavigator(string _xmlName) {
            if(!xmlDocumentList.ContainsKey(_xmlName)) {
                Debug.LogError(string.Format("Can't Unload XmlDocument : XmlDocument is None. : {0}", _xmlName));
                return null;
            }

            XPathNavigator navigator = xmlDocumentList[_xmlName].document.CreateNavigator();

            return navigator;
        }
    }
}