using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiscreteMathLab9_G
{
    class Program
    {
        enum Operation { PushBack, Pop, RequestMin }

        private static void Main(string[] args)
        {
            var queries = from s in File.ReadAllLines("queuemin.in").Skip(1)
                          let terms = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                          select new
                              {
                                  Operation = terms[0] == "+"
                                                  ? Operation.PushBack
                                                  : terms[0] == "-"
                                                        ? Operation.Pop
                                                        : Operation.RequestMin,
                                  ItemToPush = terms.Length > 1 ? int.Parse(terms[1]) : 0
                              };
            QueueWithMin queue = new QueueWithMin();
            StreamWriter sw = new StreamWriter("queuemin.out");
            int min = 0;
            foreach (var query in queries)
            {
                switch (query.Operation)
                {
                    case Operation.PushBack:
                        {
                            queue.PushBack(query.ItemToPush);
                            break;
                        }
                    case Operation.Pop:
                        {
                            queue.Pop();
                            break;
                        }
                    case Operation.RequestMin:
                        {
                            sw.WriteLine(queue.CurrentMin);
                            break;
                        }
                }
            }
            sw.Close();
        }

        class QueueWithMin
        {
            class ValueMinPair
            {
                public int Value { get; private set; }
                public int Min { get; private set; }

                public ValueMinPair(int i, int min)
                {
                    Value = i;
                    Min = min;
                }
            }

            private Stack<ValueMinPair> s1 = new Stack<ValueMinPair>(),
                                        s2 = new Stack<ValueMinPair>();

            public void PushBack(int Item)
            {
                ValueMinPair p = new ValueMinPair(Item, s1.IsEmpty ? Item : Math.Min(s1.Peek().Min, Item));
                s1.Push(p);
            }

            public int Pop()
            {
                if (s2.IsEmpty)
                {
                    while (!s1.IsEmpty)
                    {
                        ValueMinPair p = s1.Pop();
                        s2.Push(new ValueMinPair(p.Value, s2.IsEmpty ? p.Value : Math.Min(s2.Peek().Min, p.Value)));
                    }
                }
                return s2.Pop().Value;
            }

            public int CurrentMin
            {
                get
                {
                    if (s1.IsEmpty || s2.IsEmpty)
                        return (!s1.IsEmpty ? s1.Peek().Min : s2.Peek().Min);
                    else
                        return Math.Min(s1.Peek().Min, s2.Peek().Min);
                }
            }
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
            {
                throw new IndexOutOfRangeException("Cannot pop an element from an empty stack");
            }
        }

        public T Peek()
        {
            if (!IsEmpty)
                return items[Size - 1];
            else
                throw new IndexOutOfRangeException("Cannot peek an element from an empty stack");
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
            T[] extendedItems = new T[capacity * ExtensionMultiplier];
            for (int i = 0; i < capacity; i++)
                extendedItems[i] = items[i];
            items = extendedItems;
            capacity *= ExtensionMultiplier;
        }
    }
}
