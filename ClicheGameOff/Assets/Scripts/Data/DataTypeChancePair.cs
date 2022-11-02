using System;
using Utils;

namespace Data
{
    [Serializable]
    public class DataTypeChancePair : Pair<DataType, float>
    {
        public DataTypeChancePair(DataType one, float two) : base(one, two)
        { }
    }
}