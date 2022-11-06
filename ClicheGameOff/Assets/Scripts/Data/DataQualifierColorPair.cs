using System;
using UnityEngine;
using Utils;

namespace Data
{
    [Serializable]
    public class DataQualifierColorPair : Pair<DataQualifier, Color>
    {
        public DataQualifierColorPair(DataQualifier one, Color two) : base(one, two)
        { }
    }
}