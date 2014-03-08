using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscreteMathLab9_G_Alt
{
    internal class Program
    {
        private static void Main(string[] rgs)
        {
            StreamReader sr = new StreamReader("queuemin2.in");
            string[] words = sr.ReadLine().Split(' ');
            int n = int.Parse(words[0]),
                m = int.Parse(words[1]),
                k = int.Parse(words[2]);
            words = sr.ReadLine().Split(' ');
            int a = int.Parse(words[0]),
                b = int.Parse(words[1]),
                c = int.Parse(words[2]);
            int[] array = new int[k + 1];
            words = sr.ReadLine().Split(' ');
            for (int i = 1; i <= k; i++)
                array[i] = int.Parse(words[i - 1]);
            int[] prevs = new int[3] { array[0], array[1], 0 };
            long result = 0;
            QueueWithMin q = new QueueWithMin();
            StreamWriter log = new StreamWriter("log.txt");
            for (int i = 1; i <= m; i++)
            {
                prevs[2] = prevs[1];
                prevs[1] = prevs[0];
                if (i <= k)
                    prevs[0] = array[i];
                else
                    prevs[0] = a * prevs[2] + b * prevs[1] + c;
                q.PushBack(prevs[0]);
                if (i <= 100)
                    log.WriteLine("i = {0};  head = {1}, tail = {2}", i, q.items.Front, q.items.Back);
            }
            int[] rear_prevs = new int[3];
            log.WriteLine("\\n\\n\\n PART TWO \\n\\n\\n");
            for (int i = 1; i <= n - m + 1; i++)
            {
                result += q.CurrentMin;
                prevs[2] = prevs[1];
                prevs[1] = prevs[0];
                if (i + m <= k)
                    prevs[0] = array[i + m];
                else
                    prevs[0] = a * prevs[2] + b * prevs[1] + c;
                rear_prevs[2] = rear_prevs[1];
                rear_prevs[1] = rear_prevs[0];
                if (i <= k)
                    rear_prevs[0] = array[i];
                else
                    rear_prevs[0] = a * rear_prevs[2] + b * rear_prevs[1] + c;
                if (i <= 100)
                log.WriteLine("i = {0}; y0 = {1}; head = {2}, tail = {3}", i, rear_prevs[0], q.items.Front, q.items.Back);
                q.PushBack(prevs[0]);
                q.Extract(rear_prevs[0]);
            }
            log.Close();
            StreamWriter sw = new StreamWriter("queuemin2.out");
            sw.WriteLine(result);
            sw.Close();
        }
    }

    internal class QueueWithMin
    {
        public Staque<int> items = new Staque<int>();

        public int Size { get { return items.Size; } }
        public bool IsEmpty
        {
            get { return Size == 0; }
        }

        public int CurrentMin
        {
            get { return items.PeekFront(); }
        }

        public void PushBack(int Item)
        {
            while (!IsEmpty && items.PeekBack() > Item)
                items.PopBack();
            items.PushBack(Item);
        }

        public void Extract(int Item)
        {
            if (!IsEmpty && items.PeekFront() == Item)
                items.PopFront();
        }
    }


    internal class Staque<T> where T : new()
    {
        private Vector<T> items = new Vector<T>();
        private int head = -1, tail = -1;

        public const int ExtensionRate = Vector<T>.ExtensionRate;

        public int Size { get; private set; }

        public bool IsEmpty
        {
            get { return Size == 0; }
        }

        public T Front { get { return PeekFront(); } }
        public T Back { get { return PeekBack(); } }

        public void PushBack(T Item)
        {
            tail++;
            if (tail == items.Capacity)
                tail = 0;
            if (tail == head)
                Extend();
            if (tail < items.Size)
                items[tail] = Item;
            else
                items.Add(Item);
            Size++;
        }

        public T PopFront()
        {
            if (!IsEmpty)
            {
                head++;
                if (head == items.Capacity)
                {
                    head = 0;
                    return items[items.Capacity - 1];
                }
                Size--;
                return items[head];
            }
            else
            {
                throw new IndexOutOfRangeException("Cannot pop an element from an empty queue");
            }
        }

        public T PopBack()
        {
            if (!IsEmpty)
            {
                Size--;
                tail--;
                if (tail == -1)
                {
                    tail = head + Size > items.Capacity ? (items.Capacity - 1) : head + Size;
                    return items[0];
                }
                return items[tail + 1];
            }
            else
            {
                throw new IndexOutOfRangeException("Cannot pop an element from an empty queue");
            }
        }

        public T PeekBack()
        {
            if (!IsEmpty)
            {
                return items[tail];
            }
            else
            {
                throw new IndexOutOfRangeException("Cannot peek an element from an empty queue");
            }
        }


        public T PeekFront()
        {
            if (!IsEmpty)
            {
                int i = head + 1;
                if (i == items.Capacity)
                {
                    return items[items.Capacity - 1];
                }
                return items[i];
            }
            else
            {
                throw new IndexOutOfRangeException("Cannot peek an element from an empty queue");
            }
        }

        private void Extend()
        {
            int old_capacity = items.Capacity,
                new_capacity = items.Capacity * ExtensionRate;
            for (int i = head; i < old_capacity - 1; i++)
                items[i + 1 + new_capacity - old_capacity] = items[i];
            head += items.Capacity - old_capacity;
        }

    }

    internal class Vector<T>
    {
        private T[] items;
        private const int InitialCapacity = 4;

        public const int ExtensionRate = 2;
        public int Capacity { get; private set; }
        public int Size { get; private set; }

        public Vector()
        {
            Capacity = InitialCapacity;
            items = new T[InitialCapacity];
        }

        public T this[int Index]
        {
            get
            {
                if (Index < Size)
                    return items[Index];
                else
                    throw new IndexOutOfRangeException();
            }
            set
            {
                while (Index >= Capacity)
                    Extend();
                items[Index] = value;
                if (Index >= Size) Size = Index + 1;
            }
        }

        public void Add(T Item)
        {
            Size++;
            if (Size == Capacity)
                Extend();
            items[Size - 1] = Item;
        }

        private void Extend()
        {
            T[] extendedItems = new T[Capacity * ExtensionRate];
            for (int i = 0; i < Capacity; i++)
                extendedItems[i] = items[i];
            items = extendedItems;
            Capacity *= ExtensionRate;
        }
    }

}