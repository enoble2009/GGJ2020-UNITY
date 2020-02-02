using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMaterialSource : MonoBehaviour
{

    // cambiar los tipos para un item fisico
    public const int TYPE_WOOD = 0;
    public const int TYPE_WATER = 1;
    public const int TYPE_ROPE = 2;
    public const int TYPE_CLOTH = 3;
    public const int TYPE_AMMO = 4;
    public const int TYPE_POWDER = 5;
    public const int TYPE_MEDICINE = 6;
    public const int TYPE_RUM = 7;
    public const int TYPE_NAIL = 8;

    public int type;

    private bool stayOnThis = false;

    [SerializeField]
    private GameObject materialTemplate;

    


    // Segun el type carga un sprite

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("aparecio  " + type);
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log(GameController.Inst.GetMaterialsSize());

        if ((GameController.Inst.getPlayer().GetSubState() == PlayerController.SUBSTATE_ON_MATERIAL))
            //if ((GameController.Inst.getPlayer().GetSubState() == PlayerController.SUBSTATE_ON_MATERIAL) && (!(GameController.Inst.getPlayer().isCollecting())))
        {
            if (Input.GetKeyUp(KeyCode.E) && stayOnThis)
            {
                if ((!(GameController.Inst.findMaterial(type))) && (GameController.Inst.GetMaterialsSize() < 2))
                {
                    GameController.Inst.getPlayer().setCollecting(true);
                    //Debug.Log("agarraste  " + type);
                    takeMaterial();
                }
            }
        }
    }

    private void OnTriggereEnter2D(Collider2D col)
    {

        if (col.CompareTag("Player") )
        {
            col.GetComponent<PlayerController>().SetSubState(PlayerController.SUBSTATE_ON_MATERIAL);
            stayOnThis = true;
        }


        
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        
        if (col.CompareTag("Player"))
        {
            
            col.GetComponent<PlayerController>().SetSubState(PlayerController.SUBSTATE_ON_MATERIAL);
            stayOnThis = true;



        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<PlayerController>().SetSubState(PlayerController.SUBSTATE_OFF_MATERIAL);
            stayOnThis = false;
        }
    }


    public void takeMaterial()
    {
        // Cargar un material dependiendo del tipo, ej se agarra bandera se carga tela a la lista

        AudioManager.Inst.Play("agarrar_item");

        GameObject matObject = Instantiate<GameObject>(materialTemplate, GameController.Inst.player.transform);
        //Debug.Log(matObject);
        CMaterial mat = matObject.GetComponent<CMaterial>();
        
        mat.setMaterialType(type);


        GameController.Inst.PushMaterials(mat);

        //Destroy(this.gameObject);
    }

}
