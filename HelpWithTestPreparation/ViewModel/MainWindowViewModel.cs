using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.RegularExpressions;
using HelpWithTestPreparation.Models;
using System.Text.Json;
using ControlzEx.Theming;


namespace HelpWithTestPreparation.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private List<QuestionItem> _questions = new();
        private readonly Random _random = new();
        private List<QuestionItem> _unansweredQuestions = new();

        [ObservableProperty]
        private string themeIcon = "WeatherSunny";
        
        [ObservableProperty]
        private string _filePath = "Файл не выбран";

        [ObservableProperty]
        private List<string> selectedAnswers = new();

        [ObservableProperty]
        private string currentQuestion;

        [ObservableProperty]
        private List<string> currentOptions;

        [ObservableProperty]
        private List<string> correctAnswers;

        [ObservableProperty]
        private List<OptionItem> optionItems;


        [RelayCommand]
        private void SelectPath()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Word Documents|*.docx",
                Title = "Выберите файл DOCX"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FilePath);

                try
                {
                    int question = 0, answer = 0;

                    using (var wordDoc = WordprocessingDocument.Open(FilePath, false))
                    {
                        var body = wordDoc.MainDocumentPart.Document.Body;
                        var paragraphs = body.Elements<Paragraph>().ToList();

                        for (int i = 0; i < paragraphs.Count; i++)
                        {
                            var paragraph = paragraphs[i];
                            string fullText = paragraph.InnerText.Trim();

                            if (Regex.IsMatch(fullText, @"^\d+\.\s"))
                            {
                                var questionText = fullText;
                                var options = new List<string>();
                                var correctAnswers = new List<string>();

                                int j = i + 1;
                                while (j < paragraphs.Count)
                                {
                                    var optionParagraph = paragraphs[j];
                                    string optionText = optionParagraph.InnerText.Trim();

                                    if (Regex.IsMatch(optionText, @"^[a-l]\)", RegexOptions.IgnoreCase))
                                    {
                                        string cleanOption = Regex.Replace(optionText, @"^[a-l]\)\s*", "", RegexOptions.IgnoreCase);
                                        options.Add(cleanOption);

                                        bool isBold = optionParagraph
                                            .Descendants<Run>()
                                            .Any(run =>
                                                run.RunProperties?.Bold != null &&
                                                (run.RunProperties.Bold.Val == null || run.RunProperties.Bold.Val.Value));

                                        if (isBold)
                                            correctAnswers.Add(cleanOption);

                                        j++;
                                    }
                                    else break;
                                }

                                if (!string.IsNullOrWhiteSpace(questionText) && options.Any())
                                {
                                    _questions.Add(new QuestionItem
                                    {
                                        Question = questionText,
                                        Options = options,
                                        Answers = correctAnswers
                                    });

                                    question++;
                                    if (correctAnswers.Any())
                                        answer++;
                                }

                                i = j - 1;
                            }
                        }
                    }

                    string outputPath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        $"{fileNameWithoutExtension}.json"
                    );


                    var json = JsonSerializer.Serialize(_questions, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(outputPath, json);

                    MessageBox.Show($"Файл сохранён: {outputPath}\nНайдено вопросов: {question}, с ответами: {answer}", "Успешно сохранено");
                    ShowRandomQuestion();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка");
                }
            }
        }
        [RelayCommand]
        private void ToggleTheme()
        {
            var currentTheme = ThemeManager.Current.DetectTheme();

            if (currentTheme?.BaseColorScheme == "Light")
                ThemeManager.Current.ChangeTheme(App.Current, "Dark.Purple");
            else
                ThemeManager.Current.ChangeTheme(App.Current, "Light.Purple");
            
            UpdateThemeIcon();
        }
        
        private void UpdateThemeIcon()
        {
            var currentTheme = ThemeManager.Current.DetectTheme();
            ThemeIcon = currentTheme?.BaseColorScheme == "Light" ? "WeatherNight" : " WeatherSunny";
        }

        [RelayCommand]
        private void NextQuestion()
        {
            var selectedSet = OptionItems
                .Where(x => x.IsSelected)
                .Select(x => x.Text)
                .ToHashSet();

            var correctSet = new HashSet<string>(CorrectAnswers);

            if (selectedSet.SetEquals(correctSet))
            {
                MessageBox.Show("✅ Верно!", "Успешно");

                var current = _unansweredQuestions.FirstOrDefault(q => q.Question == CurrentQuestion);
                if (current != null)
                {
                    _unansweredQuestions.Remove(current);
                }
            }
            else
            {
                string correct = string.Join(", ", CorrectAnswers);
                MessageBox.Show($"❌ Неверно!\nВерный ответ: {correct}", "Ошибка");
            }

            ShowRandomQuestion();
        }


        private void ShowRandomQuestion()
        {
            if (_unansweredQuestions == null || !_unansweredQuestions.Any())
            {
                _unansweredQuestions = new List<QuestionItem>(_questions);
            }

            if (_unansweredQuestions.Count == 0)
            {
                MessageBox.Show("🎉 Все вопросы решены верно!", "Успех");
                return;
            }

            var q = _unansweredQuestions[_random.Next(_unansweredQuestions.Count)];

            CurrentQuestion = q.Question;

            var shuffledOptions = q.Options
                .OrderBy(_ => _random.Next())
                .Select(x => new OptionItem { Text = x })
                .ToList();

            OptionItems = shuffledOptions;
            CorrectAnswers = q.Answers;
        }

        public bool IsFileSelected => !string.IsNullOrWhiteSpace(FilePath) && FilePath != "Файл не выбран";

        partial void OnFilePathChanged(string oldValue, string newValue)
        {
            OnPropertyChanged(nameof(IsFileSelected));
        }
    }
}
