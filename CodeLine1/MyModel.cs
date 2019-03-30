using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeLine1
{
    class MyModel : INotifyPropertyChanged
    {
        public void load_folder(string aPath)//选择文件夹
        {
            DirectoryInfo dir = new DirectoryInfo(aPath);

            FileInfo[] files = dir.GetFiles();//目录下文件
            DirectoryInfo[] dii = dir.GetDirectories();//目录下子文件夹
           
            foreach (FileInfo info in files)
            {
                if (Regex.IsMatch(info.Name, @".*\.v$") || Regex.IsMatch(info.Name, @".*\.vhd$"))
                {
                    FileName.Add(info.FullName);
                }
            }

            foreach (DirectoryInfo d in dii)
            {
                load_folder(d.FullName);
            }
            FileNum = FileName.Count();

        }

        public void load_file(string aPath)//选择文件
        {
            StreamReader sr = File.OpenText(aPath);
            string s = "";

            while ((s = sr.ReadLine()) != null)
            {
                s = s.Trim();

                if (!s.StartsWith("//") && !Regex.IsMatch(s, @"^[\s]*$"))
                {
                    FileName.Add(s);
                }
            }
            FileNum = FileName.Count();
        }

        public void vhdlineCount(string path)
        {
            StreamReader sr = File.OpenText(path);
            string s = "";

            while ((s = sr.ReadLine()) != null)
            {
                s = s.Trim();
                AllNum++;

                if (Regex.IsMatch(s, @"^[\s]*$"))//匹配空白行
                {
                    WhiteNum++;
                }
                else if (s.StartsWith("--"))
                {
                    CommentNum++;
                }
                else
                    EffectNum++;
            }
            
        }

        public void vlineCount(string path)
        {
            StreamReader sr = File.OpenText(path);
            string s = "";

            while ((s = sr.ReadLine()) != null)
            {
                s = s.Trim();
                AllNum++;

                if (Regex.IsMatch(s, @"^[\s]*$"))//匹配空白行
                {
                    WhiteNum++;
                }
                else if (s.StartsWith("//"))
                {
                    CommentNum++;
                }
                else if (s.StartsWith("/*") && s.EndsWith("*/"))
                {
                    CommentNum++;
                }
                else if (s.StartsWith("/*") && !s.EndsWith("*/"))
                {
                    CommentNum++;
                    comment = true;
                }
                else if (comment == true)
                {
                    CommentNum++;
                    if (s.EndsWith("*/"))
                        comment = false;
                }
                else
                    EffectNum++;
            }
          //  CommentRate = (double)CommentNum / (double)AllNum;
        }

        private string _Path;
        private ObservableCollection<string> _FileName = new ObservableCollection<string>();
        private long _AllNum;
        private long _WhiteNum;
        private long _EffectNum;
        private long _CommentNum;
        private string _CommentRate;
        private bool comment = false;
        private long _FileNum;

        public string Path
        {
            get { return _Path; }
            set
            {
                _Path = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Path"));
            }
        }

        public long AllNum
        {
            get { return _AllNum; }
            set
            {
                _AllNum = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("AllNum"));
            }
        }

        public long WhiteNum
        {
            get { return _WhiteNum; }
            set
            {
                _WhiteNum = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("WhiteNum"));
            }
        }

        public long EffectNum
        {
            get { return _EffectNum; }
            set
            {
                _EffectNum = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("EffectNum"));
            }
        }

        public long CommentNum
        {
            get { return _CommentNum; }
            set
            {
                _CommentNum = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("CommentNum"));
            }
        }

        public string CommentRate
        {
            get { return _CommentRate; }
            set
            {
                _CommentRate = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("CommentRate"));
            }
        }

        public ObservableCollection<string> FileName
        {
            get { return _FileName; }
            set
            {
                _FileName = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("FileName"));
            }
        }

        public long FileNum
        {
            get { return _FileNum; }
            set
            {
                _FileNum = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("FileNum"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
