using System;
using System.Linq;
using System.IO;
using System.Text;

namespace DiscreteMathLab9_F
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamWriter sw = new StreamWriter("postfix.out");
            StreamReader sr = new StreamReader("postfix.in");
            string[] terms = sr.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Stack<int> stack = new Stack<int>();
            for (int i = 0; i < terms.Length; i++)
            {
                if (terms[i][0] >= '0' && terms[i][0] <= '9')
                    stack.Push(terms[i][0] - '0');
                else
                if (terms[i] == "+")
                    stack.Push(stack.Pop() + stack.Pop());
                else if (terms[i] == "*")
                    stack.Push(stack.Pop()*stack.Pop());
                else  //if (terms[i] == "-")
                    stack.Push(-1 * (stack.Pop() - stack.Pop()));
            }
            sw.WriteLine(stack.Pop());
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
