
using System;
namespace XHD.Model
{
    /// <summary>
    /// Sys_role:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Sys_role
    {
        public Sys_role()
        { }
        #region Model
        private string _id;
        private string _rolename;
        private string _roledscript;
        private int? _rolesort;
        private int? _dataauth;
        private int? _publicauth;
        
        private string _create_id;
        private DateTime? _create_time;
        /// <summary>
        /// 
        /// </summary>
        public string id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RoleName
        {
            set { _rolename = value; }
            get { return _rolename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RoleDscript
        {
            set { _roledscript = value; }
            get { return _roledscript; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? RoleSort
        {
            set { _rolesort = value; }
            get { return _rolesort; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DataAuth
        {
            set { _dataauth = value; }
            get { return _dataauth; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PublicAuth
        {
            set { _publicauth = value; }
            get { return _publicauth; }
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

