using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DiscreteMathLab9_E
{
    class Program
    {
        static void Main(string[] args)
        {
            long a = FastPow.Pow(3, 4);
            StreamWriter sw = new StreamWriter("brackets.out");
            foreach (string s in File.ReadAllLines("brackets.in"))
            {
                Stack<char> stack = new Stack<char>();
                bool answer = true;
                for (int i = 0; i < s.Length && answer; i++)
                {
                    if ((s[i] == '(') || (s[i] == '['))
                        stack.Push(s[i]);
                    else if (stack.IsEmpty || stack.Pop() != TurnBracket(s[i]))
                        answer = false;
                }
                sw.WriteLine(answer && stack.IsEmpty ? "YES" : "NO");
            }
            sw.Close();
        }

        static char TurnBracket(char bracket)
        {
            if (bracket == '(') return ')';
            if (bracket == ')') return '(';
            if (bracket == '[') return ']';
            if (bracket == ']') return '[';
            return (char) 0;
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
