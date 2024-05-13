using System;
using System.IO;
using UnityEngine;
using MultiOSFileSystem;
using Assets.Scripts;

public class WorldLoader : MonoBehaviour
{

    public static void SaveWorldJSON(WorldManager worldGen)
    {
        try
        {
            string path = Path.Combine(WorldManager.GlobalGameFilePath, "worlds", WorldManager.WorldName, "chunks");
            //text to be written to file
            string chunkData;
            //number of chunks
            int n = worldGen.WorldBlocks.Count / Chunk.ChunkSize;

            for (int i = 0; i < n; i++)
            {
                Chunk tempChunk = new Chunk();
                for (int j = 0; j < Chunk.ChunkSize; j++)
                {
                    if ((i * Chunk.ChunkSize + j) < worldGen.WorldBlocks.Count)
                        tempChunk.blocks[j] = worldGen.WorldBlocks[i * Chunk.ChunkSize + j];
                    else
                        tempChunk.blocks[j] = new Block(0, 0, 0, (int)Blocks.empty);
                }
                chunkData = Newtonsoft.Json.JsonConvert.SerializeObject(tempChunk);
                File.WriteAllText(Path.Combine(path, $"{i}.chunk"), chunkData);
            }
            Debug.Log($"Saved world {WorldManager.WorldName}");
            //worldGen.ConsoleLog($"Saved world {WorldManager.WorldName} as JSON");
        }catch(Exception e)
        {
            //worldGen.ConsoleLog(e.Message);
            Debug.Log(e.ToString());
        }
        
    }
    public static void SaveToFileLW(WorldManager worldGen)
    {
        try
        {
            string path = Path.Combine(WorldManager.GlobalGameFilePath, "worlds", WorldManager.WorldName, "chunks");
            int n = worldGen.WorldBlocks.Count / Chunk.ChunkSize;
            for (int i = 0; i < n; i++)
            {
                LWPackage package = new LWPackage();
                Chunk tempChunk = new Chunk();
                for (int j = 0; j < Chunk.ChunkSize; j++)
                {
                    if ((i * Chunk.ChunkSize + j) < worldGen.WorldBlocks.Count)
                        tempChunk.blocks[j] = worldGen.WorldBlocks[i * Chunk.ChunkSize + j];
                    else
                        tempChunk.blocks[j] = new Block(0, 0, 0, (int)Blocks.empty);
                }
                package.Write(tempChunk);
                byte[] bytes = package.ToArray();
                File.WriteAllBytes(Path.Combine(path, $"{i}.chunk"), bytes);
            }
            Debug.Log($"Saved world {WorldManager.WorldName}");
            worldGen.ShowError($"Saved world {WorldManager.WorldName} as LW");
        }
        catch (Exception e)
        {
            //worldGen.ShowError(e.Message);
            Debug.Log(e.ToString());
        }
    }
    public static void SaveWorldGenSettings()
    {
        string json_ = JsonUtility.ToJson(WorldManager.GlobalGenerationSettings);
        string path_ = FileSystems.Combine($"{WorldManager.GlobalGameFilePath}^worlds^{WorldManager.WorldName}^GenerationSettings.json", '^');
        if (File.Exists(path_))
        {
            File.WriteAllText(path_, json_);
        }
        else
        {
            File.CreateText(path_).Close();
            File.WriteAllText(path_,json_);
        }
        Debug.Log($"Saved gen settings to {path_} \r\n {json_}");
        path_ = FileSystems.Combine($"{WorldManager.GlobalGameFilePath}^worlds^{WorldManager.WorldName}^Gen Settings.save", '^');
        string strSave_ = SaveObject.ToSaveDataString(WorldManager.GlobalGenerationSettings);
        File.WriteAllText(path_,strSave_);
        Debug.Log($"Test save:\r\n {strSave_}");
    }
    public static void LoadWorldJSon(WorldManager worldGen)
    {
        try
        {
            int _nofchunks = 0;
            string path = Path.Combine(WorldManager.GlobalGameFilePath, "worlds", WorldManager.WorldName, "chunks");
            Debug.Log($"Loading world {path}");
            for(int i = 0; File.Exists(Path.Combine(path, $"{i}.chunk")); i++)
            {
                _nofchunks++;
            }
            for (int i = 0; File.Exists(Path.Combine(path, $"{i}.chunk")); i++)
            {
                try
                {
                    Chunk chunk = Newtonsoft.Json.JsonConvert.DeserializeObject<Chunk>(File.ReadAllText(Path.Combine(path, $"{i}.chunk")));
                    foreach (Block block in chunk.blocks)
                    {
                        worldGen.WorldBlocks.Add(block);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                }
            }
            worldGen.DrawBlocksToWorld();
            //worldGen.ShowError($"Loaded world {WorldManager.WorldName}");
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
            //worldGen.ShowError(e.Message);
        }
    }
    public static GenerationSettings LoadWorldGenSettings()
    {
        string path_ = FileSystems.Combine($"{WorldManager.GlobalGameFilePath}^worlds^{WorldManager.WorldName}^Gen Settings.save", '^');
        string data_ = File.ReadAllText(path_);
        Debug.Log(data_);
        return SaveObject.ToGenerationSettings(data_);
        //return JsonUtility.FromJson<GenerationSettings>(json_);
    }
    public static void LoadWorldLW(WorldManager worldGen)
    {
        try
        {
            string path = Path.Combine(WorldManager.GlobalGameFilePath, "worlds", WorldManager.WorldName, "chunks");
            Debug.Log($"Loading world {path}");
            for (int i = 0; File.Exists(Path.Combine(path, $"{i}.chunk")); i++)
            {
                try
                {
                    LWPackage package = new LWPackage();
                    byte[] bytes = File.ReadAllBytes(Path.Combine(path, $"{i}.chunk"));
                    package.SetBytes(bytes);
                    Chunk temp = package.ReadChunk();
                    foreach (Block block in temp.blocks)
                    {
                        worldGen.WorldBlocks.Add(block);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log("Caught an exc!");
                    Debug.Log(e.ToString());
                }
            }
            worldGen.DrawBlocksToWorld();
            //worldGen.ShowError($"Loaded world {WorldManager.WorldName}");
        }
        catch(Exception e)
        {
            //worldGen.ShowError(e.Message);
            Debug.Log(e.ToString());
        }
    }
    
}
