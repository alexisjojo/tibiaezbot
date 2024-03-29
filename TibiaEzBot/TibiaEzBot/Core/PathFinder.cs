﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using TibiaEzBot.Core.Entities;

namespace TibiaEzBot.Core
{
    public class PathFinder
    {
        #region Singleton
        private static PathFinder instance;

        public static PathFinder GetInstance()
        {
            if (instance == null)
                instance = new PathFinder();

            return instance;
        }
        #endregion

        #region Variables Declaration
        private byte[,] mGrid = null;
        private PriorityQueueB<PathFinderNode> mOpen = new PriorityQueueB<PathFinderNode>(new ComparePFNode());
        private List<PathFinderNode> mClose = new List<PathFinderNode>();
        private bool mStop = false;
        private bool mStopped = true;
        private int mHEstimate = 2;
        private DateTime lastMapUpdate;
        #endregion

        #region Constructors
        private PathFinder()
        {
            mGrid = new byte[18, 14];

            for (int y = 0; y < mGrid.GetUpperBound(1); y++)
                for (int x = 0; x < mGrid.GetUpperBound(0); x++)
                    mGrid[x, y] = 0;

        }
        #endregion

        #region Properties
        public byte[,] Grid
        {
            get { return mGrid; }
            set { mGrid = value; }
        }

        public bool Stopped
        {
            get { return mStopped; }
        }

        public int HeuristicEstimate
        {
            get { return mHEstimate; }
            set { mHEstimate = value; }
        }
        #endregion

        #region Methods
        public void FindPathStop()
        {
            mStop = true;
        }

        public bool FindPath(Position start, Position end)
        {
            return FindPath(start, end, false);
        }

        public bool FindPath(Position start, Position end, bool adjacent)
        {
            Position playerPos = GlobalVariables.GetPlayerPosition();

            if (start.Z != playerPos.Z || end.Z != playerPos.Z)
                return false;

            int x = (int)playerPos.X - 8;
            int y = (int)playerPos.Y - 6;

            Point sPoint = new Point((int)start.X - x, (int)start.Y - y);
            Point ePoint = new Point((int)end.X - x, (int)end.Y - y);

            //Verificar se as localizações estão dentro do tamanho da nossa matriz.
            if (sPoint.X < 0 || sPoint.X >= 18 || sPoint.Y < 0 || sPoint.Y >= 14 ||
               ePoint.X < 0 || ePoint.X >= 18 || ePoint.Y < 0 || ePoint.Y >= 14)
            {
                Logger.Log("Falha ao efetuar a busca por um caminho. Start: " + sPoint + ", End: " + ePoint);
                return false;
            }

            return FindPath(sPoint, ePoint, adjacent);
        }

        private void upDateMap(int x, int y, int z, int width, int height)
        {
            Map map = Map.GetInstance();
            for (int nx = 0; nx < width; nx++)
            {
                for (int ny = 0; ny < height; ny++)
                {
                    Position pos = new Position((uint)(x + nx), (uint)(y + ny), (uint)z);
                    Tile tile = map.GetTile(pos);

                    mGrid[nx, ny] = tile != null ? Convert.ToByte(!tile.IsTileBlocking()) : Byte.MinValue;
                }
            }
        }


        public bool FindPath(Point start, Point end, bool adjacent)
        {
            lock (this)
            {
                if ((DateTime.UtcNow - lastMapUpdate).TotalMilliseconds > 200)
                {
                    Position currentPos = GlobalVariables.GetPlayerPosition();
                    upDateMap((int)currentPos.X - 8, (int)currentPos.Y - 6, (int)currentPos.Z, 18, 14);
                    lastMapUpdate = DateTime.UtcNow;
                }

                PathFinderNode parentNode;
                bool found = false;
                int gridX = mGrid.GetUpperBound(0);
                int gridY = mGrid.GetUpperBound(1);

                mStop = false;
                mStopped = false;
                mOpen.Clear();
                mClose.Clear();

                parentNode.G = 0;
                parentNode.H = mHEstimate;
                parentNode.F = parentNode.G + parentNode.H;
                parentNode.X = start.X;
                parentNode.Y = start.Y;
                parentNode.PX = parentNode.X;
                parentNode.PY = parentNode.Y;
                mOpen.Push(parentNode);

                while (mOpen.Count > 0 && !mStop)
                {
                    parentNode = mOpen.Pop();

                    if (parentNode.X == end.X && parentNode.Y == end.Y ||
                        (adjacent && new Point(parentNode.X, parentNode.Y).IsAdjacent(end)))
                    {
                        mClose.Add(parentNode);
                        found = true;
                        break;
                    }

                    //Lets calculate each successors
                    for (int i = 0; i < 8; i++)
                    {
                        PathFinderNode newNode;
                        newNode.X = parentNode.X + Extensions.Direction[i, 0];
                        newNode.Y = parentNode.Y + Extensions.Direction[i, 1];

                        if (newNode.X < 0 || newNode.Y < 0 || newNode.X >= gridX || newNode.Y >= gridY)
                            continue;

                        int newG = parentNode.G + mGrid[newNode.X, newNode.Y];

                        if (newG == parentNode.G)
                        {
                            //Unbrekeable
                            continue;
                        }

                        int foundInOpenIndex = -1;
                        for (int j = 0; j < mOpen.Count; j++)
                        {
                            if (mOpen[j].X == newNode.X && mOpen[j].Y == newNode.Y)
                            {
                                foundInOpenIndex = j;
                                break;
                            }
                        }

                        if (foundInOpenIndex != -1 && mOpen[foundInOpenIndex].G <= newG)
                            continue;

                        int foundInCloseIndex = -1;
                        for (int j = 0; j < mClose.Count; j++)
                        {
                            if (mClose[j].X == newNode.X && mClose[j].Y == newNode.Y)
                            {
                                foundInCloseIndex = j;
                                break;
                            }
                        }

                        if (foundInCloseIndex != -1 && mClose[foundInCloseIndex].G <= newG)
                            continue;

                        newNode.PX = parentNode.X;
                        newNode.PY = parentNode.Y;
                        newNode.G = newG;
                        newNode.H = mHEstimate * (Math.Abs(newNode.X - end.X) + Math.Abs(newNode.Y - end.Y));
                        newNode.F = newNode.G + newNode.H;
                        mOpen.Push(newNode);
                    }

                    mClose.Add(parentNode);
                }

                mStopped = true;
                return found;
            }
        }

        #endregion

        #region Enum
        internal enum PathFinderNodeType
        {
            Start = 1,
            End = 2,
            Open = 4,
            Close = 8,
            Current = 16,
            Path = 32
        }

        internal enum HeuristicFormula
        {
            Manhattan = 1,
            MaxDXDY = 2,
            DiagonalShortCut = 3,
            Euclidean = 4,
            EuclideanNoSQR = 5,
            Custom1 = 6
        }
        #endregion

        #region Interfaces
        internal interface IPriorityQueue<T>
        {
            int Push(T item);
            T Pop();
            T Peek();
            void Update(int i);
        }
        #endregion

        #region Structs
        internal struct PathFinderNode
        {
            public int F;
            public int G;
            public int H;  // f = gone + heuristic
            public int X;
            public int Y;
            public int PX; // Parent
            public int PY;
        }
        #endregion

        #region Classes
        internal class ComparePFNode : IComparer<PathFinderNode>
        {
            public int Compare(PathFinderNode x, PathFinderNode y)
            {
                if (x.F > y.F)
                    return 1;
                else if (x.F < y.F)
                    return -1;
                return 0;
            }
        }

        internal class PriorityQueueB<T> : IPriorityQueue<T>
        {
            #region Variables Declaration
            private List<T> InnerList = new List<T>();
            private IComparer<T> mComparer;
            #endregion

            #region Contructors
            public PriorityQueueB()
            {
                mComparer = Comparer<T>.Default;
            }

            public PriorityQueueB(IComparer<T> comparer)
            {
                mComparer = comparer;
            }

            public PriorityQueueB(IComparer<T> comparer, int capacity)
            {
                mComparer = comparer;
                InnerList.Capacity = capacity;
            }
            #endregion

            #region Methods
            private void SwitchElements(int i, int j)
            {
                T h = InnerList[i];
                InnerList[i] = InnerList[j];
                InnerList[j] = h;
            }

            protected virtual int OnCompare(int i, int j)
            {
                return mComparer.Compare(InnerList[i], InnerList[j]);
            }

            /// <summary>
            /// Push an object onto the PQ
            /// </summary>
            /// <param name="O">The new object</param>
            /// <returns>The index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.</returns>
            public int Push(T item)
            {
                int p = InnerList.Count, p2;
                InnerList.Add(item); // E[p] = O
                do
                {
                    if (p == 0)
                        break;
                    p2 = (p - 1) / 2;
                    if (OnCompare(p, p2) < 0)
                    {
                        SwitchElements(p, p2);
                        p = p2;
                    }
                    else
                        break;
                } while (true);
                return p;
            }

            /// <summary>
            /// Get the smallest object and remove it.
            /// </summary>
            /// <returns>The smallest object</returns>
            public T Pop()
            {
                T result = InnerList[0];
                int p = 0, p1, p2, pn;
                InnerList[0] = InnerList[InnerList.Count - 1];
                InnerList.RemoveAt(InnerList.Count - 1);
                do
                {
                    pn = p;
                    p1 = 2 * p + 1;
                    p2 = 2 * p + 2;
                    if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
                        p = p1;
                    if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
                        p = p2;

                    if (p == pn)
                        break;
                    SwitchElements(p, pn);
                } while (true);

                return result;
            }

            /// <summary>
            /// Notify the PQ that the object at position i has changed
            /// and the PQ needs to restore order.
            /// Since you dont have access to any indexes (except by using the
            /// explicit IList.this) you should not call this function without knowing exactly
            /// what you do.
            /// </summary>
            /// <param name="i">The index of the changed object.</param>
            public void Update(int i)
            {
                int p = i, pn;
                int p1, p2;
                do	// aufsteigen
                {
                    if (p == 0)
                        break;
                    p2 = (p - 1) / 2;
                    if (OnCompare(p, p2) < 0)
                    {
                        SwitchElements(p, p2);
                        p = p2;
                    }
                    else
                        break;
                } while (true);
                if (p < i)
                    return;
                do	   // absteigen
                {
                    pn = p;
                    p1 = 2 * p + 1;
                    p2 = 2 * p + 2;
                    if (InnerList.Count > p1 && OnCompare(p, p1) > 0) // links kleiner
                        p = p1;
                    if (InnerList.Count > p2 && OnCompare(p, p2) > 0) // rechts noch kleiner
                        p = p2;

                    if (p == pn)
                        break;
                    SwitchElements(p, pn);
                } while (true);
            }

            /// <summary>
            /// Get the smallest object without removing it.
            /// </summary>
            /// <returns>The smallest object</returns>
            public T Peek()
            {
                if (InnerList.Count > 0)
                    return InnerList[0];
                return default(T);
            }

            public void Clear()
            {
                InnerList.Clear();
            }

            public int Count
            {
                get { return InnerList.Count; }
            }

            public void RemoveLocation(T item)
            {
                int index = -1;
                for (int i = 0; i < InnerList.Count; i++)
                {

                    if (mComparer.Compare(InnerList[i], item) == 0)
                        index = i;
                }

                if (index != -1)
                    InnerList.RemoveAt(index);
            }

            public T this[int index]
            {
                get { return InnerList[index]; }
                set
                {
                    InnerList[index] = value;
                    Update(index);
                }
            }
            #endregion
        }
        #endregion
    }
}
