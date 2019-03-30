using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeLine1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _Model = new MyModel();
            this.DataContext = _Model;
        }
        private MyModel _Model;

        private void OpenFolder_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _Model.FileName.Clear();
            _Model.AllNum = 0;
            _Model.WhiteNum = 0;
            _Model.EffectNum = 0;
            _Model.CommentNum = 0;
            _Model.CommentRate = null;

            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            try
            {
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    _Model.Path = dialog.FileName;
                    
                }
                _Model.load_folder(dialog.FileName);
                progressbar(_Model.FileName);

                double result = (double)_Model.CommentNum / (double)_Model.AllNum;
                _Model.CommentRate = string.Format("{0:0.0%}", result);//得到5.8%
                MessageBox.Show("统计完毕");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void OpenFolder_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            _Model.FileName.Clear();
            _Model.AllNum = 0;
            _Model.WhiteNum = 0;
            _Model.EffectNum = 0;
            _Model.CommentNum = 0;
            _Model.CommentRate = null;

            try
            {
                OpenFileDialog aDlg = new OpenFileDialog();
                aDlg.Filter = "文本文件|*.txt;|所有文件|*.*";
                if (aDlg.ShowDialog() != true) return;
                //  MessageBox.Show(aDlg.FileName);
                _Model.Path = aDlg.FileName;
                _Model.load_file(aDlg.FileName);

                progressbar(_Model.FileName);
                
                double result = (double)_Model.CommentNum / (double)_Model.AllNum;
                _Model.CommentRate = string.Format("{0:0.0%}", result);//得到5.8%
                MessageBox.Show("统计完毕");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenFile_CanExecuted(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void progressbar(ObservableCollection<string> filename)//进度条
        {
            for (int i = 0; i < filename.Count(); i=i+3)
            {

                if(i+1< filename.Count())
                {
                    ParameterizedThreadStart s = new ParameterizedThreadStart(lineNum);
                    Thread thread = new Thread(s);//创建一个线程
                    thread.Start(i + 1);//开始一个线程
                    thread.Join();
                }

                if (i + 2 < filename.Count())
                {
                    ParameterizedThreadStart a = new ParameterizedThreadStart(lineNum);
                    Thread thread = new Thread(a);//创建一个线程
                    thread.Start(i + 2);//开始一个线程
                    thread.Join();
                }

                //if (filename[i].EndsWith(".v"))
                //{
                //    _Model.vlineCount(@filename[i]);
                //}
                //else if (filename[i].EndsWith(".vhd"))
                //{
                //    _Model.vhdlineCount(@filename[i]);
                //}
                lineNum(i);

                double value = (i + 3) * 100.0 / filename.Count();
                pbBar.Dispatcher.Invoke(new Action<System.Windows.DependencyProperty, object>(pbBar.SetValue), System.Windows.Threading.DispatcherPriority.Background, ProgressBar.ValueProperty, value);

            }
        }

        private void lineNum(object arg)
        {
            int i= (int)arg;
            if (_Model.FileName[i].EndsWith(".v"))
            {
                _Model.vlineCount(@_Model.FileName[i]);
            }
            else if (_Model.FileName[i].EndsWith(".vhd"))
            {
                _Model.vhdlineCount(@_Model.FileName[i]);
            }
        }
    }
}
