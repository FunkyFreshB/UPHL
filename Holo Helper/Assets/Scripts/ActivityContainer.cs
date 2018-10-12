using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("AcitivitlistsAndInstructions")]
public class ActivityContainer
{
    [XmlArray("Activities"), XmlArrayItem("Activity")]
    public List<Activity> activities;

    public ActivityContainer()
    {
        activities = new List<Activity>();
    }


    public void Save(string path)
    {
        var serializer = new XmlSerializer(typeof(ActivityContainer));
        using (var stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static ActivityContainer Load(string path)
    {
        var serializer = new XmlSerializer(typeof(ActivityContainer));
        using (var stream = new FileStream(path, FileMode.Open))
        {
            return serializer.Deserialize(stream) as ActivityContainer;
        }
    }

    //Loads the xml directly from the given string. Useful in combination with www.text.
    //Kommer förmodligen inte användas
    public static ActivityContainer LoadFromText(string text)
    {
        var serializer = new XmlSerializer(typeof(ActivityContainer));
        return serializer.Deserialize(new StringReader(text)) as ActivityContainer;
    }


    public void createAcitivity(string name)
    {
        activities.Add(new Activity(name));
    }


    public void removeActivity()
    {

    }

}