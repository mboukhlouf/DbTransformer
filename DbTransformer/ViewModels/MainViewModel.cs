using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DbTransformer.Services;
using DbTransformer.Transform;
using DbTransformer.ViewModels.Commands;
using Microsoft.Win32;

namespace DbTransformer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string inputFilePath;
        private bool inputFileLoading;
        private string outputFilePath;
        private string format;


        private string[] inputFileLines;

        private readonly Transformer transformer = new Transformer(":");

        public string InputFilePath
        {
            get => inputFilePath;
            set
            {
                if (inputFilePath != null && inputFilePath.Equals(value))
                    return;
                inputFilePath = value;
                OnPropertyChanged(nameof(InputFilePath));
            }
        }

        public bool InputFileLoading
        {
            get => inputFileLoading;
            set
            {
                if (inputFileLoading == value)
                    return;
                inputFileLoading = value;
                OnPropertyChanged(nameof(InputFileLoading));
            }
        }

        public string[] InputFileLines
        {
            get => inputFileLines;
            set
            {
                if (inputFileLines == value)
                    return;
                inputFileLines = value;
                OnPropertyChanged(nameof(InputFileLines));
                OnPropertyChanged(nameof(InputFileLoaded));
                OnPropertyChanged(nameof(LineFromFile));
            }
        }

        public bool InputFileLoaded
        {
            get => InputFileLines != null;
        }

        public string OutputFilePath
        {
            get => outputFilePath;
            set
            {
                if (outputFilePath != null && outputFilePath.Equals(value))
                    return;
                outputFilePath = value;
                OnPropertyChanged(nameof(OutputFilePath));
            }
        }

        public string LineFromFile
        {
            get
            {
                if (InputFileLines == null)
                    return null;
                return InputFileLines[0];
            }
        }

        public string OutputPreview
        {
            get
            {
                if (LineFromFile != null)
                    return transformer.Transform(LineFromFile, Format);
                return null;
            }
        }

        public string Format
        {
            get => format;
            set
            {
                if (format != null && format.Equals(value))
                    return;
                format = value;
                OnPropertyChanged(nameof(Format));
            }
        }

        public string Delimiter
        {
            get => transformer.Delimiter;
            set
            {
                if (transformer.Delimiter != null && transformer.Delimiter == value)
                    return;

                if(value != "")
                {
                    transformer.Delimiter = value;
                }
                OnPropertyChanged(nameof(Delimiter));
            }
        }

        public ICommand BrowseInputCommand { get; }

        public ICommand BrowseOutputCommand { get; }

        public ICommand TransformCommand { get; }

        public MainViewModel()
        {
            BrowseInputCommand = new RelayCommand(BrowseInput);
            BrowseOutputCommand = new RelayCommand(BrowseOutput);
            TransformCommand = new RelayCommand(Transform);
        }

        private async void BrowseInput(object parameter)
        {
            IFileDialog dialog = IocKernel.Get<IFileDialog>();
            string fileName = dialog.OpenFileDialog();

            var messageBox = IocKernel.Get<IMessageBox>();
            try
            {
                InputFileLoading = true;
                var lines = await File.ReadAllLinesAsync(fileName);
                InputFilePath = fileName;
                InputFileLines = lines;
                messageBox.ShowMessage("File loaded successfully.");
            }
            catch (IOException e)
            {
                messageBox.ShowMessage($"Error: {e.Message}");
            }
            finally
            {
                InputFileLoading = false;
            }
        }

        private void BrowseOutput(object parameter)
        {
            IFileDialog dialog = IocKernel.Get<IFileDialog>();
            string fileName = dialog.SaveFileDialog();
            OutputFilePath = fileName;
        }

        private void Transform(object parameter)
        {
            var messageBox = IocKernel.Get<IMessageBox>();

            if (inputFileLines == null)
            {
                messageBox.ShowMessage("You didn't set an input");
                return;
            }
            if(string.IsNullOrWhiteSpace(outputFilePath))
            {
                messageBox.ShowMessage("You didn't set an output");
                return;
            }

            var transformedLines = inputFileLines.Select(line => transformer.Transform(line, Format))
                                                 .ToList();
            try
            {
                File.WriteAllLines(outputFilePath, transformedLines);
                messageBox.ShowMessage("Done!");
            }
            catch(IOException e)
            {
                messageBox.ShowMessage($"Error: {e.Message}");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            if (propertyName == nameof(Format) || propertyName == nameof(Delimiter))
            {
                OnPropertyChanged(nameof(OutputPreview));
            }
        }
    }
}
