using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Inst
    {
        get
        {
            return _inst;
        }
    }
    private static GameController _inst;

    [SerializeField]
    private List<CMaterial> materials = new List<CMaterial>();
    [SerializeField]
    private List<Item> items = new List<Item>();
    public PlayerController player;

    [SerializeField]
    private float boatSpeed = 1f;
    [SerializeField]
    private float boatSpeedMax = 1f;
    [SerializeField]
    private float boatHealth;
    [SerializeField]
    private float boatHealthMax;
    [SerializeField]
    private float losingBoatHealthSpeed;
    private float distanceRemaining;
    [SerializeField]
    private float distanceTotal;

    private float counter;

    private float timeToNextDisaster;
    [SerializeField]
    private float minTimeToDisaster;
    [SerializeField]
    private float maxTimeToDisaster;

    [SerializeField]
    private RectTransform uiBoat;
    [SerializeField]
    private RectTransform uiBoatLiquid;
    [SerializeField]
    private RectTransform uiPlayerLiquid;
    [SerializeField]
    private EndGameController endGameController;

    private BackgroundController bgController;

    public bool gameFinished = false;

    void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;
        timeToNextDisaster = NewTimeToDisaster();
        if (player == null) player = FindObjectOfType<PlayerController>();
        //DontDestroyOnLoad(this.gameObject); //no destruye el objeto entre escenas
    }

    private void Start()
    {
        uiBoat.localPosition = new Vector3(15f, 0f, 0f);
        distanceRemaining = distanceTotal;
        bgController = FindObjectOfType<BackgroundController>();
    }

    private float NewTimeToDisaster()
    {
        return UnityEngine.Random.Range(minTimeToDisaster, maxTimeToDisaster);
    }

    public int GetMaterialsSize()
    {
        return materials.Count;
    }

    public List<CMaterial> GetMaterials()
    {
        return materials;
    }

    public void PushMaterials(CMaterial material)
    {
        player.AddMaterialToHands(material, materials.Count);
        materials.Add(material);
        //Debug.Log("Agregado con exito");
    }

    public void ThrowMaterials()
    {
        materials.Clear();
        player.ThrowMaterialsFromHands();
    }

    public void setPlayer(PlayerController newplayer)
    {
        player = newplayer;
    }

    public PlayerController getPlayer()
    {
        return player;
    }

    public bool findMaterial(int matType)
    {
        CMaterial found = materials.Find(x => x.getMaterialType() == matType);

        return (found != null);
    }

    public float getBoatSpeed()
    {
        return boatSpeed;
    }

    public void setBoatSpeed(float newSpeed)
    {
        boatSpeed = newSpeed;
    }


    public float getBoatSpeedMax()
    {
        return boatSpeedMax;
    }

    public void setBoatSpeedMax(float newSpeed)
    {
        boatSpeedMax = newSpeed;
    }

    public float getBoatHealth()
    {
        return boatHealth;
    }

    public void setBoatHealth(float newHealth)
    {
        boatHealth = newHealth;
        float top = -33 * (1 - boatHealth / boatHealthMax);
        uiBoatLiquid.offsetMax = new Vector2(uiBoatLiquid.offsetMax.x, top);
    }

    public void setBottle()
    {
        float newFatigue = player.getFatigue();
        float top = -100 * (1 - newFatigue / player.getMaxFatigue());
        uiPlayerLiquid.offsetMax = new Vector2(uiPlayerLiquid.offsetMax.x, top);
    }


    void Update()
    {
        counter += Time.deltaTime;

        //el 10 es un numero ajustable
        if (counter > 10)
        {
            counter = 1f;
        }

        if (timeToNextDisaster >= 0)
        {
            timeToNextDisaster -= Time.deltaTime;
        } else
        {
            timeToNextDisaster = NewTimeToDisaster();
            DisasterIsHere();
        }

        if (boatSpeed > 0f)
        {
            //bgController.SetSpeed(-boatSpeed * 8f);
            distanceRemaining -= boatSpeed * Time.deltaTime;
            uiBoat.localPosition = new Vector3(-285f + 570f * ((distanceTotal - distanceRemaining) / distanceTotal), 0f, 0f);
            if (distanceRemaining <= 0f)
            {
                boatSpeed = 0f;
                gameFinished = true;
                endGameController.WinGame();
            }
        }
        if (boatHealth <= 0f)
        {
            boatSpeed = 0f;
            player.SetState(PlayerController.STATE_LOSE);
            gameFinished = true;
            endGameController.LoseGame();
        }
        items.ForEach(i => { if (i.IsBroken()) setBoatHealth(boatHealth - Time.deltaTime * losingBoatHealthSpeed); });

        setBottle();
    }

    private void DisasterIsHere()
    {
        Item disasterItem = items[UnityEngine.Random.Range(0, items.Count)];
        disasterItem.Broke();

        player.SendAlert(disasterItem.transform.position);
    }

    public void removeItemFromList(int matType)
    {
        CMaterial found = materials.Find(x => x.getMaterialType() == matType);
        if (found != null)
        {
            materials.Remove(found);
        }

    }

}
