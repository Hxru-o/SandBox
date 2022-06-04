using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("DisconnectPanel")]
    public InputField NickNameInput;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;

    [Header("ETC")]
    public Text StatusText;
    List<RoomInfo> myList = new List<RoomInfo>();
    public PhotonView PV;

    #region ConnectServer
    void Awake() => Screen.SetResolution(960, 540, false);

     void Update() 
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
    }
     public void Connect() => PhotonNetwork.ConnectUsingSettings();


     public void Disconnect() => PhotonNetwork.Disconnect();

     public override void OnDisconnected(DisconnectCause cause)
     {
         RoomPanel.SetActive(false);
     }
    #endregion

    #region  room
    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnJoinedRoom()
    {
        RoomPanel.SetActive(true);
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++) ChatText[i].text = "";
        RoomRenewal();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomRenewal();
        PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow>" + newPlayer.NickName + "Joined</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomRenewal();
        PV.RPC("ChatRPC", RpcTarget.All, "<color=yellow" + otherPlayer.NickName + "Disconnected/color>");
    }
    
    
    void RoomRenewal()
    {
        RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + "/" + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers + "Max";
    }
    #endregion

    #region  chat
    public void Send()
    {
        PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC]
    
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for(int i = 0; i < ChatText.Length; i++)
           if(ChatText[i].text == "")
           {
               isInput = true;
               ChatText[i].text = msg;
               break;
           }
        if(!isInput)
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }

    }
    #endregion

    

}