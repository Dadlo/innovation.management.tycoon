using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

/// <summary>
/// This script controls all the saving and loading procedures called by the other scripts from this mod.
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
			Debug.LogError("an error occurred while trying to save gang mod data! error: " + e.ToString());
		}

	}
}

