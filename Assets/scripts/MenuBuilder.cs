using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class MenuBuilder : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        Dropdown mazeDropdown = GameObject.Find("MazeDropdown").GetComponent<Dropdown>();

        DirectoryInfo levelDirectoryPath = new DirectoryInfo(Application.dataPath + "/Resources/levels");

        FileInfo[] fileInfo = levelDirectoryPath.GetFiles("*.*", SearchOption.AllDirectories);

        foreach (FileInfo file in fileInfo)
        {

            if (file.Extension == ".txt")
            {

                Debug.Log(file.Name);

                string filename = file.Name.Substring(0, file.Name.Length - 4);

                Debug.Log(filename);

                Dropdown.OptionData list = new Dropdown.OptionData(filename);

                mazeDropdown.options.Add(list);

                
            }

        }

        int TempInt = mazeDropdown.value;
        mazeDropdown.value = mazeDropdown.value + 1;
        mazeDropdown.value = TempInt;


    }

}
