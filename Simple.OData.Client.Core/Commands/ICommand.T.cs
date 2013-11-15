﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Simple.OData.Client
{
    public interface ICommand<T>
        where T : class
    {
        new IClientWithCommand<U> As<U>(string derivedCollectionName = null) where U : class;
        new IClientWithCommand<ODataEntry> As(ODataExpression expression);
        new IClientWithCommand<T> Key(params object[] entryKey);
        new IClientWithCommand<T> Key(IEnumerable<object> entryKey);
        new IClientWithCommand<T> Key(IDictionary<string, object> entryKey);
        new IClientWithCommand<T> Key(T entryKey);
        new IClientWithCommand<T> Filter(string filter);
        new IClientWithCommand<ODataEntry> Filter(ODataExpression expression);
        new IClientWithCommand<T> Filter(Expression<Func<T, bool>> expression);
        new IClientWithCommand<T> Skip(int count);
        new IClientWithCommand<T> Top(int count);
        new IClientWithCommand<T> Expand(IEnumerable<string> associations);
        new IClientWithCommand<T> Expand(params string[] associations);
        new IClientWithCommand<ODataEntry> Expand(params ODataExpression[] associations);
        new IClientWithCommand<T> Expand(Expression<Func<T, object>> expression);
        new IClientWithCommand<T> Select(IEnumerable<string> columns);
        new IClientWithCommand<T> Select(params string[] columns);
        new IClientWithCommand<ODataEntry> Select(params ODataExpression[] columns);
        new IClientWithCommand<T> Select(Expression<Func<T, object>> expression);
        new IClientWithCommand<T> OrderBy(IEnumerable<KeyValuePair<string, bool>> columns);
        new IClientWithCommand<T> OrderBy(params string[] columns);
        new IClientWithCommand<ODataEntry> OrderBy(params ODataExpression[] columns);
        new IClientWithCommand<T> OrderBy(Expression<Func<T, object>> expression);
        new IClientWithCommand<T> ThenBy(Expression<Func<T, object>> expression);
        new IClientWithCommand<T> OrderByDescending(params string[] columns);
        new IClientWithCommand<ODataEntry> OrderByDescending(params ODataExpression[] columns);
        new IClientWithCommand<T> OrderByDescending(Expression<Func<T, object>> expression);
        new IClientWithCommand<T> ThenByDescending(Expression<Func<T, object>> expression);
        new IClientWithCommand<T> Count();
        new IClientWithCommand<U> NavigateTo<U>(string linkName = null) where U : class;
        new IClientWithCommand<ODataEntry> NavigateTo(ODataExpression expression);
        new IClientWithCommand<T> Set(object value);
        new IClientWithCommand<T> Set(IDictionary<string, object> value);
        new IClientWithCommand<ODataEntry> Set(params ODataExpression[] value);
        new IClientWithCommand<T> Set(T entry);
        new IClientWithCommand<T> Function(string functionName);
        new IClientWithCommand<T> Parameters(IDictionary<string, object> parameters);
    }
}