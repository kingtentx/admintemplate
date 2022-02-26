﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace King.Interface
{
    public interface IBllService<T> where T : class, new()
    {
        #region 添加
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add(T entity);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool Add(List<T> list);
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> AddAsync(List<T> list);

        #endregion

        #region 更新
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(T entity);
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        bool Update(List<T> list);
        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        Task<bool> UpdateAsync(List<T> list);

        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete<TKey>(TKey id);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync<TKey>(TKey id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        bool Delete(Expression<Func<T, bool>> where);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Expression<Func<T, bool>> where);

        #endregion

        #region  获取数据        

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetOne<TKey>(TKey id);
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetOneAsync<TKey>(TKey id);

        /// <summary>
        /// 条件获取单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        T GetOne(Expression<Func<T, bool>> where);
        /// <summary>
        /// 条件获取单个实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        Task<T> GetOneAsync(Expression<Func<T, bool>> where);
        /// <summary>
        /// 条件获取总条数
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        int GetCount(Expression<Func<T, bool>> where);
        /// <summary>
        /// 条件获取实体
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IQueryable<T> GetModel(Expression<Func<T, bool>> where);
        /// <summary>
        ///  条件获取实体
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        (IQueryable<T> Queryable, int Count) GetModel<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false);

        /// <summary>
        /// 条件获取实体
        /// </summary>
        /// <param name="where"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        IQueryable<T> GetModel(Expression<Func<T, bool>> where, string include);
        /// <summary>
        /// 条件获取实体
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc">true:升序 , false:倒序 </param>
        /// <returns></returns>
        IQueryable<T> GetModel<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, bool isAsc = false);

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        List<T> GetList(Expression<Func<T, bool>> where);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetListAsync(Expression<Func<T, bool>> where);
        /// <summary>
        ///  获取列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        List<T> GetList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, bool isAsc = false);
        /// <summary>
        ///  获取列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="topNum"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        List<T> GetList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int topNum, bool isAsc = false);
        /// <summary>
        ///  获取列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, bool isAsc = false);
        /// <summary>
        ///  获取列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="topNum"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        Task<List<T>> GetListAsync<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int topNum, bool isAsc = false);
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        (List<T> List, int Count) GetList(Expression<Func<T, bool>> where, int pageIndex, int pageSize);
        /// <summary>
        /// 分页获取列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<(List<T> List, int Count)> GetListAsync(Expression<Func<T, bool>> where, int pageIndex, int pageSize);

        /// <summary>
        /// 分页获取列表 [有排序]
        /// </summary>
        /// <typeparam name="K">排序列</typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="isAsc">true:升序 , false:倒序 </param>
        /// <returns></returns>
        (List<T> List, int Count) GetList<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false);
        /// <summary>
        /// 分页获取列表 [有排序]
        /// </summary>
        /// <typeparam name="K">排序列</typeparam>
        /// <param name="where"></param>
        /// <param name="orderBy"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="isAsc">true:升序 , false:倒序 </param>
        /// <returns></returns>
        Task<(List<T> List, int Count)> GetListAsync<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false);

        Task<(List<T> List, int Count)> GetListAsync<TKey>(Expression<Func<T, bool>> where, string include, Expression<Func<T, TKey>> orderBy, int pageIndex, int pageSize, bool isAsc = false);
        #endregion

        #region 自定义SQL
        /// <summary>
        /// 执行自定义SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSql(string sql, params object[] parameters);
        /// <summary>
        /// 执行自定义SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlAsync(string sql, params object[] parameters);

        #endregion

        #region 扩展方法
        /// <summary>
        /// 获取sql结果 第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        object ExecuteScalar(string sql, params object[] parameters);
        /// <summary>
        /// 获取sql结果 第一行第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<object> ExecuteScalarAsync(string sql, params object[] parameters);

        /// <summary>
        /// 获取sql结果 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable SqlQuery(string sql, params object[] parameters);
        /// <summary>
        /// 获取sql结果 返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<DataTable> SqlQueryAsync(string sql, params object[] parameters);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<T> SqlQueryList(string sql, params object[] parameters);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<List<T>> SqlQueryListAsync(string sql, params object[] parameters);

        /// <summary>
        /// 批量插入 [仅支持MSSQL Server]
        /// </summary>
        /// <typeparam name="T">泛型集合的类型</typeparam>
        /// <param name="conn">连接对象</param>
        /// <param name="tableName">将泛型集合插入到本地数据库表的表名</param>
        /// <param name="list">要插入大泛型集合</param>
        void BulkInsert(string tableName, IList<T> list);
        #endregion


    }
}
