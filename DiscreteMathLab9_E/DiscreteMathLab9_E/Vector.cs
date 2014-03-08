using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscreteMathLab9_Z
{
    class Vector
    {
        private int[] data = new int[32];
        private int size = 0;

        public void Add(int item)
        {
            EnsureCapacity();
            data[size++] = item;
        }

        private void EnsureCapacity()
        {
            if (size == data.Length)
            {
                int[] extendedData = new int[data.Length * 2];
                for (int i = 0; i < data.Length; i++)
                    extendedData[i] = data[i];
                data = extendedData;
            }
        }

        public int Get(int index)
        {
            return data[index];
        }

        public void Set(int index, int item)
        {
            data[index] = item;
        }

        public int GetSize()
        {
            return size;
        }

        public bool IsEmpty()
        {
            return size == 0;
        }
    }
}
