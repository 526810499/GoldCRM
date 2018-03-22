
using System;
namespace XHD.Model
{
    /// <summary>
    /// Sys_data_authority:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Sys_data_authority
    {
        public Sys_data_authority()
        { }
        #region Model
        private string _role_id;
        private string _dep_id;
        
        private string _create_id;
        private DateTime? _create_time;
        /// <summary>
        /// 
        /// </summary>
        public string Role_id
        {
            set { _role_id = value; }
            get { return _role_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string dep_id
        {
            set { _dep_id = value; }
            get { return _dep_id; }
        }
       
        /// <summary>
        /// 
        /// </summary>
        public string create_id
        {
            set { _create_id = value; }
            get { return _create_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        #endregion Model

    }
}

