using System.Collections.Generic;
using Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Constants", menuName = "Cliche/Game Constants", order = 0)]
public class GameConstants : ScriptableObject
{
    [SerializeField]
    public int initialHardDriveSize = 10;
    [SerializeField]
    public Color defaultColorQualifier;
    [SerializeField] 
    private List<DataQualifierColorPair> colorPerQualifier;

    public Color GetColorForQualifier(DataQualifier qualifier)
    {
        var color = colorPerQualifier.Find(pair => pair.One.Equals(qualifier));
        return color?.Two ?? defaultColorQualifier;
    }
}