using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using TMPro;
using JetBrains.Annotations;
using System.Threading;
using MultiOSFileSystem;

public class WorldManager : MonoBehaviour
{
    public static string WorldName = "";
    public static string GlobalGameFilePath;
    public static int DeviceOS = (int)Devices.Android;
    public static WorldSettings GlobalWorldSettings = new WorldSettings();
    public static GenerationSettings GlobalGenerationSettings = new GenerationSettings();
    public static bool CanSaveData = false;

    public WorldLoader loader;
    public WorldGeneration worldStructures;
    public MobileControler mobileControler;

    // Start is called before the first frame update
    public List<Block> WorldBlocks = new List<Block>();
    public Dictionary<Vector3, Block> WorldBlockDictionary = new Dictionary<Vector3, Block>();
    public List<GameObject> WorldBlocksGameObject = new List<GameObject>();

    public List<GameObject> DirtUnionMeshList = new List<GameObject>();
    public List<GameObject> StoneUnionMeshList = new List<GameObject>();
    public List<GameObject> GrassUnionMeshList = new List<GameObject>();
    public List<GameObject> CobblestoneUnionMeshList = new List<GameObject>();
    public List<GameObject> OakPlankUnionMeshList = new List<GameObject>();
    public List<GameObject> OakLogUnionMeshList = new List<GameObject>();
    public List<GameObject> OakLeavesUnionMeshList = new List<GameObject>();
    public List<GameObject> GlassUnionMeshList = new List<GameObject>();
    public List<GameObject> SandUnionMeshList = new List<GameObject>();

    [Header("World Generation")]
    public string Mode;
    public int Scale, SandLevel;
    public float TreeFrequency = 1f;

    public int ChunkX;
    public int ChunkY;
    public int[] Seed;
    public int SeedRandomMax = 4000000;
    public int SeedRandomMin = -4000000;

    public bool LoadWorld;
    public bool IntegrateToMesh;

    public GameObject player;

    [Header("Union Mesh")]
    public Transform UnionMeshes;
    public GameObject BlockOrigin;
    public GameObject DirtUnionMesh;
    public GameObject StoneUnionMesh;
    public GameObject GrassUnionMesh;
    public GameObject CobblestoneUnionMesh;
    public GameObject OakPlankUnionMesh;
    public GameObject OakLogUnionMesh;
    public GameObject OakLeavesUnionMesh;
    public GameObject GlassUnionMesh;
    public GameObject SandUnionMesh;
    public GameObject WorldBlockSpace;

    [Header("Block instance")]
    public GameObject Dirt;
    public GameObject Grass;
    public GameObject Stone;
    public GameObject Cobblestone;
    public GameObject OakPlank;
    public GameObject OakLog;
    public GameObject OakLeaves;
    public GameObject Glass;
    public GameObject Sand;
    public GameObject Water;

    [Header("Mesh Instance")]
    public GameObject Cube_tube;

    [Header("UI")]
    public TextMeshProUGUI ConsoleText;

    [Header("test")]
    public GameObject test_mesh;
    public GameObject test_mesh_prefab;

    public void Start()
    {

        Debug.Log("Initating world...");
        InitiateWorld();
        //test_MeshMaker();
    }
    public void drawMeshQuad(string _type, Vector3 offset)
    {
        float x_ = 1, y_ = 1;

        MeshRenderer meshRenderer = test_mesh.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = test_mesh.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(x_, 0, 0),
            new Vector3(0, 0, y_),
            new Vector3(x_, 0, y_)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }
    public void InitiateWorld()
    {
        try
        {
            if(DeviceOS == (int)Devices.Android)
            {
                mobileControler.SetActiveControls(true);
            }
            //ShowError($"Device : {DeviceOS}");

            //gets world settings
            WorldSettings _ws = JsonUtility.FromJson<WorldSettings>(File.ReadAllText(Path.Combine(new string[] { WorldManager.GlobalGameFilePath, "worlds", WorldManager.WorldName, "WorldSettings.txt" })));
            Debug.Log($"Initiated world {WorldName} with file setting {_ws.SaveType}.");
            //string path = $"{Application.streamingAssetsPath}\\worlds\\{WorldName}\\chunks\\0.chunk";
            string path = "";
            path = Path.Combine(new string[] { WorldManager.GlobalGameFilePath, "worlds", WorldName, "chunks", "0.chunk" });
            
            //load the world
            if (File.Exists(path))
            {
                Debug.Log("Found save file(s)");
                if (_ws.SaveType == 0)
                {
                    WorldLoader.LoadWorldJSon(this);
                }
                else
                {
                    WorldLoader.LoadWorldLW(this);
                }
            }
            else if (File.Exists(Path.Combine(WorldManager.GlobalGameFilePath,"worlds",WorldName,"GenerationSettings.json")))
            {
                GlobalGenerationSettings = WorldLoader.LoadWorldGenSettings();
                Debug.Log($"Found generation settings. generating world using generation settings. \r\n {JsonUtility.ToJson(WorldLoader.LoadWorldGenSettings(),true)}");
                worldStructures.GeneratePresetWorld();
            }
            else
            {
                //make new world since it does not exists
                Debug.Log($"Could not find any save save file(s). Generating new world. {_ws.BlockGenType}");
                //worldStructures.DrawRandomTerrainMountains(Chunk.ChunkLength * _ws.ChunkLength, _ws.YScale, new Vector3(0, 0, 0), new TreeSettings(true, 24, 48, _ws.TreeSpawnRate), SandLevel, _ws.YOffset);
                System.Random r = new System.Random(); 
                int s1 = r.Next(SeedRandomMin, SeedRandomMax), s2 = r.Next(-1000000, 1000000) , s3 = r.Next(SeedRandomMin, SeedRandomMax);
                Debug.Log($"seed 1 : {s1} , seed 2 : {s2} , seed 3: {s3}");
                GenerationSettings gs_ = new GenerationSettings()
                {
                    Seed = new int[3] { s1, s2 , s3},
                    treeSettings = new TreeSettings(true, 24, 48, _ws.TreeSpawnRate),
                    ChunkPos = new Vector3(0,0,0),
                    ChunksLength = _ws.ChunkLength * Chunk.ChunkLength,
                    YScale = _ws.YScale,
                    YOffset = _ws.YOffset,
                    SandLevel = SandLevel
                };
                GlobalGenerationSettings = gs_;
                worldStructures.NewWorld(gs_);
                    

                Debug.Log($"world settings:\r\n _ws.ChunkLength:{_ws.ChunkLength} \r\n Blocks:{WorldBlocks.Count}");
                
            }
            //i tried multi threading
            //ThreadStart ts_ = new ThreadStart(DrawBlocksToWorld);
            //Thread t_ = new Thread(ts_);
            //t_.Start();

            //put the blocks in the world
            DrawBlocksToWorld();
        }
        catch (Exception e)
        {
            Debug.Log($"No WorldSettings file found. using \"GlobalWorldSettings\". \r\n {e.Message}");
            ShowError($"Caught Exception while initiating world. \r\n {e.ToString()}");
            WorldSettings _ws = GlobalWorldSettings;
            worldStructures.DrawRandomTerrainMountains(Chunk.ChunkLength * _ws.ChunkLength, _ws.YScale, new Vector3(0, 0, 0), new TreeSettings(true, 24, 48, _ws.TreeSpawnRate), SandLevel, _ws.YOffset);
            Debug.Log("Generating new terrain...");
            Debug.Log("world block count:" + WorldBlocks.Count);
            //ThreadStart ts_ = new ThreadStart(DrawBlocksToWorld);
            //Thread t_ = new Thread(ts_);
            //t_.Start();
            DrawBlocksToWorld();
        }

    }
    public void OptimisedWorldLoad()
    {
        
    }
    public void DrawBlocksToWorld()
    {
        int numberOfChunks = WorldBlocks.Count / Chunk.ChunkSize;

        for(int i = 0; i < numberOfChunks; i++)
        {
            GameObject dirtUnionMesh = Instantiate(DirtUnionMesh, UnionMeshes.position,UnionMeshes.rotation,UnionMeshes);
            dirtUnionMesh.name = $"{i}_DirtUnionMesh";
            GameObject stoneUnionMesh = Instantiate(StoneUnionMesh, UnionMeshes.position,UnionMeshes.rotation,UnionMeshes);
            stoneUnionMesh.name = $"{i}_StoneUnionMesh";
            GameObject grassUnionMesh = Instantiate(GrassUnionMesh, UnionMeshes.position,UnionMeshes.rotation,UnionMeshes);
            grassUnionMesh.name = $"{i}_GrassUnionMesh";
            GameObject cobblestoneUnionMesh = Instantiate(CobblestoneUnionMesh, UnionMeshes.position,UnionMeshes.rotation,UnionMeshes);
            cobblestoneUnionMesh.name = $"{i}_CobblestoneUnionMesh";
            GameObject oakPlankUnionMesh = Instantiate(OakPlankUnionMesh, UnionMeshes.position,UnionMeshes.rotation,UnionMeshes);
            oakPlankUnionMesh.name = $"{i}_OakPlankUnionMesh";
            GameObject oakLogUnionMesh = Instantiate(OakLogUnionMesh, UnionMeshes.position, UnionMeshes.rotation, UnionMeshes);
            oakLogUnionMesh.name = $"{i}_OakLogUnionMesh";
            GameObject oakLeavesUnionMesh = Instantiate(OakLeavesUnionMesh, UnionMeshes.position, UnionMeshes.rotation, UnionMeshes);
            oakLeavesUnionMesh.name = $"{i}_OakLeavesUnionMesh";
            GameObject glassUnionMesh = Instantiate(GlassUnionMesh, UnionMeshes.position, UnionMeshes.rotation, UnionMeshes);
            glassUnionMesh.name = $"{i}_GlassUnionMesh";
            GameObject sandUnionMesh = Instantiate(SandUnionMesh, UnionMeshes.position, UnionMeshes.rotation, UnionMeshes);
            sandUnionMesh.name = $"{i}_SandUnionMesh";

            DirtUnionMeshList.Add(dirtUnionMesh);
            StoneUnionMeshList.Add(stoneUnionMesh);
            GrassUnionMeshList.Add(grassUnionMesh);
            CobblestoneUnionMeshList.Add(cobblestoneUnionMesh);
            OakPlankUnionMeshList.Add(oakPlankUnionMesh);
            OakLogUnionMeshList.Add(oakLogUnionMesh);
            OakLeavesUnionMeshList.Add(oakLeavesUnionMesh);
            GlassUnionMeshList.Add(glassUnionMesh);
            SandUnionMeshList.Add(sandUnionMesh);
        }
        Debug.Log($"Number of chunks : {numberOfChunks}");

        int chunkNumber = 0;
        for (int i = 0; i < WorldBlocks.Count; i++)
        {
            chunkNumber = i / Chunk.ChunkSize;
            DrawNewBlock(WorldBlocks[i], chunkNumber);
        }
        //Debug.Log($"Max chunk: {chunkNumber}");

        if (IntegrateToMesh)
        {
            RemoveEmpties();
            CombineMeshs();
        }
        
        Debug.Log("Done pushing blocks!");
    }

    private int t_aloc = 0;
    public void DrawNewBlock(Block block,int chunkIndex)
    {
        //snow layer = 56
        t_aloc++;
        int _mt_aloc = t_aloc / Chunk.ChunkSize;

        BlockOrigin.transform.position = new Vector3(block.x, block.y, block.z);
        GameObject temp;
        try
        {
            if (!WorldBlockDictionary.ContainsKey(new Vector3(block.x, block.y, block.z))) 
            {
                //WorldBlockDictionary.Add(new Vector3(block.x, block.y, block.z), block);
                //temp = Instantiate(Stone, BlockOrigin.transform.position, BlockOrigin.transform.rotation, StoneUnionMeshList[chunkIndex].transform);
                //StoneUnionMeshList[chunkIndex + _mt_aloc].name = $"StoneUnionMeshList_{chunkIndex + _mt_aloc}";
                //WorldBlocksGameObject.Add(temp);

                switch (block.id)
                {
                    case (int)Blocks.Dirt:
                        temp = Instantiate(Dirt, BlockOrigin.transform.position, BlockOrigin.transform.rotation, DirtUnionMeshList[chunkIndex + _mt_aloc].transform);
                        DirtUnionMeshList[chunkIndex + _mt_aloc].name = $"DirtUnionMeshList_{chunkIndex + _mt_aloc}";
                        if (temp.transform.position.y >= 56)
                        {
                            temp.GetComponent<Renderer>().material.color = Color.white;
                        }
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.Grass:
                        temp = Instantiate(Grass, BlockOrigin.transform.position, BlockOrigin.transform.rotation, GrassUnionMeshList[chunkIndex + _mt_aloc].transform);
                        if (temp.transform.position.y >= 56)
                        {
                            temp.GetComponent<Renderer>().material.color = Color.white;
                        }
                        GrassUnionMeshList[chunkIndex + _mt_aloc].name = $"GrassUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.Stone:
                        temp = Instantiate(Stone, BlockOrigin.transform.position, BlockOrigin.transform.rotation, StoneUnionMeshList[chunkIndex + _mt_aloc].transform);
                        StoneUnionMeshList[chunkIndex + _mt_aloc].name = $"StoneUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.Cobblestone:
                        temp = Instantiate(Cobblestone, BlockOrigin.transform.position, BlockOrigin.transform.rotation, CobblestoneUnionMeshList[chunkIndex + _mt_aloc].transform);
                        CobblestoneUnionMeshList[chunkIndex + _mt_aloc].name = $"CobblestoneUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.OakPlanks:
                        temp = Instantiate(OakPlank, BlockOrigin.transform.position, BlockOrigin.transform.rotation, OakPlankUnionMeshList[chunkIndex + _mt_aloc].transform);
                        OakPlankUnionMeshList[chunkIndex + _mt_aloc].name = $"OakPlankUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.OakLog:
                        temp = Instantiate(OakLog, BlockOrigin.transform.position, BlockOrigin.transform.rotation, OakLogUnionMeshList[chunkIndex + _mt_aloc].transform);
                        OakLogUnionMeshList[chunkIndex + _mt_aloc].name = $"OakLogUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.OakLeaves:
                        temp = Instantiate(OakLeaves, BlockOrigin.transform.position, BlockOrigin.transform.rotation, OakLeavesUnionMeshList[chunkIndex + _mt_aloc].transform);
                        OakLeavesUnionMeshList[chunkIndex + _mt_aloc].name = $"OakLeavesUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.Glass:
                        temp = Instantiate(Glass, BlockOrigin.transform.position, BlockOrigin.transform.rotation, GlassUnionMeshList[chunkIndex + _mt_aloc].transform);
                        GlassUnionMeshList[chunkIndex + _mt_aloc].name = $"GlassUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.Sand:
                        temp = Instantiate(Sand, BlockOrigin.transform.position, BlockOrigin.transform.rotation, SandUnionMeshList[chunkIndex + _mt_aloc].transform);
                        GlassUnionMeshList[chunkIndex + _mt_aloc].name = $"GlassUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    default:
                        Debug.Log($"Block {Newtonsoft.Json.JsonConvert.SerializeObject(block)} is an invalid block or empty.");
                        break;
                }
            }

        }
        catch(Exception e)
        {
            //Debug.Log($"index:{chunkIndex},UnionMeshes:{{DirtUnionMeshList.Count:{DirtUnionMeshList.Count},StoneUnionMeshList.Count:{StoneUnionMeshList.Count},CobblestoneMeshUnionList.Count:{CobblestoneUnionMeshList.Count},GrassUnionMeshList.Count:{GrassUnionMeshList.Count},OakPlankUnionMeshList.Count:{OakPlankUnionMeshList.Count}}}");
            //Debug.Log(e.ToString());
            //ShowError("DrawNewBlock(Block block,int chunkIndex):" + e.Message);
        }
        
    }
    public void RemoveEmpties()
    {
        foreach (GameObject temp in DirtUnionMeshList)
        {
            if (!(temp.transform.childCount>0))
                Destroy(temp);
        }
        foreach (GameObject temp in GrassUnionMeshList)
        {
            if (!(temp.transform.childCount > 0))
                Destroy(temp);
        }
        foreach (GameObject temp in StoneUnionMeshList)
        {
            if (!(temp.transform.childCount > 0))
                Destroy(temp);
        }
        foreach (GameObject temp in CobblestoneUnionMeshList)
        {
            if (!(temp.transform.childCount > 0))
                Destroy(temp);
        }
        foreach (GameObject temp in OakPlankUnionMeshList)
        {
            if (!(temp.transform.childCount > 0))
                Destroy(temp);
        }
        foreach (GameObject temp in SandUnionMeshList)
        {
            if (!(temp.transform.childCount > 0))
                Destroy(temp);
        }

        Debug.Log("Removed empties!");
    }
    public void CombineMeshs()
    {
        foreach(GameObject temp in DirtUnionMeshList)
        {
            temp.GetComponent<MeshCombiner>().CombineMeshes();
        }
        foreach (GameObject temp in GrassUnionMeshList)
        {
            temp.GetComponent<MeshCombiner>().CombineMeshes();
        }
        foreach (GameObject temp in StoneUnionMeshList)
        {
            temp.GetComponent<MeshCombiner>().CombineMeshes();
        }
        foreach (GameObject temp in CobblestoneUnionMeshList)
        {
            temp.GetComponent<MeshCombiner>().CombineMeshes();
        }
        foreach (GameObject temp in OakPlankUnionMeshList)
        {
            temp.GetComponent<MeshCombiner>().CombineMeshes();
        }
        foreach (GameObject temp in OakLogUnionMeshList)
        {
            temp.GetComponent<MeshCombiner>().CombineMeshes();
        }
        foreach (GameObject temp in OakLeavesUnionMeshList)
        {
            temp.GetComponent<MeshCombiner>().CombineMeshes();
        }
        foreach (GameObject temp in GlassUnionMeshList)
        {
            temp.GetComponent<MeshCombiner>().CombineMeshes();
        }
        foreach (GameObject temp in SandUnionMeshList)
        {
            temp.GetComponent<MeshCombiner>().CombineMeshes();
        }

        for (int i = 0; i < WorldBlocksGameObject.Count; i++)
        {
            Destroy(WorldBlocksGameObject[i]);
        }
    }
    public void ShowError(string _msg)
    {
        string tmsg_ = ConsoleText.text;
        tmsg_ += $"[{DateTime.Now}]{_msg}\r\n";
        Debug.Log($"[{DateTime.Now}]{_msg}");
        ConsoleText.text = tmsg_;
    }

}

public enum Blocks
{
    empty=-1,Dirt,Grass,Stone,Cobblestone,OakPlanks,OakLog,OakLeaves,Glass,Sand
}

public class Verts
{
    public static Vector3[] PlaneXY_plusZ = new Vector3[4]
            {
                new Vector3(0, 0, 0) ,
                new Vector3(-1, 0, 0) ,
                new Vector3(0, 1, 0) ,
                new Vector3(-1, 1, 0)
            };
    public static Vector3[] PlaneXY_minusZ = new Vector3[4]
            {
                new Vector3(0, 0, 0) ,
                new Vector3(1, 0, 0) ,
                new Vector3(0, 1, 0) ,
                new Vector3(1, 1, 0)
            };
}

public class WorldMesh
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public Mesh mesh = new Mesh();
}



public class Chunk
{
    public Block[] blocks= new Block[ChunkSize];
    public static readonly int ChunkSize = 1024;
    public static int ChunkLength { 
        get 
        {
            return (int)Math.Sqrt(ChunkSize);
        } 
    }
    public Chunk()
    {
        for(int i = 0; i < blocks.Length; i++)
        {
            blocks[i] = new Block();
        }
    }
}

public class Block
{
    public int x,y,z,id;
    public Block(int x,int y,int z,int id)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.id = id;
    }
    public Block(Vector3 pos,int id)
    {
        x = (int)pos.x;
        y = (int)pos.y;
        z = (int)pos.z;
        this.id = id;
    }
    public bool isTranspart()
    {
        if (id == (int)Blocks.Glass || id == (int)Blocks.OakLeaves)
            return true;
        else
            return false;
    }
    public Vector3 GetVector3()
    {
        return new Vector3(x,y,z);
    }
    public Block() { }
}


//int[] ids_ = { (int)Blocks.Grass, (int)Blocks.Dirt, (int)Blocks.Cobblestone, (int)Blocks.OakPlanks, (int)Blocks.Stone };

/*
for(int x = 0; x < _ws.ChunkLength; x++)
{
    for (int z = 0; z < _ws.ChunkLength; z++)
    {
        ///GenerateChunks(ids_, _ws.YScale, toStringGenType(_ws.GenType), new Vector3(x,0,z));
        worldStructures.DrawGrassTerrainRandom(Chunk.ChunkLength, _ws.YScale, new Vector3(x, 0, z), new TreeSettings(true,36,48, TreeFrequency));
    }
}
*/

/*
                    List <Chunk> chunks_ = new List<Chunk>();
                    for(int i=0 ; i< _ws.ChunkLength ; i++)
                    {
                        for (int j = 0; j < _ws.ChunkLength; j++)
                        {
                            chunks_.Add(worldStructures.ReturnChunkRandom(new Vector3(i,0,j), _ws.YScale,_ws.YOffset, SandLevel, new TreeSettings(true, 24, 48, _ws.TreeSpawnRate)));
                        }
                    }
                    Debug.Log("chunk data:\r\n"+Newtonsoft.Json.JsonConvert.SerializeObject(chunks_.ToArray()));

                    for(int i=0; i < _ws.ChunkLength * _ws.ChunkLength; i++)
                    {
                        for(int j = 0; j < Chunk.ChunkSize; j++)
                        {
                            WorldBlocks.Add(chunks_[i].blocks[j]);
                            Vector3 v_ = new Vector3(chunks_[i].blocks[j].x, chunks_[i].blocks[j].y, chunks_[i].blocks[j].z);
                            WorldBlockDictionary.Add(v_, chunks_[i].blocks[j]);
                        }
                    }
 */


/*
            List<Chunk> chunks_ = new List<Chunk>();
            for (int i = 0; i < _ws.ChunkLength; i++)
            {
                for (int j = 0; j < _ws.ChunkLength; j++)
                {
                    Debug.Log($"i:{i},j:{j}");
                    Chunk _t = worldStructures.ReturnChunkRandom(new Vector3(i, 0, j), _ws.YScale, _ws.YOffset, SandLevel, new TreeSettings(true, 24, 48, _ws.TreeSpawnRate));
                    chunks_.Add(_t);
                    //Debug.Log($"chunk data:{JsonUtility.ToJson(_t)}");
                }
            }
            for(int i=0;i< _ws.ChunkLength* _ws.ChunkLength; i++)
            {
                for(int j = 0; j < Chunk.ChunkSize; j++)
                {
                    WorldBlocks.Add(chunks_[i].blocks[j]);
                }
            }
 
 */

/*
       public void test_MeshMaker()
   {
       //drawQuadPlaneAxisYUp(new Vector3(0, 0,0));
       MeshRenderer meshRenderer = test_mesh.AddComponent<MeshRenderer>();
       if (meshRenderer != null)
       {
           if (meshRenderer.sharedMaterial == null)
           {
               meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));
           }
       }
       else
       {
           Debug.Log("meshRenderer is null");
       }


       MeshFilter meshFilter = test_mesh.AddComponent<MeshFilter>();

       Mesh mesh = new Mesh();

       Vector3[] vertices = new Vector3[8]
       {
           new Vector3(0, 0, 0) ,
           new Vector3(-1, 0, 0) ,
           new Vector3(0, 1, 0) ,
           new Vector3(-1, 1, 0) ,
           new Vector3(0, 0, 0) ,
           new Vector3(1, 0, 0) ,
           new Vector3(0, 1, 0) ,
           new Vector3(1, 1, 0)
       };
       mesh.vertices = vertices;

       int[] tris = new int[6]
       {
           // lower left triangle
           0, 2, 1,
           // upper right triangle
           2, 3, 1
       };
       mesh.triangles = tris;

       Vector3[] normals = new Vector3[8]
       {
           -Vector3.forward,
           -Vector3.forward,
           -Vector3.forward,
           -Vector3.forward,
           -Vector3.forward,
           -Vector3.forward,
           -Vector3.forward,
           -Vector3.forward
       };
       mesh.normals = normals;

       Vector2[] uv = new Vector2[8]
       {
           new Vector2(0, 0),
           new Vector2(1, 0),
           new Vector2(0, 1),
           new Vector2(1, 1),
           new Vector2(0, 0),
           new Vector2(1, 0),
           new Vector2(0, 1),
           new Vector2(1, 1)
       };
       mesh.uv = uv;

       meshFilter.mesh = mesh;
   }
*/

/*
    public void GenerateChunks(int id,int yscale,string mode,Vector3 chunkPos)
    {
        System.Random r = new System.Random();

        worldStructures.DrawTree(chunkPos * Chunk.ChunkLength);

        for (int x = 0; x < (int)Mathf.Sqrt(Chunk.ChunkSize); x++)
        {
            for(int z = 0; z < (int)Mathf.Sqrt(Chunk.ChunkSize); z++)
            {
                Block block = new Block();
                if (mode == "sinx")
                {
                    block.id = id;
                    block.y = (int)(Mathf.Sin(x) * yscale);
                }
                else if(mode == "sinz")
                {
                    block.id = id;
                    block.y = (int)(Mathf.Sin(z) * yscale);
                }
                else if (mode == "sinxz")
                {
                    block.id = id;
                    block.y = (int)(Mathf.Sin(z*x) * yscale);
                }
                else if (mode == "random")
                {
                    block.id = id;
                    block.y = r.Next(0,1)*yscale;
                }
                else if(mode == "random2")
                {
                    block.id = id;
                    block.y = r.Next(-(yscale/2),(yscale/2));
                }
                else
                {
                    block.id = id;
                    block.y = yscale;
                }
                block.x = x + (int)(chunkPos.x * Mathf.Sqrt(Chunk.ChunkSize));
                block.z = z + (int)(chunkPos.y * Mathf.Sqrt(Chunk.ChunkSize));
                WorldBlocks.Add(block);
            }
        }
        Debug.Log("Done generating terrain!");
    }
*/


/*
    public void GenerateChunks(int[] id, int yScale, string mode, Vector3 chunkPos)
    {
        System.Random r = new System.Random();
        worldStructures.DrawTree(chunkPos * Chunk.ChunkLength);

        for (int x = 0; x < (int)Mathf.Sqrt(Chunk.ChunkSize); x++)
        {
            for (int z = 0; z < (int)Mathf.Sqrt(Chunk.ChunkSize); z++)
            {
                Block block = new Block();
                if (mode == "sinx")
                {
                    block.id = id[r.Next(0,id.Length)];
                    block.y = (int)(Mathf.Sin(x) * yScale) + (int)chunkPos.y;
                }
                else if (mode == "sinz")
                {
                    block.id = id[r.Next(0, id.Length)];
                    block.y = (int)(Mathf.Sin(z) * yScale) + (int)chunkPos.y;
                }
                else if (mode == "sinxz")
                {
                    block.id = id[r.Next(0, id.Length)];
                    block.y = (int)(Mathf.Sin(z * x) * yScale)+ (int)chunkPos.y;
                }
                else if (mode == "random")
                {
                    block.id = id[r.Next(0, id.Length)];
                    block.y = (int)Math.Round((double)(r.Next(0, yScale)))+ (int)chunkPos.y;
                }
                else if (mode == "random2")
                {
                    block.id = id[r.Next(0, id.Length)];
                    block.y = r.Next(-(yScale / 2), (yScale / 2))+ (int)chunkPos.y;
                }
                else
                {
                    block.id = id[r.Next(0, id.Length)];
                    block.y = yScale+ (int)chunkPos.y;
                }
                block.x = x + (int)(chunkPos.x * (int)Mathf.Sqrt(Chunk.ChunkSize));
                block.z = z + (int)(chunkPos.z * (int)Mathf.Sqrt(Chunk.ChunkSize));
                WorldBlocks.Add(block);
            }
        }
        Debug.Log("Done generating terrain!");
    }
 */


/*  public static string[] Split(string _string, char _separater, bool _canHaveSeparator = true, bool _canHaveQuotes = false)
    {
        List<string> _r = new List<string>();
        string _e = "";
        bool _incSeparaterChar = false;
        for (int i = 0; i < _string.Length; i++)
        {
            if ((_string[i] == '\"' || _string[i] == '\'') && _canHaveSeparator)
            {
                _incSeparaterChar = Toggle(_incSeparaterChar);
                if (_canHaveQuotes)
                {
                    _r.Add($"{_string[i]}");
                }
            }
            else if (_string[i] != _separater || _incSeparaterChar)
            {
                _e += _string[i];
            }
            else
            {
                _r.Add(_e);
                _e = "";
            }
        }
        _r.Add(_e);
        for (int i = 0; i < _r.Count; i++)
        {
            if (_r[i] == "" || _r[i] == " ")
            {
                _r.RemoveAt(i);
            }
        }
        if (_r.Count == 0)
        {
            _r.Add("");
        }
        return _r.ToArray();
    }
    public static bool Toggle(bool b)
    {
        if (b)
            return false;
        else
            return true;
    }    */

/*
                switch (block.id)
                {
                    case (int)Blocks.Dirt:
                        if(WorldBlockDictionary.ContainsKey(new Vector3(block.x - 1, block.y,block.z)) && WorldBlockDictionary.ContainsKey(new Vector3(block.x + 1, block.y, block.z)))
                        {
                            temp = Instantiate(Cube_tube, BlockOrigin.transform.position, BlockOrigin.transform.rotation, DirtUnionMeshList[chunkIndex + _mt_aloc].transform);
                        }
                        else
                        {
                            temp = Instantiate(Dirt, BlockOrigin.transform.position, BlockOrigin.transform.rotation, DirtUnionMeshList[chunkIndex + _mt_aloc].transform);
                        }
                        DirtUnionMeshList[chunkIndex + _mt_aloc].name = $"DirtUnionMeshList_{chunkIndex + _mt_aloc}";
                        if (temp.transform.position.y >= 56)
                        {
                            temp.GetComponent<Renderer>().material.color = Color.white;
                        }
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.Grass:                        
                            temp = Instantiate(Grass, BlockOrigin.transform.position, BlockOrigin.transform.rotation, GrassUnionMeshList[chunkIndex + _mt_aloc].transform);                        
                        if (temp.transform.position.y >= 56)
                        {
                            temp.GetComponent<Renderer>().material.color = Color.white;
                        }
                        GrassUnionMeshList[chunkIndex + _mt_aloc].name = $"GrassUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.Stone:
                        temp = Instantiate(Stone, BlockOrigin.transform.position, BlockOrigin.transform.rotation, StoneUnionMeshList[chunkIndex + _mt_aloc].transform);
                        StoneUnionMeshList[chunkIndex + _mt_aloc].name = $"StoneUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.Cobblestone:
                        temp = Instantiate(Cobblestone, BlockOrigin.transform.position, BlockOrigin.transform.rotation, CobblestoneUnionMeshList[chunkIndex + _mt_aloc].transform);
                        CobblestoneUnionMeshList[chunkIndex + _mt_aloc].name = $"CobblestoneUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.OakPlanks:
                        temp = Instantiate(OakPlank, BlockOrigin.transform.position, BlockOrigin.transform.rotation, OakPlankUnionMeshList[chunkIndex + _mt_aloc].transform);
                        OakPlankUnionMeshList[chunkIndex + _mt_aloc].name = $"OakPlankUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.OakLog:
                        temp = Instantiate(OakLog, BlockOrigin.transform.position, BlockOrigin.transform.rotation, OakLogUnionMeshList[chunkIndex + _mt_aloc].transform);
                        OakLogUnionMeshList[chunkIndex + _mt_aloc].name = $"OakLogUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.OakLeaves:
                        temp = Instantiate(OakLeaves, BlockOrigin.transform.position, BlockOrigin.transform.rotation, OakLeavesUnionMeshList[chunkIndex + _mt_aloc].transform);
                        OakLeavesUnionMeshList[chunkIndex + _mt_aloc].name = $"OakLeavesUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.Glass:
                        temp = Instantiate(Glass, BlockOrigin.transform.position, BlockOrigin.transform.rotation, GlassUnionMeshList[chunkIndex + _mt_aloc].transform);
                        GlassUnionMeshList[chunkIndex + _mt_aloc].name = $"GlassUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    case (int)Blocks.Sand:
                        temp = Instantiate(Sand, BlockOrigin.transform.position, BlockOrigin.transform.rotation, SandUnionMeshList[chunkIndex + _mt_aloc].transform);
                        GlassUnionMeshList[chunkIndex + _mt_aloc].name = $"GlassUnionMeshList_{chunkIndex + _mt_aloc}";
                        WorldBlocksGameObject.Add(temp);
                        break;
                    default:
                        Debug.Log($"Block {Newtonsoft.Json.JsonConvert.SerializeObject(block)} is an invalid block or empty.");
                        break;
                }
 */