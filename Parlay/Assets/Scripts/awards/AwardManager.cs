using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
* This file has the business logic for the award manager.
* The award manager checks all of the different Award objects to see if their 
* requirements have been met by the user when a question is answered. If it has 
* been met then an instance of that award is stored locally and then the top
* award the user has recieved is recalculated for future use.
* 
* @author: Andres Montoya
*/

public class AwardManager : MonoBehaviour
{
    public List<Award> award_list;
    public List<string> award_ranking;
    private List<Award> awards_won;
    public List<string> top_award;

    void Start()
    {
        award_ranking = new List<string>();
        awards_won = new List<Award>();
        top_award = new List<string>();
    }

    public bool WinsAward()
    {
        foreach (Award award in award_list)
        {
            if (award.WinsAward())
            {
                awards_won.Add(award);
                calculateTopAward();
                award.DisplayAward();
                return true;
            }
        }
        return false;
    }

    private void calculateTopAward()
    {
        print("Calculating award");
        award_ranking.Clear();
        foreach (Award award in award_list)
        {
            award_ranking.Add(award.awardName);
        }
        string temp_award = "";
        foreach (Award award in awards_won)
        {
            if (award_ranking.IndexOf(award.awardName) > award_ranking.IndexOf(temp_award))
            {
                temp_award = award.awardName;
            }
        }
        top_award.Clear();
        top_award.Add(temp_award);
    }

    public void DisplayAward()
    {
        WinsAward();
    }
}
