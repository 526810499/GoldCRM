using System.Collections;
using System.Collections.Generic;
using System.Xml;
namespace XHD.Excel
{
    public class XmlHelper
	{
		private string fullName = string.Empty;
		public XmlHelper(string fullName)
		{
			this.fullName = fullName;
		}
		public void createXML(string content)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(content);
			xmlDocument.Save(this.fullName);
		}
		public void addChild(string childName, Hashtable element)
		{
			int count = element.Count;
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			XmlElement newChild = xmlDocument.CreateElement(childName);
			xmlDocument.DocumentElement.AppendChild(newChild);
			foreach (DictionaryEntry dictionaryEntry in element)
			{
				XmlElement xmlElement = xmlDocument.CreateElement(dictionaryEntry.Key.ToString());
				xmlElement.InnerText = dictionaryEntry.Value.ToString();
				xmlDocument.DocumentElement.LastChild.AppendChild(xmlElement);
			}
			xmlDocument.Save(this.fullName);
		}
		public void addChild(string querystr, string nodeName, Dictionary<string, string> attributes, string innerText)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			XmlNode xmlNode = xmlDocument.SelectSingleNode(querystr);
			XmlElement xmlElement = xmlDocument.CreateElement(nodeName);
			foreach (string current in attributes.Keys)
			{
				xmlElement.SetAttribute(current, attributes[current]);
			}
			if (!string.IsNullOrEmpty(innerText))
			{
				xmlElement.InnerText = innerText;
			}
			xmlNode.AppendChild(xmlElement);
			xmlDocument.Save(this.fullName);
		}
		public void modifyNodeContent(string querystr, string newcontent)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes(querystr);
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				xmlNode.InnerText = newcontent;
			}
			xmlDocument.Save(this.fullName);
		}
		public void modifyNodeContent(string querystr, Dictionary<string, string> attributes, string innerText)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			XmlNode xmlNode = xmlDocument.SelectSingleNode(querystr);
			if (attributes != null && attributes.Keys.Count > 0)
			{
				foreach (string current in attributes.Keys)
				{
					xmlNode.Attributes[current].Value = attributes[current];
				}
			}
			if (!string.IsNullOrEmpty(innerText))
			{
				xmlNode.InnerText = innerText;
			}
			xmlDocument.Save(this.fullName);
		}
		public void deleteNodes(string querystr)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes(querystr);
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				xmlNode.RemoveAll();
			}
			xmlDocument.Save(this.fullName);
		}
		public XmlNodeList GetChildNodes(XmlNode node)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			return node.ChildNodes;
		}
		public XmlNode GetFirstChildNode(XmlNode node)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			return node.FirstChild;
		}
		public XmlNodeList GetNodes(string queryString)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			return xmlDocument.SelectNodes(queryString);
		}
		public string GetNodeAttribute(XmlNode node, string attributeName)
		{
			string result = "";
			foreach (XmlAttribute xmlAttribute in node.Attributes)
			{
				if (xmlAttribute.Name == attributeName)
				{
					result = xmlAttribute.Value;
					break;
				}
			}
			return result;
		}
		public string GetSingleNodeAttribute(string queryString, string attribute)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			XmlNode xmlNode = xmlDocument.SelectSingleNode(queryString);
			string result = "";
			if (xmlNode.Attributes[attribute] != null)
			{
				result = xmlNode.Attributes[attribute].Value;
			}
			return result;
		}
		public List<string> GetNodeAttributes(string queryString, string conditionContent, Dictionary<string, string> conditionAttributes, string attribute, int? start, int? length)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes(queryString);
			List<string> list = new List<string>();
			int num = start.HasValue ? start.Value : 0;
			int num2 = length.HasValue ? length.Value : (xmlNodeList.Count - num);
			for (int i = num; i < num2; i++)
			{
				if (conditionContent != null && conditionContent.ToString() != string.Empty)
				{
					if (xmlNodeList[i].InnerText == conditionContent)
					{
						if (conditionAttributes != null && conditionAttributes.Count > 0)
						{
							bool flag = true;
							foreach (string current in conditionAttributes.Keys)
							{
								if (conditionAttributes[current] != xmlNodeList[i].Attributes[current].Value)
								{
									flag = false;
								}
							}
							if (flag)
							{
								list.Add(xmlNodeList[i].Attributes[attribute].Value);
							}
						}
						else
						{
							list.Add(xmlNodeList[i].Attributes[attribute].Value);
						}
					}
				}
				else if (conditionAttributes != null && conditionAttributes.Count > 0)
				{
					bool flag = true;
					foreach (string current in conditionAttributes.Keys)
					{
						if (conditionAttributes[current] != xmlNodeList[i].Attributes[current].Value)
						{
							flag = false;
						}
					}
					if (flag)
					{
						list.Add(xmlNodeList[i].Attributes[attribute].Value);
					}
				}
				else
				{
					list.Add(xmlNodeList[i].Attributes[attribute].Value);
				}
			}
			xmlDocument.Save(this.fullName);
			return list;
		}
		public string GetNodeContent(XmlNode node)
		{
			return node.InnerText;
		}
		public List<string> GetNodeContents(string querystr)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			List<string> list = new List<string>();
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes(querystr);
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				list.Add(xmlNode.InnerText);
			}
			xmlDocument.Save(this.fullName);
			return list;
		}
		public bool isBeingNode(string querystr)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(this.fullName);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes(querystr);
			return xmlNodeList.Count > 0;
		}
		public string GetXmlString()
		{
			TxtHelper TxtHelper = new TxtHelper();
			return TxtHelper.read(this.fullName);
		}
	}
}
