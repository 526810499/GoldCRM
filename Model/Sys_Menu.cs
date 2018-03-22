
using System;
namespace XHD.Model
{
    /// <summary>
    /// Sys_Menu:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Sys_Menu
    {
        public Sys_Menu()
        { }
        #region Model
        private string _menu_id;
        private string _menu_name;
        private string _parentid;
        private string _app_id;
        private string _menu_url;
        private string _menu_icon;
        private int? _menu_order;
        private string _menu_type;
        private int? _ismobile;
        private string _m_css;
        private string _m_color;
       
        /// <summary>
        /// 
        /// </summary>
        public string Menu_id
        {
            set { _menu_id = value; }
            get { return _menu_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Menu_name
        {
            set { _menu_name = value; }
            get { return _menu_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string parentid
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string App_id
        {
            set { _app_id = value; }
            get { return _app_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Menu_url
        {
            set { _menu_url = value; }
            get { return _menu_url; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Menu_icon
        {
            set { _menu_icon = value; }
            get { return _menu_icon; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Menu_order
        {
            set { _menu_order = value; }
            get { return _menu_order; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Menu_type
        {
            set { _menu_type = value; }
            get { return _menu_type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? isMobile
        {
            set { _ismobile = value; }
            get { return _ismobile; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string m_css
        {
            set { _m_css = value; }
            get { return _m_css; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string m_color
        {
            set { _m_color = value; }
            get { return _m_color; }
        }
        
        #endregion Model

    }
}

