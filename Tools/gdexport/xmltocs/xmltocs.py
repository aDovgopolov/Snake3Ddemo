# -*- coding: utf-8 -*-

import argparse

parser = argparse.ArgumentParser(description="Exports xml file to c# code")
parser.add_argument("-s", "--src", help = "source spreadsheet", required = True)
parser.add_argument("-d", "--dest", help = "destination file", required = True)
parser.add_argument("-ns", "--namespace", help = "namespace", required = True)

args = parser.parse_args()

xmlfile = args.src
csfile = args.dest
namespace = args.namespace

static_class_read_template = \
"""
{
    XmlNode item = xDoc.DocumentElement.GetElementsByTagName("%s")[0];
    %s
}
"""

class_read_template = \
"""
        %s.items.Clear();
        XmlNode %s_node = xDoc.DocumentElement.GetElementsByTagName("%s")[0];
  
        %s %s_prev = null;
            
        foreach (XmlNode item in %s_node.ChildNodes)
        {
%s
            if (%s.get(id, false) == null)
            {
                %s current = new %s(%s);
                
                if (%s_prev != null)
                {
                    %s_prev._next = current;
                    current._prev = %s_prev;
                }
                %s_prev = current;
                %s.items.Add(current);
            }
            else
                Debug.LogError("%s::%s: already contains key '" + id + "'");

        }
"""

#name, vars
static_subclass_template = \
"""
        public static class %s {
			%s
		}
"""

#type, name, init_value
static_field_template = \
"""
        public static  %s %s;
"""

#name, vars, subclasses
static_class_template = \
"""
public static class %s {
%s
%s
	}
"""

#name, name, name, name, fields_read_lines, name, name, name, field_id_list, name
class_template = \
"""
public class %s
    {
%s
        
        public %s _prev = null;
        public %s _next = null;        
        
        public static List<%s> items = new List<%s>();

        public %s(%s)
        {
%s
        }

        public static %s get(string id, bool showError = true)
        {
            int index = items.FindIndex(x => x.id == id);

            if (index == -1)
            {
                if (showError)
                    Debug.LogError("%s::%s: key not found '" + id + "'");
                return null;
            }

            return items[index];
        }
    }
"""
#name, fields, name, name, name, params, fields_init, name, name


cs_template = \
"""
using System;
using System.Collections.Generic;
//using System.Collections.Specialized;
using System.Xml;
using UnityEngine;
//TODO: use dictinary instead of list
public static class %s
{
%s
    
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
%s
        }


    public static void free()
    {
%s
    }
}
"""

#namespace, classes_init, classes_read, classes_free
class Field:
    def __init__(self, id, type):
        self.id = id
        self.original_id = id
        self.base_type = type
        
        self.init_value = "0"

        if id in "base,lock":
            self.id = id + "_"

        self.type = type
        if self.type == "*":
            self.type = "string[]"

        self.isArray = "[]" in self.type
        if self.isArray:
            self.base_type = self.base_type.replace("[]","")

        if self.base_type == "bool":
            self.init_value = "false"

        if self.base_type == "string":
            self.init_value = ""

        if self.base_type == "float":
            self.init_value = "0f"

        if self.type == "":
            self.type = "string"

    def get_static_field_line(self):
        return "\t\t\tpublic static %s %s;" % (self.type, self.id)

    def get_field_line(self):
        return "\t\tpublic %s %s;" % (self.type, self.id)

    def get_param_line(self):
        return "%s %s" % (self.type, self.id)

    def get_init_line(self):
        return "\t\t\tthis.%s = %s;" % (self.id, self.id)

    def get_static_read_line(self, parent_class_id):
        if len(parent_class_id) > 0:
            parent_class_id += "."

        if not self.isArray:
            if "string" in self.type:
                return "\t\t\t%s%s = FindSubNode(item, \"%s\").Attributes[\"value\"].Value;" % (parent_class_id, self.id, self.original_id)
            elif "bool" in self.type:
                return "\t\t\t%s%s = FindSubNode(item, \"%s\").Attributes[\"value\"].Value == \"1\";" % (parent_class_id, self.id, self.original_id);
            else:
                return "\t\t\t%s%s = %s; \n" \
                       "\t\t\t%s.TryParse(FindSubNode(item, \"%s\").Attributes[\"value\"].Value, out %s%s);" % (
                       parent_class_id, self.id, self.init_value, self.type, self.original_id, parent_class_id, self.id)
        else:
            if "string" in self.type:
                return "\t\t\t%s%s = (FindSubNode(item, \"%s\").Attributes[\"value\"].Value).Split(',');" % (parent_class_id, self.id, self.original_id)
            else:
                return "\t\t\t%s%s = Array.ConvertAll((FindSubNode(item, \"%s\").Attributes[\"value\"].Value).Split(','), %s.Parse);" % (parent_class_id, self.id, self.original_id, self.base_type)


    def get_read_line(self):
        if not self.isArray:
            if "string" in self.type:
                return "\t\t\t%s %s = item.Attributes[\"%s\"].Value;" % (self.type, self.id, self.original_id)
            elif "bool" in self.type:
                return "\t\t\t%s %s = item.Attributes[\"%s\"].Value == \"1\";" % (self.type, self.id, self.original_id);
            else:
                return "\t\t\t%s %s = %s; \n" \
                       "\t\t\t%s.TryParse(item.Attributes[\"%s\"].Value, out %s);" % (self.type, self.id, self.init_value, self.type, self.original_id, self.id)
        else:
            if "string" in self.type:
                return "\t\t\t%s %s = (item.Attributes[\"%s\"].Value).Split(',');" % (self.type, self.id, self.original_id)
            else:
                return "\t\t\t%s %s = Array.ConvertAll((item.Attributes[\"%s\"].Value).Split(','), %s.Parse);" % (self.type, self.id, self.original_id, self.base_type)

class StaticClass:
    def __init__(self, id):
        self.id = id
        self.vars = []
        self.subclasses = {}

    def add_var(self, var):
        self.vars.append(var);

    def add_value(self, id, type):
        if "." in id:
            subclassid, itemid = id.split(".")

            if not subclassid in self.subclasses:
                self.subclasses[subclassid] = StaticClass(subclassid)

            subclass = self.subclasses[subclassid]
            var = Field(itemid, type)
            var.original_id = id
            subclass.add_var(var)
        else:
            var = Field(id, type)
            self.add_var(var)

    def get_read_line(self, parent_class_id = ""):
        fields_read_str = ""

        if len(parent_class_id) > 0:
            parent_class_id += "." + self.id
        else:
            parent_class_id = self.id

        for f in self.vars:
            fields_read_str += f.get_static_read_line(parent_class_id) + "\n"

        for key in self.subclasses.keys():
            s = self.subclasses[key]
            fields_read_str += s.get_read_line(self.id)

        if parent_class_id == self.id:
            return static_class_read_template % (self.id, "\n\n\n" + fields_read_str + "\n\n\n")
        else:
            return fields_read_str

    def get_def_line(self):
        vars_str = ""
        subclasses_str = ""

        for f in self.vars:
            vars_str += f.get_static_field_line() + "\n"

        for key in self.subclasses.keys():
            s = self.subclasses[key]
            subclasses_str += s.get_def_line()

        return static_class_template % (self.id, vars_str, subclasses_str)


class Class:
    def __init__(self, id):
        self.id = id
        self.fields = []

    def add_field(self, field):
        self.fields.append(field)

    def get_def_line(self):
        fields_str = ""
        params_str = ""
        fields_init_str = ""
        lastField = self.fields[len(self.fields) - 1]
        for f in self.fields:
            fields_str += f.get_field_line() + "\n"
            if f == lastField:
                params_str += f.get_param_line()
            else:
                params_str += f.get_param_line() + ", "
            fields_init_str += f.get_init_line() + "\n"

        return class_template % (self.id, fields_str, self.id, self.id, self.id, self.id, self.id, params_str, fields_init_str, self.id, namespace, self.id)

    def get_read_line(self):
        fields_read_str = ""
        field_ids_str = ""
        lastField = self.fields[len(self.fields) - 1]
        for f in self.fields:
            fields_read_str += f.get_read_line() + "\n"
            if f == lastField:
                field_ids_str += f.id
            else:
                field_ids_str += f.id + ", "

        return class_read_template % (self.id,
                                      self.id, self.id,
                                      self.id, self.id,
                                      self.id,
                                      fields_read_str,
                                      self.id,
                                      self.id, self.id, field_ids_str,
                                      self.id,
                                      self.id,
                                      self.id,
                                      self.id,
                                      self.id,
                                      namespace, self.id)

#name, name, name, name, fields_read_lines, name, name, name, field_id_list
import xml.etree.ElementTree
root = xml.etree.ElementTree.parse(xmlfile).getroot()
result = ""
classes_init_str = "";
classes_read_str = "";
classes_free_str = "";

for child in root:
    print "Processing ", child.tag
    if not child.attrib.get("_static"):
        new_class = Class(child.tag)

        for key, value in child.attrib.items():
            if value == "type":
                value = "string"
            if "-" in key:
                continue
            new_class.add_field(Field(key, value))

        classes_init_str += new_class.get_def_line()
        classes_read_str += new_class.get_read_line()
        classes_free_str += "\t\t%s.items.Clear();\n" % (child.tag)

    else:
        new_class = StaticClass(child.tag)

        for item in child:
            new_class.add_value(item.attrib.get('id'), item.attrib.get('type'))

        classes_init_str += new_class.get_def_line()
        classes_read_str += new_class.get_read_line()

print "Making code"

result = cs_template % (namespace, classes_init_str, classes_read_str, classes_free_str)

write = False;
try:
    with open(csfile, "r") as rd:
        data = rd.read()
        if data != result:
            write = True
except IOError:
    write = True

if write:
    with open(csfile, "w") as rd:
        rd.write(result)
        print "File saved: ", csfile
else:
    print "No changes for: ", csfile

