using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscreteMathLab9_E
{
    class SingleLinkedList
    {
        class Node
        {
            public int Value;
            public Node Prev;
        }

        private Node head = null;

        public void Add(int item)
        {
            Node n = new Node();
            n.Value = item;
            n.Prev = head;
            head = n;
        }

        public int Last()
        {
            if (!IsEmpty())
                return head.Value;
            else
                throw new Exception("Нет последнего элемента.");
        }

        public void RemoveLast()
        {
            if (!IsEmpty())
            {
                head = head.Prev;
            }
            //иначе head == null, список уже пуст
        }

        public bool IsEmpty()
        {
            return head == null;
        }
    }
}
