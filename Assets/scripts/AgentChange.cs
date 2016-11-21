using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AgentChange : MonoBehaviour {

    public Dropdown agentDropdown;
    public Dropdown optionsDropdown;
    public Text optionsLabel;
    public Dropdown optionsDropdown2;
    public Text optionsLabel2;

    public void setOptions()
    {

        int selected = agentDropdown.value;

        switch (selected)
        {

            case 0:
                optionsDropdown.gameObject.SetActive(false);
                optionsLabel.gameObject.SetActive(false);
                break;
            case 1:
                optionsDropdown.gameObject.SetActive(true);
                optionsLabel.gameObject.SetActive(true);
                Dropdown.OptionData list = new Dropdown.OptionData("Right");

                optionsDropdown.options.Add(list);
                Dropdown.OptionData list2 = new Dropdown.OptionData("Left");
                optionsDropdown.options.Add(list2);

                Dropdown.OptionData list3 = new Dropdown.OptionData("Down");
                optionsDropdown.options.Add(list3);

                Dropdown.OptionData list4 = new Dropdown.OptionData("Up");
                optionsDropdown.options.Add(list4);

                int TempInt = optionsDropdown.value;
                optionsDropdown.value = optionsDropdown.value + 1;
                optionsDropdown.value = TempInt;

                break;

            case 4:
                optionsDropdown.gameObject.SetActive(true);
                optionsLabel.gameObject.SetActive(true);

                optionsDropdown.options.Add(new Dropdown.OptionData("DFS"));
                optionsDropdown.options.Add(new Dropdown.OptionData("BFS"));
                optionsDropdown.options.Add(new Dropdown.OptionData("UCS"));
                optionsDropdown.options.Add(new Dropdown.OptionData("ASTAR"));

                TempInt = optionsDropdown.value;
                optionsDropdown.value = optionsDropdown.value + 1;
                optionsDropdown.value = TempInt;

                break;

            case 5:
                optionsDropdown.gameObject.SetActive(true);
                optionsLabel.gameObject.SetActive(true);

                optionsLabel.text = "Pacman Agent";

                optionsDropdown.options.Add(new Dropdown.OptionData("Reaction"));
                optionsDropdown.options.Add(new Dropdown.OptionData("MiniMax - 2"));
                optionsDropdown.options.Add(new Dropdown.OptionData("MiniMax AB - 2"));
                optionsDropdown.options.Add(new Dropdown.OptionData("MiniMax - 3"));
                optionsDropdown.options.Add(new Dropdown.OptionData("MiniMax Ab - 3"));
                optionsDropdown.options.Add(new Dropdown.OptionData("MiniMax - 4"));
                optionsDropdown.options.Add(new Dropdown.OptionData("MiniMax Ab - 4"));
                optionsDropdown.options.Add(new Dropdown.OptionData("ExpectiMax - 2"));
                optionsDropdown.options.Add(new Dropdown.OptionData("ExpectiMax - 3"));
                optionsDropdown.options.Add(new Dropdown.OptionData("ExpectiMax - 4"));
                optionsDropdown.options.Add(new Dropdown.OptionData("Human"));

                TempInt = optionsDropdown.value;
                optionsDropdown.value = optionsDropdown.value + 1;
                optionsDropdown.value = TempInt;

                break;

        }



    }


}
