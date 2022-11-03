using System;
using UnityEngine;

/**
 * This class is responsible for keeping the player's progress
 */
[Serializable]
public class PlayerData
{
    [SerializeField]
    private int goodData;
    [SerializeField]
    private int badData;

    public PlayerData() : this(0, 0) { }

    public PlayerData(int goodData, int badData)
    {
        this.goodData = goodData;
        this.badData = badData;
    }

    public int GoodData
    {
        get => goodData;
        set => goodData = value;
    }

    public int BadData
    {
        get => badData;
        set => badData = value;
    }
}