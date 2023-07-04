using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System;
using System.Runtime.Serialization.Json;


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
        Debug.Log("[Method] SaveData");

        // MarkData取得
        UnityWebRequest request4MarkData = new UnityWebRequest(serverAddress);
        yield return request4MarkData.SendWebRequest();
        if(request4MarkData.result == UnityWebRequest.Result.ProtocolError) {
            // レスポンスコードを見て処理
            Debug.Log($"[Error] Failed to load MarkData. Response Code : "+request4MarkData.responseCode);
        }
        else if (request4MarkData.result == UnityWebRequest.Result.ConnectionError) {
            // エラーメッセージを見て処理
            Debug.Log($"[Error] Failed to load MarkData. Message : "+request4MarkData.error);
        }
        else{
            Debug.Log($"[Success] MarkData has been loaded successfully");
            string downloadHandler = request4MarkData.downloadHandler.text;
            Debug.Log(downloadHandler);
            MarkData markData = JsonUtility.FromJson<MarkData>(downloadHandler);
            Debug.Log(markData.id);
        }

        // AngerData取得
        UnityWebRequest request4AngerData = new UnityWebRequest(serverAddress);
        yield return request4AngerData.SendWebRequest();
        if(request4AngerData.result == UnityWebRequest.Result.ProtocolError) {
            // レスポンスコードを見て処理
            Debug.Log($"[Error] Failed to load AngerData. Response Code : "+request4AngerData.responseCode);
        }
        else if (request4AngerData.result == UnityWebRequest.Result.ConnectionError) {
            // エラーメッセージを見て処理
            Debug.Log($"[Error] Failed to load AngerData. Message : "+request4AngerData.error);
        }
        else{
            Debug.Log($"[Success] AngerData has been loaded successfully");
            string downloadHandler = request4AngerData.downloadHandler.text;
            Debug.Log(downloadHandler);
            AngerData angerData = JsonUtility.FromJson<AngerData>(downloadHandler);
            Debug.Log(angerData.id);
        }

    }

    /**
    * データを保存.
    */
    IEnumerator SaveData() {
        Debug.Log("[Method] SaveData");
        // セーブ前に最新情報を取得する.
        // StartCoroutine ( LoadData() );

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