using System.Collections.Generic;
using UnityEngine;

namespace MainController.Operator.Range
{
    [CreateAssetMenu(fileName ="Range Name", menuName ="New Range")]
    public class RangeOfOperator : ScriptableObject
    {
        public GameObject tileRangePrefab;
        public Material tileRangeMaterial;

        public int OperatorPositionX = 0;
        public int OperatorPositionY = 0;

        public List<Wrapper> RangeData = new List<Wrapper>();

        public void RangeDataZero()
        {
            RangeData.Clear();

            Wrapper temp = new Wrapper();

            List<bool> tempInner = new List<bool>();
            tempInner.Add(false);

            temp.inner = tempInner;
            RangeData.Add(temp);
        }
        public int getRangeDataCount()
        {
            return RangeData.Count;
        }
        public int getRangeDataCount(int n)
        {
            return RangeData[n].inner.Count;
        }
        public bool getRangeDataElementbyIndex(int n, int m)
        {
            return RangeData[n].inner[m];
        }
        public void setRangeDataElementbyIndex(int n, int m, bool num)
        {
            RangeData[n].inner[m] = num;
        }
        public void RangeDataAdd(List<bool> num)
        {
            Wrapper temp = new Wrapper();
            temp.inner = num;
            RangeData.Add(temp);
        }
        public void RangeDataAdd(int n, bool num)
        {
            RangeData[n].inner.Add(num);
        }
        public void RangeDataClear()
        {
            RangeData.Clear();
        }
        public void RangeDataRemoveLastX()
        {
            RangeData.RemoveAt(getRangeDataCount() - 1);
        }
        public void RangeDataRemoveLastY()
        {
            for (int x = 0; x < getRangeDataCount(); x++)
            {
                RangeData[x].inner.RemoveAt(getRangeDataCount(x) - 1);
            }
        }
        public Range ArrayTransformRightMain()
        {

            bool[,] arrayTransform = new bool[getRangeDataCount(), getRangeDataCount(0)];
            Range indexOfOperator = new Range();
            int newX = -1;
            for (int x = 0; x < getRangeDataCount(); x++)
            {
                newX++;
                int newY = -1;

                for (int y = 0; y < getRangeDataCount(0); y++)
                {
                    newY++;

                    bool tempIndex = TogleOperator(x, y);
                    if (tempIndex)
                    {
                        indexOfOperator = new Range(true, newX, newY);
                    }
                    arrayTransform[newX, newY] = getRangeDataElementbyIndex(x, y);

                }

            }
            indexOfOperator.array = arrayTransform;
            return indexOfOperator;
        }
        public Range ArrayTransformLeft()
        {
            bool[,] arrayTransform = new bool[getRangeDataCount(), getRangeDataCount(0)];
            Range indexOfOperator = new Range();
            int newX = -1;

            for (int x = getRangeDataCount() - 1; x >= 0; x--)
            {
                newX++;
                int newY = -1;

                for (int y = 0; y < getRangeDataCount(0); y++)
                {

                    newY++;
                    bool tempIndex = TogleOperator(x, y);
                    if (tempIndex)
                    {
                        indexOfOperator = new Range(true, newX, newY);
                    }
                    arrayTransform[newX, newY] = getRangeDataElementbyIndex(x, y);

                }

            }
            indexOfOperator.array = arrayTransform;
            return indexOfOperator;
        }
        public Range ArrayTransformTop()
        {

            bool[,] arrayTransform = new bool[getRangeDataCount(0), getRangeDataCount()];
            Range indexOfOperator = new Range();
            int newX = -1;
            for (int y = getRangeDataCount(0) - 1; y >= 0; y--)
            {
                newX++;
                int newY = -1;

                for (int x = 0; x < getRangeDataCount(); x++)
                {

                    newY++;
                    bool tempIndex = TogleOperator(x, y);
                    if (tempIndex)
                    {
                        indexOfOperator = new Range(true, newX, newY);
                    }
                    arrayTransform[newX, newY] = getRangeDataElementbyIndex(x, y);

                }

            }
            indexOfOperator.array = arrayTransform;
            return indexOfOperator;
        }
        public Range ArrayTransformBottom()
        {
            bool[,] arrayTransform = new bool[getRangeDataCount(0), getRangeDataCount()];
            Range indexOfOperator = new Range();
            int newX = -1;
            for (int y = 0; y < getRangeDataCount(0); y++)
            {
                newX++;
                int newY = -1;

                for (int x = getRangeDataCount() - 1; x >= 0; x--)
                {

                    newY++;
                    bool tempIndex = TogleOperator(x, y);
                    if (tempIndex)
                    {
                        indexOfOperator = new Range(true, newX, newY);
                    }
                    arrayTransform[newX, newY] = getRangeDataElementbyIndex(x, y);

                }

            }
            indexOfOperator.array = arrayTransform;
            return indexOfOperator;
        }
        private bool TogleOperator(int x, int y)
        {
            if (OperatorPositionX == x && OperatorPositionY == y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [System.Serializable]
        public class Wrapper
        {
            [SerializeField]
            public List<bool> inner = new List<bool>();
        }
        public class Range
        {
            public bool isHas = false;
            public int x = 0;
            public int y = 0;
            public bool[,] array = { { false } };

            public Range()
            {

            }
            public Range(bool _isHas, int _x, int _y)
            {
                isHas = _isHas;
                if (isHas)
                {
                    x = _x;
                    y = _y;
                }
                else
                {
                    x = 0;
                    y = 0;
                    array = new bool[,] { { false } };
                }
            }
            public Range(bool _isHas, int _x, int _y, bool[,] _array)
            {
                isHas = _isHas;
                if (isHas)
                {
                    x = _x;
                    y = _y;
                    array = _array;
                }
                else
                {
                    x = 0;
                    y = 0;
                    array = new bool[,] { { false } };
                }
            }
        }
    }
}
