using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreItem
{
   
   public enum ItemType {
       BasicBall,
       FrostBall,
       NatureBall,
       XmasBall,
       LoveBall,
       ThunderBall,
       BoostTo15
   }

   public static int GetCost(ItemType itemType) {
       switch (itemType) {
           default:
           case ItemType.BasicBall:             return 0;
           case ItemType.FrostBall:             return 1000;
           case ItemType.NatureBall:            return 1000;
           case ItemType.XmasBall:              return 1000;
           case ItemType.LoveBall:              return 1000;
           case ItemType.ThunderBall:           return 2000;
           case ItemType.BoostTo15:             return 1000;
       }
   }

   public static GameObject GetPlayerPrefabMenu(ItemType itemType) {
        switch (itemType) {
           default:
           case ItemType.BasicBall:             return StoreAssets.i.pf_Menu_basicBall;
           case ItemType.FrostBall:             return StoreAssets.i.pf_Menu_frostBall;
           case ItemType.NatureBall:            return StoreAssets.i.pf_Menu_natureBall;
           case ItemType.XmasBall:              return StoreAssets.i.pf_Menu_xmasBall;
           case ItemType.LoveBall:              return StoreAssets.i.pf_Menu_loveBall;
           case ItemType.ThunderBall:           return StoreAssets.i.pf_Menu_thunderBall;
       }
       
   }

   public static GameObject GetPlayerPrefabGM0(ItemType itemType) {
        switch (itemType) {
           default:
           case ItemType.BasicBall:             return StoreAssets.i.pf_GM0_basicBall;
           case ItemType.FrostBall:             return StoreAssets.i.pf_GM0_frostBall;
           case ItemType.NatureBall:            return StoreAssets.i.pf_GM0_natureBall;
           case ItemType.XmasBall:              return StoreAssets.i.pf_GM0_xmasBall;
           case ItemType.LoveBall:              return StoreAssets.i.pf_GM0_loveBall;
           case ItemType.ThunderBall:           return StoreAssets.i.pf_GM0_thunderBall;
           
       }   
   }

    public static GameObject GetPlayerPrefabGM1(ItemType itemType) {
        switch (itemType) {
           default:
           case ItemType.BasicBall:             return StoreAssets.i.pf_GM1_basicBall;
           case ItemType.FrostBall:             return StoreAssets.i.pf_GM1_frostBall;
           case ItemType.NatureBall:            return StoreAssets.i.pf_GM1_natureBall;
           case ItemType.XmasBall:              return StoreAssets.i.pf_GM1_xmasBall;
           case ItemType.LoveBall:              return StoreAssets.i.pf_GM1_loveBall;
           case ItemType.ThunderBall:           return StoreAssets.i.pf_GM1_thunderBall;
       }
   }
    public static GameObject GetPlayerPrefabGM2(ItemType itemType) {
        switch (itemType) {
           default:
           case ItemType.BasicBall:             return StoreAssets.i.pf_GM2_basicBall;
           case ItemType.FrostBall:             return StoreAssets.i.pf_GM2_frostBall;
           case ItemType.NatureBall:            return StoreAssets.i.pf_GM2_natureBall;
           case ItemType.XmasBall:              return StoreAssets.i.pf_GM2_xmasBall;
           case ItemType.LoveBall:              return StoreAssets.i.pf_GM2_loveBall;
           case ItemType.ThunderBall:           return StoreAssets.i.pf_GM2_thunderBall;

       }
   }
   public static GameObject GetPlayerPrefabGM3(ItemType itemType) {
        switch (itemType) {
           default:
           case ItemType.BasicBall:             return StoreAssets.i.pf_GM3_basicBall;
           case ItemType.FrostBall:             return StoreAssets.i.pf_GM3_frostBall;
           case ItemType.NatureBall:            return StoreAssets.i.pf_GM3_natureBall;
           case ItemType.XmasBall:              return StoreAssets.i.pf_GM3_xmasBall;
           case ItemType.LoveBall:              return StoreAssets.i.pf_GM3_loveBall;
           case ItemType.ThunderBall:           return StoreAssets.i.pf_GM3_thunderBall;
       }
   }
    public static GameObject GetPlayerPrefabGM4(ItemType itemType) {
        switch (itemType) {
           default:
           case ItemType.BasicBall:             return StoreAssets.i.pf_GM4_basicBall;
           case ItemType.FrostBall:             return StoreAssets.i.pf_GM4_frostBall;
           case ItemType.NatureBall:            return StoreAssets.i.pf_GM4_natureBall;
           case ItemType.XmasBall:              return StoreAssets.i.pf_GM4_xmasBall;
           case ItemType.LoveBall:              return StoreAssets.i.pf_GM4_loveBall;
           case ItemType.ThunderBall:           return StoreAssets.i.pf_GM4_thunderBall;
       }
   }

    public static GameObject GetLvlUpFX(ItemType itemType) {
        switch (itemType) {
           default:
           case ItemType.BasicBall:             return StoreAssets.i.fx_basicLvlUp;
           case ItemType.FrostBall:             return StoreAssets.i.fx_frostLvlUp;
           case ItemType.NatureBall:            return StoreAssets.i.fx_natureLvlUp;
           case ItemType.XmasBall:              return StoreAssets.i.fx_xmasLvlUp;
           case ItemType.LoveBall:              return StoreAssets.i.fx_loveLvlUp;
           case ItemType.ThunderBall:           return StoreAssets.i.fx_thunderLvlUp;
       }
   }

   public static GameObject GetImpactFX(ItemType itemType) {
        switch (itemType) {
           default:
           case ItemType.BasicBall:             return StoreAssets.i.fx_basicImpact;
           case ItemType.FrostBall:             return StoreAssets.i.fx_frostImpact;
           case ItemType.NatureBall:            return StoreAssets.i.fx_natureImpact;
           case ItemType.XmasBall:              return StoreAssets.i.fx_xmasImpact;
           case ItemType.LoveBall:              return StoreAssets.i.fx_loveImpact;
           case ItemType.ThunderBall:           return StoreAssets.i.fx_thunderImpact;
       }
   }

    public static GameObject GetBulletPrefab(ItemType itemType) {
        switch (itemType) {
           default:
           case ItemType.BasicBall:             return StoreAssets.i.pf_Bullet_basicBall;
           case ItemType.FrostBall:             return StoreAssets.i.pf_Bullet_frostBall;
           case ItemType.NatureBall:            return StoreAssets.i.pf_Bullet_natureBall;
           case ItemType.XmasBall:              return StoreAssets.i.pf_Bullet_natureBall; //not used anymore so can leave anything
           case ItemType.LoveBall:              return StoreAssets.i.pf_Bullet_natureBall;
           case ItemType.ThunderBall:           return StoreAssets.i.pf_Bullet_natureBall;
       }
   }


}
