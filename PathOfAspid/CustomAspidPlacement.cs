using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;

namespace PathOfAspid
{
    public class CustomAspidPlacement
    {
        public Dictionary<string, (bool isManaged, List<(float x, float y)> aspidPositions)> AspidLocations;
        Assembly exAsm;
        string eaFolder;

        /*
            switch (to.name)
            {
                case "White_Palace_18": // Path of Pain - first room
                    managedAspids.Add(AddPrimalAspid(new Vector2(284.52f, 17.7f)));
                    if (Settings.AspidQuantity > 1) managedAspids.Add(AddPrimalAspid(new Vector2(285.77f, 17.01f)));
                    if (Settings.AspidQuantity > 2) managedAspids.Add(AddPrimalAspid(new Vector2(283.19f, 16.96f)));
                    if (Settings.AspidQuantity > 3) managedAspids.Add(AddPrimalAspid(new Vector2(283.14f, 18.43f)));
                    if (Settings.AspidQuantity > 4) managedAspids.Add(AddPrimalAspid(new Vector2(285.81f, 18.46f)));
                case "White_Palace_17": // Path of Pain - second room
                    managedAspids.Add(AddPrimalAspid(new Vector2(75.74f, 4.28f)));
                    if (Settings.AspidQuantity > 1) managedAspids.Add(AddPrimalAspid(new Vector2(77.06f, 3.35f)));
                    if (Settings.AspidQuantity > 2) managedAspids.Add(AddPrimalAspid(new Vector2(74.84f, 3.25f)));
                    if (Settings.AspidQuantity > 3) managedAspids.Add(AddPrimalAspid(new Vector2(74.68f, 5.19f)));
                    if (Settings.AspidQuantity > 4) managedAspids.Add(AddPrimalAspid(new Vector2(77.08f, 5.12f)));
                case "White_Palace_19": // Path of Pain - third room
                    managedAspids.Add(AddPrimalAspid(new Vector2(109.76f, 39.11f)));
                    if (Settings.AspidQuantity > 1) managedAspids.Add(AddPrimalAspid(new Vector2(111.23f, 38.03f)));
                    if (Settings.AspidQuantity > 2) managedAspids.Add(AddPrimalAspid(new Vector2(108.1f, 38.04f)));
                    if (Settings.AspidQuantity > 3) managedAspids.Add(AddPrimalAspid(new Vector2(111.07f, 40.31f)));
                    if (Settings.AspidQuantity > 4) managedAspids.Add(AddPrimalAspid(new Vector2(108.18f, 40.27f)));
                case "White_Palace_20": // Path of Pain - fourth room
                    managedAspids.Add(AddPrimalAspid(new Vector2(12.68f, 162.07f)));
                    if (Settings.AspidQuantity > 1) managedAspids.Add(AddPrimalAspid(new Vector2(9.94f, 161.87f)));
                    if (Settings.AspidQuantity > 2) managedAspids.Add(AddPrimalAspid(new Vector2(7.87f, 160.25f)));
                    if (Settings.AspidQuantity > 3) managedAspids.Add(AddPrimalAspid(new Vector2(7.42f, 158.43f)));
                    if (Settings.AspidQuantity > 4) managedAspids.Add(AddPrimalAspid(new Vector2(19.37f, 158.27f)));
        */

        public CustomAspidPlacement()
        {
            AspidLocations = new Dictionary<string, (bool isManaged, List<(float x, float y)>)>();
            exAsm = Assembly.GetExecutingAssembly();
            eaFolder = new FileInfo(exAsm.Location).Directory.FullName;

            if (!Directory.Exists($@"{eaFolder}\Aspid Placement"))
            {
                Directory.CreateDirectory($@"{eaFolder}\Aspid Placement");

                var wp18 = new AspidPlacementJson(new List<(float x, float y)>
                {
                    (284.52f, 17.7f),
                    (285.77f, 17.01f),
                    (283.19f, 16.96f),
                    (283.14f, 18.43f),
                    (285.81f, 18.46f)
                }, true);

                var wp17 = new AspidPlacementJson(new List<(float x, float y)>
                {
                    (75.74f, 4.28f),
                    (77.06f, 3.35f),
                    (74.84f, 3.25f),
                    (74.68f, 5.19f),
                    (77.08f, 5.12f)
                }, true);

                var wp19 = new AspidPlacementJson(new List<(float x, float y)>
                {
                    (109.76f, 39.11f),
                    (111.23f, 38.03f),
                    (108.1f, 38.04f),
                    (111.07f, 40.31f),
                    (108.18f, 40.27f)
                }, true);

                var wp20 = new AspidPlacementJson(new List<(float x, float y)>
                {
                    (12.68f, 162.07f),
                    (9.94f, 161.87f),
                    (7.87f, 160.25f),
                    (7.42f, 158.43f),
                    (19.37f, 158.27f)
                }, true);

                File.WriteAllText($@"{eaFolder}\Aspid Placement\White_Palace_18.json", JsonConvert.SerializeObject(wp18));
                File.WriteAllText($@"{eaFolder}\Aspid Placement\White_Palace_17.json", JsonConvert.SerializeObject(wp17));
                File.WriteAllText($@"{eaFolder}\Aspid Placement\White_Palace_19.json", JsonConvert.SerializeObject(wp19));
                File.WriteAllText($@"{eaFolder}\Aspid Placement\White_Palace_20.json", JsonConvert.SerializeObject(wp20));
            }

            IEnumerable<string> s_aspidpfiles = Directory.EnumerateFiles($@"{eaFolder}\Aspid Placement", "*.json", SearchOption.AllDirectories);
            List<FileInfo> aspidpfiles = new List<FileInfo>();
            foreach (string file in s_aspidpfiles)
            {
                aspidpfiles.Add(new FileInfo(file));
            }

            foreach (FileInfo file in aspidpfiles)
            {
                AspidPlacementJson aspidJson = JsonConvert.DeserializeObject<AspidPlacementJson>(File.ReadAllText(file.FullName));
                if (aspidJson.Disable) continue;

                AspidLocations.Add(file.Name.Split('.')[0], (aspidJson.TeleportIfTooFarAway, aspidJson.AspidList));
            }
        }
    }

    public class AspidPlacementJson
    {
        public AspidPlacementJson(List<(float x, float y)> aspids, bool isManaged)
        {
            TeleportIfTooFarAway = isManaged;
            AspidList = aspids;
        }

        public bool Disable = false;
        public bool TeleportIfTooFarAway;
        public List<(float x, float y)> AspidList;
    }
}
