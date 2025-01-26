using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scenes.Juan
{
    public class PlayerManager : MonoBehaviour
    {
        public UnityEvent OnStart = new();
        public UnityEvent GameStarted = new();
        public UnityEvent OnPlayerJoined = new();
        public UnityEvent On2PlayersJoined = new();
        public UnityEvent On3PlayersJoined = new();

        private List<PlayerController3D> _players = new List<PlayerController3D>();

        [SerializeField]
        private List<Transform> spawnPoints = new List<Transform>();

        [SerializeField]
        private List<OutputChannels> playerLayers = new List<OutputChannels>();

        private PlayerInputManager playerInputManager;

        [SerializeField]
        private GameObject menuCamera;

        private bool _gameStarted = false;

        private void Awake()
        {
            playerInputManager = GetComponent<PlayerInputManager>();
            playerInputManager.onPlayerJoined += OnPlayerJoinedd;
            playerInputManager.onPlayerLeft += OnPlayerLeft;
        }

        private void Start()
        {
            OnStart?.Invoke();
        }

        private void OnPlayerLeft(PlayerInput input)
        {
            _players.Remove(input.GetComponent<PlayerController3D>());
        }

        private void OnPlayerJoinedd(PlayerInput input)
        {
            _players.Add(input.GetComponent<PlayerController3D>());

            var player = input.transform;
            player.position = spawnPoints[_players.Count - 1].position;

            var layerToAdd = playerLayers[_players.Count - 1];

            player.GetComponentInChildren<CinemachineCamera>().OutputChannel = layerToAdd;
            player.GetComponentInChildren<CinemachineBrain>().ChannelMask = layerToAdd;

            menuCamera.gameObject.SetActive(false);

            input.actions["Start"].performed += ctx => StartGame();

            OnPlayerJoined?.Invoke();

            if (_players.Count == 2)
            {
                On2PlayersJoined?.Invoke();
            }

            else if (_players.Count == 3)
            {
                On3PlayersJoined?.Invoke();
            }
        }

        private void StartGame()
        {
            if (_gameStarted == false)
            {
                GameStarted?.Invoke();
            }
        }
    }
}
