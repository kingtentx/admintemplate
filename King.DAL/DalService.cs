using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using King.Data;
using King.Interface;

namespace King.DAL
{
    /// <summary>
    /// 数据访问层：DAL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DalService<T> : IDalService<T> where T : class, new()
    {

        protected DbContext _dbContext;

        /// <summary>
        /// 获得数据库上下文
        /// </summary>
        /// <param name="dbContext">数据库上下文类</param>
        public DalService(DataBase dbContext)
        {
            _dbContext = dbContext;
        }

        #region 添加

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Add(List<T> list)
        {
            _dbContext.Set<T>().AddRange(list);
            return _dbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(List<T> list)
        {
            await _dbContext.Set<T>().AddRangeAsync(list);
            return await _dbContext.SaveChangesAsync() > 0;
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
            _dbContext.Set<T>().Update(entity);
            return _dbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool Update(List<T> list)
        {
            _dbContext.Set<T>().UpdateRange(list);
            return _dbContext.SaveChanges() > 0;
        }
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(List<T> list)
        {
            _dbContext.Set<T>().UpdateRange(list);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        #endregion

        #region 删除
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete<TKey>(TKey id)
        {
            var entity = _dbContext.Set<T>().Find(id);
            if (entity != null)
                _dbContext.Set<T>().Remove(entity);

            return _dbContext.SaveChanges() > 0;

        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync<TKey>(TKey id)
        {
            var entity = await _dbContext.Set<T>().FindAsync(id);
            if (entity != null)
                _dbContext.Set<T>().Remove(entity);

            return await _dbContext.SaveChangesAsync() > 0;

        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(Expression<Func<T, bool>> where)
        {
            var query = _dbContext.Set<T>().Where(where);
            _dbContext.Set<T>().RemoveRange(query);
            return _dbContext.SaveChanges() > 0;

        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Expression<Func<T, bool>> where)
        {
            var query = _dbContext.Set<T>().Where(where);
            _dbContext.Set<T>().RemoveRange(query);
            return await _dbContext.SaveChangesAsync() > 0;

        }
        #endregion

        #region 获取数据

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetOne<TKey>(TKey id)
        {
            return _dbContext.Set<T>().Find(id);
        }
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetOneAsync<TKey>(TKey id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        /// <summary>
        /// 条件获取单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T GetOne(Expression<Func<T, bool>> where)
        {
            var model = _dbContext.Set<T>().Where(where);
            return model != null ? model.FirstOrDefault() : null;
        }
        /// <summary>
        /// 条件获取单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public async Task<T> GetOneAsync(Expression<Func<T, bool>> where)
        {
            var model = _dbContext.Set<T>().Where(where);
            return await (model != null ? model.FirstOrDefaultAsync() : null);
        }

        /// <summary>
        /// 条件获取总条数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int GetCount(Expression<Func<T, bool>> where)
        {
            return _dbContext.Set<T>().Where(where).Count();
        }
        /// <summary>
        ///  条件获取实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IQueryable<T> GetModel(Expression<Func<T, bool>> where)
        {
            return _dbContext.Set<T>().Where(where);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public (IQueryable<T> Queryable, int Count) GetModel<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false)
        {
            int count = _dbContext.Set<T>().Where(where).Count();
            if (isAsc)
            {
                var queryable = _dbContext.Set<T>().OrderBy(orderBy).Where(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => p);
                return (queryable, count);
            }
            else
            {
                var queryable = _dbContext.Set<T>().OrderByDescending(orderBy).Where(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => p);
                return (queryable, count);
            }
        }

        /// <summary>
        /// 条件获取实体
        /// </summary>
        /// <param name="where"></param>
        /// <param name="Include"></param>
        /// <returns></returns>
        public IQueryable<T> GetModel(Expression<Func<T, bool>> where, string include)
        {
            return _dbContext.Set<T>().Where(where).Include(include);
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
            if (isAsc)
                return _dbContext.Set<T>().OrderBy(orderBy).Where(where);
            else
                return _dbContext.Set<T>().OrderByDescending(orderBy).Where(where);
        }

        /// <summary>
        ///  获取列表
        /// </summary>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> where)
        {
            return GetModel(where).ToList();
        }
        /// <summary>
        ///  获取列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>> where)
        {
            return await GetModel(where).ToListAsync();
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
            if (isAsc)
                return GetModel(where).OrderBy(orderBy).ToList();
            else
                return GetModel(where).OrderByDescending(orderBy).ToList();
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
            if (isAsc)
                return GetModel(where).OrderBy(orderBy).Take(topNum).ToList();
            else
                return GetModel(where).OrderByDescending(orderBy).Take(topNum).ToList();
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
            if (isAsc)
                return await GetModel(where).OrderBy(orderBy).ToListAsync();
            else
                return await GetModel(where).OrderByDescending(orderBy).ToListAsync();
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
        public async Task<List<T>> GetListAsync<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int topNum, bool isAsc = false)
        {
            if (isAsc)
                return await GetModel(where).OrderBy(orderBy).Take(topNum).ToListAsync();
            else
                return await GetModel(where).OrderByDescending(orderBy).Take(topNum).ToListAsync();
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
            int count = _dbContext.Set<T>().Where(where).Count();
            var list = _dbContext.Set<T>().Where(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => p);
            return (list.ToList(), count);
        }

        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<(List<T> List, int Count)> GetListAsync(Expression<Func<T, bool>> where, int pageIndex, int pageSize)
        {
            int count = await _dbContext.Set<T>().Where(where).CountAsync();
            var list = _dbContext.Set<T>().Where(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => p);
            return (await list.ToListAsync(), count);
        }

        /// <summary>
        /// 分页获取列表 [有排序]
        /// </summary>
        /// <typeparam name="K">排序列</typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>      
        /// <param name="isAsc">true:升序 , false:倒序</param>
        /// <returns></returns>
        public (List<T> List, int Count) GetList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false)
        {
            int count = _dbContext.Set<T>().Where(where).Count();
            if (isAsc)
            {
                var list = _dbContext.Set<T>().OrderBy(orderBy).Where(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => p);
                return (list.ToList(), count);
            }
            else
            {
                var list = _dbContext.Set<T>().OrderByDescending(orderBy).Where(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => p);
                return (list.ToList(), count);
            }
        }
        /// <summary>
        ///  分页获取列表 [有排序]
        /// </summary>
        /// <typeparam name="K">排序列</typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAsc">true:升序 , false:倒序</param>
        /// <returns></returns>
        public async Task<(List<T> List, int Count)> GetListAsync<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false)
        {
            int count = await _dbContext.Set<T>().Where(where).CountAsync();
            if (isAsc)
            {
                var list = _dbContext.Set<T>().OrderBy(orderBy).Where(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => p);
                return (await list.ToListAsync(), count);
            }
            else
            {
                var list = _dbContext.Set<T>().OrderByDescending(orderBy).Where(where).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => p);
                return (await list.ToListAsync(), count);
            }
        }

        public async Task<(List<T> List, int Count)> GetListAsync<TKey>(Expression<Func<T, bool>> where, string include, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false)
        {
            int count = await _dbContext.Set<T>().Where(where).CountAsync();
            if (isAsc)
            {
                var list = _dbContext.Set<T>().OrderBy(orderBy).Where(where).Include(include).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => p);
                return (await list.ToListAsync(), count);
            }
            else
            {
                var list = _dbContext.Set<T>().OrderByDescending(orderBy).Where(where).Include(include).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(p => p);
                return (await list.ToListAsync(), count);
            }
        }

        #endregion

        #region 自定义SQL
        /// <summary>
        /// 执行自定义SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSql(string sql, params object[] parameters)
        {
            return _dbContext.Database.ExecuteSqlRaw(sql, parameters);
        }
        /// <summary>
        /// 执行自定义SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<int> ExecuteSqlAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
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
            return _dbContext.Database.ExecuteScalar(sql, parameters);
        }
        /// <summary>
        /// 获取sql结果 第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<object> ExecuteScalarAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.ExecuteScalarAsync(sql, parameters);
        }

        /// <summary>
        /// 获取sql结果 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable SqlQuery(string sql, params object[] parameters)
        {
            return _dbContext.Database.SqlQuery(sql, parameters);
        }
        /// <summary>
        /// 获取sql结果 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DataTable> SqlQueryAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.SqlQueryAsync(sql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<T> SqlQueryList(string sql, params object[] parameters)
        {
            return _dbContext.Database.SqlQuery<T>(sql, parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<List<T>> SqlQueryListAsync(string sql, params object[] parameters)
        {
            return await _dbContext.Database.SqlQueryAsync<T>(sql, parameters);
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
            _dbContext.Database.BulkInsert(tableName, list);
        }
        #endregion
    }

}
