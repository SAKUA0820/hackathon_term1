using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class DAO : MonoBehaviour
{
    // サーバアドレスを指定
    public string serverAddress = "http://localhost/data.php"; // 要変更

    void Start() {
        // StartCoroutine ( LoadData() );
        StartCoroutine ( SaveData() );  // テスト用でSaveDataメソッドを呼び出す
    }

    // ボタン押下でSaveするようにする

    /**
    * データを取得.
    */
    IEnumerator LoadData() {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        dic.Add("height", "500");
        dic.Add("width", "500");

        StartCoroutine(GetData(serverAddress, dic));

        yield return 0;
    }

    private IEnumerator GetData(string url, Dictionary<string, string> data) {
        // WWWForm form = new WWWForm();
        // foreach (KeyValuePair<string, string> datum in data) {
        //     form.AddField(datum.Key, datum.Value);
        // }
        // UnityWebRequest request = new UnityWebRequest(url, JsonSerializer.Serialize(data));

    //     yield return StartCoroutine(CheckTimeOut(www, 3f)); //TimeOutSecond = 3s;

    //     if (www.error != null) {
    //         //そもそも接続ができていないとき
    //         Debug.Log("HttpPost NG: " + www.error);

    //     } else if (www.isDone) {
    //         //送られてきたデータをテキストに反映
    //         //ResultText_.GetComponent<Text>().text = www.text;
    //         // 送られたデータをもとにキャラクターのサイズを決定
    //     }
        yield return 0;
    }

    /**
    * データを保存.
    */
    IEnumerator SaveData() {
        Parameters parameters = new Parameters ();
        parameters.id = 1;
        parameters.name = "Mark1";
        parameters.height = 30; // ボタン押下時のマーク君のサイズを取得するように変更する
        parameters.width = 30;  // 同上
        // WWWForm form = new WWWForm ();
        // form.AddField ("parameter", JsonMapper.ToJson(parameters));
        UnityWebRequest save = new UnityWebRequest(serverAddress, JsonMapper.ToJson(parameters));

        yield return save;
    }
}

[System.Serializable]
public class Parameters : MonoBehaviour
{
    public int id;
    // public string name;
    public int height;
    public int width;
}
