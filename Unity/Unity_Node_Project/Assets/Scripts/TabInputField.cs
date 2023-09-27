using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabInputField : MonoBehaviour
{
    public InputField[] InputField;         //TAB 키로 이동할 InputField 배열

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InputField currentInputField = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();

            if(currentInputField != null)
            {
                int currentIndex = System.Array.IndexOf(InputField, currentInputField);
                int nextIndex = (currentIndex + 1) % InputField.Length; //다음 InputField 인덱스 계산

                //다음 Input로 포커스 이동
                InputField[nextIndex].Select();
                InputField[nextIndex].ActivateInputField();
            }
        }
    }
}
