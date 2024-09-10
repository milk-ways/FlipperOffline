using Fusion.Sockets;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class NetworkRunnerHandler : MonoBehaviour, INetworkRunnerCallbacks
{
    public static NetworkRunnerHandler Instance;

    private INetworkSceneManager sceneManager;
    public NetworkRunner networkRunner;

    public GameObject[] PlayerPrefab;
    public int SelectedPlayer = 0;

    public GameObject GameManagerPrefab;
    public GameObject PanManagerPrefab;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        networkRunner = GetComponent<NetworkRunner>();
        if (networkRunner == null)
        {
            networkRunner = gameObject.AddComponent<NetworkRunner>();
        }

        sceneManager = networkRunner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();
        if (sceneManager == null)
        {
            sceneManager = networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }
    }

    public void FindOneVsOneMatch(string customLobby = "1vs1")
    {
        networkRunner.ProvideInput = true;
        networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            Address = NetAddress.Any(),
            Scene = SceneRef.FromIndex(1),
            CustomLobbyName = customLobby,
            PlayerCount = 2,
            SceneManager = sceneManager,
        });
    }

    public void FindTwoVsTwoMatch(string customLobby = "2vs2")
    {
        networkRunner.ProvideInput = true;
        networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            Address = NetAddress.Any(),
            Scene = SceneRef.FromIndex(1),
            CustomLobbyName = customLobby,
            PlayerCount = 4,
            SceneManager = sceneManager,
        });
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {

    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.SessionInfo.PlayerCount == 1 && runner.IsSharedModeMasterClient)
        {
            runner.SessionInfo.IsOpen = false;
            runner.Spawn(PanManagerPrefab);
            runner.Spawn(GameManagerPrefab);
        }

        if (player == runner.LocalPlayer)
        {
            var localCharacter = runner.Spawn(PlayerPrefab[SelectedPlayer], 
                                        new Vector3(UnityEngine.Random.Range(0f, 3f), 0.75f, UnityEngine.Random.Range(0f, 3f)), 
                                        Quaternion.identity, player);
            Debug.Log($"Spawn Character : {runner.LocalPlayer}");
            runner.SetPlayerObject(player, localCharacter);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        foreach(var item in runner.ActivePlayers)
        {
            if(item == player)
            {
                runner.Despawn(runner.GetPlayerObject(item));
            }
        }

        if (player == runner.LocalPlayer && runner.IsSharedModeMasterClient)
        {
            foreach (var item in runner.ActivePlayers)
            {
                if (item == player) continue;
                if (runner.IsPlayerValid(item))
                {
                    runner.SetMasterClient(item);
                    break;
                }
            }
        }
        
        if (runner.IsSharedModeMasterClient)
        {
            GameManager.Instance.WaitingForStart = false;
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (SceneManager.GetActiveScene().buildIndex != 1) return;

        var data = new CharacterInputData();
        VariableJoystick joy;

        data.direction = Vector3.zero;
#if UNITY_EDITOR || PLATFORM_STANDALONE_WIN
        if (Input.GetKey(KeyCode.W))
        {
            data.direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            data.direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            data.direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            data.direction += Vector3.right;
        }
        data.direction.Normalize();
#endif
#if UNITY_ANDROID
        joy = InputManager.Instance.joystick;
        data.direction += new Vector3(joy.Horizontal, 0, joy.Vertical);
#endif
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            if (Input.GetKey(KeyCode.W))
            {
                data.direction += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                data.direction += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                data.direction += Vector3.back;
            }
            if (Input.GetKey(KeyCode.D))
            {
                data.direction += Vector3.right;
            }
            data.direction.Normalize();
            joy = InputManager.Instance.joystick;
            data.direction += new Vector3(joy.Horizontal, 0, joy.Vertical);
        }

        var charObj = runner.GetPlayerObject(runner.LocalPlayer);
        if (charObj != null)
        {
            data.ability = charObj.GetComponent<Character>().isAbilityPressed;
        }

        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SceneManager.LoadScene("Title");
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {

    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {

    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {

    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {

    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {

    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        if (runner.SessionInfo.PlayerCount == runner.SessionInfo.MaxPlayers)
        {
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.RpcSetWaitingForStart(true);
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
}
