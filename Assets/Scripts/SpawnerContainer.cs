using UnityEngine;
using UnityEngine.UI;

public class SpawnerContainer : MonoBehaviour
{
    [SerializeField] SpawnerItem _prefab = null;

    string[] _packs = new string[]
    {
        "Ammo_01",
        "Armor_01",
        "Artefacts_01",
        "Backpacks_01",
        "Backpacks_02",
        "Devices_01",
        "DrinksAndSmokes_01",
        "Explosives_01",
        "Food_01",
        "MedsAndDrugs_01",
        "MedsAndDrugs_02",
        "Melee_01",
        "Misc_01",
        "MutantsMeat_01",
        "Notes_01",
        "Parts_01",
        "Pistols_01",
        "Quest_01",
        "Repairs_01",
        "Rifles_01",
        "Shotguns_01",
        "SMGs_01",
        "Snipers_01",
        "Tools_01",
        "Upgrades_01"
    };

    void Awake()
    {
        foreach (var pack in _packs)
        {
            var packArray = Resources.LoadAll<Sprite>("Sprites/" + pack);
            foreach (var item in packArray)
            {
                var inst = Instantiate(_prefab, transform);
                inst.GetComponent<Image>().sprite = item;
            }
        }
    }
}
