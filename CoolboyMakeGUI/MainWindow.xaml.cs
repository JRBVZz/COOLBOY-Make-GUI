using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace MakefileRunner
{
    public partial class MainWindow : Window
    {
        private readonly List<string> predefinedTargets = new() { "nes20", "unif", "bin", "all", "clean" };
        
        public MainWindow()
        {
            InitializeComponent();
            LoadDefaultTargets();
            EnableSubmapperCheckBox.IsChecked = true;
            SubmapperComboBox.SelectedIndex = 0;
            SoundCheckBox.IsChecked = true;
            RCursorBox.IsChecked = true;
            StarsBox.Text = "30";
            GameListPathBox.Text = "games.list";
            
        }

        private void LoadDefaultTargets()
        {
            TargetComboBox.ItemsSource = predefinedTargets;
            TargetComboBox.SelectedIndex = 0;
        }

        private void ChooseMakefile_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Makefile|Makefile|All Files|*.*",
                InitialDirectory = Directory.GetCurrentDirectory()
            };

            if (dialog.ShowDialog() == true)
            {
                MakefilePathBox.Text = dialog.FileName;
                Log("Makefile выбран: " + dialog.FileName);
            }
        }

        private void ChooseGameList_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "All Files|*.*",
                InitialDirectory = Directory.GetCurrentDirectory()
            };

            if (dialog.ShowDialog() == true)
            {
                GameListPathBox.Text = dialog.FileName;
                Log("Games List выбран: " + dialog.FileName);
            }
        }

        private string ConvertPathToMsys(string windowsPath)
        {
            string fullPath = Path.GetFullPath(windowsPath)
                .Replace('\\', '/')
                .Replace(":", "");

            if (fullPath.Length >= 2 && fullPath[1] == '/')
                return "/" + fullPath;  // Пример: C:/Users/... => /C/Users/...

            return fullPath;
        }

        private void RunTarget_Click(object sender, RoutedEventArgs e)
        {
            if (TargetComboBox.SelectedItem is not string target)
            {
                Log("❌ Не выбрана цель.");
                return;
            }

            string makefilePath = MakefilePathBox.Text;
            if (!File.Exists(makefilePath))
            {
                Log("❌ Makefile не найден по указанному пути.");
                return;
            }

            string gamelistPath = string.IsNullOrWhiteSpace(GameListPathBox.Text)
                ? "games.list"
                : Path.GetFileName(GameListPathBox.Text);
            
            string bashPath = BashPathBox.Text;
            if (!File.Exists(bashPath))
            {
                Log("❌ Указанный путь к bash.exe недействителен.");
                return;
            }

            string makeDir = Path.GetDirectoryName(makefilePath);
            string msysMakefile = ConvertPathToMsys(makefilePath);
            string msysMakeDir = ConvertPathToMsys(makeDir);

            string options = "";

            if (EnableSubmapperCheckBox.IsChecked == true && SubmapperComboBox.SelectedItem is int submapperValue)
                options += $" SUBMAPPER={submapperValue}";

            options += SaveCheckBox.IsChecked == true ? " ENABLE_SAVES=1 ENABLE_LAST_GAME_SAVING=1" : " ENABLE_SAVES=0";
            options += SoundCheckBox.IsChecked == true ? " ENABLE_SOUND=1" : " ENABLE_SOUND=0";
            options += RCursorBox.IsChecked == true ? " ENABLE_RIGHT_CURSOR=1" : " ENABLE_RIGHT_CURSOR=0";
            options += $" GAMES={gamelistPath}";

            if (int.TryParse(StarsBox.Text, out int starsValue) && starsValue >= 0 && starsValue <= 62)
            {
                options += $" STARS={starsValue}";
            }
            else
            {
                Log("⚠️ Значение STARS должно быть от 0 до 62. Используется по умолчанию: 30.");
                options += " STARS=30";
            }

            if (!string.IsNullOrWhiteSpace(ExtraOptionsBox.Text))
            {
                options += " " + ExtraOptionsBox.Text;
            }

            string bashCommand = $"cd {msysMakeDir} && make -f {msysMakefile} {target} {options}";

            var psi = new ProcessStartInfo
            {
                FileName = bashPath,
                Arguments = $"-lc \"{bashCommand}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Log($"▶ Запуск: {bashCommand}");

            try
            {
                using var process = Process.Start(psi);
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrWhiteSpace(output))
                    Log(output);

                if (!string.IsNullOrWhiteSpace(error))
                    Log("⚠️ Ошибки:\n" + error);

                if (process.ExitCode == 0)
                    Log("✅ Завершено успешно.");
                else
                    Log($"❌ Код выхода: {process.ExitCode}");
            }
            catch (Exception ex)
            {
                Log("❌ Ошибка запуска: " + ex.Message);
            }
        }

        private void BrowseBashPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "bash.exe|bash.exe|All Files|*.*",
                FileName = "bash.exe",
                InitialDirectory = @"C:\Program Files\msys64\usr\bin"
            };

            if (dialog.ShowDialog() == true)
            {
                BashPathBox.Text = dialog.FileName;
            }
        }

        private void EnableSubmapperCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            SubmapperComboBox.IsEnabled = true;
        }

        private void EnableSubmapperCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SubmapperComboBox.IsEnabled = false;
        }

        private void Log(string text)
        {
            LogBox.AppendText(text + Environment.NewLine);
            LogBox.ScrollToEnd();
        }
    }
}
