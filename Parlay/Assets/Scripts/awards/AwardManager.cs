using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AwardManager : MonoBehaviour
{
    public List<Award> award_list;
    private List<string> award_ranking;
    private List<Award> awards_won;
    public List<string> top_award;

    void Start()
    {
        award_ranking = new List<string>();
        foreach (Award award in award_list)
        {
            award_ranking.Add(award.awardName);
        }
        top_award = new List<string>();
    }

    public bool WinsAward(int count)
    {
        foreach (Award award in award_list)
        {
            if (award.WinsAward(count))
            {
                awards_won.Add(award);
                calculateTopAward();
                return true;
            }
        }
        return false;
    }

    private void calculateTopAward()
    {
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
        foreach (Award award in award_list)
        {
            award.DisplayAward();
        }
    }
}
