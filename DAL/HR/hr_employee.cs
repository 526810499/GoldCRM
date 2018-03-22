
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using XHD.DBUtility;//Please add references
namespace XHD.DAL
{
    /// <summary>
    /// 数据访问类:hr_employee
    /// </summary>
    public partial class hr_employee
    {
        public hr_employee()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(XHD.Model.hr_employee model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hr_employee(");
            strSql.Append("id,uid,pwd,name,idcard,birthday,dep_id,post_id,email,sex,tel,status,position_id,sort,EntryDate,address,remarks,education,level,professional,schools,title,portal,theme,canlogin,create_id,create_time)");
            strSql.Append(" values (");
            strSql.Append("@id,@uid,@pwd,@name,@idcard,@birthday,@dep_id,@post_id,@email,@sex,@tel,@status,@position_id,@sort,@EntryDate,@address,@remarks,@education,@level,@professional,@schools,@title,@portal,@theme,@canlogin,@create_id,@create_time)");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50),
                    new SqlParameter("@uid", SqlDbType.VarChar,50),
                    new SqlParameter("@pwd", SqlDbType.VarChar,50),
                    new SqlParameter("@name", SqlDbType.NVarChar,50),
                    new SqlParameter("@idcard", SqlDbType.VarChar,50),
                    new SqlParameter("@birthday", SqlDbType.VarChar,50),
                    new SqlParameter("@dep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@post_id", SqlDbType.VarChar,50),
                    new SqlParameter("@email", SqlDbType.VarChar,50),
                    new SqlParameter("@sex", SqlDbType.NVarChar,50),
                    new SqlParameter("@tel", SqlDbType.VarChar,50),
                    new SqlParameter("@status", SqlDbType.NVarChar,50),
                    new SqlParameter("@position_id", SqlDbType.VarChar,50),
                    new SqlParameter("@sort", SqlDbType.Int,4),
                    new SqlParameter("@EntryDate", SqlDbType.VarChar,50),
                    new SqlParameter("@address", SqlDbType.NVarChar,255),
                    new SqlParameter("@remarks", SqlDbType.NVarChar,255),
                    new SqlParameter("@education", SqlDbType.NVarChar,50),
                    new SqlParameter("@level", SqlDbType.VarChar,50),
                    new SqlParameter("@professional", SqlDbType.NVarChar,50),
                    new SqlParameter("@schools", SqlDbType.NVarChar,255),
                    new SqlParameter("@title", SqlDbType.NVarChar,255),
                    new SqlParameter("@portal", SqlDbType.VarChar,250),
                    new SqlParameter("@theme", SqlDbType.VarChar,250),
                    new SqlParameter("@canlogin", SqlDbType.Int,4),
                    new SqlParameter("@create_id", SqlDbType.VarChar,50),
                    new SqlParameter("@create_time", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.uid;
            parameters[2].Value = model.pwd;
            parameters[3].Value = model.name;
            parameters[4].Value = model.idcard;
            parameters[5].Value = model.birthday;
            parameters[6].Value = model.dep_id;
            parameters[7].Value = model.post_id;
            parameters[8].Value = model.email;
            parameters[9].Value = model.sex;
            parameters[10].Value = model.tel;
            parameters[11].Value = model.status;
            parameters[12].Value = model.position_id;
            parameters[13].Value = model.sort;
            parameters[14].Value = model.EntryDate;
            parameters[15].Value = model.address;
            parameters[16].Value = model.remarks;
            parameters[17].Value = model.education;
            parameters[18].Value = model.level;
            parameters[19].Value = model.professional;
            parameters[20].Value = model.schools;
            parameters[21].Value = model.title;
            parameters[22].Value = model.portal;
            parameters[23].Value = model.theme;
            parameters[24].Value = model.canlogin;
            parameters[25].Value = model.create_id;
            parameters[26].Value = model.create_time;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(XHD.Model.hr_employee model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update hr_employee set ");
            strSql.Append("uid=@uid,");
            strSql.Append("name=@name,");
            strSql.Append("idcard=@idcard,");
            strSql.Append("birthday=@birthday,");
            strSql.Append("dep_id=@dep_id,");
            strSql.Append("post_id=@post_id,");
            strSql.Append("email=@email,");
            strSql.Append("sex=@sex,");
            strSql.Append("tel=@tel,");
            strSql.Append("status=@status,");
            strSql.Append("position_id=@position_id,");
            strSql.Append("sort=@sort,");
            strSql.Append("EntryDate=@EntryDate,");
            strSql.Append("address=@address,");
            strSql.Append("remarks=@remarks,");
            strSql.Append("education=@education,");
            strSql.Append("level=@level,");
            strSql.Append("professional=@professional,");
            strSql.Append("schools=@schools,");
            strSql.Append("title=@title,");
            strSql.Append("portal=@portal,");
            strSql.Append("theme=@theme,");
            strSql.Append("canlogin=@canlogin");
            strSql.Append(" where id=@id  ");
            SqlParameter[] parameters = {
                    new SqlParameter("@uid", SqlDbType.VarChar,50),
                    new SqlParameter("@pwd", SqlDbType.VarChar,50),
                    new SqlParameter("@name", SqlDbType.NVarChar,50),
                    new SqlParameter("@idcard", SqlDbType.VarChar,50),
                    new SqlParameter("@birthday", SqlDbType.VarChar,50),
                    new SqlParameter("@dep_id", SqlDbType.VarChar,50),
                    new SqlParameter("@post_id", SqlDbType.VarChar,50),
                    new SqlParameter("@email", SqlDbType.VarChar,50),
                    new SqlParameter("@sex", SqlDbType.NVarChar,50),
                    new SqlParameter("@tel", SqlDbType.VarChar,50),
                    new SqlParameter("@status", SqlDbType.NVarChar,50),
                    new SqlParameter("@position_id", SqlDbType.VarChar,50),
                    new SqlParameter("@sort", SqlDbType.Int,4),
                    new SqlParameter("@EntryDate", SqlDbType.VarChar,50),
                    new SqlParameter("@address", SqlDbType.NVarChar,255),
                    new SqlParameter("@remarks", SqlDbType.NVarChar,255),
                    new SqlParameter("@education", SqlDbType.NVarChar,50),
                    new SqlParameter("@level", SqlDbType.VarChar,50),
                    new SqlParameter("@professional", SqlDbType.NVarChar,50),
                    new SqlParameter("@schools", SqlDbType.NVarChar,255),
                    new SqlParameter("@title", SqlDbType.NVarChar,255),
                    new SqlParameter("@portal", SqlDbType.VarChar,250),
                    new SqlParameter("@theme", SqlDbType.VarChar,250),
                    new SqlParameter("@canlogin", SqlDbType.Int,4),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = model.uid;
            parameters[1].Value = model.pwd;
            parameters[2].Value = model.name;
            parameters[3].Value = model.idcard;
            parameters[4].Value = model.birthday;
            parameters[5].Value = model.dep_id;
            parameters[6].Value = model.post_id;
            parameters[7].Value = model.email;
            parameters[8].Value = model.sex;
            parameters[9].Value = model.tel;
            parameters[10].Value = model.status;
            parameters[11].Value = model.position_id;
            parameters[12].Value = model.sort;
            parameters[13].Value = model.EntryDate;
            parameters[14].Value = model.address;
            parameters[15].Value = model.remarks;
            parameters[16].Value = model.education;
            parameters[17].Value = model.level;
            parameters[18].Value = model.professional;
            parameters[19].Value = model.schools;
            parameters[20].Value = model.title;
            parameters[21].Value = model.portal;
            parameters[22].Value = model.theme;
            parameters[23].Value = model.canlogin;
            parameters[24].Value = model.id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from hr_employee ");
            strSql.Append(" where id=@id  ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)           };
            parameters[0].Value = id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XHD.Model.hr_employee GetModel(string id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,uid,pwd,name,idcard,birthday,dep_id,post_id,email,sex,tel,status,position_id,sort,EntryDate,address,remarks,education,level,professional,schools,title,portal,theme,canlogin,create_id,create_time,default_city from hr_employee(nolock) ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.VarChar,50)           };
            parameters[0].Value = id;

            XHD.Model.hr_employee model = new XHD.Model.hr_employee();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XHD.Model.hr_employee DataRowToModel(DataRow row)
        {
            XHD.Model.hr_employee model = new XHD.Model.hr_employee();
            if (row != null)
            {
                if (row["id"] != null)
                {
                    model.id = row["id"].ToString();
                }
                if (row["uid"] != null)
                {
                    model.uid = row["uid"].ToString();
                }
                if (row["pwd"] != null)
                {
                    model.pwd = row["pwd"].ToString();
                }
                if (row["name"] != null)
                {
                    model.name = row["name"].ToString();
                }
                if (row["idcard"] != null)
                {
                    model.idcard = row["idcard"].ToString();
                }
                if (row["birthday"] != null)
                {
                    model.birthday = row["birthday"].ToString();
                }
                if (row["dep_id"] != null)
                {
                    model.dep_id = row["dep_id"].ToString();
                }
                if (row["post_id"] != null)
                {
                    model.post_id = row["post_id"].ToString();
                }
                if (row["email"] != null)
                {
                    model.email = row["email"].ToString();
                }
                if (row["sex"] != null)
                {
                    model.sex = row["sex"].ToString();
                }
                if (row["tel"] != null)
                {
                    model.tel = row["tel"].ToString();
                }
                if (row["status"] != null)
                {
                    model.status = row["status"].ToString();
                }
                if (row["position_id"] != null)
                {
                    model.position_id = row["position_id"].ToString();
                }
                if (row["sort"] != null && row["sort"].ToString() != "")
                {
                    model.sort = int.Parse(row["sort"].ToString());
                }
                if (row["EntryDate"] != null)
                {
                    model.EntryDate = row["EntryDate"].ToString();
                }
                if (row["address"] != null)
                {
                    model.address = row["address"].ToString();
                }
                if (row["remarks"] != null)
                {
                    model.remarks = row["remarks"].ToString();
                }
                if (row["education"] != null)
                {
                    model.education = row["education"].ToString();
                }
                if (row["level"] != null)
                {
                    model.level = row["level"].ToString();
                }
                if (row["professional"] != null)
                {
                    model.professional = row["professional"].ToString();
                }
                if (row["schools"] != null)
                {
                    model.schools = row["schools"].ToString();
                }
                if (row["title"] != null)
                {
                    model.title = row["title"].ToString();
                }
                if (row["portal"] != null)
                {
                    model.portal = row["portal"].ToString();
                }
                if (row["theme"] != null)
                {
                    model.theme = row["theme"].ToString();
                }
                if (row["canlogin"] != null && row["canlogin"].ToString() != "")
                {
                    model.canlogin = int.Parse(row["canlogin"].ToString());
                }

                if (row["default_city"] != null)
                {
                    model.default_city = row["default_city"].ToString();
                }

                if (row["create_id"] != null)
                {
                    model.create_id = row["create_id"].ToString();
                }
                if (row["create_time"] != null && row["create_time"].ToString() != "")
                {
                    model.create_time = DateTime.Parse(row["create_time"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select id,uid,name,idcard,birthday,dep_id,post_id,email,sex,tel,status,position_id,sort,EntryDate,address,remarks,education,level,professional,schools,title,portal,theme,canlogin,create_id,create_time,default_city ");
            strSql.Append(",(select dep_name from hr_department where id = hr_employee.dep_id) as dep_name ");
            strSql.Append(",(select position_name from hr_position where id = hr_employee.position_id) as position_name ");
            strSql.Append(",(select post_name from hr_post where id = hr_employee.post_id) as post_name ");

            strSql.Append(" FROM hr_employee ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" id,uid,name,idcard,birthday,dep_id,post_id,email,sex,tel,status,position_id,sort,EntryDate,address,remarks,education,level,professional,schools,title,portal,theme,canlogin,create_id,create_time,default_city ");
            strSql.Append(",(select dep_name from hr_department where id = hr_employee.dep_id) as dep_name ");
            strSql.Append(",(select position_name from hr_position where id = hr_employee.position_id) as position_name ");
            strSql.Append(",(select post_name from hr_post where id = hr_employee.post_id) as post_name ");

            strSql.Append(" FROM hr_employee ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
        {
            StringBuilder strSql_grid = new StringBuilder();
            StringBuilder strSql_total = new StringBuilder();
            strSql_total.Append(" SELECT COUNT(id) as n FROM hr_employee ");
            strSql_grid.Append("SELECT ");
            strSql_grid.Append("      n,id,uid,name,idcard,birthday,dep_id,post_id,email,sex,tel,status,position_id,sort,EntryDate,address,remarks,education,level,professional,schools,title,portal,theme,canlogin,create_id,create_time,default_city ");
            strSql_grid.Append(",(select dep_name from hr_department where id = w1.dep_id) as dep_name ");
            strSql_grid.Append(",(select position_name from hr_position where id = w1.position_id) as position_name ");
            strSql_grid.Append(",(select post_name from hr_post where id = w1.post_id) as post_name ");
            strSql_grid.Append(" FROM ( SELECT id,uid,name,idcard,birthday,dep_id,post_id,email,sex,tel,status,position_id,sort,EntryDate,address,remarks,education,level,professional,schools,title,portal,theme,canlogin,create_id,create_time,default_city, ROW_NUMBER() OVER( Order by " + filedOrder + " ) AS n from hr_employee");
            if (strWhere.Trim() != "")
            {
                strSql_grid.Append(" WHERE " + strWhere);
                strSql_total.Append(" WHERE " + strWhere);
            }
            strSql_grid.Append("  ) as w1  ");
            strSql_grid.Append("WHERE n BETWEEN " + (PageSize * (PageIndex - 1) + 1) + " AND " + PageSize * PageIndex);
            strSql_grid.Append(" ORDER BY " + filedOrder);
            Total = DbHelperSQL.Query(strSql_total.ToString()).Tables[0].Rows[0]["n"].ToString();
            return DbHelperSQL.Query(strSql_grid.ToString());
        }

        #endregion  BasicMethod
        #region  ExtensionMethod
        /// <summary>
        ///     获取密码
        /// </summary>
        public DataSet GetPWD(string id)
        {
            var strSql = new StringBuilder();
            strSql.Append("select pwd ");
            strSql.Append(" FROM hr_employee ");
            strSql.Append(" WHERE id = @id");

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", SqlDbType.VarChar, 50)
            };

            parameters[0].Value = id;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }

        public bool changepwd(Model.hr_employee model)
        {
            var strSql = new StringBuilder();
            strSql.Append("update hr_employee set ");
            strSql.Append("pwd=@pwd");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters =
            {
                new SqlParameter("@pwd", SqlDbType.VarChar, 50),
                new SqlParameter("@id", SqlDbType.VarChar, 50)
            };

            parameters[0].Value = model.pwd;
            parameters[1].Value = model.id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     更新岗位
        /// </summary>
        public bool UpdatePost(Model.hr_employee model)
        {
            var strSql = new StringBuilder();
            strSql.Append("update hr_employee set ");

            strSql.Append("dep_id=@dep_id,");
            strSql.Append("post_id=@postid,");
            strSql.Append("position_id=@position_id");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters =
            {
                new SqlParameter("@dep_id", SqlDbType.VarChar, 50),
                new SqlParameter("@postid", SqlDbType.VarChar, 50),
                new SqlParameter("@position_id", SqlDbType.VarChar, 50),
                new SqlParameter("@id", SqlDbType.VarChar, 50)
            };

            parameters[0].Value = model.dep_id;
            parameters[1].Value = model.post_id;
            parameters[2].Value = model.position_id;
            parameters[3].Value = model.id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        ///     更新默认城市
        /// </summary>
        public bool UpdateDefaultCity(Model.hr_employee model)
        {
            var strSql = new StringBuilder();
            strSql.Append("update hr_employee set ");

            strSql.Append("default_city=@default_city");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters =
            {
                new SqlParameter("@default_city", SqlDbType.VarChar, 50),
                new SqlParameter("@id", SqlDbType.VarChar, 50)
            };

            parameters[0].Value = model.default_city;
            parameters[1].Value = model.id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        ///     获取角色
        /// </summary>
        public DataSet GetRole(string id)
        {
            var strSql = new StringBuilder();
            strSql.Append("select * from Sys_role where Roleid in ");
            strSql.Append("(select Roleid from Sys_role_emp where empid=@id)  ");

            SqlParameter[] parameters =
            {
                new SqlParameter("@id", SqlDbType.VarChar, 50)
            };

            parameters[0].Value = id;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        ///     个人信息修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool PersonalUpdate(Model.hr_employee model)
        {
            var strSql = new StringBuilder();
            strSql.Append("update hr_employee set ");
            strSql.Append("name=@name,");
            strSql.Append("idcard=@idcard,");
            strSql.Append("birthday=@birthday,");
            strSql.Append("email=@email,");
            strSql.Append("sex=@sex,");
            strSql.Append("tel=@tel,");
            strSql.Append("address=@address,");
            strSql.Append("education=@education,");
            strSql.Append("professional=@professional,");
            strSql.Append("schools=@schools,");
            strSql.Append("title=@title");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters =
            {
                new SqlParameter("@name", SqlDbType.VarChar, 50),
                new SqlParameter("@idcard", SqlDbType.VarChar, 50),
                new SqlParameter("@birthday", SqlDbType.VarChar, 50),
                new SqlParameter("@email", SqlDbType.VarChar, 50),
                new SqlParameter("@sex", SqlDbType.VarChar, 50),
                new SqlParameter("@tel", SqlDbType.VarChar, 50),
                new SqlParameter("@address", SqlDbType.VarChar, 255),
                new SqlParameter("@education", SqlDbType.VarChar, 50),
                new SqlParameter("@professional", SqlDbType.VarChar, 50),
                new SqlParameter("@schools", SqlDbType.VarChar, 255),
                new SqlParameter("@title", SqlDbType.VarChar, 255),
                new SqlParameter("@id", SqlDbType.VarChar, 50)
            };
            parameters[0].Value = model.name;
            parameters[1].Value = model.idcard;
            parameters[2].Value = model.birthday;
            parameters[3].Value = model.email;
            parameters[4].Value = model.sex;
            parameters[5].Value = model.tel;
            parameters[6].Value = model.address;
            parameters[7].Value = model.education;
            parameters[8].Value = model.professional;
            parameters[9].Value = model.schools;
            parameters[10].Value = model.title;
            parameters[11].Value = model.id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新最后登录
        /// </summary>
        /// <param name="last_login"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool lastlogin(DateTime last_login, string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_CRM_Customer set ");
            strSql.Append("last_login=@last_login");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@last_login", SqlDbType.DateTime),
                    new SqlParameter("@id", SqlDbType.VarChar,50)};
            parameters[0].Value = last_login;
            parameters[1].Value = id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListWithCompany(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM hr_employee ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }
        #endregion  ExtensionMethod
    }
}

