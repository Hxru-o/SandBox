using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftBox
{
    public string craftName;
    public GameObject go_Prefab;
    public GameObject go_PreviewPrefab;
}
public class Craft : MonoBehaviour
{
    //bool
    private bool isActivated = false;
    private bool isPreviewActivated = false;

    //GameObject
    [SerializeField]
    private GameObject go_BaseUI; // BaseUI
    private GameObject go_Preview; //prefab preview
    [SerializeField]
    private GameObject go_prefab; // original prefab
    
    [SerializeField]
    private CraftBox[] craft_Box;
    
    [SerializeField]
    private Transform tf_Player;

    // raycast
    private RaycastHit hitinfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;

    public void SlotClick(int _slotNumber)
    {
      go_Preview = Instantiate(craft_Box[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
      go_prefab = craft_Box[_slotNumber].go_Prefab;

      isPreviewActivated = true;
      go_BaseUI.SetActive(false);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
        {
            Window();
        }

        if(isPreviewActivated)
         {
           PreviewPositionUpdate();
         }  
        
        if(Input.GetKeyDown(KeyCode.E))
        {
            Build();
        }
     
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    private void Build()
    {
        if(isPreviewActivated)
        {
            Instantiate(go_prefab, hitinfo.point, Quaternion.identity);
            Destroy(go_Preview);
            isActivated = false;
            isPreviewActivated = false;
            go_Preview = null;
            go_prefab = null;
        }
    }

    private void PreviewPositionUpdate()
    {
      if(Physics.Raycast(tf_Player.position, tf_Player.forward, out hitinfo, range, layerMask))
      {
          if(hitinfo.transform != null)
          {
              Vector3 _location = hitinfo.point;
              go_Preview.transform.position = _location;
          }
      }
    }


    private void Cancel()
    {
       if(isPreviewActivated) 
        Destroy(go_Preview);

       isActivated = false;
       isPreviewActivated = false;
       go_Preview = null;
       go_prefab = null;

       go_BaseUI.SetActive(false);
    }
    
    private void Window()
    {
        if(!isActivated)
           OpenWindow();
        else
           CloseWindow();
    }

    private void OpenWindow()
    {
      isActivated = true;
      go_BaseUI.SetActive(true);

    }

    private void CloseWindow()
    {
        isActivated = false;
        go_BaseUI.SetActive(false);
    }
}
