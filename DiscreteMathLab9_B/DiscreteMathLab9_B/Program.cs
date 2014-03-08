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
            var queries = from s in File.ReadAllLines("stack2.in").Skip(1)
                          let words = s.Split(' ')
                          select
                              new
                              {
                                  Pushing = words[0] == "+",
                                  ItemToPush = words.Length > 1 ? int.Parse(words[1]) : 0
                              };
            StreamWriter sw = new StreamWriter("stack2.out");
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

    class Stack<T> where T : new()
    {
        private List<T> itemList = new List<T>();

        public int Size { get; private set; }

        public bool IsEmpty
        {
            get { return Size == 0; }
        }

        public void Push(T Item)
        {
            itemList.InsertFirst(Item);
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
                throw new IndexOutOfRangeException("Cannot pop an element from an empty stack");
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