using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using King.Interface;

namespace King.BLL
{
    /// <summary>
    /// 数据逻辑层：BLL
    /// </summary>
    public class BllService<T> : IBllService<T> where T : class, new()
    {

        /// <summary>
        /// 数据库服务
        /// </summary>
        protected IDalService<T> _dalService;

        public BllService(IDalService<T> dalService)
        {
            this._dalService = dalService;
        }

        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public T Add(T entity)
        {
            return _dalService.Add(entity);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity)
        {
            return await _dalService.AddAsync(entity);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Add(List<T> list)
        {
            return _dalService.Add(list);
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(List<T> list)
        {
            return await _dalService.AddAsync(list);
        }

        #endregion

        #region 更新

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            return _dalService.Update(entity);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            return await _dalService.UpdateAsync(entity);
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Update(List<T> list)
        {
            return _dalService.Update(list);
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(List<T> list)
        {
            return await _dalService.UpdateAsync(list);
        }
        #endregion

        #region  删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<TKey>(TKey id)
        {
            return _dalService.Delete(id);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<TKey>(TKey id)
        {
            return await _dalService.DeleteAsync(id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> where)
        {
            return _dalService.Delete(where);
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> where)
        {
            return await _dalService.DeleteAsync(where);
        }
        #endregion

        #region  获取数据

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetOne<TKey>(TKey id)
        {
            return _dalService.GetOne(id);
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetOneAsync<TKey>(TKey id)
        {
            return await _dalService.GetOneAsync(id);
        }

        /// <summary>
        /// 条件获取单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T GetOne(Expression<Func<T, bool>> where)
        {
            return _dalService.GetOne(where);
        }
        /// <summary>
        /// 条件获取单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<T> GetOneAsync(Expression<Func<T, bool>> where)
        {
            return await _dalService.GetOneAsync(where);
        }
        /// <summary>
        /// 条件获取总条数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetCount(Expression<Func<T, bool>> where)
        {
            return _dalService.GetCount(where);
        }
        /// <summary>
        /// 条件获取实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IQueryable<T> GetModel(Expression<Func<T, bool>> where)
        {
            return _dalService.GetModel(where);
        }

        public (IQueryable<T> Queryable, int Count) GetModel<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false)
        {
            return _dalService.GetModel(where, orderBy, pageIndex, pageSize, isAsc);
        }
        /// <summary>
        /// 条件获取实体
        /// </summary>
        /// <param name="where"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<T> GetModel(Expression<Func<T, bool>> where, string include)
        {
            return _dalService.GetModel(where, include);
        }
        /// <summary>
        /// 条件获取实体
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc">true:升序 , false:倒序 </param>
        /// <returns></returns>
        public IQueryable<T> GetModel<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, bool isAsc = false)
        {
            return _dalService.GetModel(where, orderBy, isAsc);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> where)
        {
            return _dalService.GetList(where);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where)
        {
            return await _dalService.GetListAsync(where);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public List<T> GetList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, bool isAsc = false)
        {
            return _dalService.GetList(where, orderBy, isAsc);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="topNum"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public List<T> GetList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int topNum, bool isAsc = false)
        {
            return _dalService.GetList(where, orderBy, topNum, isAsc);
        }
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, bool isAsc = false)
        {
            return await _dalService.GetListAsync(where, orderBy, isAsc);
        }
        /// <summary>
        ///  获取列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="topNum"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int topNum, bool isAsc = false)
        {
            return await _dalService.GetListAsync(where, orderBy, topNum, isAsc);
        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public (List<T> List, int Count) GetList(Expression<Func<T, bool>> where, int pageIndex, int pageSize)
        {
            return _dalService.GetList(where, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<(List<T> List, int Count)> GetListAsync(Expression<Func<T, bool>> where, int pageIndex, int pageSize)
        {
            return await _dalService.GetListAsync(where, pageIndex, pageSize);
        }

        /// <summary>
        /// 分页获取列表 [有排序]
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public (List<T> List, int Count) GetList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false)
        {
            return _dalService.GetList(where, orderBy, pageIndex, pageSize, isAsc);
        }
        /// <summary>
        /// 分页获取列表 [有排序]
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAsc">true:升序 , false:倒序</param>
        /// <returns></returns>
        public async Task<(List<T> List, int Count)> GetListAsync<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false)
        {
            return await _dalService.GetListAsync(where, orderBy, pageIndex, pageSize, isAsc);
        }
        /// <summary>
        /// 分页获取列表 [有排序]
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="include"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public async Task<(List<T> List, int Count)> GetListAsync<TKey>(Expression<Func<T, bool>> where, string include, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false)
        {
            return await _dalService.GetListAsync(where, include, orderBy, pageIndex, pageSize, isAsc);
        }
        #endregion

        #region 自定义SQL
        /// <summary>
        /// 执行自定义SQL语句
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql, params object[] parameters)
        {
            return _dalService.ExecuteSql(sql, parameters);
        }
        /// <summary>
        /// 执行自定义SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string sql, params object[] parameters)
        {
            return await _dalService.ExecuteSqlAsync(sql, parameters);
        }

        #endregion

        #region 扩展方法
        /// <summary>
        /// 获取sql结果 第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params object[] parameters)
        {
            return _dalService.ExecuteScalar(sql, parameters);
        }
        /// <summary>
        /// 获取sql结果 第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(string sql, params object[] parameters)
        {
            return await _dalService.ExecuteScalarAsync(sql, parameters);
        }

        /// <summary>
        /// 获取sql结果 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable SqlQuery(string sql, params object[] parameters)
        {
            return _dalService.SqlQuery(sql, parameters);
        }
        /// <summary>
        /// 获取sql结果 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> SqlQueryAsync(string sql, params object[] parameters)
        {
            return await _dalService.SqlQueryAsync(sql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> SqlQueryList(string sql, params object[] parameters)
        {
            return _dalService.SqlQueryList(sql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> SqlQueryListAsync(string sql, params object[] parameters)
        {
            return await _dalService.SqlQueryListAsync(sql, parameters);
        }


        /// <summary>
        /// 批量插入 [仅支持MSSQL Server]
        /// </summary>
        /// <typeparam name="T">泛型集合的类型</typeparam>
        /// <param name="conn">连接对象</param>
        /// <param name="tableName">将泛型集合插入到本地数据库表的表名</param>
        /// <param name="list">要插入大泛型集合</param>
        public void BulkInsert(string tableName, IList<T> list)
        {
            _dalService.BulkInsert(tableName, list);
        }
        #endregion

    }
}
