// PersistenceHandler

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// This script controls all the saving and loading procedures that require XML stuff
/// </summary>
public class PersistenceHandler {
	public static T LoadFromFile<T>(string fileName) {
		try {
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			string filePath = Application.persistentDataPath + "/saveData/" + fileName + ".xml";
			if (File.Exists(filePath)) {
				FileStream readStream = new FileStream(filePath, FileMode.Open);
				T loadedData = (T)serializer.Deserialize(readStream);
				readStream.Close();
				return loadedData;
			}
			else return default(T);
		}
		catch (Exception e) {
			Debug.LogError("an error occurred when trying to load xml file " + fileName + "! error: " + e.ToString());
			return default(T);
		}
	}

	public static T LoadFromFile<T>(TextAsset textAssetFile) {
		try {
			XmlSerializer serializer = new XmlSerializer(typeof(T));
			using(StringReader reader = new StringReader(textAssetFile.text)) {
				return (T)serializer.Deserialize(reader);
			}
		}
		catch (Exception e) {
			Debug.LogError("an error occurred when trying to load textAsset xml file " + textAssetFile.name + "! error: " + e.ToString());
			return default(T);
		}
	}

	public static void SaveToFile<T>(T dataToSave, string fileName, bool notifyMsg = true) {
		try {
			XmlSerializer serializer = new XmlSerializer(typeof(T));

			if (!Directory.Exists(Application.persistentDataPath + "/saveData/")) {
				Directory.CreateDirectory(Application.persistentDataPath + "/saveData/");
			}

			string filePath = Application.persistentDataPath + "/saveData/" + fileName + ".xml";

			StreamWriter writer = new StreamWriter(filePath);
			serializer.Serialize(writer, dataToSave);
			writer.Close();
			if (notifyMsg) {
				Debug.LogError("saved at: " + filePath);
			}
		}
		catch (Exception e) {
			Debug.LogError("an error occurred while trying to save game data! error: " + e.ToString());
		}

	}
}

