using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PhotonNetworkManager : Photon.PunBehaviour {

    public string playerName;

    public GameObject infoDialog;
    public Text       infoText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ConnectPhotonNetwork(string playerName) {
        //Photonロビーに接続
        Debug.Log("ConnectPhotonNetwork");
        PhotonNetwork.ConnectUsingSettings("v1.0");
        this.playerName = playerName;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("PhotonNetwork ロビーに接続成功");

        //プレイヤー名を設定
        PhotonNetwork.playerName = playerName;

        //ランダムなルームに入室を試みる
        PhotonNetwork.JoinRandomRoom();
    }

    //ルームへの入室に失敗したときに呼ばれる
    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("ルームへの入室に失敗、ルームを作成します");
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("ルームへの入室に成功");
        Debug.Log("プレイヤー名：" + PhotonNetwork.playerName);

        infoText.text = "ログインに成功しました\nプレイヤー名：" + PhotonNetwork.playerName;
        infoDialog.SetActive(true);

    }
   
}
