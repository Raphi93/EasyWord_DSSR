using CsvHelper;
using CsvHelper.Configuration;
using EasyWord_DSSR.Models;
using EasyWord_DSSR.Utility;
using EasyWord_DSSR.View;
using EasyWord_DSSR.ViewModel;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Media;

namespace EasyWord_DSSR
{
    public class MainViewModel : ViewModelBase
    {

        public MainWindowView mainWindowView = new();


        private RelayCommand _cmdButtonExistingLearning;
        private RelayCommand _cmdButtonImport;
        private RelayCommand _cmdButtonDeleteWords;
        private RelayCommand _cmdButtonDeleteBuckets;
        private RelayCommand _cmdButtonLearn;
        private RelayCommand _cmdButtonCheck;
        private RelayCommand _cmdButtonSave;
        private RelayCommand _cmdButtonCancel;
        private RelayCommand _cmdCheckBoxCaseSensitive;
        private RelayCommand _cmdButtonSwitch;
        private RelayCommand _cmdButtonAdd;
        private RelayCommand _cmdButtonOverWrite;
        private RelayCommand _cmdButtonListCancel;
        private RelayCommand _cmdButtonShowAnswer;

        private SeriesCollection _barSeries { get; set; }
        public SeriesCollection BarSeries
        {
            get { return _barSeries; }
            set
            {
                _barSeries = value;
                OnPropertyChanged(nameof(BarSeries));
            }
        }

        private MediaPlayer player;

        /// <summary>
        /// Konstruktor Methode
        /// </summary>
        public MainViewModel()
        {
            Content = mainWindowView;
            _cmdButtonCheck = new RelayCommand(param => Execute_ButtonCheck(), param => CanExecute_ButtonCheck());
            _cmdButtonCancel = new RelayCommand(param => Execute_ButtonCancel());
            _cmdButtonExistingLearning = new RelayCommand(param => Execute_ButtonExistingLearning());
            _cmdButtonDeleteWords = new RelayCommand(param => Execute_ButtonDeleteWords());
            _cmdButtonDeleteBuckets = new RelayCommand(param => Execute_ButtonDeleteBuckets());
            _cmdButtonImport = new RelayCommand(param => Execute_ButtonImport());
            _cmdButtonLearn = new RelayCommand(param => Execute_ButtonLearn());
            _cmdButtonSave = new RelayCommand(param => Execute_ButtonSave(), param => CanExecute_ButtonSave());
            _cmdCheckBoxCaseSensitive = new RelayCommand(param => Execute_CheckBoxCaseSensitive());
            _cmdButtonSwitch = new RelayCommand(param => Execute_ButtonSwitch(), param => CanExecute_ButtonSwitch());
            _cmdButtonAdd = new RelayCommand(param => Execute_ButtonAdd());
            _cmdButtonOverWrite = new RelayCommand(param => Execute_ButtonOverWrite());
            _cmdButtonListCancel = new RelayCommand(param => Execute_ButtonListCancel());
            _cmdButtonShowAnswer = new RelayCommand(param => Excute_ButtonShowAnswer());

            LanguageStart();
            CreateSeriesCollection();

        }


        private string _checkBoxIsChecked;
        public string CheckBoxIsChecked
        {
            get { return _checkBoxIsChecked; }
            set
            {
                _checkBoxIsChecked = value;
                OnPropertyChanged(nameof(CheckBoxIsChecked));
            }
        }

        private string _direction = string.Empty;
        public string Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                OnPropertyChanged(nameof(Direction));
            }
        }

        private string _showanswer;
        public string ShowAnswer
        {
            get
            {
                return _showanswer;
            }
            set
            {
                _showanswer = value;
                OnPropertyChanged(nameof(ShowAnswer));
            }
        }

        private string _showAnswerText;
        public string ShowAnswerText
        {
            get
            {
                return _showAnswerText;
            }
            set
            {
                _showAnswerText = value;
                OnPropertyChanged(nameof(ShowAnswerText));
            }
        }

        private string _visibilityw;
        public string VisibilityWrong
        {
            get
            {
                return _visibilityw;
            }
            set
            {
                _visibilityw = value;
                OnPropertyChanged(nameof(VisibilityWrong));
            }
        }

        private string _visibilityr;
        public string VisibilityRight
        {
            get
            {
                return _visibilityr;
            }
            set
            {
                _visibilityr = value;
                OnPropertyChanged(nameof(VisibilityRight));
            }
        }

        private string _visibilityiconde;
        public string VisibilityIconDE
        {
            get
            {
                return _visibilityiconde;
            }
            set
            {
                _visibilityiconde = value;
                OnPropertyChanged(nameof(VisibilityIconDE));
            }
        }

        private string _languageIcon;
        public string LanguageIcon
        {
            get
            {
                return _languageIcon;
            }
            set
            {
                _languageIcon = value;
                OnPropertyChanged(nameof(LanguageIcon));
            }
        }

        private string _visibilityiconen;
        public string VisibilityIconEN
        {
            get
            {
                return _visibilityiconen;
            }
            set
            {
                _visibilityiconen = value;
                OnPropertyChanged(nameof(VisibilityIconEN));
            }
        }

        private string _color;
        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        private bool _isChecked = false;
        public void SetIsChecked(bool isChecked)
        {
            _isChecked = isChecked;
        }

        public bool GetIsChecked()
        {
            return _isChecked;
        }

        private object _content;
        public object Content
        {
            get { return _content; }
            set
            {
                _content = value;
                SetProperty<object>(ref _content, value);
                OnPropertyChanged(nameof(Content));
            }
        }

        private string _askedWord;
        public string AskedWord
        {
            get { return _askedWord; }
            set
            {
                _askedWord = value;
                SetProperty<string>(ref _askedWord, value);
                OnPropertyChanged(nameof(AskedWord));
            }
        }

        private string _answerWord = string.Empty;
        public string AnswerWord
        {
            get { return _answerWord; }
            set
            {
                _answerWord = value;
                SetProperty<string>(ref _answerWord, value);
                OnPropertyChanged(nameof(AnswerWord));
            }
        }

        #region RelayCommand
        public RelayCommand CmdButtonImport
        {
            get { return _cmdButtonImport; }
            set { _cmdButtonImport = value; }
        }

        public RelayCommand CmdButtonExistingLearning
        {
            get { return _cmdButtonExistingLearning; }
            set { _cmdButtonExistingLearning = value; }
        }

        public RelayCommand CmdButtonSave
        {
            get { return _cmdButtonSave; }
            set { _cmdButtonSave = value; }
        }

        public RelayCommand CmdButtonDeleteWords
        {
            get { return _cmdButtonDeleteWords; }
            set { _cmdButtonDeleteWords = value; }
        }

        public RelayCommand CmdButtonDeleteBuckets
        {
            get { return _cmdButtonDeleteBuckets; }
            set { _cmdButtonDeleteBuckets = value; }
        }

        public RelayCommand CmdButtonLearn
        {
            get { return _cmdButtonLearn; }
            set { _cmdButtonLearn = value; }
        }
        public RelayCommand CmdButtonCancel
        {
            get { return _cmdButtonCancel; }
            set { _cmdButtonCancel = value; }
        }

        public RelayCommand CmdButtonCheck
        {
            get { return _cmdButtonCheck; }
            set { _cmdButtonCheck = value; }
        }

        public RelayCommand CmdButtonSwitch
        {
            get { return _cmdButtonSwitch; }
            set { _cmdButtonSwitch = value; }
        }

        public RelayCommand CmdCheckBoxCaseSensitive
        {
            get { return _cmdCheckBoxCaseSensitive; }
            set { _cmdCheckBoxCaseSensitive = value; }
        }
        public RelayCommand CmdButtonAdd
        {
            get { return _cmdButtonAdd; }
            set { _cmdButtonAdd = value; }
        }
        public RelayCommand CmdButtonOverWrite
        {
            get { return _cmdButtonOverWrite; }
            set { _cmdButtonOverWrite = value; }
        }
        public RelayCommand CmdButtonListConcel
        {
            get { return _cmdButtonListCancel; }
            set { _cmdButtonListCancel = value; }
        }

        public RelayCommand CmdButtonShowAnswer
        {
            get { return _cmdButtonShowAnswer; }
            set { _cmdButtonShowAnswer = value; }
        }
        #endregion


        private string path = string.Empty;
        public List<WordsModel> myList = new();
        public List<WordsModel> MyList
        {
            get { return myList; }
            set
            {
                myList = value;
                OnPropertyChanged(nameof(MyList));
            }
        }
        public List<WordsModel> myListReference = new();
        public List<WordsModel> MyListReference
        {
            get { return myListReference; }
            set
            {
                myListReference = value;
                OnPropertyChanged(nameof(MyListReference));
            }
        }
        static bool language = Settings.Default.Language;
        private string answer = string.Empty;


        #region Importe
        /// <summary>
        /// Abfrage was bei erneutem Import ausgeführt werden soll
        /// </summary>
        public void Execute_ButtonImport()
        {
            if (myList.Count > 0)
            {
                ListUpdate listUpdate = new();
                Content = listUpdate;
            }
            else
            {
                Content = mainWindowView;
                ImportList();
            }
        }
        /// <summary>
        /// Öffnet den Explorer, um eine CSV-Datei zu importieren.
        /// Die Liste wird auf Fehler kontrolliert und in ein WordsModel Objekt gespeichert.
        /// </summary>
        ///
        public void ImportList()
        {
            OpenFileDialog Opendlg = new()
            {
                DefaultExt = ".csv",
                Filter = "CSV file(.csv)|*.csv"
            };
            try
            {
                if (Opendlg.ShowDialog() == true)
                {
                    string selectedFile = Opendlg.FileName;
                    path = selectedFile;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            try
            {

                using var streamReader = new StreamReader(path);
                while (!streamReader.EndOfStream)
                {
                    var splitLine = streamReader.ReadLine().Split(';');
                    var count = splitLine.Length;
                    if (count > 2)
                    {
                        MessageBox.Show("Wortliste ist fehlerhaft");
                    }
                    else if (splitLine[0] == "" || splitLine[1] == "")
                    { MessageBox.Show("In der Wortliste fehlen Wörter"); }
                    else
                    {
                        var myNewEntity = new WordsModel()
                        {
                            En = splitLine[0].Trim(),
                            De = splitLine[1].Trim()
                        };
                        myList.Add(myNewEntity);                        
                    }                   
                    
                }
                streamReader.Close();
                language = false;
                Direction = "Auf Deutsche Eingabe wechseln";
                Settings.Default.Language = false;
                Settings.Default.Save();
                myListReference = myList;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); };
        }

        /// <summary>
        /// Stelt die Gespeicherte lern Datei wieder ins Programm
        /// </summary>
        public void Execute_ButtonExistingLearning()
        {
            try
            {
                bool correctFile = false;
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.Filter = "CSV files (*.csv)|*.csv";

                if (openFile.ShowDialog() == true)
                {
                    string learnFile = openFile.FileName;
                    path = learnFile;
                }
                MainWindowView mainWindowView = new();
                Content = mainWindowView;

                int rowNumber = 0;
                var reader = new StreamReader(path);
                Execute_ButtonDeleteBuckets();
                myList.Clear();
                while (!reader.EndOfStream)
                {
                    rowNumber++;
                    var line = reader.ReadLine();
                    var data = line.Split(";");

                    if ((data[0] == "De") && (data[1] == "En") && (data[2] == "IsCorrect") && (data[3] == "BucketStatus") && (rowNumber == 1))
                    {
                        correctFile = true;
                        continue;
                    }
                    else if (correctFile)
                    {
                        var learningExist = new WordsModel()
                        {
                            De = data[0],
                            En = data[1],
                            IsCorrect = bool.Parse(data[2]),
                            BucketStatus = int.Parse(data[3])
                        };
                        myList.Add(learningExist);
                    }
                }
                if (!correctFile)
                {
                    MessageBox.Show($"Fehler nicht das Richtige Datei", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


        public int _listCount;
        public int _wrongCount;
        public void Execute_ButtonLearn()
        {
            if (myList.Count == 0)
            {
                MessageBox.Show("Bitte Daten importieren");
            }
            else
            {
                ShowAnswerText = "Hidden";
                LearnView learnView = new();
                Content = learnView;
                _listCount = myList.Count - 1;
                _wrongCount = _listCount;
                Randomizer.Shuffle(myList);
                OutputList();
                if (language == false)
                {
                    VisibilityIconDE = "Visible";
                    VisibilityIconEN = "Hidden";
                    LanguageIcon = "/Images/ENicon.png";
                }
                else
                {
                    VisibilityIconDE = "Hidden";
                    VisibilityIconEN = "Visible";
                    LanguageIcon = "/Images/DEicon.png";
                }

                Color = "#FFC1D7E9";
                VisibilityWrong = "Hidden";
                VisibilityRight = "Hidden";
            }
            BucketWithImport();
        }

        /// <summary>
        /// Überprüft die Korrektheit der Wörter
        /// </summary>
        public void OutputList()
        {
            if (_answerWord == null)
            {
                if (myList[_listCount].IsCorrect == true)
                {
                    _listCount--;
                }
                AskedWord = myList[_listCount].De;
                _answerWord = "";
            }
            else
            {
                if (GetIsChecked() == true)
                {
                    if (_answerWord == myList[_listCount].En)
                    {
                        AnsweredWordIsCorrect();
                        AskedWord = myList[_listCount].De;
                    }
                    else
                    {
                        AnsweredWordIsFalse();
                    }

                }
                else if (GetIsChecked() == false)
                {
                    if (_answerWord.ToLower() == myList[_listCount].En.ToLower())
                    {
                        AnsweredWordIsCorrect();
                        if (checkArrayIsCorrect(myList) == false)
                        {
                            AskedWord = myList[_listCount].De;
                        }
                    }
                    else if (_answerWord == "")
                    {
                        AskedWord = myList[_listCount].De;
                        AnswerWord = "";
                    }
                    else
                    {
                        AnsweredWordIsFalse();
                    }
                }
            }
        }

        public void WordIsCaseSensitive(bool isChecked)
        {

            if (isChecked == true)
            {
                SetIsChecked(false);
            }
            else
            {
                SetIsChecked(true);
            }
        }

        public bool CanExecute_ButtonCheck()
        {
            if (_answerWord != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Execute_ButtonCheck()
        {
            ShowAnswerText = "Hidden";
            OutputList();

            if (checkArrayIsCorrect(myList) == true)
            {
                MessageBox.Show("Alle Wörter sind richtig");
                Execute_ButtonDeleteBuckets();
                Content = mainWindowView;

                for (int i = 0; i < 5; i++)
                {
                    _barSeries[0].Values[i] = 0;
                }
                _barSeries[0].Values[2] = myList.Count();

                foreach (var item in myList)
                {
                    item.BucketStatus = 3;
                    item.IsCorrect = false;
                }

                _answerWord = "";
                PlaySound("../../../Images/win.mp3");
            }
        }

        public bool CanExecute_ButtonSave()
        {
            return true;
        }

        #region Save and Cancel
        /// <summary>
        /// Stellt ein csv von der Liste aus
        /// </summary>
        public void Execute_ButtonSave()
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (saveFileDialog.ShowDialog() == true)
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ";"
                    };
                    using (var writer = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    using (var csv = new CsvWriter(writer, config))
                    {
                        csv.WriteRecords(myList);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Lernen abbrechen und auf Hauptseite zurück kommen.
        /// </summary>
        public void Execute_ButtonCancel()
        {
            MainWindowView mainWindowView = new();
            Content = mainWindowView;
        }

        public void Execute_CheckBoxCaseSensitive()
        {
            WordIsCaseSensitive(GetIsChecked());
        }

        /// <summary>
        /// Resetet die Anwendung nach schaffen aller Wörter 
        /// </summary>
        public void RestartLearn()
        {
            myList = myListReference;
            Randomizer.Shuffle(myList);
            _listCount = myList.Count() - 1;
            _wrongCount = _listCount;
            VisibilityWrong = "Hidden";
            VisibilityRight = "Hidden";
            Color = "#FFC1D7E9";

            for (int i = 0; i < 5; i++)
            {
                _barSeries[0].Values[i] = 0;
            }
        }
        #endregion

        #region Correct and False
        /// <summary>
        /// Wird ausgeführt sobald ein Wort korrekt eingegeben wurde
        /// </summary>
        public void AnsweredWordIsCorrect()
        {
            SetCurrentColumnToTrue();
            AnswerWord = "";
            _listCount--;
            _wrongCount--;
            Color = "Green";
            VisibilityWrong = "Hidden";
            VisibilityRight = "Visible";
            if (_listCount == -1 && _wrongCount != -1)
            {
                _listCount = _wrongCount;
                List<WordsModel> myList1 = new();
                foreach (var item in myList.Where(n => n.IsCorrect == false))
                {
                    myList1.Add(item);
                }
                _listCount = myList1.Count - 1;
                myList = myList1;
            }
            if (_listCount == -1)
            {
                _listCount = myList.Count - 1;
            }
        }

        /// <summary>
        /// Wird ausgeführt sobald ein Wort korrekt eingegeben wurde
        /// </summary>
        public void AnsweredWordIsFalse()
        {
            SetCurrentColumnToFalse();
            AnswerWord = "";
            Color = "Red";
            VisibilityRight = "Hidden";
            VisibilityWrong = "Visible";
            ShowAnswer = $"{AskedWord} = {myList[_listCount].En}";
            _listCount--;
            if (_listCount == -1 && _wrongCount != -1)
            {
                _listCount = _wrongCount;
                List<WordsModel> myList1 = new();
                foreach (var item in myList.Where(n => n.IsCorrect == false))
                {
                    myList1.Add(item);
                }
                _listCount = myList1.Count - 1;
                myList = myList1;
            }
            if (_listCount == -1)
            {
                _listCount = myList.Count - 1;
            }
            AskedWord = myList[_listCount].De;


        }
        #endregion

        /// <summary>
        /// Kontrollier ob eine Liste importiert wurde, wenn nicht wird der Switch Button deaktiviert
        /// </summary>
        /// <returns></returns>
        public bool CanExecute_ButtonSwitch()
        {
            if (myList.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //public bool defaultEingabe = true;
        /// <summary>
        /// Die Eigenschaften De und En von myList werden getauscht.
        /// </summary>
        public void Execute_ButtonSwitch()
        {
            try
            {
                List<WordsModel> myList2 = new();
                if (myList.Count == 0)
                {
                    MessageBox.Show("Bitte Daten importieren");
                }
                else
                {
                    if (language == false)
                    {
                        language = true;
                        Direction = "Auf Englische Eingabe wechseln";
                        Settings.Default.Language = true;
                        Settings.Default.Save();
                    }
                    else
                    {
                        language = false;
                        Direction = "Auf Deutsche Eingabe wechseln";
                        Settings.Default.Language = false;
                        Settings.Default.Save();                        
                    }
                    foreach (var item in myList)
                    {
                        myList2.Add(new WordsModel { De = item.En, En = item.De });
                    }

                    myList = myList2;
                    myListReference = myList2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region Delete
        /// <summary>
        /// lösch alle wörter aus der Liste
        /// </summary>
        public void Execute_ButtonDeleteWords()
        {
            try
            {
                myList.Clear();
                Content = mainWindowView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Stelt alle Buckets auf standart wert 3 aus
        /// </summary>
        public void Execute_ButtonDeleteBuckets()
        {
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    _barSeries[0].Values[i] = 0;
                }
                    _barSeries[0].Values[2] = myList.Count();

                foreach (var item in myList)
                {
                    item.BucketStatus = 3;
                    item.IsCorrect = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// Liste wird Hinzugefügt
        /// </summary>
        public void Execute_ButtonAdd()
        {
            if(language == true)
            {
                Execute_ButtonSwitch();
            }
            ImportList();
            Content = mainWindowView;
        }

        /// <summary>
        /// Liste wird neu Erstellt
        /// </summary>
        public void Execute_ButtonOverWrite()
        {
            myList.Clear();
            ImportList();
            Content = mainWindowView;
        }
        /// <summary>
        /// Abbruch zurück zur MainPage
        /// </summary>
        public void Execute_ButtonListCancel()
        {
            Content = mainWindowView;
        }

        /// <summary>
        /// Initialisiert den Text für den SwitchButton für die Sprache
        /// </summary>
        public void LanguageStart()
        {
            if (language == false)
                Direction = "Auf Deutsche Eingabe wechseln";
            else
                Direction = "Auf Englische Eingabe wechseln";
        }

        /// <summary>
        /// Zeigt die Antwort
        /// </summary>
        public void Excute_ButtonShowAnswer()
        {
            ShowAnswerText = "Visible";
        }

        public void CreateSeriesCollection()
        {
            _barSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = new ChartValues<int> { 0, 0, 0, 0, 0 }
                }
            };
        }

        /// <summary>
        /// Setzt das aktuelle Wort in den nächsten Eimer nach vorn
        /// </summary>
        public void SetCurrentColumnToTrue()
        {
            int intValue;
            if (myList[_listCount].BucketStatus == 1)
            {
                var objValue = BarSeries[0].Values[1];
                intValue = Convert.ToInt16(objValue);
                intValue++;
                BarSeries[0].Values[1] = intValue;

                objValue = BarSeries[0].Values[0];
                intValue = Convert.ToInt16(objValue);
                intValue--;
                BarSeries[0].Values[0] = intValue;

                myList[_listCount].BucketStatus = 2;
            }
            else if (myList[_listCount].BucketStatus == 2)
            {
                var objValue = BarSeries[0].Values[2];
                intValue = Convert.ToInt16(objValue);
                intValue++;
                BarSeries[0].Values[2] = intValue;

                objValue = BarSeries[0].Values[1];
                intValue = Convert.ToInt16(objValue);
                intValue--; ;
                BarSeries[0].Values[1] = intValue;

                myList[_listCount].BucketStatus = 3;
            }
            else if (myList[_listCount].BucketStatus == 3)
            {
                var objValue = BarSeries[0].Values[3];
                intValue = Convert.ToInt16(objValue);
                intValue++;
                BarSeries[0].Values[3] = intValue;
                objValue = BarSeries[0].Values[2];

                intValue = Convert.ToInt16(objValue);
                intValue--;
                BarSeries[0].Values[2] = intValue;

                myList[_listCount].BucketStatus = 4;
            }
            else if (myList[_listCount].BucketStatus == 4)
            {
                var objValue = BarSeries[0].Values[4];
                intValue = Convert.ToInt16(objValue);
                intValue++;
                BarSeries[0].Values[4] = intValue;

                objValue = BarSeries[0].Values[3];
                intValue = Convert.ToInt16(objValue);
                intValue--;
                BarSeries[0].Values[3] = intValue;

                myList[_listCount].BucketStatus = 5;
            }
            else if (myList[_listCount].BucketStatus == 5)
            {
                myList[_listCount].IsCorrect = true;

                var objValue = BarSeries[0].Values[4];
                intValue = Convert.ToInt16(objValue);
                intValue--;
                BarSeries[0].Values[4] = intValue;
            }
            PlaySound("../../../Images/correct.mp3");
        }

        /// <summary>
        /// Setzt das aktuelle Wort in den nächsten Eimer nach hinten
        /// </summary>
        public void SetCurrentColumnToFalse()
        {
            if (AnswerWord != "")
            {
                if (myList[_listCount].BucketStatus == 2)
                {
                    var objValue = BarSeries[0].Values[1];
                    int intValue = Convert.ToInt16(objValue);
                    intValue--;
                    BarSeries[0].Values[1] = intValue;

                    objValue = BarSeries[0].Values[0];
                    intValue = Convert.ToInt16(objValue);
                    intValue++; ;
                    BarSeries[0].Values[0] = intValue;

                    myList[_listCount].BucketStatus = 1;
                }
                else if (myList[_listCount].BucketStatus == 3)
                {
                    var objValue = BarSeries[0].Values[2];
                    int intValue = Convert.ToInt16(objValue);
                    intValue--;
                    BarSeries[0].Values[2] = intValue;

                    objValue = BarSeries[0].Values[1];
                    intValue = Convert.ToInt16(objValue);
                    intValue++;
                    BarSeries[0].Values[1] = intValue;

                    myList[_listCount].BucketStatus = 2;
                }
                else if (myList[_listCount].BucketStatus == 4)
                {
                    var objValue = BarSeries[0].Values[3];
                    int intValue = Convert.ToInt16(objValue);
                    intValue--;
                    BarSeries[0].Values[3] = intValue;

                    objValue = BarSeries[0].Values[2];
                    intValue = Convert.ToInt16(objValue);
                    intValue++;
                    BarSeries[0].Values[2] = intValue;

                    myList[_listCount].BucketStatus = 3;
                }
                else if (myList[_listCount].BucketStatus == 5)
                {
                    var objValue = BarSeries[0].Values[4];
                    int intValue = Convert.ToInt16(objValue);
                    intValue--;
                    BarSeries[0].Values[4] = intValue;

                    objValue = BarSeries[0].Values[3];
                    intValue = Convert.ToInt16(objValue);
                    intValue++;
                    BarSeries[0].Values[3] = intValue;

                    myList[_listCount].BucketStatus = 4;
                }
                PlaySound("../../../Images/wrong.mp3");
            }
        }

        /// <summary>
        /// Speilt einen Sound ab
        /// </summary>
        /// <param name="Url"></param>
        public void PlaySound(string Url)
        {
            if (true)
            {
                player = new MediaPlayer();
                player.MediaFailed += (o, args) =>
                {
                    MessageBox.Show("Media Fehlgeschlagen");
                };
                player.Open(new Uri(Url, UriKind.RelativeOrAbsolute));
                player.Play();
            }
        }

        /// <summary>
        /// True wenn das prop. IsCorrect in alles instanzen True ist
        /// </summary>
        /// <param name="myList"></param>
        /// <returns></returns>
        public bool checkArrayIsCorrect(List<WordsModel> myList)
        {
            for (int i = 0; i < myList.Count(); ++i)
            {
                if (myList[i].IsCorrect == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gibt beim Lernen Button die Koreckte Buckets anzahl an
        /// </summary>
        public void BucketWithImport()
        {
            int first = 0;
            int second = 0;
            int third = 0;
            int fourth = 0;
            int fifth = 0;

            for (int i = 0; i < 5; i++)
            {
                _barSeries[0].Values[i] = 0;
            }
            foreach (var word in myList) 
            {
               switch (word.BucketStatus)
                {
                    case 1:
                        ++first;
                        BarSeries[0].Values[0] =+ first;
                        break;
                    case 2:
                        ++second;
                        BarSeries[0].Values[1] =+ second;
                        break;
                    case 3:
                        ++third;
                        BarSeries[0].Values[2] =+ third;
                        break;
                    case 4:
                        ++fourth;
                        BarSeries[0].Values[3] =+ fourth;
                        break;
                    case 5:
                        ++fifth;
                        BarSeries[0].Values[4] =+ fifth;
                        break;
                } 
            }
        }
    }
}