using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System;

public class DAO : MonoBehaviour
{
    // サーバアドレスを指定
    public string serverAddress = "http://localhost:8080/server.php"; // 要変更

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
        Debug.Log("[Method] SaveData");
        MarkData markData = new MarkData ();
        markData.id = 1;
        markData.width = 30;  // ボタン押下時のマーク君のサイズを取得するように変更する
        markData.height = 30; // 同上
        markData.anger_point = 5;
        markData.burst_flag = false;
        StartCoroutine(SendJsonData(markData));

        AngerData angerData = new AngerData();
        angerData.id = 1;
        angerData.mark_table_id = 1;
        angerData.comment = "残業してもタスクが終わらなくてイライラいする！！！";
        angerData.anger_point = 2;
        StartCoroutine(SendJsonData(angerData));

        yield return 0;

    }
    
    /**
    * JSON形式でデータをPHPに送る.
    */
    IEnumerator SendJsonData<T>(T data) {
        Debug.Log("[Method] SendJsonData");
        UnityWebRequest request = new UnityWebRequest(serverAddress, JsonMapper.ToJson(data));
        // request.timeout = 1;
        yield return request.SendWebRequest();
        string dataName = data.GetType().Name;
        if(request.result == UnityWebRequest.Result.ProtocolError) {
            // レスポンスコードを見て処理
            Debug.Log($"[Error] Failed to send "+dataName+". Response Code : "+request.responseCode);
        }
        else if (request.result == UnityWebRequest.Result.ConnectionError) {
            // エラーメッセージを見て処理
            Debug.Log($"[Error] Failed to send "+dataName+". Message : "+request.error);
        }
        else{
            Debug.Log($"[Success] "+dataName+ "has been sent successfully");
        }
    }
}

[System.Serializable]
public class MarkData
{
    public int id;
    public DateTime create_at;
    public int width;
    public int height;
    public int anger_point;
    public bool burst_flag;
}

[System.Serializable]
public class AngerData
{
    public int id;
    public DateTime create_at;
    public int mark_table_id;
    public string comment;
    public int anger_point;
}