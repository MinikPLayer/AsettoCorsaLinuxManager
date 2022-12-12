namespace AcUtils.DataManagers;

public class IniElementAttribute : Attribute
{
    public string Section;
    public string Name;

    public IniElementAttribute(string section, string name)
    {
        this.Section = section;
        this.Name = name;
    }
}

public class IniListAttribute : Attribute
{
    public string Name;
    public IniListAttribute(string name) => this.Name = name;
}
