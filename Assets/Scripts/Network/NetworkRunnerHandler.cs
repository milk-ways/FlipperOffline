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

    public GameObject PlayerPrefab;
    public GameObject GameManagerPrefab;

    public NetworkObject LocalCharacter;

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

    public void FindOneVsOneMatch()
    {
        networkRunner.ProvideInput = true;
        networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            Address = NetAddress.Any(),
            Scene = SceneRef.FromIndex(1),
            CustomLobbyName = "1vs1",
            PlayerCount = 2,
            SceneManager = sceneManager,
        });
    }

    public void FindThreeVsThreeMatch()
    {
        networkRunner.ProvideInput = true;
        networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            Address = NetAddress.Any(),
            Scene = SceneRef.FromIndex(1),
            CustomLobbyName = "3vs3",
            PlayerCount = 6,
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
            runner.Spawn(GameManagerPrefab);
        }

        if (player == runner.LocalPlayer)
        {
            var localCharacter = runner.Spawn(PlayerPrefab, new Vector3(5f, 0.75f, 5f), Quaternion.identity);
            Debug.Log($"Spawn Character : {runner.LocalPlayer}");
            runner.SetPlayerObject(player, localCharacter);
        }

        if (runner.SessionInfo.PlayerCount == runner.SessionInfo.MaxPlayers)
        {
            //GameManager.Instance.WaitingForStart = true;
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

        if(runner.IsSharedModeMasterClient)
        {
            GameManager.Instance.WaitingForStart = false;
        }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        SceneManager.LoadScene("Lobby");
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

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
}