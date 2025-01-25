using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scenes.Juan
{
    public class PlayerManager : MonoBehaviour
    {
        private List<PlayerInput> playerInputs = new List<PlayerInput>();

        [SerializeField]
        private List<Transform> spawnPoints = new List<Transform>();

        [SerializeField]
        private List<OutputChannels> playerLayers = new List<OutputChannels>();

        private PlayerInputManager playerInputManager;

        [SerializeField]
        private GameObject menuCamera;

        private void Awake()
        {
            playerInputManager = GetComponent<PlayerInputManager>();
            playerInputManager.onPlayerJoined += OnPlayerJoined;
            playerInputManager.onPlayerLeft += OnPlayerLeft;
        }

        private void OnPlayerLeft(PlayerInput input)
        {
            
        }

        private void OnPlayerJoined(PlayerInput input)
        {
            playerInputs.Add(input);

            var player = input.transform;
            player.position = spawnPoints[playerInputs.Count - 1].position;

            var layerToAdd = playerLayers[playerInputs.Count - 1];

            player.GetComponentInChildren<CinemachineCamera>().OutputChannel = layerToAdd;
            player.GetComponentInChildren<CinemachineBrain>().ChannelMask = layerToAdd;

            menuCamera.gameObject.SetActive(false);
        }
    }
}
