
using System;
using System.Collections.Generic;
//using System.Collections.Specialized;
using System.Xml;
using UnityEngine;
//TODO: use dictinary instead of list
public static class data
{

public class affect
    {
		public string value;
		public string[] values;
		public string goal;
		public string action;
		public string type;
		public string id;
		public string param;

        public static List<affect> items = new List<affect>();

        public affect(string value, string[] values, string goal, string action, string type, string id, string param)
        {
			this.value = value;
			this.values = values;
			this.goal = goal;
			this.action = action;
			this.type = type;
			this.id = id;
			this.param = param;

        }

        public static affect get(string id, bool showError = true)
        {
            int index = items.FindIndex(x => x.id == id);

            if (index == -1)
            {
                if (showError)
                    Debug.LogError("data::affect: key not found '" + id + "'");
                return null;
            }

            return items[index];
        }
    }

public class business
    {
		public string base_production;
		public bool locked;
		public float time;
		public float pow;
		public string id;
		public string base_price;

        public static List<business> items = new List<business>();

        public business(string base_production, bool locked, float time, float pow, string id, string base_price)
        {
			this.base_production = base_production;
			this.locked = locked;
			this.time = time;
			this.pow = pow;
			this.id = id;
			this.base_price = base_price;

        }

        public static business get(string id, bool showError = true)
        {
            int index = items.FindIndex(x => x.id == id);

            if (index == -1)
            {
                if (showError)
                    Debug.LogError("data::business: key not found '" + id + "'");
                return null;
            }

            return items[index];
        }
    }

public class general
    {
		public true _list;

        public static List<general> items = new List<general>();

        public general(true _list)
        {
			this._list = _list;

        }

        public static general get(string id, bool showError = true)
        {
            int index = items.FindIndex(x => x.id == id);

            if (index == -1)
            {
                if (showError)
                    Debug.LogError("data::general: key not found '" + id + "'");
                return null;
            }

            return items[index];
        }
    }

public class upgrade
    {
		public string product;
		public string thread;
		public string[] affects;
		public string[] cost;
		public string req_upgrade;
		public string req_trophy;
		public string id;
		public string condition1;
		public string req_param;

        public static List<upgrade> items = new List<upgrade>();

        public upgrade(string product, string thread, string[] affects, string[] cost, string req_upgrade, string req_trophy, string id, string condition1, string req_param)
        {
			this.product = product;
			this.thread = thread;
			this.affects = affects;
			this.cost = cost;
			this.req_upgrade = req_upgrade;
			this.req_trophy = req_trophy;
			this.id = id;
			this.condition1 = condition1;
			this.req_param = req_param;

        }

        public static upgrade get(string id, bool showError = true)
        {
            int index = items.FindIndex(x => x.id == id);

            if (index == -1)
            {
                if (showError)
                    Debug.LogError("data::upgrade: key not found '" + id + "'");
                return null;
            }

            return items[index];
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


        affect.items.Clear();
        XmlNode affect_node = xDoc.DocumentElement.GetElementsByTagName("affect")[0];
        foreach (XmlNode item in affect_node.ChildNodes)
        {
			string value = item.Attributes["value"].Value;
			string[] values = (item.Attributes["values"].Value).Split(',');
			string goal = item.Attributes["goal"].Value;
			string action = item.Attributes["action"].Value;
			string type = item.Attributes["type"].Value;
			string id = item.Attributes["id"].Value;
			string param = item.Attributes["param"].Value;


            if (affect.get(id, false) == null)
                affect.items.Add(new affect(value, values, goal, action, type, id, param));
            else
                Debug.LogError("data::affect: already contains key '" + id + "'");

        }

        business.items.Clear();
        XmlNode business_node = xDoc.DocumentElement.GetElementsByTagName("business")[0];
        foreach (XmlNode item in business_node.ChildNodes)
        {
			string base_production = item.Attributes["base_production"].Value;
			bool locked = item.Attributes["locked"].Value == "1";
			float time = 0f; 
			float.TryParse(item.Attributes["time"].Value, out time);
			float pow = 0f; 
			float.TryParse(item.Attributes["pow"].Value, out pow);
			string id = item.Attributes["id"].Value;
			string base_price = item.Attributes["base_price"].Value;


            if (business.get(id, false) == null)
                business.items.Add(new business(base_production, locked, time, pow, id, base_price));
            else
                Debug.LogError("data::business: already contains key '" + id + "'");

        }

        general.items.Clear();
        XmlNode general_node = xDoc.DocumentElement.GetElementsByTagName("general")[0];
        foreach (XmlNode item in general_node.ChildNodes)
        {
			true _list = 0; 
			true.TryParse(item.Attributes["_list"].Value, out _list);


            if (general.get(id, false) == null)
                general.items.Add(new general(_list));
            else
                Debug.LogError("data::general: already contains key '" + id + "'");

        }

        upgrade.items.Clear();
        XmlNode upgrade_node = xDoc.DocumentElement.GetElementsByTagName("upgrade")[0];
        foreach (XmlNode item in upgrade_node.ChildNodes)
        {
			string product = item.Attributes["product"].Value;
			string thread = item.Attributes["thread"].Value;
			string[] affects = (item.Attributes["affects"].Value).Split(',');
			string[] cost = (item.Attributes["cost"].Value).Split(',');
			string req_upgrade = item.Attributes["req_upgrade"].Value;
			string req_trophy = item.Attributes["req_trophy"].Value;
			string id = item.Attributes["id"].Value;
			string condition1 = item.Attributes["condition1"].Value;
			string req_param = item.Attributes["req_param"].Value;


            if (upgrade.get(id, false) == null)
                upgrade.items.Add(new upgrade(product, thread, affects, cost, req_upgrade, req_trophy, id, condition1, req_param));
            else
                Debug.LogError("data::upgrade: already contains key '" + id + "'");

        }

        }


    public static void free()
    {
		affect.items.Clear();
		business.items.Clear();
		general.items.Clear();
		upgrade.items.Clear();

    }
}
