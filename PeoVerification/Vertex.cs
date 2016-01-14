using System.Collections.Generic;

namespace PeoVerification
{
    class Vertex
    {
        public HashSet<short> predecessors = new HashSet<short>();
        public short lastPredecessor = -1;
        public short number;

        public Vertex(short number)
        {
            this.number = number;
        }
    }
}
