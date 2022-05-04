using UnityEngine;

namespace Game
{
    public class TerrainDetector
    {
        private float[,,] _alphamaps;
        private int _textureCount;
        private Terrain _terrain;
        private TerrainData _data;
        
        public TerrainDetector(Terrain terrain)
        {
            _terrain = terrain;
            _data = terrain.terrainData;
            _alphamaps = _data.GetAlphamaps(0, 0, _data.alphamapWidth, _data.alphamapHeight);
            _textureCount= _alphamaps.Length / (_data.alphamapWidth * _data.alphamapHeight);
        }

        public int GetTextureAt(Vector3 worldPosition)
        {
            // Convert from world space to terrain space.
            Vector3 terrainPosition = _terrain.transform.position;
            int terrainX = (int) ((worldPosition.x - terrainPosition.x) / _data.size.x * _data.alphamapWidth);
            int terrainZ = (int) ((worldPosition.z - terrainPosition.z) / _data.size.z * _data.alphamapHeight);

            // Find the least-transparent texture at the desired position.
            int activeTerrainIndex = 0;
            float largestOpacity = 0f;
 
            for (int i = 0; i < _textureCount; i++)
            {
                float currentOpacity = _alphamaps[terrainZ, terrainX, i];
                
                if (largestOpacity < currentOpacity)
                {
                    activeTerrainIndex = i;
                    largestOpacity = currentOpacity;
                }
            }
 
            return activeTerrainIndex;
        }
    }
}