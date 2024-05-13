using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class WorldGeneration : MonoBehaviour
{
    public WorldManager manager;
    static System.Random r__ = new System.Random();
    public int WorldSeed = r__.Next(-1000000, 1000000);
    public void DrawTree(Vector3 pos)
    {
        manager.WorldBlocks.Add(new Block(pos,(int)Blocks.OakLog));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x,pos.y + 1,pos.z),(int)Blocks.OakLog));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x,pos.y + 2,pos.z),(int)Blocks.OakLog));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x,pos.y + 3,pos.z),(int)Blocks.OakLog));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x,pos.y + 4,pos.z),(int)Blocks.OakLeaves));

        manager.WorldBlocks.Add(new Block(new Vector3(pos.x,pos.y + 2,pos.z+1),(int)Blocks.OakLeaves));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x,pos.y + 2,pos.z-1),(int)Blocks.OakLeaves));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x+1,pos.y + 2,pos.z),(int)Blocks.OakLeaves));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x-1,pos.y + 2,pos.z),(int)Blocks.OakLeaves));

        manager.WorldBlocks.Add(new Block(new Vector3(pos.x, pos.y + 3, pos.z + 1), (int)Blocks.OakLeaves));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x, pos.y + 3, pos.z - 1), (int)Blocks.OakLeaves));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x + 1, pos.y + 3, pos.z), (int)Blocks.OakLeaves));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x - 1, pos.y + 3, pos.z), (int)Blocks.OakLeaves));

        manager.WorldBlocks.Add(new Block(new Vector3(pos.x-1,pos.y + 2,pos.z+1),(int)Blocks.OakLeaves));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x-1,pos.y + 2,pos.z-1),(int)Blocks.OakLeaves));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x+1,pos.y + 2,pos.z+1),(int)Blocks.OakLeaves));
        manager.WorldBlocks.Add(new Block(new Vector3(pos.x+1,pos.y + 2,pos.z-1),(int)Blocks.OakLeaves));

        //Debug.Log("Drew tree!");
    }

    public GameObject Map1,Map2,Supermap;
    //snow layer : 56

    public void DrawGrassTerrain(int chunklength,int yScale,Vector3 pos)
    {
        int[,] map = GeneratePerlineMap(chunklength, yScale,0,pos);
        for(int i = 0; i < chunklength; i++)
        {
            for(int j = 0; j < chunklength; j++)
            {
                int x = i + (int)(pos.x * Chunk.ChunkLength), y = map[i, j] + (int)pos.y, z = j + (int)(pos.z * Chunk.ChunkLength);
                manager.WorldBlocks.Add(new Block(x,y,z,(int)Blocks.Grass));
            }
        }
        Debug.Log($"Done DrawGrassTerrain({chunklength},{yScale},{pos})");
    }

    public void DrawRandomTerrainMountains(int chunklength, int yScale, Vector3 pos, TreeSettings treeSettings, int sandLevel, int yOffset)
    {
        System.Random r = new System.Random();
        int seed1 = r.Next(-10000,-10000) , seed2 = r.Next(-10000, -10000);
        int[,] minimap = GeneratePerlineMiniMapRandom(chunklength, yScale, pos, yOffset,seed2);
        int[,] map = GeneratePerlineMapRandom(chunklength, yScale, pos, yOffset,seed1);
    
        for(int i =0;i< chunklength; i++){
            for (int j = 0;j< chunklength; j++)
            {
                map[i, j] = Mathf.RoundToInt((int)minimap[i, j] +  map[i, j]) / 2;
               
            }
        }
        for (int i = 0; i < chunklength; i++)
        {
            for (int j = 0; j < chunklength; j++)
            {
                int x = i + (int)(pos.x * Chunk.ChunkLength);
                int y = map[i, j] + (int)pos.y, z = j + (int)(pos.z * Chunk.ChunkLength);
                if (y > sandLevel)
                {
                    manager.WorldBlocks.Add(new Block(x, y, z, (int)Blocks.Grass));
                }
                else
                {
                    manager.Water.SetActive(true);
                    manager.WorldBlocks.Add(new Block(x, y, z, (int)Blocks.Sand));
                    manager.Water.transform.position = new Vector3(Mathf.RoundToInt(GetAvgWorldQuads().x), 4, Mathf.RoundToInt(GetAvgWorldQuads().z));
                    manager.Water.transform.localScale = new Vector3(chunklength, sandLevel - 0.1f, chunklength);
                }

                if (treeSettings.addTrees)
                {
                    if (y > treeSettings.treeLayerMin && y < treeSettings.treeLayerMax && r.Next(0, 10) < treeSettings.spawnProbabilty)
                    {
                        DrawTree(new Vector3(x, y + 1, z));
                    }
                }

            }
        }
        Debug.Log($"Done DrawGrassTerrainRandom({chunklength},{yScale},{pos})");
    }

    public void DrawGrassTerrainRandom(int chunklength,int yScale,Vector3 pos, TreeSettings treeSettings,int sandLevel,int yOffset)
    {
        System.Random r = new System.Random();
        int[,] map = GeneratePerlineMapRandom(chunklength, yScale,pos, yOffset,r.Next(-10000,10000));
        for (int i = 0; i < chunklength; i++)
        {
            for (int j = 0; j < chunklength; j++)
            {
                int x = i + (int)(pos.x * Chunk.ChunkLength);
                int y = map[i, j] + (int)pos.y, z = j + (int)(pos.z * Chunk.ChunkLength);
                if (y > sandLevel)
                {
                    manager.WorldBlocks.Add(new Block(x, y, z, (int)Blocks.Grass));
                }
                else
                {
                    manager.Water.SetActive(true);
                    manager.WorldBlocks.Add(new Block(x, y, z, (int)Blocks.Sand));
                    manager.Water.transform.position = new Vector3(Mathf.RoundToInt(GetAvgWorldQuads().x), 4, Mathf.RoundToInt(GetAvgWorldQuads().z));
                    manager.Water.transform.localScale = new Vector3(chunklength, sandLevel-0.1f,chunklength);
                }

                if (treeSettings.addTrees)
                {
                    if(y > treeSettings.treeLayerMin && y < treeSettings.treeLayerMax && r.Next(0,10) < treeSettings.spawnProbabilty)
                    {
                        DrawTree(new Vector3(x,y+1,z));
                    }
                }

            }
        }
        Debug.Log($"Done DrawGrassTerrainRandom({chunklength},{yScale},{pos})");
    }
    
    public Chunk ReturnChunkRandom(Vector3 chunkPos,int yScale,int yOffset,int SandLevel,TreeSettings treeSettings)
    {
        Chunk temp = new Chunk();
        System.Random r = new System.Random();

        int[,] map = GeneratePerlineMap(Chunk.ChunkLength,yScale, WorldSeed, new Vector3(chunkPos.x, 0,chunkPos.z));
        List<Block> blocks_ = new List<Block>();
        for(int i = 0; i < Chunk.ChunkLength; i++)
        {       
            for (int j = 0; j < Chunk.ChunkLength; j++)
            {   
                int x = i + (int)chunkPos.x * Chunk.ChunkLength;
                int z = j + (int)chunkPos.z * Chunk.ChunkLength;
                int y = map[i,j] + yOffset + (int)chunkPos.y;

                if (y > SandLevel)
                { 
                    blocks_.Add(new Block(x, y, z, (int)Blocks.Grass));
                    //Debug.Log(JsonUtility.ToJson(new Block(x, y, z, (int)Blocks.Grass)));
                }
                else
                {
                    blocks_.Add(new Block(x, y, z, (int)Blocks.Sand));
                    //Debug.Log(JsonUtility.ToJson(new Block(x, y, z, (int)Blocks.Grass)));
                }

                if (treeSettings.addTrees)
                {
                    if (y > treeSettings.treeLayerMin && y < treeSettings.treeLayerMax && r.Next(0, 10) < treeSettings.spawnProbabilty)
                    {
                        DrawTree(new Vector3(x, y + 1, z));
                    }
                }
            }
        }

        temp.blocks = blocks_.ToArray();
        return temp;
    }        
    Vector3 GetAvgWorldQuads()
    {
        int avgX = 0, avgY = 0 , avgZ = 0 ;
        for(int i = 0; i < manager.WorldBlocks.Count; i++)
        {
            avgX += manager.WorldBlocks[i].x;
            avgY += manager.WorldBlocks[i].y;
            avgZ += manager.WorldBlocks[i].z;
        }
        avgX /= manager.WorldBlocks.Count;
        avgY /= manager.WorldBlocks.Count;
        avgZ /= manager.WorldBlocks.Count;

        return new Vector3(avgX,avgY,avgZ);
    }
    int[,] GeneratePerlineMap(int chunklength,int yScale,int _XZOffset, Vector3 pos)
    {
        int[,] map = new int[chunklength, chunklength];
        for (int i = 0; i < chunklength; i++)
        {
            for (int j = 0; j < chunklength; j++)
            {
                float x = ((Chunk.ChunkLength * pos.x) + i + _XZOffset) * 0.01f;
                float y = ((Chunk.ChunkLength * pos.z) + j + _XZOffset) * 0.01f;
                map[i, j] = Mathf.RoundToInt(Mathf.PerlinNoise(x, y) * yScale);
            }
        }

        return map;
    }
    int[,] GeneratePerlineMapRandom(int chunklength, int yScale,Vector3 pos,int yOffset , int seed)
    {
        int[,] map = new int[chunklength, chunklength];
        //System.Random r = new System.Random();
        int rand = seed;
        for (int i = 0; i < chunklength; i++)
        {
            for (int j = 0; j < chunklength; j++)
            {
                float x = ((Chunk.ChunkLength * pos.x) + i + rand) * 0.01f;
                float y = ((Chunk.ChunkLength * pos.z) + j + rand) * 0.01f;
                map[i, j] = Mathf.RoundToInt(Mathf.PerlinNoise(x,y) * yScale) + yOffset;
            }
        }

        return map;
    }
    int[,] GeneratePerlineMiniMapRandom(int chunklength, int yScale, Vector3 pos, int yOffset,int seed)
    {
        int[,] map = new int[chunklength, chunklength];
        //System.Random r = new System.Random();
        int rand = seed;
        for (int i = 0; i < chunklength; i++)
        {
            for (int j = 0; j < chunklength; j++)
            {
                float x = ((Chunk.ChunkLength * pos.x) + i + rand) * 0.01f;
                float y = ((Chunk.ChunkLength * pos.z) + j + rand) * 0.01f;
                map[i, j] = Mathf.RoundToInt((Mathf.PerlinNoise(x, y) * yScale) + yOffset);
            }
        }

        return map;
    }

    public void NewWorld(GenerationSettings settings)
    {        
        WorldManager.GlobalGenerationSettings = settings;
        int[,] supermap = new int[settings.ChunksLength,settings.ChunksLength];
        int[,] map1 = GeneratePerlineMapRandom(settings.ChunksLength,settings.YScale,settings.ChunkPos,settings.YOffset,settings.Seed[0]);
        int[,] map2 = GeneratePerlineMapRandom(settings.ChunksLength, settings.YScale, settings.ChunkPos, settings.YOffset, settings.Seed[1]);
        int[,] map3 = GeneratePerlineMapRandom(settings.ChunksLength, settings.YScale, settings.ChunkPos, settings.YOffset, settings.Seed[2]);

        System.Random r_ = new System.Random();
        for (int i = 0; i < settings.ChunksLength; i++)
        {
            for (int j = 0; j < settings.ChunksLength; j++)
            {
                supermap[i, j] = (map1[i, j] + map2[i, j] + map3[i,j])/3;                
            }
        }

        int chunks_ = settings.ChunksLength / Chunk.ChunkLength;
        for (int cx = 0; cx < chunks_; cx++)
        {
            for (int cy = 0; cy < chunks_; cy++)
            {
                for (int i = 0; i < Chunk.ChunkLength; i++)
                {
                    for (int j = 0; j < Chunk.ChunkLength; j++)
                    {
                        int cxoff_ = (cx * Chunk.ChunkLength) + i, cyoff_ = (cy * Chunk.ChunkLength) + j;

                        Block b_ = new Block();
                        b_.x = cxoff_;
                        b_.y = supermap[cxoff_, cyoff_];
                        b_.z = cyoff_;
                        b_.id = (b_.y > settings.SandLevel) ? (int)Blocks.Grass : b_.id = (int)Blocks.Sand;

                        manager.WorldBlocks.Add(b_);


                        if (b_.y < settings.SandLevel)
                        {
                            manager.Water.SetActive(true);
                            manager.Water.transform.position = new Vector3(Mathf.RoundToInt(GetAvgWorldQuads().x), 4, Mathf.RoundToInt(GetAvgWorldQuads().z));
                            manager.Water.transform.localScale = new Vector3(settings.ChunksLength, settings.SandLevel - 0.1f, settings.ChunksLength);
                        }
                        if (settings.treeSettings.addTrees)
                        {
                            if (b_.y > settings.treeSettings.treeLayerMin && b_.y < settings.treeSettings.treeLayerMax && r_.Next(0, 10) < settings.treeSettings.spawnProbabilty)
                            {
                                DrawTree(new Vector3(b_.x, b_.y + 1, b_.z));
                            }
                        }
                    }
                }
            }
        }
        WorldLoader.SaveWorldGenSettings();
        manager.DrawBlocksToWorld();
    }
    public void GeneratePresetWorld()
    {
        try
        {
            GenerationSettings settings = WorldLoader.LoadWorldGenSettings();
            Debug.Log($"settings is null : {settings == null}");
            Debug.Log($"treesettings is null : {settings.treeSettings == null}");
            int[,] supermap = new int[settings.ChunksLength, settings.ChunksLength];
            int[,] map1 = GeneratePerlineMapRandom(settings.ChunksLength, settings.YScale, settings.ChunkPos, settings.YOffset, settings.Seed[0]);
            int[,] map2 = GeneratePerlineMapRandom(settings.ChunksLength, settings.YScale, settings.ChunkPos, settings.YOffset, settings.Seed[1]);
            int[,] map3 = GeneratePerlineMapRandom(settings.ChunksLength, settings.YScale, settings.ChunkPos, settings.YOffset, settings.Seed[2]);

            System.Random r_ = new System.Random();
            for (int i = 0; i < settings.ChunksLength; i++)
            {
                for (int j = 0; j < settings.ChunksLength; j++)
                {
                    supermap[i, j] = (map1[i, j] + map2[i, j] + map3[i, j])/3;
                }
            }
            int chunks_ = settings.ChunksLength / Chunk.ChunkLength;
            
            
            for(int cx =0; cx < chunks_;cx++)
            {
                for (int cy = 0; cy < chunks_; cy++)
                {
                    for (int i = 0; i < Chunk.ChunkLength; i++)
                    {
                        for (int j = 0; j < Chunk.ChunkLength; j++)
                        {
                            int cxoff_ = (cx * Chunk.ChunkLength) + i, cyoff_ = (cy * Chunk.ChunkLength) + j;

                            Block b_ = new Block();
                            b_.x = cxoff_;
                            b_.y = supermap[cxoff_, cyoff_];
                            b_.z = cyoff_;
                            b_.id = (b_.y > settings.SandLevel) ? (int)Blocks.Grass : b_.id = (int)Blocks.Sand;

                            manager.WorldBlocks.Add(b_);

                            /*
                            try
                            {
                                if (b_.y > settings.treeSettings.treeLayerMin && b_.y < settings.treeSettings.treeLayerMax && r_.Next(0, 10) < settings.treeSettings.spawnProbabilty)
                                {
                                    DrawTree(new Vector3(b_.x, b_.y + 1, b_.z));
                                }
                            }
                            catch(Exception e)
                            {
                                Debug.Log($"Exception caught while drawing trees {e}");
                            }
                            */
                        }
                    }
                }
            }

            manager.DrawBlocksToWorld();
        }
        catch(Exception e)
        {
            Debug.Log($"Exception caught while generation preset world:{e}");
            manager.ShowError($"Exception caught while generation preset world:{e}");
        }
    }
}

public class TreeSettings
{
    public bool addTrees;
    public int treeLayerMin;
    public int treeLayerMax;
    public float spawnProbabilty;
    public TreeSettings() { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_addTrees"></param>
    /// <param name="_treeLayerMin"></param>
    /// <param name="_treeLayerMax"></param>
    /// <param name="_spawnProbabilty">out of 10</param>
    public TreeSettings(bool _addTrees,int _treeLayerMin,int _treeLayerMax, float _spawnProbabilty)
    {
        addTrees = _addTrees;
        treeLayerMin = _treeLayerMin;
        treeLayerMax = _treeLayerMax;
        spawnProbabilty = _spawnProbabilty;
    }
}

public class GenerationSettings
{
    public int  YScale, SandLevel, YOffset, ChunksLength;
    public int[] Seed;
    public TreeSettings treeSettings;
    public Vector3 ChunkPos;
}


/*
                    Block b_ = new Block();
                    b_.x = i;
                    b_.y = supermap[i, j];
                    b_.z = j;
                    b_.id = (b_.y > settings.SandLevel) ? (int)Blocks.Grass : b_.id = (int)Blocks.Sand;

                    manager.WorldBlocks.Add(b_);

                    /*
                    try
                    {
                        if (b_.y > settings.treeSettings.treeLayerMin && b_.y < settings.treeSettings.treeLayerMax && r_.Next(0, 10) < settings.treeSettings.spawnProbabilty)
                        {
                            DrawTree(new Vector3(b_.x, b_.y + 1, b_.z));
                        }
                    }
                    catch(Exception e)
                    {
                        Debug.Log($"Exception caught while drawing trees {e}");
                    }
                    */
//*/





/*
                Block b_ = new Block();
                b_.x = i;
                b_.y = supermap[i,j];
                b_.z = j;
                b_.id = (b_.y > settings.SandLevel) ? (int)Blocks.Grass : b_.id = (int)Blocks.Sand;

                manager.WorldBlocks.Add(b_);
  
 
                 if (b_.y < settings.SandLevel)
                {
                    manager.Water.SetActive(true);                   
                    manager.Water.transform.position = new Vector3(Mathf.RoundToInt(GetAvgWorldQuads().x), 4, Mathf.RoundToInt(GetAvgWorldQuads().z));
                    manager.Water.transform.localScale = new Vector3(settings.ChunksLength, settings.SandLevel - 0.1f, settings.ChunksLength);
                }
                if (settings.treeSettings.addTrees)
                {
                    if (b_.y > settings.treeSettings.treeLayerMin && b_.y < settings.treeSettings.treeLayerMax && r_.Next(0, 10) < settings.treeSettings.spawnProbabilty)
                    {
                        DrawTree(new Vector3(b_.x, b_.y + 1, b_.z));
                    }
                }
 */