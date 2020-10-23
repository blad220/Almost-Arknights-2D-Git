using System.Collections.Generic;
using UnityEngine;

namespace MainController.Level.Data
{
    [CreateAssetMenu(fileName = "Level X", menuName = "Levels/New Level")]
    public class LevelCreateData : ScriptableObject
    {
        public List<Wrapper> mapData = new List<Wrapper>();

        [System.Serializable]
        public class Wrapper
        {
            public List<float> inner = new List<float>();
        }

        public string description = "Some";

        public int cost;

        public Sprite levelImage;

        public int[] rewards;

        [HideInInspector]
        public GameObject map;

        public void MapDataZero()
        {
            mapData.Clear();

            Wrapper temp = new Wrapper();

            List<float> tempInner = new List<float>();
            tempInner.Add(0);

            temp.inner = tempInner;
            mapData.Add(temp);
        }
        public int getMapDataCount()
        {
            return mapData.Count;
        }
        public int getMapDataCount(int n)
        {
            return mapData[n].inner.Count;
        }
        public float getMapDataElementbyIndex(int n, int m)
        {
            return mapData[n].inner[m];
        }
        public void setMapDataElementbyIndex(int n, int m, float num)
        {
            mapData[n].inner[m] = num;
        }
        public void MapDataAdd(List<float> num)
        {
            Wrapper temp = new Wrapper();
            temp.inner = num;
            mapData.Add(temp);
        }
        public void MapDataAdd(int n, float num)
        {
            mapData[n].inner.Add(num);
        }
        public void MapDataClear()
        {
            mapData.Clear();
        }
        public void MapDataRemoveLastX()
        {
            mapData.RemoveAt(getMapDataCount() - 1);
        }
        public void MapDataRemoveLastY()
        {
            for (int x = 0; x < getMapDataCount(); x++)
            {
                mapData[x].inner.RemoveAt(getMapDataCount(x) - 1);
            }
        }

    }
}