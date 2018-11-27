using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    //The buttons need to be attached in the inspector
    public Button btn_ReturnToMap;
    public MoveTo moveTo;

    private void Start()
    {
        btn_ReturnToMap.onClick.AddListener(ReturnToMap);
    }

    void ReturnToMap()
    {
        moveTo.ReturnToDefaultState();
    }

}
