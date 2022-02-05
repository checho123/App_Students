using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StudentData
{
    public int id;
    public string firstName;
    public string lastName;
    public string email;
    public string status;
    public string material;
    [Range(0f, 5f)]
    public float note;
    [Range (0,2)]
    public int score;
}

[System.Serializable]
public class ListStudents
{
    public List<StudentData> students;
}


