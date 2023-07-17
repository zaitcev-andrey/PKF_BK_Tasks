using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    #region private Members
    [SerializeField] private GameObject _openedInterface;
    [SerializeField] private GameObject _closedInterface;

    [SerializeField] private Transform _panelForElementsTransform;

    [SerializeField] private GameObject _listCellPrefab;
    [SerializeField] private GameObject _checkboxPrefab;
    [SerializeField] private GameObject _eyePrefab;

    [SerializeField] private GameObject _globalCheckbox;
    [SerializeField] private GameObject _globalEye;    

    [SerializeField] private Sprite _turnOnCheckboxSprite;
    [SerializeField] private Sprite _turnOffCheckboxSprite;
    [SerializeField] private Sprite _turnOnEyeSprite;
    [SerializeField] private Sprite _turnOffEyeSprite;
   
    private List<GameObject> _allObjectsOnScene;
    private int countOfElements;
    private int _turnOnEyesCounter;
    private int _turnOnCheckboxexCounter;
    #endregion

    #region public Members
    public List<CellComponents> AllObjectsInInterface;
    public bool IsGlobalCheckboxesChange;
    public bool IsGlobalEyesChange;
    #endregion

    #region private Methods
    private void Start()
    {
        _openedInterface.SetActive(true);
        _closedInterface.SetActive(false);

        // добавляем в список интерфейса все объекты с тегом "ForInterface" на сцене
        _allObjectsOnScene = new List<GameObject>(GameObject.FindGameObjectsWithTag("ForInterface"));
        AllObjectsInInterface = new List<CellComponents>();

        IsGlobalCheckboxesChange = false;
        IsGlobalEyesChange = false;
        countOfElements = _allObjectsOnScene.Count;
        _turnOnEyesCounter = countOfElements;
        _turnOnCheckboxexCounter = 0;

        for (int i = 0; i < countOfElements; i++)
        {
            AddElementOfListToInterface(i);
        }       
    }

    private void Update()
    {
        ApplyChanges();
    }

    private void AddElementOfListToInterface(int index)
    {
        GameObject cell = Instantiate(_listCellPrefab, _panelForElementsTransform);
        GameObject checkBox = Instantiate(_checkboxPrefab, cell.transform);
        GameObject eye = Instantiate(_eyePrefab, cell.transform);

        // обязательно создаём копию, так как метод принимает значение по ссылке. Передавая i, мы всегда
        // бы получали в качестве индекса размер нашего массива, что было бы ошибкой
        int copy_i = index;
        checkBox.GetComponent<Button>().onClick.AddListener(() => CellCheckbox.SwitchLocalCheckboxOnClick(copy_i));
        eye.GetComponent<Button>().onClick.AddListener(() => CellEye.SwitchLocalEyeOnClick(copy_i));

        AllObjectsInInterface.Add(new CellComponents(checkBox, eye));
    }

    /// <summary>
    /// В этом методе происходит обновление спрайтов кнопок и видимости объектов на сцене
    /// </summary>
    private void ApplyChanges()
    {
        // в случае глобального обновления чекбоксов
        if (IsGlobalCheckboxesChange)
        {
            if (_globalCheckbox.GetComponent<Image>().sprite == _turnOnCheckboxSprite)
            {
                _globalCheckbox.GetComponent<Image>().sprite = _turnOffCheckboxSprite;
                _turnOnCheckboxexCounter = 0;
                for (int i = 0; i < AllObjectsInInterface.Count; i++)
                {
                    CellComponents obj = AllObjectsInInterface[i];
                    obj.CheckboxPrefab.GetComponent<Image>().sprite = _turnOffCheckboxSprite;
                    obj.IsCheckBoxTurnOn = false;
                }
            }
            else
            {
                _globalCheckbox.GetComponent<Image>().sprite = _turnOnCheckboxSprite;
                _turnOnCheckboxexCounter = countOfElements;
                for (int i = 0; i < AllObjectsInInterface.Count; i++)
                {
                    CellComponents obj = AllObjectsInInterface[i];
                    obj.CheckboxPrefab.GetComponent<Image>().sprite = _turnOnCheckboxSprite;
                    obj.IsCheckBoxTurnOn = true;
                }
            }

            IsGlobalCheckboxesChange = false;
        }

        // в случае глобального обновления глазиков
        else if (IsGlobalEyesChange)
        {
            if (_globalEye.GetComponent<Image>().sprite == _turnOnEyeSprite)
            {
                _globalEye.GetComponent<Image>().sprite = _turnOffEyeSprite;
                _turnOnEyesCounter = 0;
                for (int i = 0; i < AllObjectsInInterface.Count; i++)
                {
                    AllObjectsInInterface[i].EyePrefab.GetComponent<Image>().sprite = _turnOffEyeSprite;
                    _allObjectsOnScene[i].SetActive(false);
                }
            }
            else
            {
                _globalEye.GetComponent<Image>().sprite = _turnOnEyeSprite;
                _turnOnEyesCounter = countOfElements;
                for (int i = 0; i < AllObjectsInInterface.Count; i++)
                {
                    AllObjectsInInterface[i].EyePrefab.GetComponent<Image>().sprite = _turnOnEyeSprite;
                    _allObjectsOnScene[i].SetActive(true);
                }
            }

            IsGlobalEyesChange = false;
        }

        // в случае локального обновления чего-либо
        for (int i = 0; i < AllObjectsInInterface.Count; i++)
        {
            CellComponents obj = AllObjectsInInterface[i];

            if (obj.IsChange)
            {
                if (obj.IsCheckBoxChange)
                {
                    if (obj.CheckboxPrefab.GetComponent<Image>().sprite == _turnOnCheckboxSprite)
                    {
                        obj.CheckboxPrefab.GetComponent<Image>().sprite = _turnOffCheckboxSprite;
                        obj.IsCheckBoxTurnOn = false;

                        _turnOnCheckboxexCounter--;
                        if (_globalCheckbox.GetComponent<Image>().sprite != _turnOffCheckboxSprite)
                            _globalCheckbox.GetComponent<Image>().sprite = _turnOffCheckboxSprite;
                    }
                    else
                    {
                        obj.CheckboxPrefab.GetComponent<Image>().sprite = _turnOnCheckboxSprite;
                        obj.IsCheckBoxTurnOn = true;

                        _turnOnCheckboxexCounter++;
                        if (_turnOnCheckboxexCounter == countOfElements)
                            _globalCheckbox.GetComponent<Image>().sprite = _turnOnCheckboxSprite;
                    }

                    obj.IsCheckBoxChange = false;
                }
                if (obj.IsEyeChange)
                {
                    if (obj.EyePrefab.GetComponent<Image>().sprite == _turnOnEyeSprite)
                    {
                        obj.EyePrefab.GetComponent<Image>().sprite = _turnOffEyeSprite;
                        _allObjectsOnScene[i].SetActive(false);

                        _turnOnEyesCounter--;
                        if (_globalEye.GetComponent<Image>().sprite != _turnOffEyeSprite)
                            _globalEye.GetComponent<Image>().sprite = _turnOffEyeSprite;
                    }
                    else
                    {
                        obj.EyePrefab.GetComponent<Image>().sprite = _turnOnEyeSprite;
                        _allObjectsOnScene[i].SetActive(true);

                        _turnOnEyesCounter++;
                        if (_turnOnEyesCounter == countOfElements)
                            _globalEye.GetComponent<Image>().sprite = _turnOnEyeSprite;
                    }

                    obj.IsEyeChange = false;
                }

                obj.IsChange = false;
            }

            // здесь меняем прозрачность для объектов на сцене
            if (obj.IsCheckBoxTurnOn)
            {
                if (obj.CurrentOpacity != obj.NewOpacity)
                {
                    obj.CurrentOpacity = obj.NewOpacity;

                    MeshRenderer mr = _allObjectsOnScene[i].GetComponent<MeshRenderer>();
                    Color col = mr.material.color;
                    col.a = (float)obj.CurrentOpacity / 100;
                    mr.material.color = col;
                }
            }
        }
    }
    #endregion

    #region public Methods
    public void CloseInterfaceButton()
    {
        _openedInterface?.SetActive(false);
        _closedInterface?.SetActive(true);
    }

    public void OpenInterfaceButton()
    {
        _openedInterface?.SetActive(true);
        _closedInterface?.SetActive(false);
    }

    public void AddNewObjectToList(GameObject obj)
    {        
        obj.tag = "ForInterface";
        _allObjectsOnScene.Add(obj);        

        AddElementOfListToInterface(countOfElements++);
    }
    #endregion

    #region Inner classes
    // Класс, хранящий все внутренние префабы ячейки из списка элементов интерфейса
    public class CellComponents
    {
        public GameObject CheckboxPrefab;
        public GameObject EyePrefab;

        public bool IsCheckBoxChange = false;
        public bool IsEyeChange = false;
        public bool IsChange = false;

        public bool IsCheckBoxTurnOn = false;
        public int CurrentOpacity = 100;
        public int NewOpacity = 100;

        public CellComponents(GameObject checkboxPrefab, GameObject eyePrefab)
        {
            CheckboxPrefab = checkboxPrefab;
            EyePrefab = eyePrefab;
        }
    }
    #endregion
}
