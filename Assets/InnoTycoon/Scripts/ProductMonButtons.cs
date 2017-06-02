namespace innovation.tycoon
{

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using System.IO;


public class ProductMonButtons : UIBuilder
{
    public Button prefab;
    public TextAsset textAsset;

    protected override void Awake()
    {
        base.Awake();
        //if (Application.systemLanguage.ToString() == "Portuguese"){ // referencia posterior para internalizar
        // xml.Load(UnityEngine.Application.dataPath + "/InnoTycoon/Schemas/productMonetization.xml");

        XmlDocument xml = new XmlDocument ();
        xml.LoadXml (textAsset.text);
        XmlNode root = xml.FirstChild;

        for (int i = 0; i < root.SelectNodes("descendant::monetization").Count; i++) {
            XmlNodeList item = root.SelectNodes("descendant::monetization")[i].ChildNodes;
            string name = item[0].InnerXml;
            string price = item[4].InnerXml;
            string temp = name + " (" + price + ")";
            bool interactable = false;
            if (item[5].InnerXml == "true")
                interactable = true;
            AddButton(prefab, temp, () => OnButton_Extra(temp), interactable);
            //source: https://forum.unity3d.com/threads/xml-reading-a-xml-file-in-unity-how-to-do-it.44441/
        }
    }

    private void OnButton_Extra(string message)
    {
        Debug.Log("Ativou item: " + message);
    }
}

}