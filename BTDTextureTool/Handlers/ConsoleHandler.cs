using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace BTDTextureTool
{
    public class ConsoleHandler
    {
        public StringBuilder sb;
        public const int  LinesCount = 11;
        public const int charcount = 52;
        public string[] Texts;
        public int index = 0;
        public TextBlock Output;
        public ScrollViewer Scroller;
        public ConsoleHandler(TextBlock text, ScrollViewer s)
        {
            Output = text;
            Texts = new string[LinesCount];
            sb = new StringBuilder();
            Scroller = s;
            
        }
        public void Log(string text)
        {
            //sb.Clear();
            //if (text.Length > 52)
            //{
                
            //    char[] s = new char[52];
            //    bool loop = true;
            //    int j = 0;
            //    int k = 0;
            //    while (j < text.Length)
            //    {
            //        s[k]= text[j];
            //        j++;
            //        k++;
            //        if (k == 52)
            //        {
            //            k = 0;
            //            Texts[index] = new string(s) ;
            //            s = new char[52];
            //            index++;
            //            if (index ==LinesCount)
            //            {
            //                index = 0;
            //            }
            //        }

            //    }
            //    Texts[index] = new string(s);
            //    index++;
            //    if (index == LinesCount)
            //    {
            //        index = 0;
            //    }
            //}
            //else {
            //    Texts[index] = text;
            //    index++;
            //    if (index == LinesCount)
            //    {
            //        index = 0;
            //    }
            //}
            //for (int i = 0; i < LinesCount; i++)
            //{
            //    sb.AppendLine(Texts[(index + i  )%LinesCount]);
            //}
            
            Output.Text +="\n" + text;
            Scroller.ScrollToBottom();
            AllowUIToUpdate();
        }
        void AllowUIToUpdate()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, new DispatcherOperationCallback(delegate (object parameter)
            {
                frame.Continue = false;
                return null;
            }), null);

            Dispatcher.PushFrame(frame);
            //EDIT:
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                          new Action(delegate { }));
        }
    }
}
public class ConsoleContent : INotifyPropertyChanged
{
    string consoleInput = string.Empty;
    ObservableCollection<string> consoleOutput = new ObservableCollection<string>() {};

    public string ConsoleInput
    {
        get
        {
            return consoleInput;
        }
        set
        {
            consoleInput = value;
            OnPropertyChanged("ConsoleInput");
        }
    }

    public ObservableCollection<string> ConsoleOutput
    {
        get
        {
            return consoleOutput;
        }
        set
        {
            consoleOutput = value;
            OnPropertyChanged("ConsoleOutput");
        }
    }

    public void RunCommand()
    {
        ConsoleOutput.Add(ConsoleInput);
        // do your stuff here.
        ConsoleInput = String.Empty;
    }


    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged(string propertyName)
    {
        if (null != PropertyChanged)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
}