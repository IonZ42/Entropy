using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class sl : MonoBehaviour
{
    private const string path = @"save.txt";
    public static bool isSaved = File.Exists(path);
    public static float score = 0.0f;//可为负数
    public static short stageUnlock = 1;
    public static bool BGMon = true;
    public static bool soundOn = true;

    //void Awake() {}
    void Start() {createFile(); getData(); }// if (isSaved) { getData(); } else { createFile(); } 
    /// <summary>
    /// initialize a txt file if not have
    /// </summary>
    public void createFile()
    {
        if (!isSaved)
        {
            score = 0.0f;
            stageUnlock = 1;
            BGMon = true;
            soundOn = true;
            setData();
            isSaved = true;
            print("initialize save OK");
        }
    }
    /// <summary>
    /// para:score,stageUnlock,BGMon,soundOn ; -1.0f -1 as notchange
    /// </summary>
    public void setData()
    {
        StreamWriter streamWriter = File.CreateText(path);
        streamWriter.Write(score + "X" + stageUnlock + "X" + BGMon + "X" + soundOn + "X");
        streamWriter.Close();
        print("data saved");
    }
    public void deleteData()
    {
        isSaved = false;
        createFile();
    }
  /// <summary>
  /// load data
  /// </summary>
  /// <returns></returns>
    public void getData()
    {
        StreamReader streamReader = File.OpenText(path);
        string data = streamReader.ReadToEnd();
        string[] temp = data.Split('X');
        score = float.Parse(temp[0]);
        stageUnlock = short.Parse(temp[1]);
        BGMon = bool.Parse(temp[2]);
        soundOn = bool.Parse(temp[3]);
        print("data loaded");
    }
    public void setBGMon() { BGMon = !BGMon; print("BGM switch"); }
    public bool getBGMon() { return BGMon; }
    public void setSoundOn() { soundOn = !soundOn; print("sound switch"); }
    public bool getSoundOn() { return soundOn; }
    public void setScore(float sco) { score = sco; }
    public void scoreAdd(float adder) { score += adder; }
    public void scoreMns(float mns) { score -= mns; }
    public float getScore() { return score; }
    public void setStageUnlock(short number) { stageUnlock = number; }
    public short getStageUnlock() { return stageUnlock; }
}
