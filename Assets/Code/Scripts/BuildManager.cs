using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;
    [SerializeField] private Hero[] heroes;

    [Header("Built Turrets")]
    public List<Turret> builtTurrets;

    private int selectedTower;
    private int selectedHero;

    private void Awake()
    {
        main = this;
    }
    public Hero GetSelectedHero()
    {
        return heroes[selectedHero];
    }

    public Tower GetSelectedTower()
    {
        return towers[selectedTower];
    }
    public void SetSelectedHero(int _selectedTower)
    {
        selectedHero = _selectedTower;
    }
    public void SetSelectedTower(int _selectedTower)
    {
        selectedTower = _selectedTower;
    }
}
