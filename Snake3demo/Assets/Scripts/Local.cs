
using System;
using System.Collections.Generic;
//using System.Collections.Specialized;
using System.Xml;
using UnityEngine;
//TODO: use dictinary instead of list
public static class local
{

public static class general {


public static class field {
			public static int width;
			public static int height;
			public static int depth;


	}

public static class snake {
			public static int count;
			public static int start_size;


	}

public static class apple {
			public static int count;
			public static int delay;


	}

	}

    
    private static XmlNode FindSubNode(XmlNode node, string id)
	{
		foreach (XmlNode item in node.ChildNodes)
		{
			if (item.Attributes["id"].Value == id)
				return item;
		}
		
		return null;
	}

    public static void init(string xmlfile)
        {
            TextAsset textAsset = (TextAsset)Resources.Load(xmlfile);
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(textAsset.text);

{
    XmlNode item = xDoc.DocumentElement.GetElementsByTagName("general")[0];
    


			general.field.width = 0; 
			int.TryParse(FindSubNode(item, "field.width").Attributes["value"].Value, out general.field.width);
			general.field.height = 0; 
			int.TryParse(FindSubNode(item, "field.height").Attributes["value"].Value, out general.field.height);
			general.field.depth = 0; 
			int.TryParse(FindSubNode(item, "field.depth").Attributes["value"].Value, out general.field.depth);
			general.snake.count = 0; 
			int.TryParse(FindSubNode(item, "snake.count").Attributes["value"].Value, out general.snake.count);
			general.snake.start_size = 0; 
			int.TryParse(FindSubNode(item, "snake.start_size").Attributes["value"].Value, out general.snake.start_size);
			general.apple.count = 0; 
			int.TryParse(FindSubNode(item, "apple.count").Attributes["value"].Value, out general.apple.count);
			general.apple.delay = 0; 
			int.TryParse(FindSubNode(item, "apple.delay").Attributes["value"].Value, out general.apple.delay);




}

        }


    public static void free()
    {

    }
}
