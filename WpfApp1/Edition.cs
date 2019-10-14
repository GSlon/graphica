using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Edition
    {
        private LinkedList<Graph> graphs;
        private LinkedListNode<Graph> node;

        public Edition(Graph graph)
        {
            graphs = new LinkedList<Graph>();
            node = new LinkedListNode<Graph>(graph);
        }

        public void AddGraph(Graph graph)
        {
            if (node.Next == null)
            {
                if (graphs.Count <= 10)
                {
                    node = graphs.AddLast(graph);
                }
                else
                {
                    graphs.RemoveFirst();
                    node = graphs.AddLast(graph);
                }
            }
            else
            {
                if (graph == node.Next.Value)
                {
                    node = node.Next;
                }
                else
                {
                    do
                    {
                        graphs.Remove(node.Next);
                    }
                    while (node.Next != null);

                    node = graphs.AddLast(graph);
                }

            }

        }

        public Graph Undo()
        {
            if (node.Previous != null)
                node = node.Previous;


            return node.Value;
        }

        public Graph Redo()
        {
            if (node.Next != null)
                node = node.Next;

            return node.Value;
        }

        public Graph CurrentGraph()
        {
            return node.Value;
        }
    }
}
