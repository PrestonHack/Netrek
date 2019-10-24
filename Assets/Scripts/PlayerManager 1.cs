using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

namespace Netrek
{

    public class PlayerManager : MonoBehaviour
    {
        private static PlayerManager _instance;

        public int actorNumber;
        public string playerName;
        public string team;
        public string ship;



        /*
         * commented out because was getting two instances
         * 
            static public PlayerManager Instance
            {
                get
                {
                    if(_instance == null)
                    {
                        Debug.Log("PlayerManager is null");
                    }
                    return _instance;
                }
            }
            */
        private void Awake()
        {
            _instance = this;
            // SceneManager.sceneLoaded += SceneLoaded;

            playerName = "preston";
            team = "Fed";
            ship = "Fed_AS";
            DontDestroyOnLoad(this.gameObject);

            //Player p = PhotonNetwork.LocalPlayer;       
        }
        /*
        void SceneLoaded(Scene scene, LoadSceneMode mode)
        {

            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);
            if (scene.buildIndex == 1 && SceneManager.GetActiveScene().isLoaded && PhotonNetwork.InRoom)
            {

                Player p = new Player(name, team, ship);
                p.spawnPlayer();
            }
                //StartCoroutine(spawnTimer());
        }
        */

    }
}