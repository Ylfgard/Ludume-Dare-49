using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("dialogue")]  
public class DialogueViewer
{
    [XmlElement("sentence")] public Sentence[] sentences;

    public static DialogueViewer Load(TextAsset _xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(DialogueViewer));
        StringReader reader = new StringReader(_xml.text);
        return serializer.Deserialize(reader) as DialogueViewer;
    }
}

public class Sentence
{
    [XmlElement("text")] public string text;
    [XmlElement("duration")] public float duration;
    [XmlElement("fmodSoundPath")] public string fmodSoundPath;
    [XmlElement("triggerEvent")] public bool triggerEvent;
}