using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Lucky
{

    public class Prefabs
    {

        public const string BuyTicketPopup = "Prefabs/BuyTicketPopup";
        public const string BuyTickets = "Prefabs/BuyTickets";
        public const string Calendar = "Prefabs/Calendar";
        public const string LocationPanel = "Prefabs/LocationPanel";
        public const string Maps = "Prefabs/Maps";
        public const string SelectTrain = "Prefabs/SelectTrain";
        public const string InfoPanel = "Prefabs/InfoPanel";
        public const string NoteBook = "Prefabs/NoteBook";


        public const string MessageItem = "Prefabs/MessageItem";
        public const string TrainItem = "Prefabs/TrainItem";
        public const string CityItem = "Prefabs/CityItem";
        public const string AccidentItem = "Prefabs/AccidentItem";
        public const string BuyTrainItem = "Prefabs/BuyTrainItem";
    }

    public class PrefabManager : BaseInstance<PrefabManager>
    {


        private Dictionary<string, GameObject> prefabsdict = new Dictionary<string, GameObject>();

        public GameObject GetPrefabs(string name)
        {
            if(prefabsdict.ContainsKey(name))
            {
                return prefabsdict[name];
            }
            return null;
        }

        public IEnumerator Init(string mainscene)
        {
            yield return null;
            string[] s =
            {
                Prefabs.BuyTicketPopup,
                Prefabs.BuyTickets,
                Prefabs.Calendar,
                Prefabs.LocationPanel,
                Prefabs.Maps,
                Prefabs.SelectTrain,
                Prefabs.InfoPanel,
                Prefabs.NoteBook
            };

            foreach(string prefab in s)
            {
                ResourceRequest rr = Resources.LoadAsync(prefab);
                yield return rr;
                if (rr.asset != null)
                {
                    prefabsdict.Add(prefab, (GameObject)rr.asset);
                    Debug.Log("load " + prefab);
                }
                else
                    Debug.Log(prefab + "load failed");
            }
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(mainscene);
        }

        
    }
}

