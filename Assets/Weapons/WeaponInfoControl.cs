using System.IO;
using System.Collections.Generic;
using UnityEngine;

//adding a new WeaponType, you should also update the WeaponInfo maps
public enum WeaponTypeEnum { MONEY, MISSILE, DEFENSIVE, GUIDANCE }
//adding a new Weapon, you should also update the WeaponInfo maps
public enum WeaponEnum { MONEY, BABY_MISSILE, MIRV }

[System.Serializable]
public class WeaponInfo
{
    public WeaponEnum id;
    public WeaponTypeEnum type;
    public string description;
    public Sprite icon;
    public int price;
    public Rigidbody2D prefab;
}

public class WeaponInfoControl : MonoBehaviour
{
    private static WeaponInfoControl _instance;
    [SerializeField] private List<WeaponInfo> AllWeapons;

    /**
     * STATIC PART (semi-generated, by hand)
     */
    private static Dictionary<WeaponTypeEnum, List<WeaponEnum>> TypeToWeapons = new Dictionary<WeaponTypeEnum, List<WeaponEnum>>();
    private static Dictionary<WeaponEnum, WeaponInfo> WeaponToInfo = new Dictionary<WeaponEnum, WeaponInfo>();

    private void OnEnable()
    {
        if (_instance == null)
        {
            lock (typeof(WeaponInfoControl))
            {
                if (_instance == null)
                {
                    _instance = this;
                    DontDestroyOnLoad(this.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }

            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start ()
    {
        LoadAllWeapons();
        foreach (var weapon in AllWeapons)
        {
            if (!TypeToWeapons.ContainsKey(weapon.type)) TypeToWeapons.Add(weapon.type, new List<WeaponEnum>(AllWeapons.Count));
            TypeToWeapons[weapon.type].Add(weapon.id);

            WeaponToInfo.Add(weapon.id, weapon);
        }
    }

    private void LoadAllWeapons()
    {
        //AllWeapons = new List<WeaponInfo>(100);
        string prefablist = File.ReadAllText(Application.streamingAssetsPath + "/WeaponList.txt", System.Text.Encoding.UTF8);
        string[] prefabFiles = prefablist.Split("\r\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
        Debug.Log("Found " + prefabFiles.Length + " files:");
        foreach (var file in prefabFiles)
        {
            if (file.StartsWith("#")) continue; //ignore comments
            string resname = Path.GetFileNameWithoutExtension(file);
            Rigidbody2D w = Resources.Load<Rigidbody2D>(resname);
            WeaponSelfDescription desc = w.GetComponent<WeaponSelfDescription>();
            if (desc == null)
            {
                Debug.LogError("No description found for " + w.gameObject.name + " in file " + file);
                continue;
            }
            WeaponInfo i = new WeaponInfo
            {
                id = desc.Id,
                type = desc.Type,
                description = desc.Description,
                price = desc.Price,
                icon = desc.Icon,
                prefab = w
            };
            AllWeapons.Add(i);
        }

    }
    public static IEnumerable<WeaponEnum> WeaponsOfType(WeaponTypeEnum t)
    {
        return TypeToWeapons[t];
    }

    public static WeaponInfo GetInfo(WeaponEnum w)
    {
        return WeaponToInfo[w];
    }
}
