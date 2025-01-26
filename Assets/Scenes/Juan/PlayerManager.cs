using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Assets.Scenes.Juan
{
    public class PlayerManager : MonoBehaviour
    {
        public UnityEvent<PlayerController3D> OnGameFinished = new();
        public UnityEvent OnStart = new();
        public UnityEvent GameStarted = new();
        public UnityEvent OnPlayerJoined = new();
        public UnityEvent On2PlayersJoined = new();
        public UnityEvent On3PlayersJoined = new();

        private List<PlayerController3D> _players = new List<PlayerController3D>();

        [SerializeField]
        private float scorePerSecond = 1f;
        [SerializeField]
        private List<Transform> spawnPoints = new List<Transform>();

        [SerializeField]
        private List<OutputChannels> playerLayers = new List<OutputChannels>();

        private PlayerInputManager playerInputManager;

        [SerializeField]
        private GameObject menuCamera;

        private bool _gameStarted = false;

        private BallController ballController;

        [SerializeField]
        private List<LayoutElement> playerLayoutElements = new List<LayoutElement>();

        private void Awake()
        {
            playerInputManager = GetComponent<PlayerInputManager>();
            playerInputManager.onPlayerJoined += OnPlayerJoinedd;
            playerInputManager.onPlayerLeft += OnPlayerLeft;

            ballController = FindFirstObjectByType<BallController>();           
        }

        private void Start()
        {
            OnStart?.Invoke();
        }

        private void Update()
        {
            if(_gameStarted == false) return;

            int scoringPlayerIndex = ballController.transform.position.z > 0 ? 0 : 1;
            int losingPlayerIndex = scoringPlayerIndex == 0 ? 1 : 0;

            var scoringPlayer = playerLayoutElements[scoringPlayerIndex];
            var losingPlayer = playerLayoutElements[losingPlayerIndex];

            scoringPlayer.preferredWidth += scorePerSecond * Time.deltaTime;
            losingPlayer.preferredWidth -= scorePerSecond * Time.deltaTime;

            //Process scoring

            if(playerLayoutElements.Any(x=> x.preferredWidth <= 0))
            {
                //Someone Lost!!

                var playerWhoWinLayout = playerLayoutElements.FirstOrDefault(x => x.preferredWidth > 0);
                var playerWhoWon = _players[playerLayoutElements.IndexOf(playerWhoWinLayout)];

                Debug.Log($"{playerWhoWon.name} won!");

                OnGameFinished?.Invoke(playerWhoWon);                
            }
        }

        private void OnPlayerLeft(PlayerInput input)
        {
            _players.Remove(input.GetComponent<PlayerController3D>());
        }

        private void OnPlayerJoinedd(PlayerInput input)
        {
            var playerComponent = input.GetComponent<PlayerController3D>();

            playerComponent.name = $"Player {_players.Count + 1}";

            _players.Add(playerComponent);

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
                _gameStarted = true;
            }
        }
    }
}
