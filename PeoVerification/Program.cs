using System;
using System.IO;

namespace PeoVerification
{
    class Program
    {
        #region Variables
        static TextReader tr;
        static short[] peoPointer;
        static Vertex[] peo;
        static int max_indeg = 0;
        static string inputGraph;
        static string inputPEO;
        static string outputPath;
        #endregion

        static void Main(string[] args)
        {
            Console.WriteLine("1. inputs/graph1.txt\n2. inputs/graph2.txt\nSelect your graph: (1/2)");
            if (Console.ReadLine() == "1")
            {
                inputGraph = "inputs/graph1.txt";
                inputPEO = "inputs/q1_graph1.out";
                outputPath = "outputs/q2_graph1.out";
            }
            else
            {
                inputGraph = "inputs/graph2.txt";
                inputPEO = "inputs/q1_graph2.out";
                outputPath = "outputs/q2_graph2.out";
            }

            DateTime startTime = DateTime.Now;

            readPEO();

            readGraph();

            if (verifyPEO())
            {
                Console.WriteLine("\n" + inputGraph + " is chordal.");
                Console.WriteLine("PEO Exported to " + outputPath);
                exportPEO();
                Console.WriteLine("Clique Number: " + (max_indeg + 1));
                Console.WriteLine("Independence Number: " + getIndependenceNumber());
            }
            else
            {
                Console.WriteLine("\n" + inputGraph + " is not chordal!");
            }

            Console.WriteLine("\nDuration Time: " + (DateTime.Now - startTime).TotalSeconds + " seconds");

            Console.ReadLine();
        }

        static void readPEO()
        {
            tr = new StreamReader(inputPEO);
            peo = new Vertex[short.Parse(tr.ReadLine())];
            peoPointer = new short[peo.Length];

            //peoPointer[i] = PEO of i'th vertex
            short vertexNumber;
            for (short i = 0; i < peo.Length; i++)
            {
                vertexNumber = Int16.Parse(tr.ReadLine().Split('\t')[0]);
                peoPointer[vertexNumber] = i;
                peo[i] = new Vertex(vertexNumber);
            }
            tr.Close();
        }

        static void readGraph()
        {
            string[] lineArray;
            short v1, v2;
            tr = new StreamReader(inputGraph);

            tr.ReadLine();
            string line = tr.ReadLine();
            while (line != null)
            {
                lineArray = line.Split('\t');
                v1 = Int16.Parse(lineArray[0]);
                v2 = Int16.Parse(lineArray[1]);
                if (peoPointer[v1] > peoPointer[v2])
                {
                    //v2 is predecessor of v1
                    if (peo[peoPointer[v1]].lastPredecessor == -1)
                    {
                        peo[peoPointer[v1]].lastPredecessor = v2;
                    }
                    else
                    {
                        if (peoPointer[peo[peoPointer[v1]].lastPredecessor] < peoPointer[v2])
                        {
                            peo[peoPointer[v1]].predecessors.Add(peo[peoPointer[v1]].lastPredecessor);
                            peo[peoPointer[v1]].lastPredecessor = v2;
                        }
                        else
                        {
                            peo[peoPointer[v1]].predecessors.Add(v2);
                        }
                    }
                }
                else
                {
                    //v1 is predecessor of v2
                    if (peo[peoPointer[v2]].lastPredecessor == -1)
                    {
                        peo[peoPointer[v2]].lastPredecessor = v1;
                    }
                    else
                    {
                        if (peoPointer[peo[peoPointer[v2]].lastPredecessor] < peoPointer[v1])
                        {
                            peo[peoPointer[v2]].predecessors.Add(peo[peoPointer[v2]].lastPredecessor);
                            peo[peoPointer[v2]].lastPredecessor = v1;
                        }
                        else
                        {
                            peo[peoPointer[v2]].predecessors.Add(v1);
                        }
                    }
                }

                line = tr.ReadLine();
            }
            tr.Close();
        }

        static bool verifyPEO()
        {
            for (int i = peo.Length - 1; i >= 0; i--)
            {
                if (peo[i].predecessors.Count > max_indeg)
                {
                    //Plus 1: for lastPredecessor
                    max_indeg = peo[i].predecessors.Count + 1;
                }

                if (peo[i].predecessors.Count != 0)
                {
                    foreach (short predecessor in peo[i].predecessors)
                    {
                        if (!peo[peoPointer[peo[i].lastPredecessor]].predecessors.Contains(predecessor) && peo[peoPointer[peo[i].lastPredecessor]].lastPredecessor != predecessor)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        static void exportPEO()
        {
            TextWriter tw = new StreamWriter(outputPath);
            for (int i = 0; i < peo.Length; i++)
            {
                tw.WriteLine(peo[i].number);
            }
            tw.Close();
        }

        static short getIndependenceNumber()
        {
            short alpha = 0;
            for (int i = peo.Length - 1; i > 0; i--)
            {
                if (peo[i] != null)
                {
                    alpha++;
                    foreach (short predecessor in peo[i].predecessors)
                    {
                        peo[peoPointer[predecessor]] = null;
                    }
                    if (peo[i].lastPredecessor != -1)
                    {
                        peo[peoPointer[peo[i].lastPredecessor]] = null;
                    }
                }
            }
            return alpha;
        }
    }
}
