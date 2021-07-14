using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MetroidvaniaTools
{
    public class LevelManager : Managers
    {
        public Bounds levelSize;
        public GameObject initialPlayer;
        public Image fadeScreen;

        public List<Transform> availableSpawnLocations = new List<Transform>();
        public List<Transform> playerIndicatorSpawnLocations = new List<Transform>();

        private Vector3 startingLocation;
        private Vector3 playerIndicatorLocation;

        protected virtual void Awake()
        {
            if(availableSpawnLocations.Count <= PlayerPrefs.GetInt("SpawnReference"))
            {
                startingLocation = availableSpawnLocations[0].position;
                playerIndicatorLocation = playerIndicatorSpawnLocations[0].position;
            }
            else
            {
                startingLocation = availableSpawnLocations[PlayerPrefs.GetInt("SpawnReference")].position;
                playerIndicatorLocation = playerIndicatorSpawnLocations[PlayerPrefs.GetInt("SpawnReference")].position;
                CreatePlayer(initialPlayer, startingLocation);
            }
        }
        protected override void Initialization()
        {
            base.Initialization();
            playerIndicator.transform.position = playerIndicatorLocation;
            StartCoroutine(FadeIn());
        }
        protected virtual void OnDisable()
        {
            PlayerPrefs.SetInt("FacingLeft", character.isFacingLeft ? 1 : 0);
        }
        public virtual void NextScene(SceneReference scene, int spawnReference)
        {
            PlayerPrefs.SetInt("FacingLeft", character.isFacingLeft ? 1 : 0);
            PlayerPrefs.SetInt("SpawnReference", spawnReference);
            StartCoroutine(FadeOut(scene));
        }
        protected virtual IEnumerator FadeIn()
        {
            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / .5f;
            Color currentColor = fadeScreen.color;
            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / .5f;
                currentColor.a = Mathf.Lerp(1, 0, percentageComplete);
                fadeScreen.color = currentColor;
                if(percentageComplete >= 1)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }
        protected virtual IEnumerator FadeOut(SceneReference scene)
        {
            float timeStarted = Time.time;
            float timeSinceStarted = Time.time - timeStarted;
            float percentageComplete = timeSinceStarted / .5f;
            Color currentColor = fadeScreen.color;
            while (true)
            {
                timeSinceStarted = Time.time - timeStarted;
                percentageComplete = timeSinceStarted / .5f;
                currentColor.a = Mathf.Lerp(0, 1, percentageComplete);
                fadeScreen.color = currentColor;
                if (percentageComplete >= 1)
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            SceneManager.LoadScene(scene);
        }
        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(levelSize.center, levelSize.size);
        }
    }
}