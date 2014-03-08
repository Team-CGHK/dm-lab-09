using System;
using System.IO;
using System.Linq;

namespace DiscreteMathLab9_C
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<int> queue = new Queue<int>();
            var queries = from s in File.ReadAllLines("queue1.in").Skip(1)
                          let words = s.Split(' ')
                          select
                              new
                              {
                                  Pushing = words[0] == "+",
                                  ItemToPush = words.Length > 1 ? int.Parse(words[1]) : 0
                              };
            StreamWriter sw = new StreamWriter("queue1.out");
            foreach (var query in queries)
            {
                if (query.Pushing)
                    queue.PushBack(query.ItemToPush);
                else
                    sw.WriteLine(queue.Pop());
            }
            sw.Close();
        }
    }

    internal class Queue<T> where T : new()
    {
        private Vector<T> items = new Vector<T>();
        private int head = -1, tail = -1;

        public const int ExtensionRate = Vector<T>.ExtensionRate;

        public int Size { get; private set; }

        public bool IsEmpty
        {
            get { return Size == 0; }
        }

        public void PushBack(T Item)
        {
            tail++;
            items[tail] = Item;
            Size++;
        }

        public T Pop()
        {
            if (!IsEmpty)
            {
                head++;
                T result = items[head];
                if (Size > 4 && Size < items.Capacity/(ExtensionRate + 2))
                    Compress();
                return result;
            }
            else
            {
                 throw new IndexOutOfRangeException("Cannot pop an element from an empty queue");
            }
        }

        private void Compress()
        {
            Vector<T> compressedVector = new Vector<T>(Size+1);
            for (int i = head; i <= tail; i++)
                compressedVector[i] = items[i];
            items = compressedVector;
        }
    }


    /**
     * Эта реализация вектора поддерживает индексацию за границу вместимости при записи,
     * при этом вектор расширяется до такого размера, какой нужен, чтобы обратиться к индексу.
     * Значения промежуточных элементов будут значениями по умолчанию для обобщенного типа T. 
     */
    class Vector<T>
    {
        private T[] items;
        private const int InitialCapacity = 4;

        public const int ExtensionRate = 2;
        public int Capacity { get; private set; }
        public int Size { get; private set; }

        public Vector(int Capacity = InitialCapacity)
        {
            this.Capacity = Capacity;
            items = new T[this.Capacity];
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
