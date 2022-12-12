using System;
using System.Collections.Generic;
using System.IO;
using AcUtils.DataManagers;
using AcUtils.Utils;
using Xunit;

namespace AcUtils.Tests;

public class IniFileTests
{
    public static IEnumerable<object[]> GetTestData()
    {
        yield return new object[]
        {
            File.ReadAllText(Path.Combine("TestData", "test_race.ini")),
            new IniFile
            {
                { "REPLAY", 
                    new Dictionary<string, string> 
                        { { "FILENAME", "" }, { "ACTIVE", "0" } } },
                { "LIGHTING",
                    new Dictionary<string, string>
                        { { "SUN_ANGLE", "-48" }, { "TIME_MULT", "1" }, { "CLOUD_SPEED", "0.2" } } },
                { "GROOVE",
                    new Dictionary<string, string>
                        { { "VIRTUAL_LAPS", "10" }, { "MAX_LAPS", "1" }, { "STARTING_LAPS", "1" } } },
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void TestIniFileOutput(string expectedOutput, IniFile data)
    {
        var actualOutput = data.ToString();
        Assert.Equal(expectedOutput, actualOutput);
    }

    [Theory]
    [MemberData(nameof(GetTestData))]
    public void TestIniFileInput(string input, IniFile expectedOutput)
    {
        // Create some random variation
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '\n')
            {
                if (Random.Shared.NextDouble() <= 0.2)
                    input = input.Insert(i, ';' + StringUtils.GetRandomBase64String(Random.Shared.Next(5, 10)));

                while (Random.Shared.NextDouble() <= 0.5)
                    input = input.Insert(i++, "\n");
            }
        }
        
        var iniFile = new IniFile(input);
        Assert.Equal(expectedOutput, iniFile);
    }

    [Fact]
    public void TestInputOutputInteroperability()
    {
        var iniFile = new IniFile();
        for (int i = 0; i < 100; i++)
        {
            var sectionName = StringUtils.GetRandomBase64String(10);
            var section = new Dictionary<string, string>();
            iniFile[sectionName] = section;
            for (int j = 0; j < 100; j++)
            {
                var name = StringUtils.GetRandomBase64String(10);
                var value = StringUtils.GetRandomBase64String(10);

                section[name] = value;
            }
        }

        var serialized = iniFile.ToString();
        var deserialized = new IniFile(serialized);
        Assert.Equal(iniFile, deserialized);

        var serialized2 = deserialized.ToString();
        Assert.Equal(serialized, serialized2);
    }
}