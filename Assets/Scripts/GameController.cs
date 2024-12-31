using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Info")]
    public int attempts = 0;
    public int totalTexts;

    [Header("Game Objects")]
    public GameObject textbox1Prefab;
    public GameObject textbox2Prefab;

    [Header("UI Groups")]
    public Transform group1Container;
    public Transform group2Container;
    
    [Header("Matched Animation")]
    public GameObject matchedPanel;
    public TMP_Text matchedText;
    public GameObject matchedAnimation;

    [Header("End Game")]
    public GameObject endGamePanel;

    [Header("Debug")]
    public bool groupsLoaded = false;

    public TextboxController textbox1;
    public TextboxController textbox2;
    
    // Estructuras de datos propuestas: Elijo usar listas por la facilidad y simplicidad en su uso con C#.
    // Estas estructuras me permiten jugar con sus índices con lo que puedo ordenarlos y emparejarlos facilmente
    // y ademas es una estructura que puedo usar para mostrarlos facilmente con los elementos de UI de Unity.
    public List<string> group1 = new();
    public List<string> group2 = new();

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(WaitLoadFirstGroup());
        group1 = new()
        {
            "Usar contraseñas fuertes y únicas",
            "Mantener el software actualizado",
            "No hacer clic en enlaces sospechosos",
            "Habilitar la autenticación de dos factores",
            "Realizar copias de seguridad regularmente",
        };

        group2 = new()
        {
            "ayuda a proteger tus cuentas de accesos no autorizados.",
            "reduce el riesgo de vulnerabilidades explotables.",
            "previene ataques de phishing y malware.",
            "añade una capa extra de seguridad a tus cuentas.",
            "asegura que no pierdas datos importantes en caso de un ataque.",
        };

        var shuffledGroup1 = Shuffle(group1);
        var shuffledGroup2 = Shuffle(group2);

        totalTexts = group1.Count;

        for (int i = 0; i < group1.Count; i++)
        {
            CreateTextbox(1, shuffledGroup1[i]);
        }
        

        for (int i = 0; i < group2.Count; i++)
        {
            CreateTextbox(2, shuffledGroup2[i]);
        }

        groupsLoaded = true;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private IEnumerator WaitLoadFirstGroup()
    {
        yield return new WaitUntil(() => groupsLoaded);
        foreach (Transform child in group1Container)
        {
            child.gameObject.GetComponent<Collider>().enabled = true;
        }

        foreach (Transform child in group2Container)
        {
            child.gameObject.GetComponent<Collider>().enabled = true;
        }
    }

    public List<int> Shuffle(List<string> group) 
    {
        var random = new System.Random();
        List<int> shuffled = new();
        
        for (int i = 0; i < group.Count; i++)
        {
            shuffled.Add(i);
        }

        for(var i = shuffled.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        Debug.Log(string.Join(", ", shuffled));

        return shuffled;
    }

    public bool ConfirmMatch(TextboxController textbox1, TextboxController textbox2)
    {
        attempts++;
        if(textbox1.index == textbox2.index)
        {
            totalTexts--;
            StartMatchedAnimation(textbox1, textbox2);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CreateTextbox(int group, int i)
    {
        var textbox = Instantiate(group == 1 ? textbox1Prefab : textbox2Prefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TextboxController>();
        textbox.Initialize(i);
        textbox.transform.SetParent(group == 1 ? group1Container : group2Container, false);
    }

    public void StartMatchedAnimation(TextboxController textbox1, TextboxController textbox2)
    {
        matchedPanel.SetActive(true);
        matchedText.text = $"{textbox1.text.text}\n{textbox2.text.text}";
        matchedAnimation.SetActive(true);

        StartCoroutine(EndMatchedAnimation(textbox1, textbox2));
    }

    IEnumerator EndMatchedAnimation(TextboxController textbox1, TextboxController textbox2)
    {
        foreach (Transform child in group1Container)
        {
            child.gameObject.GetComponent<Draggable>().enabled = false;
        }

        yield return new WaitForSeconds(2f);
        matchedPanel.SetActive(false);
        matchedAnimation.SetActive(false);

        // Esconder los textos
        textbox1.gameObject.SetActive(false);
        textbox2.gameObject.SetActive(false);

        foreach (Transform child in group1Container)
        {
            child.gameObject.GetComponent<Draggable>().enabled = true;
        }

        if(totalTexts == 0)
        {
            endGamePanel.SetActive(true);
            endGamePanel.GetComponentInChildren<TMP_Text>().text = $"¡Felicidades!\n\nHas logrado aumentar tus conocimientos sobre seguridad informática.\n\nHas completado el juego en {attempts} intentos.";
            matchedAnimation.SetActive(true);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
