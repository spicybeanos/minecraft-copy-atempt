using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Dictionary<string,int> PlayerEffects = new Dictionary<string,int>();
    public static PlayerSettings settings;
    public PlayerMovement playerMovement;
    public MouseLook mouseLook;
    public PlayerUI playerUI;
    public WorldManager worldManager;

    bool b = false;
    public void Start()
    {
        PlayerEffects.Add("speed",2);
        settings = Newtonsoft.Json.JsonConvert.DeserializeObject<PlayerSettings>(File.ReadAllText(Path.Combine(WorldManager.GlobalGameFilePath,"Player settings.txt")));
        mouseLook.sensitivity = settings.Sensitivity;
        mouseLook.cam.fieldOfView = settings.FOV;
        mouseLook.cam.farClipPlane = settings.FarClip;
        mouseLook.cam.nearClipPlane = settings.NearClip;
    }
    bool toggle()
    {
        if (b)
        {
            b = false;
            return false;
        }
        else
        {
            b = true;
            return true;
        }
    }
    public void Update()
    {
        if (PlayerEffects.ContainsKey("speed"))
        {
            playerMovement.localConstants.playerSpeedScale = PlayerEffects["speed"];
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            if(toggle())
                playerUI.DebugMenu.SetActive(true);
        }
        if(transform.position.y < 8.5f)
        {
            //playerUI.WaterScreen.SetActive(true);
        }
        else
        {
           // playerUI.WaterScreen.SetActive(false);
        }
    }

    public void Pause()
    {
        playerUI.PauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        playerMovement.localConstants.playerSpeedScale = 0;
        playerMovement.localConstants.playerTimeScale = 0;
        mouseLook.localConstants.playerSpeedScale = 0;
        mouseLook.localConstants.playerTimeScale = 0;
    }

    public void Resume()
    {
        playerUI.PauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        playerMovement.localConstants.playerSpeedScale = 1;
        playerMovement.localConstants.playerTimeScale = 1;
        mouseLook.localConstants.playerSpeedScale = 1;
        mouseLook.localConstants.playerTimeScale = 1;
    }

    public void Save()
    {
        Debug.Log($"world name: {WorldManager.WorldName}");
        Debug.Log($"world settings file : {Path.Combine(WorldManager.GlobalGameFilePath, "worlds", WorldManager.WorldName, "WorldSettings.txt")}");
        WorldSettings _ws = Newtonsoft.Json.JsonConvert.DeserializeObject<WorldSettings>(File.ReadAllText(Path.Combine(WorldManager.GlobalGameFilePath,"worlds",WorldManager.WorldName,"WorldSettings.txt")));
        WorldLoader.SaveWorldGenSettings();
        if(_ws.SaveType == 0)
        {
            WorldLoader.SaveWorldJSON(worldManager);
        }
        else
        {
            WorldLoader.SaveToFileLW(worldManager);
        }
    }
}
