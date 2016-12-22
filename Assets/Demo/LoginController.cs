using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class LoginController : Photon.MonoBehaviour {

    //POSTするJSONデータ構造
    [System.Serializable]
    public class Item
    {
        public string email;
        public string password;
    }

    //サーバーから帰ってくるJSONデータ構造
    [System.Serializable]
    public class login_result
    {
        public string   player_name;
        public bool     result;
        public string   error_msg;
    }

    public InputField   mailInput;
    public InputField   passInput;
    public Button       loginButton;

    public GameObject   errorDialog;
    public Text         errorText;

    public string       LoginServer;
    public string       PlayerName;

    private login_result result;

    

    public PhotonNetworkManager photonNetworkManager;
    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //ログインボタンを押したときの動作
    public void Login() {
        if (!string.IsNullOrEmpty(mailInput.text) && !string.IsNullOrEmpty(passInput.text))
        {
            mailInput.enabled = false;
            passInput.enabled = false;
            //JSONデータを準備
            string baseJson = "{ \"email\": \"" + mailInput.text + "\", \"password\": \"" + passInput.text + "\" }";

            Item json = JsonUtility.FromJson<Item>(baseJson);
            string serialisedJson = JsonUtility.ToJson(json);
            
            //WWWをセットアップ
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Content-Type", "application/json");

            byte[] data = System.Text.Encoding.UTF8.GetBytes(serialisedJson);
            WWW www = new WWW(LoginServer, data,header);

            StartCoroutine(PostLogin(www));

        } else {
            
            errorText.text = "IDとパスワードを入力してください";
            errorDialog.SetActive(true);
        }
    }

    //HTTP POSTを行うコルーチン
    private IEnumerator PostLogin(WWW www) {

        //POST結果が返ってくるまで待つ
        yield return www;

        //POSTに成功した場合
        if (www.error == null)
        {
            result = JsonUtility.FromJson<login_result>(www.text);

            if (result.result)
            {
                PlayerName = result.player_name;

                //PhotonNetworkに接続
                photonNetworkManager.ConnectPhotonNetwork(this.PlayerName);

                mailInput.enabled = true;
                passInput.enabled = true;
                loginButton.enabled = true;

            } else {
                errorText.text = result.error_msg;
                errorDialog.SetActive(true);
            }
        } else {

            errorText.text = www.error.ToString();
            errorDialog.SetActive(true);
        }
    }
   


}

