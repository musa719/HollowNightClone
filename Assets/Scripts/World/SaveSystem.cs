using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace HollowNight.World
{
    /// <summary>
    /// Manages game save and load functionality
    /// </summary>
    public class SaveSystem : MonoBehaviour
    {
        [System.Serializable]
        public class GameSaveData
        {
            public Vector3 playerPosition;
            public int playerHealth;
            public int playerSoul;
            public List<string> unlockedAbilities = new List<string>();
            public List<string> equippedCharms = new List<string>();
            public List<string> discoveredAreas = new List<string>();
            public float playTime;
        }
        
        public static SaveSystem Instance { get; private set; }
        
        private string savePath;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            savePath = Application.persistentDataPath + "/save.json";
        }
        
        public void SaveGame()
        {
            Core.GameManager gameManager = Core.GameManager.Instance;
            if (gameManager == null)
                return;
            
            Player.Player player = gameManager.GetPlayer();
            if (player == null)
                return;
            
            GameSaveData saveData = new GameSaveData
            {
                playerPosition = player.transform.position,
                playerHealth = player.GetComponent<Player.PlayerStats>().GetHealth(),
                playerSoul = player.GetComponent<Player.PlayerStats>().GetSoul(),
                playTime = Time.time
            };
            
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(savePath, json);
            
            Debug.Log($"Game saved to {savePath}");
        }
        
        public GameSaveData LoadGame()
        {
            if (!File.Exists(savePath))
            {
                Debug.LogWarning($"Save file not found at {savePath}");
                return null;
            }
            
            string json = File.ReadAllText(savePath);
            GameSaveData saveData = JsonUtility.FromJson<GameSaveData>(json);
            
            Debug.Log($"Game loaded from {savePath}");
            return saveData;
        }
        
        public bool SaveFileExists()
        {
            return File.Exists(savePath);
        }
        
        public void DeleteSave()
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("Save file deleted");
            }
        }
    }
}
