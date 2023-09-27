using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabInputField : MonoBehaviour
{
    public InputField[] InputField;         //TAB Ű�� �̵��� InputField �迭

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InputField currentInputField = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<InputField>();

            if(currentInputField != null)
            {
                int currentIndex = System.Array.IndexOf(InputField, currentInputField);
                int nextIndex = (currentIndex + 1) % InputField.Length; //���� InputField �ε��� ���

                //���� Input�� ��Ŀ�� �̵�
                InputField[nextIndex].Select();
                InputField[nextIndex].ActivateInputField();
            }
        }
    }
}
