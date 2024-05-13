using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using MultiOSFileSystem;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class StarterSceneScript : MonoBehaviour
{

    [Header("Creat new world")]
    public TMP_InputField WorldNameInp;
    public TMP_InputField WorldChunksLength;
    public TMP_Dropdown SaveTypeDrop;
    public TMP_Dropdown GenTypeDrop;
    public TMP_Dropdown BlockTypeDrop;
    public TMP_InputField YScale;
    public TMP_InputField YOffset;
    public Slider TreeSpawnRateSlider;

    [Header("Select world")]
    public TMP_Dropdown SelectWorld;

    [Header("Settings")]
    public Slider SensitivitySlider;
    public Slider FOVSlider;
    public Slider NearClipSlider;
    public Slider FarClipSlider;
    public TMP_Dropdown quality;


    [Header("Error mag")]
    public GameObject ErrorPage;
    public TextMeshProUGUI ErrorMsg;
    public TextMeshProUGUI ErrorName;


    public void InitiateFiles()
    {
        if (WorldManager.DeviceOS == (int)Devices.Windows)
        {
            try
            {
                //= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mc_copy")

                WorldManager.GlobalGameFilePath = Path.Combine(Application.persistentDataPath);
                if (!File.Exists(Path.Combine(WorldManager.GlobalGameFilePath, "Player settings.txt")))
                {
                    File.CreateText(Path.Combine(WorldManager.GlobalGameFilePath, "Player settings.txt")).Dispose();
                    Player.settings = new PlayerSettings(SensitivitySlider.value, FOVSlider.value, FarClipSlider.value, NearClipSlider.value);
                    File.WriteAllText(Path.Combine(WorldManager.GlobalGameFilePath, "Player settings.txt"), Newtonsoft.Json.JsonConvert.SerializeObject(new PlayerSettings(SensitivitySlider.value, FOVSlider.value, FarClipSlider.value, NearClipSlider.value)));
                }
                string[] paths_ = { WorldManager.GlobalGameFilePath };
                if (!Directory.Exists(Path.Combine(paths_)))
                {
                    Directory.CreateDirectory(Path.Combine(paths_)); ;
                }
                paths_ = new string[] { WorldManager.GlobalGameFilePath, "worlds" };
                if (!Directory.Exists(Path.Combine(paths_)))
                {
                    Directory.CreateDirectory(Path.Combine(paths_));
                }
                if (!File.Exists(Path.Combine(WorldManager.GlobalGameFilePath, "Player settings.txt")))
                {
                    File.CreateText(Path.Combine(WorldManager.GlobalGameFilePath, "Player settings.txt")).Dispose();
                }

                paths_ = new string[] { WorldManager.GlobalGameFilePath, "worlds", "world list.txt" };
                if (!File.Exists(Path.Combine(paths_)))
                {
                    File.CreateText(Path.Combine(paths_)).Dispose();
                    File.WriteAllText(Path.Combine(paths_), "{\"worldArr\":[{\"Name\":\"[select world]\"}]}");
                }

                Debug.Log($"made file : {Path.Combine(paths_)}");
            }
            catch (Exception e)
            {
                ShowError("Error while initiating game", e.Message + Environment.NewLine + Environment.NewLine + "This is a critical error. the game will not function properly because of this error. " + Environment.NewLine + "please report this.");
                Debug.Log(e.ToString());
            }
        }
        else if(WorldManager.DeviceOS == (int)Devices.Android)
        {
            try
            {
                //= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mc_copy")
                
                WorldManager.GlobalGameFilePath = Path.Combine(Application.persistentDataPath);

                string[] paths_ = { WorldManager.GlobalGameFilePath };
                if (!Directory.Exists(Path.Combine(paths_)))
                {
                    Directory.CreateDirectory(Path.Combine(paths_)); ;
                }
                paths_ = new string[] { WorldManager.GlobalGameFilePath, "worlds" };
                if (!Directory.Exists(Path.Combine(paths_)))
                {
                    Directory.CreateDirectory(Path.Combine(paths_));
                }
                if (!File.Exists(Path.Combine(WorldManager.GlobalGameFilePath, "Player settings.txt")))
                {
                    File.CreateText(Path.Combine(WorldManager.GlobalGameFilePath, "Player settings.txt")).Dispose();
                }

                paths_ = new string[] { WorldManager.GlobalGameFilePath, "worlds", "world list.txt" };
                if (!File.Exists(Path.Combine(paths_)))
                {
                    File.CreateText(Path.Combine(paths_)).Dispose();
                    File.WriteAllText(Path.Combine(paths_), "{\"worldArr\":[{\"Name\":\"<PLACE HOLDER DO NOT PLAY>\"}]}");
                }

                Debug.Log($"made file : {Path.Combine(paths_)}");
            }
            catch (Exception e)
            {
                ShowError("Error while initiating game", e.Message + Environment.NewLine + Environment.NewLine + "This is a critical error. the game will not save files because of this error that is you may loose progress. " + Environment.NewLine + "please report this.");
                Debug.Log(e.ToString());
            }
        }
        else if(WorldManager.DeviceOS == (int)Devices.MacOS)
        {
            try
            {
                //= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "mc_copy")

                WorldManager.GlobalGameFilePath = Path.Combine(Application.persistentDataPath);
                if (!File.Exists(MultiOSFileSystem.FileSystems.Combine($"{WorldManager.GlobalGameFilePath}^Player settings.txt", '^')))
                {
                    File.CreateText(MultiOSFileSystem.FileSystems.Combine($"{WorldManager.GlobalGameFilePath}^Player settings.txt",'^')).Dispose();
                    Player.settings = new PlayerSettings(SensitivitySlider.value, FOVSlider.value, FarClipSlider.value, NearClipSlider.value);
                    File.WriteAllText(MultiOSFileSystem.FileSystems.Combine($"{WorldManager.GlobalGameFilePath}^Player settings.txt",'^'), Newtonsoft.Json.JsonConvert.SerializeObject(new PlayerSettings(SensitivitySlider.value, FOVSlider.value, FarClipSlider.value, NearClipSlider.value)));
                }
                string[] paths_ = { WorldManager.GlobalGameFilePath };
                if (!Directory.Exists(Path.Combine(paths_)))
                {
                    Directory.CreateDirectory(Path.Combine(paths_)); ;
                }
                paths_ = new string[] { WorldManager.GlobalGameFilePath, "worlds" };
                if (!Directory.Exists(Path.Combine(paths_)))
                {
                    Directory.CreateDirectory(Path.Combine(paths_));
                }
                if (!File.Exists(Path.Combine(WorldManager.GlobalGameFilePath, "Player settings.txt")))
                {
                    File.CreateText(Path.Combine(WorldManager.GlobalGameFilePath, "Player settings.txt")).Dispose();
                }

                paths_ = new string[] { WorldManager.GlobalGameFilePath, "worlds", "world list.txt" };
                if (!File.Exists(Path.Combine(paths_)))
                {
                    File.CreateText(Path.Combine(paths_)).Dispose();
                    File.WriteAllText(Path.Combine(paths_), "{\"worldArr\":[{\"Name\":\"[select world]\"}]}");
                }

                Debug.Log($"made file : {Path.Combine(paths_)}");
            }
            catch (Exception e)
            {
                ShowError("Error while initiating game", e.Message + Environment.NewLine + Environment.NewLine + "This is a critical error. the game will not function properly because of this error. " + Environment.NewLine + "please report this.");
                Debug.Log(e.ToString());
            }
        }
    }

    public void CreateNewWorld() 
    {
        try
        {
            string locFile = Path.Combine(WorldManager.GlobalGameFilePath, "worlds");
            WorldList worldsArr_ = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldList>(File.ReadAllText(Path.Combine(locFile, "world list.txt")));
            List<World> worldTemp_ = new List<World>();
            worldTemp_.AddRange(worldsArr_.worldArr);
            worldTemp_.Add(new World(WorldNameInp.text));
            worldsArr_.worldArr = worldTemp_.ToArray();
            File.WriteAllText(Path.Combine(locFile, "world list.txt"), Newtonsoft.Json.JsonConvert.SerializeObject(worldsArr_));

            string str__ = Path.Combine(locFile, WorldNameInp.text);

           if (File.Exists(str__))
           {
                WorldNameInp.text += "-";
           }
            
            Directory.CreateDirectory(Path.Combine(locFile, WorldNameInp.text));
            Directory.CreateDirectory(Path.Combine(locFile, WorldNameInp.text, "chunks"));
            if (!File.Exists(Path.Combine(locFile, WorldNameInp.text, "WorldSettings.txt")))
            {
                File.CreateText(Path.Combine(locFile, WorldNameInp.text, "WorldSettings.txt")).Dispose();
            }
            File.WriteAllText(Path.Combine(locFile, WorldNameInp.text, "WorldSettings.txt"), Newtonsoft.Json.JsonConvert.SerializeObject(new WorldSettings(int.Parse(WorldChunksLength.text), GenTypeDrop.value, 0, GenTypeDrop.value, SaveTypeDrop.value, 75, -20, TreeSpawnRateSlider.value)));
            WorldManager.WorldName = WorldNameInp.text;

            Debug.Log("Created new world.");
            WorldManager.CanSaveData = true;
            PlayWorld();
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
            WorldManager.CanSaveData = false;
            ShowError("Failed to create world","This is a file error so u wont be able to save worlds.\r\n"+e.Message);
            int worldchunklen_ = int.Parse(WorldChunksLength.text);
            int _gentype = GenTypeDrop.value;
            //BlockTypeDrop.value
            int _blocktype = 0;
            int gentype_ = GenTypeDrop.value;
            int savetype_ = SaveTypeDrop.value;
            //int.Parse(YScale.text)
            int yscale = 75;
            //int.Parse(YOffset.text)
            int yoffset_ = -20;
            float treespawnrate_ = TreeSpawnRateSlider.value;
            WorldSettings _worldSettings = new WorldSettings(worldchunklen_, _gentype, _blocktype, gentype_, savetype_, yscale, yoffset_, treespawnrate_);
            WorldManager.GlobalWorldSettings = _worldSettings;
            Debug.Log("Created new world (unsavable because of file error).");
            PlayWorld();
        }
    }

    public void LoadWorldsToDropDown()
    {

        try
        {
            for(int i=0;i< SelectWorld.options.Count; i++)
            {
                SelectWorld.options.RemoveAt(i);
            }
            string locFile = Path.Combine(WorldManager.GlobalGameFilePath, "worlds");
            WorldList _wl = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldList>(File.ReadAllText(Path.Combine(locFile , "world list.txt")));
            List<string> _list = new List<string>();
            for (int i = 0; i < _wl.worldArr.Length; i++)
            {
                _list.Add(_wl.worldArr[i].Name);
            }
            SelectWorld.AddOptions(_list);
            Debug.Log("Loaded worlds to drop down");
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
            ShowError("Error while loading worlds", e.Message);
        }
        
    }
    
    public void PlaySelectedWorld()
    {
        try
        {
            string locFile = Path.Combine(WorldManager.GlobalGameFilePath, "worlds");
            WorldList _wl = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldList>(File.ReadAllText(Path.Combine(locFile , "world list.txt")));
            string worldName = _wl.worldArr[SelectWorld.value].Name;
            WorldManager.WorldName = worldName;
            Debug.Log($"played world {worldName}");
            PlayWorld();
        }
        catch (Exception e)
        {
            ShowError("Error while playing world", e.Message);
            Debug.Log(e.ToString());
        }
    }

    public void DeletWorld()
    {
        //try
        //{
            string locFile = Path.Combine(WorldManager.GlobalGameFilePath,"worlds");
            WorldList _wl = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldList>(File.ReadAllText(Path.Combine(locFile , "world list.txt")));
            List<World> _ws = new List<World>();
            DeleteWorldFile(Path.Combine(WorldManager.GlobalGameFilePath, "worlds", _wl.worldArr[SelectWorld.value].Name + ""));
            for (int i = 0; i < _wl.worldArr.Length; i++)
            {
                _ws.Add(_wl.worldArr[i]);
            }
            _ws.RemoveAt(SelectWorld.value);
            _wl.worldArr = _ws.ToArray();
            File.WriteAllText(Path.Combine(locFile , "world list.txt"), Newtonsoft.Json.JsonConvert.SerializeObject(_wl));
            LoadWorldsToDropDown();
        //}
        //catch(Exception e)
        //{
        //   ShowError("Error while deleting world", e.Message);
        //   Debug.Log(e.ToString());
        //}
    }

    public void PlayWorld()
    {
        SceneManager.LoadScene(1);
    }

    public void SetPlayerSettings()
    {
        try
        {         
            QualitySettings.SetQualityLevel(quality.value);
            Player.settings = new PlayerSettings(SensitivitySlider.value, FOVSlider.value, FarClipSlider.value, NearClipSlider.value);
            File.WriteAllText(Path.Combine(WorldManager.GlobalGameFilePath, "Player settings.txt"), Newtonsoft.Json.JsonConvert.SerializeObject(new PlayerSettings(SensitivitySlider.value, FOVSlider.value, FarClipSlider.value, NearClipSlider.value)));            
        }
        catch(Exception e)
        {
            ShowError("Error while saving settings",e.Message);
            Debug.Log(e.ToString());
        }
    }

    public void ShowError(string name,string msg)
    {
        ErrorPage.SetActive(true);
        ErrorName.text = name;
        ErrorMsg.text = msg;
    }

    public void SetOS(int os)
    {
        WorldManager.DeviceOS = os;
        InitiateFiles();
    }

    public void Quit()
    {
        Debug.Log("Quitting!");
        Application.Quit();
    }


    public void DeleteWorldFile(string path)
    {
        try
        {
            File.Delete(Path.Combine(path, "GenerationSettings.json"));
            File.Delete(Path.Combine(path, "WorldSettings.txt"));
            string pathx_ = Path.Combine(path,"chunks");
            for (int i=0;File.Exists(Path.Combine(path, "chunks", i + ".chunk"));i++)
            {
                File.Delete(Path.Combine(path, "chunks", i + ".chunk"));
            }
            Directory.Delete(pathx_);
            Directory.Delete(path);
        }
        catch(Exception e)
        {
            ShowError("Error While deleting world",e.ToString());
            Debug.Log(e.ToString());
        }
    }
}

public class WorldList
{
    public World[] worldArr;
}

public class World 
{
    public string Name;


    public World(){ }
    public World(string name)
    {
        Name = name;
    }
}

public class WorldSettings
{
    public int ChunkLength, GenSize, BlockGenType,GenType,SaveType;
    public int YScale,YOffset;
    public float TreeSpawnRate;
    public int Seed;

    public WorldSettings() 
    {
        ChunkLength = 2;
        YScale = 75;
        TreeSpawnRate = 0.1f;
        YOffset = -20;
    }
    public WorldSettings(int chunklen,int gensize,int blocktype,int gentype,int savetype,int yscale,int yOffset,float treeSpawnRate)
    {
        ChunkLength = chunklen;
        GenSize = gensize;
        BlockGenType = blocktype;
        GenType = gentype;
        SaveType = savetype;
        YScale = yscale;
        YOffset = yOffset;
        TreeSpawnRate = treeSpawnRate;
    }
}


public class PlayerSettings
{
    public float Sensitivity,FOV,FarClip,NearClip;
    public PlayerSettings()
    {

    }
    public PlayerSettings(float sensitivity, float fov, float farClip, float nearClip)
    {
        Sensitivity = sensitivity;
        FOV = fov;
        FarClip = farClip;
        NearClip = nearClip;
    }
}

public enum Devices
{
    Windows,Android,MacOS
}



/*
            if (Device == (int)Devices.PC)
            {
                string locFile = $"{Application.streamingAssetsPath}\\worlds";
                WorldList worldsArr_ = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldList>(File.ReadAllText($"{locFile}\\world list.txt"));
                List<World> worldTemp_ = new List<World>();
                worldTemp_.AddRange(worldsArr_.worldArr);
                worldTemp_.Add(new World(WorldNameInp.text));
                worldsArr_.worldArr = worldTemp_.ToArray();
                File.WriteAllText($"{locFile}\\world list.txt", Newtonsoft.Json.JsonConvert.SerializeObject(worldsArr_));

                Directory.CreateDirectory($"{locFile}\\{WorldNameInp.text}");
                Directory.CreateDirectory($"{locFile}\\{WorldNameInp.text}\\chunks");
                File.WriteAllText($"{locFile}\\{WorldNameInp.text}\\WorldSettings.txt", Newtonsoft.Json.JsonConvert.SerializeObject(new WorldSettings(int.Parse(WorldChunksLength.text), GenTypeDrop.value, BlockTypeDrop.value, GenTypeDrop.value, SaveTypeDrop.value, int.Parse(YScale.text), int.Parse(YOffset.text), TreeSpawnRateSlider.value)));
                WorldManager.WorldName = WorldNameInp.text;

                Debug.Log("Created new world.");
                PlayWorld();
            }            
            else if(Device == (int)Devices.Android)
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.Personal)}/mc_copy");
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.Personal)}/mc_copy/worlds");
                
                string locFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.Personal)}/mc_copy_data/worlds";
                if (!File.Exists($"{locFile}/world list.txt"))
                {
                    File.CreateText($"{locFile}/world list.txt");
                    File.WriteAllText($"{locFile}/world list.txt", "{\"worldArr\":[{\"Name\":\"<Place holder do not play>\"}]}");
                }
                if (File.Exists($"{locFile}/world list.txt"))
                {
                    WorldList worldsArr_ = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldList>(File.ReadAllText($"{locFile}\\world list.txt"));
                    List<World> worldTemp_ = new List<World>();
                    worldTemp_.AddRange(worldsArr_.worldArr);
                    worldTemp_.Add(new World(WorldNameInp.text));
                    worldsArr_.worldArr = worldTemp_.ToArray();
                    File.WriteAllText($"{locFile}/world list.txt", Newtonsoft.Json.JsonConvert.SerializeObject(worldsArr_));
                }

                Directory.CreateDirectory($"{locFile}/{WorldNameInp.text}");
                Directory.CreateDirectory($"{locFile}/{WorldNameInp.text}/chunks");
                File.WriteAllText($"{locFile}/{WorldNameInp.text}/WorldSettings.txt", Newtonsoft.Json.JsonConvert.SerializeObject(new WorldSettings(int.Parse(WorldChunksLength.text), GenTypeDrop.value, BlockTypeDrop.value, GenTypeDrop.value, SaveTypeDrop.value, int.Parse(YScale.text), int.Parse(YOffset.text), TreeSpawnRateSlider.value)));
                WorldManager.WorldName = WorldNameInp.text;

                Debug.Log("Created new world.");
                PlayWorld();
            }
            else
            {
                ShowError("Invalid Device","The device you have selected is not supported yet.");
            }
            */

/*
if(Device == (int)Devices.PC)
{
    string locFile = $"{Application.streamingAssetsPath}\\worlds";
    WorldList _wl = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldList>(File.ReadAllText(locFile + "\\world list.txt"));
    List<string> _list = new List<string>();
    for (int i = 0; i < _wl.worldArr.Length; i++)
    {
        _list.Add(_wl.worldArr[i].Name);
    }
    SelectWorld.AddOptions(_list);
    Debug.Log("Loaded worlds to drop down");
}
else if(Device == (int)Devices.Android)
{
    string locFile = $"{Environment.GetFolderPath(Environment.SpecialFolder.Personal)}/mc_copy/worlds";
    WorldList _wl = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldList>(File.ReadAllText(locFile + "/world list.txt"));
    List<string> _list = new List<string>();
    for (int i = 0; i < _wl.worldArr.Length; i++)
    {
        _list.Add(_wl.worldArr[i].Name);
    }
    SelectWorld.AddOptions(_list);
    Debug.Log("Loaded worlds to drop down");
}
else
{
    ShowError("Invalid Device", "The device you have selected is not supported yet.");
}
*/

/*
 * if (Device == (int)Devices.PC)
        {
            string locFile = $"{Application.streamingAssetsPath}\\worlds";
            WorldList _wl = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldList>(File.ReadAllText(locFile + "\\world list.txt"));
            string worldName = _wl.worldArr[SelectWorld.value].Name;
            WorldManager.WorldName = worldName;
            Debug.Log($"played world {worldName}");
            PlayWorld();
        }
        else if(Device == (int)Devices.Android)
        {
            string locFile = $"{WorldManager.GlobalGameFilePath}/worlds";
            WorldList _wl = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldList>(File.ReadAllText(locFile + "/world list.txt"));
            string worldName = _wl.worldArr[SelectWorld.value].Name;
            WorldManager.WorldName = worldName;
            Debug.Log($"played world {worldName}");
            PlayWorld();
        }
*/

/*
 * 
    IFolder folder = FileSystem.Current.LocalStorage;
    string folderName = "mc_copy";
    folder =  folder.CreateFolder(folderName, CreationCollisionOption.ReplaceExisting);
    folderName = "worlds";
    folder = folder.CreateFolder(folderName,CreationCollisionOption.ReplaceExisting);
    folder = FileSystem.Current.LocalStorage;
    folderName = "Player settings.txt";
    folder = folder.CreateFolder(folderName, CreationCollisionOption.ReplaceExisting);
*
*/
