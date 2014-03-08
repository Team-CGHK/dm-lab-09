using System;
using System.Linq;
using System.IO;

namespace DiscreteMathLab9_D
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue<int> queue = new Queue<int>();
            var queries = from s in File.ReadAllLines("queue2.in")
                          where s.StartsWith("-") || (s.StartsWith("+"))
                          let words = s.Split(' ')
                          select
                              new
                              {
                                  Pushing = words[0] == "+",
                                  ItemToPush = words.Length > 1 ? int.Parse(words[1]) : 0
                              };
            StreamWriter sw = new StreamWriter("queue2.out");
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

    class Queue<T>
    {
        private List<T> itemList = new List<T>();
        private ListItem<T> tail;

        public int Size { get; private set; }

        public bool IsEmpty
        {
            get { return Size == 0; }
        }

        public void PushBack(T Item)
        {
            if (IsEmpty)
            {
                itemList.InsertFirst(Item);
                tail = itemList.Head;
            }
            else
            {
                itemList.InsertAfter(Item, tail);
                tail = tail.Next;
            }
            Size++;
        }

        public T Pop()
        {
            if (!IsEmpty)
            {
                T result = itemList.Head.Value;
                itemList.RemoveFirst();
                Size--;
                return result;
            }
            else
            {
                throw new IndexOutOfRangeException("Cannot pop an element from an empty queue");
            }
        }

    }

    class List<T>
    {
        public ListItem<T> Head { get; private set; }

        public void InsertFirst(T Item)
        {
            ListItem<T> i = new ListItem<T>(Item);
            i.Next = Head;
            Head = i;
        }

        public void InsertAfter(T Item, ListItem<T> After)
        {
            ListItem<T> i = new ListItem<T>(Item);
            i.Next = After.Next;
            After.Next = i;
        }

        public void RemoveFirst()
        {
            if (Head != null)
                Head = Head.Next;
        }

        public void RemoveAfter(ListItem<T> After)
        {
            if (After.Next != null)
            {
                After.Next = After.Next.Next;
            }
        }
    }

    public class ListItem<T>
    {
        public T Value;
        public ListItem<T> Next;

        public ListItem(T Value)
        {
            this.Value = Value;
        }
    }
}