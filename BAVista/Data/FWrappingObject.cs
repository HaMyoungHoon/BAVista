using System;
using System.Collections.Generic;

namespace BAVista.Data
{
    internal class FWrappingObject<TEnum, TValue> where TEnum : notnull
    {
        private readonly TValue[] data;
        private static Dictionary<TEnum, int> _enumKey = new();
        static FWrappingObject()
        {
            int[] intValues = (int[])Enum.GetValues(typeof(TEnum));
            TEnum[] enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));

            for (int i = 0; i < intValues.Length; i++)
            {
                _enumKey.Add(enumValues[i], intValues[i]);
            }
        }

        public FWrappingObject(int count)
        {
            data = new TValue[count];
        }
        public TValue this[TEnum key]
        {
            get => data[_enumKey[key]];
            set => data[_enumKey[key]] = value;
        }
    }
}
