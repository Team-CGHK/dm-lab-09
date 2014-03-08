using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DiscreteMathLab9_H
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines("quack.in");
            StreamWriter sw = new StreamWriter("quack.out");
            Queue<int> queue = new Queue<int>();
            int[] registry = new int[26];
            Dictionary<string, int> labels = new Dictionary<string, int>();
            for (int i = 0; i<lines.Length && lines[i] != ""; i++)
                if (lines[i][0] == ':')
                    labels.Add(lines[i].Remove(0, 1),i);
            for (int pos = 0; pos < lines.Length && lines[pos] != ""; pos++)
            {
                char cmd = Char.ToUpper(lines[pos][0]);
                if (cmd == '+')
                {
                    int result = (queue.Pop() + queue.Pop())%65536;
                    queue.PushBack(result >= 0 ? result : result + 65536);
                }
                if (cmd == '-' && lines[pos].Length == 1)
                {
                    int result = ((queue.Pop() - queue.Pop()))%65536;
                    queue.PushBack(result >= 0 ? result : result + 65536);
                }
                if (cmd == '*')
                {
                    int result = (int) (((long) queue.Pop()*(long) queue.Pop())%65536);
                    queue.PushBack(result >= 0 ? result : result + 65536);
                }
                if (cmd == '/')
                {
                    int a = queue.Pop(),
                        b = queue.Pop(),
                    result = (b == 0) ? 0 : a / b % 65536;
                    queue.PushBack(result >= 0 ? result : result + 65536);
                }
                if (cmd == '%')
                {
                    int a = queue.Pop(),
                        b = queue.Pop(),
                        result = (b == 0) ? 0 :a%b%65536;
                    queue.PushBack(result >= 0 ? result : result + 65536);
                }
                if (cmd == '>')
                    registry[(int)lines[pos][1] - (int)'a'] = queue.Pop(); 
                if (cmd == '<')
                    queue.PushBack(registry[(int) lines[pos][1] - (int) 'a']);
                if (cmd == 'P' && lines[pos].Length == 1)
                    sw.WriteLine(queue.Pop());
                if (cmd == 'P' && lines[pos].Length == 2)
                    sw.WriteLine(registry[(int)lines[pos][1] - (int)'a']);
                if (cmd == 'C' && lines[pos].Length == 1)
                    sw.Write((char)(queue.Pop()%256));
                if (cmd == 'C' && lines[pos].Length == 2)
                    sw.Write((char)(registry[(int)lines[pos][1] - (int)'a'] % 256));
                if (cmd == ':')
                    {} //do nothing :)
                if (cmd == 'J')
                    pos = labels[lines[pos].Remove(0, 1)];
                if (cmd == 'Z' && registry[(int)lines[pos][1] - (int)'a'] == 0)
                    pos = labels[lines[pos].Remove(0, 2)];
                if (cmd == 'E' && registry[(int)lines[pos][1] - (int)'a'] == registry[(int)lines[pos][2] - (int)'a'])
                    pos = labels[lines[pos].Remove(0, 3)];
                if (cmd == 'G' && registry[(int)lines[pos][1] - (int)'a'] > registry[(int)lines[pos][2] - (int)'a'])
                    pos = labels[lines[pos].Remove(0, 3)];
                if (cmd == 'Q')
                    break;
                if (Char.IsDigit(cmd) || lines[pos][0] == '-' && lines[pos].Length > 1)
                {
                    int result = int.Parse(lines[pos])%65536;
                    queue.PushBack(result >= 0 ? result : result + 65536);
                }
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
