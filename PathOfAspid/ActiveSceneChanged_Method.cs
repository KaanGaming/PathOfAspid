using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using Modding;
using UnityEngine;

namespace PathOfAspid
{
    public partial class PathOfAspid : Mod
    {
        CustomAspidPlacement customplacement;
        float timeSinceSceneChanged = 0f;
        bool spawned = false;

        private void SpawnAspid()
        {
            var cScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            spawned = true;

            managedAspids.Clear();

            if (!GameManager.instance.IsGameplayScene()) return;

            if (customplacement.AspidLocations.ContainsKey(cScene.name))
            {
                var locations = customplacement.AspidLocations[cScene.name];

                for (int i = 0; i < Settings.AspidQuantity; i++)
                {
                    if (i + 1 > locations.aspidPositions.Count)
                    {
                        System.Random rng = new System.Random();
                        managedAspids.Add(AddPrimalAspid(new Vector2(locations.aspidPositions[0].x + (float)(rng.NextDouble() - 0.5), locations.aspidPositions[0].y + (float)(rng.NextDouble() * 0.6 - 0.3))));
                    }
                    else
                    {
                        if (locations.isManaged)
                            managedAspids.Add(AddPrimalAspid(new Vector2(locations.aspidPositions[i].x, locations.aspidPositions[i].y)));
                    }
                }
            }
            else
            {
                bool knightfacesright = HeroController.instance.cState.facingRight;
                if (knightfacesright)
                {
                    for (int i = 0; i < Settings.AspidQuantity; i++)
                    {
                        System.Random rng = new System.Random();
                        managedAspids.Add(AddPrimalAspid(new Vector2(HeroController.instance.transform.GetPositionX() + 4f + (float)(rng.NextDouble() - 0.5), HeroController.instance.transform.GetPositionY() + 1.5f + (float)(rng.NextDouble() * 0.6 - 0.3))));
                    }
                }
                else
                {
                    for (int i = 0; i < Settings.AspidQuantity; i++)
                    {
                        System.Random rng = new System.Random();
                        managedAspids.Add(AddPrimalAspid(new Vector2(HeroController.instance.transform.GetPositionX() - 4f + (float)(rng.NextDouble() - 0.5), HeroController.instance.transform.GetPositionY() + 1.5f + (float)(rng.NextDouble() * 0.6 - 0.3))));
                    }
                }
            }
        }

        private void ActiveSceneChanged(UnityEngine.SceneManagement.Scene from, UnityEngine.SceneManagement.Scene to)
        {
            timeSinceSceneChanged = 0;
            spawned = false;

            #region Path of Pain
            /*
             * old code im gonna keep anyways
             * 
            switch (to.name)
            {
                case "White_Palace_18": // Path of Pain - first room
                    managedAspids.Add(AddPrimalAspid(new Vector2(284.52f, 17.7f)));
                    if (Settings.AspidQuantity > 1) managedAspids.Add(AddPrimalAspid(new Vector2(285.77f, 17.01f)));
                    if (Settings.AspidQuantity > 2) managedAspids.Add(AddPrimalAspid(new Vector2(283.19f, 16.96f)));
                    if (Settings.AspidQuantity > 3) managedAspids.Add(AddPrimalAspid(new Vector2(283.14f, 18.43f)));
                    if (Settings.AspidQuantity > 4) managedAspids.Add(AddPrimalAspid(new Vector2(285.81f, 18.46f)));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            managedAspids.Add(AddPrimalAspid(new Vector2(284.52f + (float)(rng.NextDouble() - 0.5), 17.7f + (float)(rng.NextDouble() * 0.6 - 0.3))));
                        }
                    }
                    break;
                case "White_Palace_17": // Path of Pain - second room
                    managedAspids.Add(AddPrimalAspid(new Vector2(75.74f, 4.28f)));
                    if (Settings.AspidQuantity > 1) managedAspids.Add(AddPrimalAspid(new Vector2(77.06f, 3.35f)));
                    if (Settings.AspidQuantity > 2) managedAspids.Add(AddPrimalAspid(new Vector2(74.84f, 3.25f)));
                    if (Settings.AspidQuantity > 3) managedAspids.Add(AddPrimalAspid(new Vector2(74.68f, 5.19f)));
                    if (Settings.AspidQuantity > 4) managedAspids.Add(AddPrimalAspid(new Vector2(77.08f, 5.12f)));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            managedAspids.Add(AddPrimalAspid(new Vector2(75.74f + (float)(rng.NextDouble() - 0.5), 4.28f + (float)(rng.NextDouble() * 0.6 - 0.3))));
                        }
                    }
                    break;
                case "White_Palace_19": // Path of Pain - third room
                    managedAspids.Add(AddPrimalAspid(new Vector2(109.76f, 39.11f)));
                    if (Settings.AspidQuantity > 1) managedAspids.Add(AddPrimalAspid(new Vector2(111.23f, 38.03f)));
                    if (Settings.AspidQuantity > 2) managedAspids.Add(AddPrimalAspid(new Vector2(108.1f, 38.04f)));
                    if (Settings.AspidQuantity > 3) managedAspids.Add(AddPrimalAspid(new Vector2(111.07f, 40.31f)));
                    if (Settings.AspidQuantity > 4) managedAspids.Add(AddPrimalAspid(new Vector2(108.18f, 40.27f)));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            managedAspids.Add(AddPrimalAspid(new Vector2(109.76f + (float)(rng.NextDouble() - 0.5), 39.11f + (float)(rng.NextDouble() * 0.6 - 0.3))));
                        }
                    }
                    break;
                case "White_Palace_20": // Path of Pain - fourth room
                    managedAspids.Add(AddPrimalAspid(new Vector2(12.68f, 162.07f)));
                    if (Settings.AspidQuantity > 1) managedAspids.Add(AddPrimalAspid(new Vector2(9.94f, 161.87f)));
                    if (Settings.AspidQuantity > 2) managedAspids.Add(AddPrimalAspid(new Vector2(7.87f, 160.25f)));
                    if (Settings.AspidQuantity > 3) managedAspids.Add(AddPrimalAspid(new Vector2(7.42f, 158.43f)));
                    if (Settings.AspidQuantity > 4) managedAspids.Add(AddPrimalAspid(new Vector2(19.37f, 158.27f)));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            managedAspids.Add(AddPrimalAspid(new Vector2(19.37f + (float)(rng.NextDouble() - 0.5), 158.27f + (float)(rng.NextDouble() * 0.6 - 0.3))));
                        }
                    }
                    break;
            #endregion
            #region Godhome Bosses
                case "GG_Gruz_Mother":
                    AddPrimalAspid(new Vector2(89.83f, 20.85f));
                    if (Settings.AspidQuantity > 1) AddPrimalAspid(new Vector2(91.17f, 20.13f));
                    if (Settings.AspidQuantity > 2) AddPrimalAspid(new Vector2(88.4f, 20.1f));
                    if (Settings.AspidQuantity > 3) AddPrimalAspid(new Vector2(91.17f, 21.5f));
                    if (Settings.AspidQuantity > 4) AddPrimalAspid(new Vector2(88.28f, 21.56f));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            AddPrimalAspid(new Vector2(89.83f + (float)(rng.NextDouble() - 0.5), 20.85f + (float)(rng.NextDouble() * 0.6 - 0.3)));
                        }
                    }
                    break;
                case "GG_Gruz_Mother_V":
                    AddPrimalAspid(new Vector2(89.49f, 21.06f));
                    if (Settings.AspidQuantity > 1) AddPrimalAspid(new Vector2(90.73f, 20.34f));
                    if (Settings.AspidQuantity > 2) AddPrimalAspid(new Vector2(88f, 20.34f));
                    if (Settings.AspidQuantity > 3) AddPrimalAspid(new Vector2(90.92f, 21.81f));
                    if (Settings.AspidQuantity > 4) AddPrimalAspid(new Vector2(88.03f, 21.74f));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            AddPrimalAspid(new Vector2(89.49f + (float)(rng.NextDouble() - 0.5), 21.06f + (float)(rng.NextDouble() * 0.6 - 0.3)));
                        }
                    }
                    break;
                case "GG_Vengefly":
                    AddPrimalAspid(new Vector2(44.48f, 18.92f));
                    if (Settings.AspidQuantity > 1) AddPrimalAspid(new Vector2(46.05f, 18.12f));
                    if (Settings.AspidQuantity > 2) AddPrimalAspid(new Vector2(42.7f, 18.05f));
                    if (Settings.AspidQuantity > 3) AddPrimalAspid(new Vector2(46.09f, 19.86f));
                    if (Settings.AspidQuantity > 4) AddPrimalAspid(new Vector2(42.73f, 19.86f));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            AddPrimalAspid(new Vector2(44.48f + (float)(rng.NextDouble() - 0.5), 18.92f + (float)(rng.NextDouble() * 0.6 - 0.3)));
                        }
                    }
                    break;
                case "GG_Vengefly_V":
                    AddPrimalAspid(new Vector2(44.48f, 18.92f));
                    if (Settings.AspidQuantity > 1) AddPrimalAspid(new Vector2(46.05f, 18.12f));
                    if (Settings.AspidQuantity > 2) AddPrimalAspid(new Vector2(42.7f, 18.05f));
                    if (Settings.AspidQuantity > 3) AddPrimalAspid(new Vector2(46.09f, 19.86f));
                    if (Settings.AspidQuantity > 4) AddPrimalAspid(new Vector2(42.73f, 19.86f));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            AddPrimalAspid(new Vector2(44.48f + (float)(rng.NextDouble() - 0.5), 18.92f + (float)(rng.NextDouble() * 0.6 - 0.3)));
                        }
                    }
                    break;
                case "GG_Brooding_Mawlek":
                    AddPrimalAspid(new Vector2(56.15f, 15.25f));
                    if (Settings.AspidQuantity > 1) AddPrimalAspid(new Vector2(57.48f, 14.45f));
                    if (Settings.AspidQuantity > 2) AddPrimalAspid(new Vector2(54.33f, 14.38f));
                    if (Settings.AspidQuantity > 3) AddPrimalAspid(new Vector2(57.93f, 16.26f));
                    if (Settings.AspidQuantity > 4) AddPrimalAspid(new Vector2(54.3f, 16.15f));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            AddPrimalAspid(new Vector2(56.15f + (float)(rng.NextDouble() - 0.5), 15.25f + (float)(rng.NextDouble() * 0.6 - 0.3)));
                        }
                    }
                    break;
                case "GG_Brooding_Mawlek_V":
                    AddPrimalAspid(new Vector2(56.15f, 15.25f));
                    if (Settings.AspidQuantity > 1) AddPrimalAspid(new Vector2(57.48f, 14.45f));
                    if (Settings.AspidQuantity > 2) AddPrimalAspid(new Vector2(54.33f, 14.38f));
                    if (Settings.AspidQuantity > 3) AddPrimalAspid(new Vector2(57.93f, 16.26f));
                    if (Settings.AspidQuantity > 4) AddPrimalAspid(new Vector2(54.3f, 16.15f));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            AddPrimalAspid(new Vector2(56.15f + (float)(rng.NextDouble() - 0.5), 15.25f + (float)(rng.NextDouble() * 0.6 - 0.3)));
                        }
                    }
                    break;
                case "GG_False_Knight":
                    AddPrimalAspid(new Vector2(17.85f, 37.96f));
                    if (Settings.AspidQuantity > 1) AddPrimalAspid(new Vector2(19.58f, 36.92f));
                    if (Settings.AspidQuantity > 2) AddPrimalAspid(new Vector2(16.11f, 36.85f));
                    if (Settings.AspidQuantity > 3) AddPrimalAspid(new Vector2(19.72f, 38.97f));
                    if (Settings.AspidQuantity > 4) AddPrimalAspid(new Vector2(16.46f, 39.01f));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            AddPrimalAspid(new Vector2(17.85f + (float)(rng.NextDouble() - 0.5), 37.96f + (float)(rng.NextDouble() * 0.6 - 0.3)));
                        }
                    }
                    break;
                case "GG_Failed_Champion":
                    AddPrimalAspid(new Vector2(17.85f, 37.96f));
                    if (Settings.AspidQuantity > 1) AddPrimalAspid(new Vector2(19.58f, 36.92f));
                    if (Settings.AspidQuantity > 2) AddPrimalAspid(new Vector2(16.11f, 36.85f));
                    if (Settings.AspidQuantity > 3) AddPrimalAspid(new Vector2(19.72f, 38.97f));
                    if (Settings.AspidQuantity > 4) AddPrimalAspid(new Vector2(16.46f, 39.01f));
                    if (Settings.AspidQuantity > 5)
                    {
                        for (int i = 1; i <= Settings.AspidQuantity - 5; i++)
                        {
                            System.Random rng = new System.Random();
                            AddPrimalAspid(new Vector2(17.85f + (float)(rng.NextDouble() - 0.5), 37.96f + (float)(rng.NextDouble() * 0.6 - 0.3)));
                        }
                    }
                    break;
            }
            */
            #endregion
        }
    }
}