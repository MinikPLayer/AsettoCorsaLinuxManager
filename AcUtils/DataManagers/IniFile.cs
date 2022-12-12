using System.Collections;
using System.Collections.Generic;
using System.IO;
using AcUtils.Utils;

namespace AcUtils.DataManagers;

public class IniFile : Dictionary<string, Dictionary<string, string>>
{
    public static class ParsingExceptions
    {
        public class ParsingException : Exception
        {
            protected ParsingException(string message, string lineStr, int line, int position = -1)
                : base(message + " at " + line + (position == -1 ? "" : ":" + position) + " - \"" + lineStr + "\"") {}
        }
        
        public class ValueWithoutSectionException : ParsingException
        {
            public ValueWithoutSectionException(string lineStr, int line, int position = -1) 
                : base("Value without section", lineStr, line, position) {}
        }
    }

    public void UpdateData(string section, string name, object value)
    {
        if(!this.ContainsKey(section))
            this.Add(section, new Dictionary<string, string>());

        string? valueStr;
        if (value is bool b)
            valueStr = b ? "1" : "0";
        else
            valueStr = value.ToString();

        if (valueStr != null) 
            this[section][name] = valueStr;
    }
    
    public IniFile()
    {
    }

    public IniFile(Dictionary<string, Dictionary<string, string>> data) : base(data)
    {
    }

    /// <summary>
    /// Constructor from custom data object
    /// </summary>
    /// <param name="customData">Object of type with "IniAttributes" added to it's fields</param>
    public IniFile(object? customData)
    {
        //var type = customData.GetType();
        // var iniElements = type.GetProperties().Select(x =>
        //     (x.GetValue(customData), x.GetCustomAttributes(typeof(IniElementAttribute), false)));
        var iniElements = ReflectionUtils.GetAttributeFilteredProperties<IniElementAttribute>(customData);

        foreach (var (value, attr) in iniElements)
        {
            var iniAttr = (IniElementAttribute)attr;
            if (value != null) 
                UpdateData(iniAttr.Section, iniAttr.Name, value);
        }

        var iniLists = ReflectionUtils.GetAttributeFilteredProperties<IniListAttribute>(customData);
        foreach (var (ob, listAttribute) in iniLists)
        {
            var listAttr = (IniListAttribute)listAttribute;

            if (ob is not IList list) 
                continue;
            
            for (var i = 0; i < list.Count; i++)
            {
                var listIniElements = ReflectionUtils.GetAttributeFilteredProperties<IniListAttribute>(list[i]);
                foreach (var (el, attr) in listIniElements)
                {
                    var elAttr = (IniListAttribute)attr;
                    if(el != null)
                        UpdateData(listAttr.Name + "_" + i, elAttr.Name, el);
                }
            }
        }
    }
    
    public IniFile(string inputData)
    {
        Dictionary<string, string>? curSubDict = null;

        var lines = inputData.Split('\n');
        for(int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            if(string.IsNullOrEmpty(line))
                continue;

            line = line.Trim();
            var semicolonIndex = line.IndexOf(';');
            if (semicolonIndex != -1)
                line = line.Substring(0, semicolonIndex);

            // Section start
            if (line.StartsWith('[') && line.EndsWith(']'))
            {
                curSubDict = new Dictionary<string, string>();
                var curSection = line.Substring(1, line.Length - 2);
                this[curSection] = curSubDict;
                
                continue;
            }

            var equalIndex = line.IndexOf('=');
            if (equalIndex != -1)
            {
                var name = line.Substring(0, equalIndex);
                var value = line.Substring(equalIndex + 1);

                if (curSubDict == null)
                    throw new ParsingExceptions.ValueWithoutSectionException(line, i);
                
                curSubDict[name] = value;
            }
        }
    }

    public IniFile Clone()
    {
        var dict = new Dictionary<string, Dictionary<string, string>>();
        foreach (var entry in this)
        {
            var subDict = new Dictionary<string, string>();
            foreach (var subEntry in entry.Value)
                subDict.Add(subEntry.Key, subEntry.Value);

            dict.Add(entry.Key, subDict);
        }

        return new IniFile(dict);
    }
    
    public override string ToString()
    {
        string ret = "";
        foreach (var sectionName in Keys)
        {
            var section = this[sectionName];
            ret += $"[{sectionName}]\n";
            foreach (var entry in section)
                ret += $"{entry.Key}={entry.Value}\n";

            ret += "\n";
        }

        // Remove one trailing \n
        return ret.Length > 0 ? ret.Substring(0, ret.Length - 1) : ret;
    }
}