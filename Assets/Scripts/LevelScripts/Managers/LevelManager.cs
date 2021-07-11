using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            startingLocation = availableSpawnLocations[0].position;
            CreatePlayer(initialPlayer, startingLocation);
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(levelSize.center, levelSize.size);
        }
    }
}