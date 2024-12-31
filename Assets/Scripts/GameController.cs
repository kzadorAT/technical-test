using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject textboxPrefab;

    public Transform group1Container;
    public Transform group2Container;

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

        DontDestroyOnLoad(gameObject);
    }

    public List<string> group1 = new();
    public List<string> group2 = new();

    private void Start()
    {
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

        for (int i = 0; i < group1.Count; i++)
        {
            CreateTextbox(1, shuffledGroup1[i]);
            CreateTextbox(2, shuffledGroup2[i]);
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

    public void CreateTextbox(int group, int i)
    {
        var textbox = Instantiate(textboxPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<TextboxController>();
        textbox.Initialize(group, i);
        textbox.transform.SetParent(group == 1 ? group1Container : group2Container, false);
    }
}
