using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
 
[System.Serializable]
public class ChatResponse
{
    public string msg;
    public long timestamp;
}
 
public class CometTest : MonoBehaviour 
{
    string db_url = "backend.php";
 
    long last_timestamp = 0;
    WWW comet_www;
    void Start()
    {
        comet_www = new WWW(db_url + "?timestamp=" + last_timestamp);
        StartCoroutine(WaitingForResponse(comet_www, ParseResponse));
    }
 
 
    string chat_msg = "";
 
 
    List<string> message_list = new List<string>();
 
    void OnGUI()
    {
 
        chat_msg = GUILayout.TextField(chat_msg, GUILayout.Width(150));
 
        if (GUILayout.Button("send"))
        {
            if (chat_msg != "")
            {
               // message_list.Add(chat_msg);
                StartCoroutine(WaitingForResponse(new WWW(db_url + "?msg=" + chat_msg), null));
            }
        }
         
        for(int i=0; i<message_list.Count; i++)
        {
            GUILayout.Label(message_list[i]);
        }
    }
 
    void OnApplicationQuit()
    {
        if (comet_www != null)
        {
            comet_www.Dispose();
            comet_www = null;
        }
    }
 
    void ParseResponse(string text)
    {
        if (text.Contains("{"))
        {
            JsonFx.Json.JsonReader reader = new JsonFx.Json.JsonReader(text);
            ChatResponse r = reader.Deserialize<ChatResponse>();
            Debug.Log(string.Format("msg: {0}, timestamp: {1}", r.msg, r.timestamp));
            last_timestamp = r.timestamp;
            message_list.Add(r.msg);
            comet_www = new WWW(db_url + "?timestamp=" + last_timestamp);
            StartCoroutine(WaitingForResponse(comet_www, ParseResponse));
        }
        else
        {
            Debug.Log("strange response");
        }
    }
 
    public IEnumerator WaitingForResponse(WWW www, RequestCallback callback)
    {
 
        //Debug.Log("" + Time.time);
        yield return www; // ожидаем пока получим с сервера данные
        //Debug.Log("" + Time.time);
 
        if (www.error == null)
        {
            //Debug.Log("Successful.");
        }
        else
        {
            Debug.Log("Failed.");
        }
 
        if (callback != null)
        {
            callback(www.text);
            callback = null;
        }
 
        //Очищаем данные
        www.Dispose();
        www = null;
    }
 
    public delegate void RequestCallback(string str);
 
    void DebugResponse(string s)
    {
        Debug.Log(string.Format("RESPONSE: {0}", s));
    }

}
//gnoblin
 
