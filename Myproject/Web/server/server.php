<?php

// 必要なもの
// ・brewよりphpをインストール&環境変数設定 (https://zenn.dev/ryuu/articles/setup-php-brew)
// ・brewよりmysqlをインストール&環境変数設定
// ・mysqlサーバ立ち上げ&ddlによりテーブル作成
// ・phpサーバ立ち上げ
// Talend API Testerなどを使ってjsonを送ってみる

// 作ったテーブルについて
// データベース名・ユーザ名・パスについては以下defineを参照
// テーブル定義は ../DB/ddl.sql を参照

define('DB_DATABASE', 'hackathon1');
define('DB_USERNAME', 'root');
define('DB_PASSWORD', 'hackathon1');
// define('DB_PASSWORD', 'nemui');  //←高橋自宅の場合
define('PDO_DSN', 'mysql:dbhost=localhost;dbname='.DB_DATABASE);

$method = $_GET['method'];

// try{
//     // json受け取り
//     $json = file_get_contents("php://input");
//     // 第4引数: エラー発生時に、JSONExceptionとする設定
//     $params = json_decode($json, true, 512, JSON_THROW_ON_ERROR);
// }catch(JSONException $e){
//     // リクエストの形式がおかしい時
//     header("HTTP/1.1 400 Bad Request");
//     header("Content-Type: application/json; charset=utf-8");
//     $result = array('code'=>400, 'message'=>'リクエストデータの形式が正しくありません','description'=>$e->getMessage());
//     echo json_encode($result, JSON_UNESCAPED_UNICODE);
//     exit;
// }

// DBアクセス 
    try {
        $db = new PDO(PDO_DSN, DB_USERNAME, DB_PASSWORD);
        $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

        // selectAllMark (送られてきたmethodの値がselectAllMarkだった場合)
        if($method == 'selectAllMark') {
            $stmt = $db->prepare("select * from mark_list");
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            // var_dump($result);

            header("HTTP/1.1 200 OK");
            header("Content-Type: application/json; charset=utf-8");
            echo json_encode($result, JSON_UNESCAPED_UNICODE);

            return;
        }

        // selectMark (送られてきたmethodの値がselectMarkだった場合)
        if($method == 'selectMark') {
            $stmt = $db->prepare("select * from mark_list where id = :id");
            $stmt->bindValue(':id', $_GET['id'], PDO::PARAM_INT);
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            // var_dump($result);

            header("HTTP/1.1 200 OK");
            header("Content-Type: application/json; charset=utf-8");
            echo json_encode($result, JSON_UNESCAPED_UNICODE);

            return;
        }

        // selectAllComment (送られてきたmethodの値がselectAllCommentだった場合)
        if($method == 'selectAllComment') {
            $stmt = $db->prepare("select * from irritation_history");
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            // var_dump($result);

            header("HTTP/1.1 200 OK");
            header("Content-Type: application/json; charset=utf-8");
            echo json_encode($result, JSON_UNESCAPED_UNICODE);

            return;
        }

        // insertMark (送られてきたmethodの値がselectAllCommentだった場合)
        if($method == 'insertMark') {
            $stmt = $db->prepare("insert into mark_list (width, height) value (:width, :height)");
            $stmt->bindValue(':width', /*$postTest->width*/$_GET['width'], PDO::PARAM_INT);
            $stmt->bindValue(':height', /*$postTest->height*/$_GET['height'], PDO::PARAM_INT);
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            // var_dump($result);

            return;
        }

        // insertComment (送られてきたmethodの値がinsertCommentだった場合)
        if($method == 'insertComment') {
            $stmt = $db->prepare("insert into irritation_history (mark_id, comment, anger_point) value (:mark_id, :comment, :anger_point)");
            $stmt->bindValue(':mark_id', $_GET['mark_id'], PDO::PARAM_INT);
            $stmt->bindValue(':comment', $_GET['comment'], PDO::PARAM_STR);
            $stmt->bindValue(':anger_point', $_GET['anger_point'], PDO::PARAM_INT);
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            // var_dump($result);

            return;
        }

        // updateMark (送られてきたmethodの値がupdateMarkだった場合)
        if($method == 'updateMark') {
            $stmt = $db->prepare("update mark_list set width = :width, height = :height, total_anger_point = :total_anger_point, burst_flag = :burst_flag where id = :id");
            $stmt->bindValue(':width', $_GET['width'], PDO::PARAM_INT);
            $stmt->bindValue(':height', $_GET['height'], PDO::PARAM_INT);
            $stmt->bindValue(':total_anger_point', $_GET['total_anger_point'], PDO::PARAM_INT);
            $stmt->bindValue(':burst_flag', $_GET['burst_flag'], PDO::PARAM_INT);
            $stmt->bindValue(':id', $_GET['id'], PDO::PARAM_INT);
            $stmt->execute();

            $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
            // var_dump($result);

            return;
        }

        // insert or update (送られてきたmethodの値がsaveだった場合)
        // if($method == 'save') {
        //     $stmt = $db->prepare("insert into test (name, height, width) value (:name, :height, :width) on duplicate key update height = values(height), width = values(width)");
        //     $stmt->bindValue(':name', /*$postTest->name*/$_GET['name'], PDO::PARAM_STR);
        //     $stmt->bindValue(':height', /*$postTest->height*/$_GET['height'], PDO::PARAM_INT);
        //     $stmt->bindValue(':width', /*$postTest->width*/$_GET['width'], PDO::PARAM_INT);
        //     $stmt->execute();

        //     $result = $stmt->fetchAll(PDO::FETCH_ASSOC);
        //     var_dump($result);

        //     return;
        // }

        // 存在しないメソッド名が送られてきた時
        header("HTTP/1.1 400 Bad Request");
        header("Content-Type: application/json; charset=utf-8");
        $result = array('code'=>400, 'message'=>'リクエストデータの形式が正しくありません');
        echo json_encode($result, JSON_UNESCAPED_UNICODE);

        $db = null;
    } catch (PDOException $e) {
    //  $db->rollback();
        echo "接続失敗\n";
        echo $e->getMessage();
        exit;
    }
?>