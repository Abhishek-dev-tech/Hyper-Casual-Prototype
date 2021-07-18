// SaveManager
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	public PlayerSave state;

	private string name = "66666666";

	public static SaveManager Instance
	{
		get;
		set;
	}

	private void Awake()
	{
		Instance = this;
		Load();
	}

	public void Save()
	{
		PlayerPrefs.SetString(name, Serialize(state));
	}

	public void Load()
	{
		if (PlayerPrefs.HasKey(name))
		{
			state = Deserialize<PlayerSave>(PlayerPrefs.GetString(name));
		}
		else
		{
			NewSave();
		}
	}

	public void NewSave()
	{
		state = new PlayerSave();
		Save();
		MonoBehaviour.print("Creating new save file");
	}

	public string Serialize<T>(T toSerialize)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		StringWriter stringWriter = new StringWriter();
		xmlSerializer.Serialize(stringWriter, toSerialize);
		return stringWriter.ToString();
	}

	public T Deserialize<T>(string toDeserialize)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		StringReader textReader = new StringReader(toDeserialize);
		return (T)xmlSerializer.Deserialize(textReader);
	}
}
