using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MetroidvaniaTools
{
    public class TitleScreen : MonoBehaviour
    {
        public SceneReference newGameScene;
        public GameObject mainMenu;
        public GameObject newGameMenu;
        public GameObject loadGameMenu;

        public List<AbilityItem> abilitiesToClear = new List<AbilityItem>();
        void Start()
        {
            MainMenu();
        }
        public virtual void MainMenu()
        {
            mainMenu.SetActive(true);
            loadGameMenu.SetActive(false);
            newGameMenu.SetActive(false);
        }
        public virtual void NewGameMenu()
        {
            mainMenu.SetActive(false);
            loadGameMenu.SetActive(false);
            newGameMenu.SetActive(true);
        }
        public virtual void LoadGameMenu()
        {
            mainMenu.SetActive(false);
            loadGameMenu.SetActive(true);
            newGameMenu.SetActive(false);
        }
        public virtual void NewGame(int slot)
        {
            PlayerPrefs.SetInt("GameFile", slot);
            PlayerPrefs.SetInt(" " + slot + "SaveSpawnReference", 0);
            PlayerPrefs.SetInt(" " + slot + "SpawnReference", 0);
            PlayerPrefs.SetInt(" " + slot + "CurrentHealth", 100);
            PlayerPrefs.SetString(" " + slot + "LoadGame", newGameScene);
            ClearAbilities(slot);
            SceneManager.LoadScene(newGameScene);
        }
        public virtual void LoadGame(int slot)
        {
            PlayerPrefs.SetInt("GameFile", slot);
            bool loadFromSave = true;
            PlayerPrefs.SetInt(" " + slot + "LoadFromSave", loadFromSave ? 1 : 0);
            SceneManager.LoadScene(PlayerPrefs.GetString(" " + slot + "LoadGame"));
        }
        public virtual void ClearAbilities(int slot)
        {
            for(int i = 0; i < abilitiesToClear.Count; i++)
            {
                PlayerPrefs.SetInt(" " + slot + abilitiesToClear[i].itemName, 0);
            }
        }
    }
}