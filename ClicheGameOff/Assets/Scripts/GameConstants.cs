using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Data;
using UnityEngine;
using Utils;

[CreateAssetMenu(fileName = "Game Constants", menuName = "Cliche/Game Constants", order = 0)]
public class GameConstants : ScriptableObject
{
    [SerializeField]
    public int initialHardDriveSize = 10;
    [SerializeField]
    public float initialPlayerRadius = 1.0f;
    [SerializeField]
    public float initialMiningRate = 100.0f;
    [SerializeField]
    public Color defaultColorQualifier;
    [SerializeField] 
    private List<DataQualifierColorPair> colorPerQualifier;
    [SerializeField]
    private TextAsset clicheNames;

    private List<string> cliches;
    
    public Color GetColorForQualifier(DataQualifier qualifier)
    {
        var color = colorPerQualifier.Find(pair => pair.One.Equals(qualifier));
        return color?.Two ?? defaultColorQualifier;
    }

    public string GetRandomTitle()
    {
        cliches = Regex.Split(clicheNames.text, Environment.NewLine).ToList();
        return RandomHelper<string>.GetRandomFromList(cliches);
    }
}