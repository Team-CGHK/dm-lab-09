using System;
using System.Linq;
using System.IO;

namespace DiscreteMathLab9_B
{
    class Program
    {
        static void Main(string[] args)
        {
            Stack<int> stack = new Stack<int>();
            var queries = from s in File.ReadAllLines("stack1.in").Skip(1)
                          let words = s.Split(' ')
                          select
                              new
                                  {
                                      Pushing = words[0] == "+",
                                      ItemToPush = words.Length > 1 ? int.Parse(words[1]) : 0
                                  };
            StreamWriter sw = new StreamWriter("stack1.out");
            foreach (var query in queries)
            {
                if (query.Pushing)
                    stack.Push(query.ItemToPush);
                else
                    sw.WriteLine(stack.Pop());
            }
            sw.Close(); 
        }
    }

    class Stack<T>
    {
        private Vector<T> items = new Vector<T>();
        public int Size { get; private set; }

        public bool IsEmpty
        {
            get { return Size == 0; }
        }

        public void Push(T Item)
        {
            if (Size < items.Size)
                items[Size] = Item;
            else
                items.Add(Item);
            Size++;
        }

        public T Pop()
        {
            if (!IsEmpty)
            {
                Size--;
                return items[Size];
            }
            else
                throw new IndexOutOfRangeException("Cannot pop an element from an empty stack");
        }

    }

    class Vector<T>
    {
        private const int ExtensionMultiplier = 2;

        private T[] items = new T[4];
        private int capacity = 4;

        public int Size { get; private set; }

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
                if (Index < Size)
                    items[Index] = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }

        public void Add(T Item)
        {
            Size++;
            if (Size == capacity)
                Extend();
            items[Size - 1] = Item;
        }

        private void Extend()
        {
            T[] extendedItems = new T[capacity*ExtensionMultiplier];
            for (int i = 0; i < capacity; i++)
                extendedItems[i] = items[i];
            items = extendedItems;
            capacity *= ExtensionMultiplier;
        }

        public void TryСompress()
        {

        }
    }
}
