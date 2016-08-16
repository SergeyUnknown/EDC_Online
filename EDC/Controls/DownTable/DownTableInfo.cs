using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC
{
    public class DownTableDataItem
    {
        string _postBackURL; //ccылка на страницы перехода

        public string PostBackURL
        {
            get { return _postBackURL; }
            set
            {
                if (value.Length > 0)
                {
                    if (value.Last() == '/')
                        _postBackURL = value;
                    else
                        _postBackURL = value + "/";
                }
                else
                    _postBackURL = "~/";
            }
        }

        int _currentPageIndex = 1;//номер текущей страницы

        public int CurrentPageIndex
        {
            get { return _currentPageIndex; }
            set
            {
                if (value >= 1)
                    _currentPageIndex = value;
                else
                    _currentPageIndex = 1;
            }
        }

        int _maxPageIndex = 1;//номер максимальной страницы

        public int MaxPageIndex
        {
            get { return _maxPageIndex; }
            set
            {
                if (value >= 1)
                    _maxPageIndex = value;
                else
                    _maxPageIndex = 1;
            }
        }

        List<int> _dropDownItems;//элементы в dropDown

        public List<int> DropDownItems
        {
            get { return _dropDownItems; }
            set { _dropDownItems = value; }
        }

        int _dropDownSelectedIndex;//индекс в dropDown

        public int DropDownSelectedValue
        {
            get { return _dropDownSelectedIndex; }
            set { _dropDownSelectedIndex = value; }
        }

        string _buttonText = "";

        public string ButtonText //текст кнопки
        {
            get { return _buttonText; }
            set { _buttonText = value; }
        }

        string _buttonURL = "";

        public string ButtonURL
        {
            get { return _buttonURL; }
            set { _buttonURL = value; }
        }

        string _pageInfo = "";

        public string PageInfo
        {
            get { return _pageInfo; }
            set { _pageInfo = value; }
        }

        public string FirstPageURL
        {
            get { return _postBackURL + "1"; }
        }

        public string PrevPageURL
        {
            get
            {
                if (_currentPageIndex > 1)
                    return _postBackURL + (_currentPageIndex - 1).ToString();
                else
                    return FirstPageURL;
            }
        }

        public string NextPageURL
        {
            get
            {
                if (_currentPageIndex < _maxPageIndex)
                    return _postBackURL + (_currentPageIndex + 1).ToString();
                else
                    return LastPageURL;
            }
        }
        public string LastPageURL
        {
            get { return _postBackURL + _maxPageIndex.ToString(); }
        }

        public DownTableDataItem(string buttonText,
            string buttonURL,
            int currentPageIndex,
            int maxPageIndex,
            string postBackURL,
            List<int> dropDownItems,
            int dropDownSelectedIndex,
            string pageInfo)
        {
            _buttonURL = buttonURL;
            _buttonText = buttonText;
            _currentPageIndex = currentPageIndex;
            _maxPageIndex = maxPageIndex;
            _postBackURL = postBackURL;
            _dropDownItems = dropDownItems;
            _dropDownSelectedIndex = dropDownSelectedIndex;
            _pageInfo = pageInfo;
        }
        public DownTableDataItem(int currentPageIndex,
            int maxPageIndex,
            string postBackURL,
            List<int> dropDownItems,
            int dropDownSelectedIndex,
            string pageInfo)
        {
            _buttonURL = "";
            _buttonText = "";
            _currentPageIndex = currentPageIndex;
            _maxPageIndex = maxPageIndex;
            _postBackURL = postBackURL;
            _dropDownItems = dropDownItems;
            _dropDownSelectedIndex = dropDownSelectedIndex;
            _pageInfo = pageInfo;
        }
    }
}