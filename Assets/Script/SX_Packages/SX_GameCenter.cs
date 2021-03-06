﻿using UnityEngine;
using System;
using SA.CrossPlatform.GameServices;
using SA.Android;
using SA.CrossPlatform.UI;
using SA.Android.Firebase.Analytics;

public class SX_GameCenter : MonoBehaviour {
    
#pragma warning disable 649

    public UM_iScore iScore;

    public int Archievements =0;
    public int UnlockedArchievements =0;
    public string iLeaderboardId = "CgkIw7PKmfEVEAIQAA";

    private void Start() {

        var UM_client = UM_GameService.SignInClient;

        if (UM_client.PlayerInfo.State != UM_PlayerState.SignedIn) {
                UM_client.SingIn(result => {
                    if (result.IsSucceeded) {
                        loadAchievements ();                
                    } 
                });
        }

    }

    public void showArchievements(){
        var client = UM_GameService.AchievementsClient;
        client.ShowUI((result) => {});
    }

    public bool SignedIn(){
        var UM_client = UM_GameService.SignInClient;
        return UM_client.PlayerInfo.State == UM_PlayerState.SignedIn;
    }
    
    public void unlockAchievement (string achievementId){

        var client = UM_GameService.AchievementsClient;

        if (SignedIn()) 
            client.Unlock(achievementId, (result) => { 
                if(result.IsSucceeded) {   
                    PlayerPrefs.SetInt (achievementId, 1);       
                	AN_FirebaseAnalytics.LogEvent("unlocked_achievement");
                }
            });  
    }

     public void incrementAchievement (string achievementId, int numSteps){

        var client = UM_GameService.AchievementsClient;
        client.Increment(achievementId, numSteps, (result) => {
            if (result.IsSucceeded) {
                Debug.Log("Incremented");
            } else {
                Debug.Log("Failed: " + result.Error.FullMessage);
            }
        });

    }

     public void loadAchievements ()
     {
        var client = UM_GameService.AchievementsClient;

        if (SignedIn()) 
            client.Load(result => 
            {
                if(result.IsSucceeded) 
                {
                    Archievements = result.Achievements.Count;

                    UnlockedArchievements = 0;
                    foreach(var achievement in result.Achievements)
                    {
                        if(PlayerPrefs.HasKey(achievement.Identifier) && achievement.State==UM_AchievementState.HIDDEN || achievement.State==UM_AchievementState.REVEALED)
                            PlayerPrefs.DeleteKey(achievement.Identifier);

                         if(achievement.State==UM_AchievementState.UNLOCKED)
                            UnlockedArchievements++;
                    }

                   // if(UnlockedArchievements>0 || Archievements>0)
                        //Main.FillAchievements((float)UnlockedArchievements/(float)Archievements);

                        //UM_DialogsUtility.ShowMessage("unlockAchievement", UnlockedArchievements + " " + Archievements);  
                } 
            });


    }

    
    public void showLeaderBoards (string leaderboardId){

        var client = UM_GameService.LeaderboardsClient;

        if(leaderboardId==null)
            client.ShowUI((result) => {});
        else
            client.ShowUI(leaderboardId, (result) => {});
             
    }

    public void submitPlayerScore (string leaderboardId, int score){

        var client = UM_GameService.LeaderboardsClient;

        int context = 0;

        if(leaderboardId==null)
            leaderboardId = iLeaderboardId;

        if (SignedIn()) 
            client.SubmitScore(leaderboardId, score, context, (result) => {
                if(result.IsSucceeded) {
                    Debug.Log("Score submitted successfully");
                } else {
                    Debug.Log("Failed to submit score: " + result.Error.FullMessage);
                }
            });

    }

    public void loadPlayerScore(string boardId) {

        var client = UM_GameService.LeaderboardsClient;

        client.LoadCurrentPlayerScore("CgkIw7PKmfEVEAIQAA", UM_LeaderboardTimeSpan.AllTime, UM_LeaderboardCollection.Public, (result) => {
            if (result.IsSucceeded)
            {
                Main.UpdateLevel(Convert.ToInt32(result.Score.Value));
				Main.LevelUp ();
            }
            else
            {
                Debug.Log("Failed to load player score.");
            }
        });

    }

}

