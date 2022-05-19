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

    [SerializeField]
    private GameObject go_BaseUI; // BaseUI
    [SerializeField]
    private CraftBox[] craft_Box;
    private GameObject go_Preview; //prefab preview
    [SerializeField]
    private Transform tf_Player;

    public void SlotClick(int _slotNumber)
    {
      go_Preview = Instantiate(craft_Box[_slotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);

      isPreviewActivated = true;
      go_BaseUI.SetActive(false);
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Window();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }

    private void Cancel()
    {
       if(isPreviewActivated) 
        Destroy(go_Preview);

       isActivated = false;
       isPreviewActivated = false;
       go_Preview = null;

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
