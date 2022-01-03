using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreAssets : MonoBehaviour
{
   
   private static StoreAssets _i;
   public static StoreAssets i {
        get {
            if (_i == null) _i = (Instantiate(Resources.Load("StoreAssets")) as GameObject).GetComponent<StoreAssets>();
            return _i;
        }
   }

 [Header("Basic")]
   public GameObject pf_Menu_basicBall;
   public GameObject pf_GM0_basicBall;
   public GameObject pf_GM1_basicBall;
   public GameObject pf_Bullet_basicBall;
   public GameObject fx_basicLvlUp;
   public GameObject fx_basicImpact;
   public GameObject pf_GM2_basicBall;
   public GameObject pf_GM3_basicBall;
   public GameObject pf_GM4_basicBall;
   

[Header("Frost")]
   public GameObject pf_Menu_frostBall;
   public GameObject pf_GM0_frostBall;
   public GameObject pf_GM1_frostBall;
   public GameObject pf_Bullet_frostBall;
   public GameObject fx_frostLvlUp;
   public GameObject fx_frostImpact;
   public GameObject pf_GM2_frostBall;
   public GameObject pf_GM3_frostBall;
   public GameObject pf_GM4_frostBall;

[Header("Nature")]
   public GameObject pf_Menu_natureBall;
   public GameObject pf_GM0_natureBall;
   public GameObject pf_GM1_natureBall;
   public GameObject pf_Bullet_natureBall;
   public GameObject fx_natureLvlUp;
   public GameObject fx_natureImpact;
   public GameObject pf_GM2_natureBall;
   public GameObject pf_GM3_natureBall;
   public GameObject pf_GM4_natureBall;

[Header("Xmas")]
   public GameObject pf_Menu_xmasBall;
   public GameObject pf_GM0_xmasBall;
   public GameObject pf_GM1_xmasBall;
   public GameObject pf_Bullet_xmasBall;
   public GameObject fx_xmasLvlUp;
   public GameObject fx_xmasImpact;
   public GameObject pf_GM2_xmasBall;
   public GameObject pf_GM3_xmasBall;
   public GameObject pf_GM4_xmasBall;

[Header("Love")]
   public GameObject pf_Menu_loveBall;
   public GameObject pf_GM0_loveBall;
   public GameObject pf_GM1_loveBall;
   public GameObject pf_Bullet_loveBall;
   public GameObject fx_loveLvlUp;
   public GameObject fx_loveImpact;
   public GameObject pf_GM2_loveBall;
   public GameObject pf_GM3_loveBall;
   public GameObject pf_GM4_loveBall;

[Header("Thunder")]
   public GameObject pf_Menu_thunderBall;
   public GameObject pf_GM0_thunderBall;
   public GameObject pf_GM1_thunderBall;
   public GameObject pf_Bullet_thunderBall;
   public GameObject fx_thunderLvlUp;
   public GameObject fx_thunderImpact;
   public GameObject pf_GM2_thunderBall;
   public GameObject pf_GM3_thunderBall;
   public GameObject pf_GM4_thunderBall;
   

[Header("Other")]
   public GameObject pf_cannon_GM1;
   public GameObject[] pf_planetTargets_GM3;
   public GameObject[] pf_lightSabers;
   public GameObject pf_galaxy;
   public GameObject[] pf_asteroids;
   public GameObject pf_levelUpText;
   public GameObject pf_boostStoreObject;
   public GameObject pf_shockwave;
[Header("Icons")]
   public Sprite sp_starIcon;
   public Sprite sp_rockIcon;
   public Sprite sp_planetIcon;
   public Sprite sp_lightSaberIcon;


}
