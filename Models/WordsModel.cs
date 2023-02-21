using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasyWord_DSSR.Models
{
    [Serializable]
    public class WordsModel
    {
        private string _de;
        private string _en;
        private bool _isCorrect = false;
        private int _bucketStatus = 3;
        public string De
        {
            get { return _de; }
            set { _de = value; }
        }

        public string En
        {
            get { return _en; }
            set { _en = value; }
        }

        public bool IsCorrect
        {
            get { return _isCorrect; }
            set { _isCorrect = value; }
        }

        public int BucketStatus
        {
            get { return _bucketStatus; }
            set { _bucketStatus = value; }
        }
    }
}
