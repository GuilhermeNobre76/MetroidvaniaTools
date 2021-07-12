using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MetroidvaniaTools
{
    public class LevelManager : Managers
    {
        public Bounds levelSize;
        public GameObject initialPlayer;

        [SerializeField]
        protected List<Transform> availableSpawnLocations = new List<Transform>();
        private Vector3 startingLocation;

        protected virtual void Awake()
        {
            if(availableSpawnLocations.Count <= PlayerPrefs.GetInt("SpawnReference"))
            {
                startingLocation = availableSpawnLocations[0].position;
            }
            else
            {
                startingLocation = availableSpawnLocations[PlayerPrefs.GetInt("SpawnReference")].position;
                CreatePlayer(initialPlayer, startingLocation);
            }
        }
        protected virtual void OnDisable()
        {
            PlayerPrefs.SetInt("FacingLeft", character.isFacingLeft ? 1 : 0);
        }
        public virtual void NextScene(SceneReference scene, int spawnReference)
        {
            PlayerPrefs.SetInt("FacingLeft", character.isFacingLeft ? 1 : 0);
            PlayerPrefs.SetInt("SpawnReference", spawnReference);
            SceneManager.LoadScene(scene);
        }
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(levelSize.center, levelSize.size);
        }
    }
}