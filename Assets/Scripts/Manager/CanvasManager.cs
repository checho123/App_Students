using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class CanvasManager : MonoBehaviour
{
    [Header("Template the Info")]
    [SerializeField]
    private GameObject templeteInfo;
    [SerializeField]
    private Transform content;

    [Header("Refrents Data Students")]
    [SerializeField]
    private ListStudents listStudent;
    private DataManager dataManager;
    [SerializeField, Range(0f,5f)]
    private List<float> listApproved;

    [Header("Update Json")]
    [SerializeField]
    private StudentData studentEdit;
    [SerializeField]
    private GameObject panelEdit;
    [SerializeField]
    private TMP_InputField[] inputs;
    [SerializeField]
    private TMP_Text noteText, idText, textScore;
    [SerializeField]
    private Slider sliderNote, sliderScore;
    private float currentNote;
    private int selectIndex, currentScore;
    private string statusCurrent;

    [Header("Create new Student")]
    [SerializeField]
    private TMP_InputField[] inputsCreate;
    [SerializeField]
    private TMP_Text noteInitialText;
    [SerializeField]
    private Slider notaInitial;
    [SerializeField]
    private bool filter;

    void Start()
    {
        dataManager = GameObject.FindGameObjectWithTag("Data").GetComponent<DataManager>();
        listStudent = dataManager.GetData;
        sliderNote.maxValue = 5;
        sliderScore.maxValue = 2;
        int count = listStudent.students.Count;
        ShowDataStudent(count);
        ScoreStudent();
        Score();
    }

    private void FixedUpdate()
    {
        noteText.text = "Note: " + sliderNote.value + " /5.0";
        noteInitialText.text = "Note :" + notaInitial.value;
        Score();
        ScoreStudent();
    }

    private void Score()
    {
        switch (sliderScore.value)
        {
            case 0:
                textScore.text = "Aprovado";
                statusCurrent = textScore.text;
                break;
            case 1:
                textScore.text = "Sin Calificacion";
                statusCurrent = textScore.text;
                break;
            case 2:
                textScore.text = "Reprovado";
                statusCurrent = textScore.text;
                break;
        }
    }

    private void ScoreStudent()
    {
        foreach (var student in listStudent.students)
        {
            if (student.status == "Aprovado" || student.status  == "Reprovado")
            {
                Debug.Log("Pasa 1 filtro");
            }
        }
    }

    private void ShowDataStudent(int allStudent)
    {
        
        for (int i = 0; i < allStudent; i++)
        {
            GameObject info = Instantiate(templeteInfo, content);
            // Id the student
            info.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = listStudent.students[i].status +  " #" + listStudent.students[i].id.ToString();
            // Name and LastName
            info.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = listStudent.students[i].firstName + " "
                + listStudent.students[i].lastName;
            // Email the student
            info.transform.GetChild(1).GetChild(2).GetComponent<TMP_Text>().text = listStudent.students[i].email;
            // Materia the teacher
            info.transform.GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = "Materia " + listStudent.students[i].material;
            // Nota the teacher
            info.transform.GetChild(1).GetChild(4).GetComponent<TMP_Text>().text = "Nota " + listStudent.students[i].note.ToString() + " /0.5";
            // Slider Note Total
            info.transform.GetChild(2).GetComponent<Slider>().value = listStudent.students[i].note;
            // Get botton for profile edit
            info.transform.GetChild(3).GetComponent<Button>().AddEventListener(i, ItemClick);
        }

        Destroy(templeteInfo);
    }

    public void ExitStudent()
    {
        panelEdit.SetActive(false);
    }

    private void ItemClick (int index)
    {
        panelEdit.SetActive(true);
        selectIndex = index;

        //Id Student
        idText.text = "identificación #" + listStudent.students[index].id.ToString();
        //frist Name
        inputs[0].text = listStudent.students[index].firstName;
        //Last Name
        inputs[1].text = listStudent.students[index].lastName;
        //Email
        inputs[2].text = listStudent.students[index].email;
        //Material
        inputs[3].text = listStudent.students[index].material;
        //Notes
        currentNote = listStudent.students[index].note;
        sliderNote.value = currentNote;
        //score
        currentScore = listStudent.students[index].score;
        sliderScore.value = currentScore;
        //status
    }

    public void UpdateStudent()
    {
        studentEdit.id = listStudent.students[selectIndex].id;
        studentEdit.firstName = inputs[0].text;
        studentEdit.lastName = inputs[1].text;
        studentEdit.email = inputs[2].text;
        studentEdit.material = inputs[3].text;
        studentEdit.note = sliderNote.value;
        studentEdit.score = (int)sliderScore.value;
        studentEdit.status = statusCurrent;
        dataManager.student = studentEdit;
        dataManager.UpdateStudent(selectIndex);
        SceneManager.LoadScene(0);        
    }

    public void DeleteStudent()
    {
        dataManager.DeleteFile(selectIndex);
        SceneManager.LoadScene(0);
    }

    //Create new Students
    public void CreateNewStudents()
    {
        studentEdit = new StudentData();
        studentEdit.firstName = inputsCreate[0].text;
        studentEdit.lastName = inputsCreate[1].text;
        studentEdit.email = inputsCreate[2].text;
        studentEdit.material = inputsCreate[3].text;
        studentEdit.score = 1;
        studentEdit.note = notaInitial.value;
        studentEdit.status = "Sin Calificacion";
        dataManager.student = studentEdit;
        dataManager.CreateNewStudent();
        SceneManager.LoadScene(0);
    }

    public void ValideStudentScore()
    {
        if (filter)
        {
            Debug.Log("Pasa Todos los filtros");
        }
        else
        {
            Debug.Log("No pasa los filtros");
        }
    }
}


public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> Onclick) 
    {
        button.onClick.AddListener(delegate ()
        {
            Onclick(param);
        });      
    }
}