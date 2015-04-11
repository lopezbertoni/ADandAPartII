using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtilities
{
    public class UnionFind
    {
        private readonly int[] _parent;
        private readonly int[] _rank;

        public UnionFind(int size)
        {
            this._parent = new int[size];
            this._rank = new int[size];
            this.Count = size;

            //Initial cluster size is size of set
            ClusterCount = size;

            //Initially each parent is itself and all ranks are zero.
            for (var i = 0; i < size; i++)
            {
                _parent[i] = i;
                _rank[i] = 0;
            }
        }

        public int Count { get; set; }
        public int ClusterCount { get; set; }

        public int Find(int x)
        {
            if (_parent[x] == x)
            {
                return x;
            }

            //Find parent of x recursively and update it
            _parent[x] = Find(_parent[x]);
            return _parent[x];
        }

        public void Union(int x, int y)
        {
            //Find leaders of x and y
            var xLeader = Find(x);
            var yLeader = Find(y);

            //If elements have the same leader, no need to merge
            if (xLeader != yLeader)
            {
                //Need to merge both
                //Get their ranks to determine how they're going to be merged
                var xRank = _rank[xLeader];
                var yRank = _rank[yLeader];

                if (xRank < yRank)
                {
                    //xTree goes below yTree
                    //xLeader is replaced by yLeader
                    _parent[x] = yLeader;
                }
                if (xRank > yRank)
                {
                    //yTree goes below xTree
                    //yLeader is replaced by xLeader
                    _parent[y] = xLeader;
                }
                if (xRank == yRank)
                {
                    //Both have same rank so arbitrarily set y below x
                    _parent[y] = xLeader;
                    //Increase rank of x
                    _rank[xLeader] = xRank + 1;
                }
                //Decrease number of clusters since we're merging
                ClusterCount--;
            }
        }
    }
}
