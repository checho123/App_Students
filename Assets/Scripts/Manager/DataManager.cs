using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    //path rute file
    private string path = "/StreamingAssets/database.json";
    private string pathFile;

    [SerializeField, Header("Data students for students")]
    private StudentData studentData;
    [SerializeField]
    private ListStudents listStudents = new ListStudents();
    
    [Header("Last Id Save")]
    [SerializeField]
    private int lastID;
    public ListStudents GetData { get { return listStudents; } }
    public StudentData student { get { return studentData; } set { studentData = value; } }
    

    void Awake()
    {
        pathFile = Application.dataPath + path;
        LoadFile();
    }

    public void LoadFile()
    {
        if (File.Exists(pathFile))
        {
            string json =File.ReadAllText(pathFile);
            listStudents = JsonUtility.FromJson<ListStudents>(json);
            LoadLocal();
        }
    }

    public void SaveFile()
    {
        if (File.Exists(pathFile))
        {
            string json = JsonUtility.ToJson(listStudents);
            File.WriteAllText(pathFile, json);
        }
    }

    [ContextMenu("Delete User")]
    public void DeleteFile(int index)
    {
        try
        {
            var studentDelete = listStudents.students[index];
            Debug.Log(studentDelete);
            listStudents.students.Remove(studentDelete);
            SaveFile();
        }
        catch
        {
            Debug.Log("No existe este estudiante"); 
        }  
    }

    [ContextMenu("Create New Student Data")]
    public void CreateNewStudent()
    {
        studentData = new StudentData
        {
            id = lastID + 1,
            firstName = studentData.firstName,
            lastName = studentData.lastName,
            email = studentData.email,
            status = studentData.status,
            material = studentData.material,
            note = studentData.note,
            score = studentData.score,
        };
        listStudents.students.Add(studentData);
        SaveFile();
        SaveLocal();
    }

    public void UpdateStudent(int index)
    {
        try
        {
            StudentData update = new StudentData
            {
                id = studentData.id,
                firstName = studentData.firstName,
                lastName = studentData.lastName,
                email = studentData.email,
                status = studentData.status,
                material = studentData.material,
                note = studentData.note,
                score= studentData.score,
            };
            listStudents.students[index] = update;
            SaveFile();
        }
        catch
        {
            Debug.Log("Ese estudiante no exite en la lista");
        }
    }

    #region Local Variables
    [ContextMenu("Save Varables Locales")]
    public void SaveLocal()
    {
        for (int i = 0; i < listStudents.students.Count; i++)
        {
            lastID = listStudents.students[i].id;
            PlayerPrefs.SetInt("Last_ID", lastID);
        }
    }

    public void LoadLocal()
    {
        lastID = PlayerPrefs.GetInt("Last_ID", 1);
    }

    #endregion
    
}
