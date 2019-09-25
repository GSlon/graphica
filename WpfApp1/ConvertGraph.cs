using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WpfApp1
{
    // преобразовывает входящие данные в Graph и наоборот
    class ConvertGraph
    {
        // набор статических функций для преобразования
        public static Graph FromFile(FileStream file, int mode)    // mode - матрица смеж/инцед и др.   // еще один mode - на сериализацию графа
        {

            // не дай неориент ребру добавиться в граф 2 раза (можешь добавить, но в конце пройди и удали всё кроме первого вхождения такого ребра(orient = 0, ))
            // петли всегда не ориетир!!! 
            return new Graph(new Vertex("d"), new Edge("d", new Vertex("d"), new Vertex("d")));         // возвращаем новый граф
        }

        public static void ToFile(FileStream file, Graph graph, int mode)   
        {

        }

        public static int[,] ToMatrIncid(Graph gr)    // mode - матрица смеж/инцед и др.
        {
            return new int[4,4];         // возвращаем двумерный массив
        }


    }
}
