using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Modding;
using UnityEngine;
using System.Diagnostics;

namespace PathOfAspid
{
    public partial class PathOfAspid : Mod, IGlobalSettings<PathOfAspidSettings>
    {
        static internal PathOfAspid Instance;

        public PathOfAspid() : base("PathOfAspid") { }

        public override string GetVersion() => "1.0.0";

        public PathOfAspidSettings Settings = new PathOfAspidSettings();

        public void OnLoadGlobal(PathOfAspidSettings s) => Settings = s;

        public PathOfAspidSettings OnSaveGlobal() => Settings;

        public GameObject aspid = null;

        public GameObject enemy = null;

        public GameObject hunter = null;

        private List<GameObject> managedAspids = new List<GameObject>();


        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
            {
                ("Deepnest_East_07","Super Spitter"),
                ("Mines_07","Crystal Flyer"),
                (Settings.CustomEnemyScene,Settings.CustomEnemyPath)
            };
        }

        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {
            Log("Initializing");

            aspid = preloadedObjects["Deepnest_East_07"]["Super Spitter"];
            hunter = preloadedObjects["Mines_07"]["Crystal Flyer"];
            enemy = preloadedObjects[Settings.CustomEnemyScene][Settings.CustomEnemyPath];

            customplacement = new CustomAspidPlacement();
            Instance = this;

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += ActiveSceneChanged;
            ModHooks.HeroUpdateHook += HeroUpdateHook;

            Log("Initialized");
        }

        private void HeroUpdateHook()
        {
            if (!GameManager.instance.isPaused) timeSinceSceneChanged += Time.deltaTime;

            if (timeSinceSceneChanged > 2f && !spawned) SpawnAspid();

            var herox = HeroController.instance.gameObject.transform.position.x;
            var heroy = HeroController.instance.gameObject.transform.position.y;
            if (HeroController.instance.cState.onGround)
            {
                foreach (GameObject maspid in managedAspids)
                {
                    if (maspid == null)
                    {
                        managedAspids.Remove(maspid);
                        continue;
                    }
                    if (maspid.transform.position.x - herox <= -25 || maspid.transform.position.x - herox >= 25)
                    {
                        maspid.transform.SetPosition2D(herox, heroy + 6f);
                    }
                    if (maspid.transform.position.y - heroy <= -25 || maspid.transform.position.y - heroy >= 25)
                    {
                        maspid.transform.SetPosition2D(herox, heroy + 6f);
                    }
                    LogDebug($"Player location: {herox}, {heroy}\nAspid location: {maspid.transform.position.x}, {maspid.transform.position.y}\nCalculation: {maspid.transform.position.x - herox}, {maspid.transform.position.y - heroy}");
                }
            }
            if (!HeroController.instance.cState.onGround)
            {
                foreach (GameObject maspid in managedAspids)
                {
                    if (maspid == null)
                    {
                        managedAspids.Remove(maspid);
                        continue;
                    }
                    if (maspid.transform.position.x - herox <= -25 || maspid.transform.position.x - herox >= 25)
                    {
                        maspid.transform.SetPosition2D(herox, heroy - 7f);
                    }
                    if (maspid.transform.position.y - heroy <= -25 || maspid.transform.position.y - heroy >= 25)
                    {
                        maspid.transform.SetPosition2D(herox, heroy - 7f);
                    }
                    LogDebug($"Player location: {herox}, {heroy}\nAspid location: {maspid.transform.position.x}, {maspid.transform.position.y}\nCalculation: {maspid.transform.position.x - herox}, {maspid.transform.position.y - heroy}");
                }
            }
        }

        // Check ActiveSceneChanged_Method.cs

        List<GameObject> GetChildren(GameObject parent)
        {
            List<GameObject> children = new List<GameObject>();
            foreach (Transform child in parent.transform)
            {
                children.Add(child.gameObject);
            }
            return children;
        }

        GameObject AddPrimalAspid(Vector2 position)
        {
            if (Settings.UseCustomEnemy)
            {
                foreach (var pfsm in enemy.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewEnemy = GameObject.Instantiate(enemy);
                NewEnemy.SetActive(true);
                NewEnemy.SetActiveChildren(true);
                HealthManager EnemyHP = NewEnemy.GetComponent<HealthManager>();
                EnemyHP.hp = int.MaxValue;

                var aspidZ = NewEnemy.transform.position.z;
                NewEnemy.transform.position = new Vector3(position.x, position.y, aspidZ);
                return NewEnemy;
            }
            if (!Settings.UseCrystalHunters)
            {
                foreach (var pfsm in aspid.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewAspid = GameObject.Instantiate(aspid);
                NewAspid.SetActive(true);
                NewAspid.SetActiveChildren(true);
                List<GameObject> AspidChildren = GetChildren(NewAspid);
                foreach (var child in AspidChildren)
                {
                    if (child.name == "Alert Range New")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                    if (child.name == "Unalert Range")
                        child.GetComponent<CircleCollider2D>().radius = 25;
                }
                HealthManager AspidHP = NewAspid.GetComponent<HealthManager>();
                AspidHP.hp = int.MaxValue;

                var aspidZ = NewAspid.transform.position.z;
                NewAspid.transform.position = new Vector3(position.x, position.y, aspidZ);
                return NewAspid;
            }
            else
            {
                foreach (var pfsm in hunter.GetComponentsInChildren<PlayMakerFSM>())
                {
                    pfsm.SetState(pfsm.Fsm.StartState);
                }
                GameObject NewHunter = GameObject.Instantiate(hunter);
                NewHunter.SetActive(true);
                NewHunter.SetActiveChildren(true);
                List<GameObject> HunterChildren = GetChildren(NewHunter);
                foreach (var child in HunterChildren)
                {
                    if (child.name == "Alert Range")
                        child.GetComponent<CircleCollider2D>().radius = 15;
                }
                HealthManager HunterHP = NewHunter.GetComponent<HealthManager>();
                HunterHP.hp = int.MaxValue;

                var aspidZ = NewHunter.transform.position.z;
                NewHunter.transform.position = new Vector3(position.x, position.y, aspidZ);
                return NewHunter;
            }
        }
    }

    public class PathOfAspidSettings
    {
        public int AspidQuantity = 1;
        public bool UseCrystalHunters = false;
        public bool UseCustomEnemy = false;
        public string CustomEnemyScene = "Deepnest_East_07";
        public string CustomEnemyPath = "Super Spitter";
    }
}
