﻿//-----------------------------------------------------------------------
// <copyright file="RootFactory.cs" company="Marimer LLC">
//     Copyright (c) Marimer LLC. All rights reserved.
//     Website: http://www.lhotka.net/cslanet/
// </copyright>
// <summary>no summary</summary>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csla.Test.ObjectFactory
{
  public class RootFactory
  {
    public object Create()
    {
      var obj = new Root();
      obj.Data = "Create";
      obj.Location = ApplicationContext.ExecutionLocation;
      obj.MarkAsNew();
      return obj;
    }

    [RunLocal]
    public object Create(SingleCriteria<Root, string> criteria)
    {
      var obj = new Root();
      obj.Data = "Create " + criteria.Value;
      obj.Location = ApplicationContext.ExecutionLocation;
      obj.MarkAsNew();
      return obj;
    }

    public object Fetch()
    {
      var obj = new Root();
      obj.Data = "Fetch";
      obj.MarkAsOld();
      return obj;
    }

    public object Fetch(SingleCriteria<Root, string> criteria)
    {
      var obj = new Root();
      obj.Data = criteria.Value;
      obj.MarkAsOld();
      return obj;
    }

    public object Update(Root obj)
    {
      obj.TransactionalType = _transactionalType;
      obj.Data = "Update";
      obj.MarkAsOld();
      return obj;
    }

    [Transactional(TransactionalTypes.TransactionScope)]
    public object Update2(Root obj)
    {
      obj.TransactionalType = _transactionalType;
      obj.Data = "Update";
      obj.MarkAsOld();
      return obj;
    }

    [Transactional(TransactionalTypes.EnterpriseServices)]
    public object Update3(Root obj)
    {
      obj.TransactionalType = _transactionalType;
      obj.Data = "Update";
      obj.MarkAsOld();
      return obj;
    }

    public void Delete(SingleCriteria<Root, string> criteria)
    {
      Csla.ApplicationContext.GlobalContext["ObjectFactory"] = "Delete";
    }

    private TransactionalTypes _transactionalType;

    public void Invoke(Csla.DataPortalEventArgs args)
    {
      _transactionalType = args.DataPortalContext.TransactionalType;
    }
  }

  public class RootFactory1
  {
    [Transactional(TransactionalTypes.TransactionScope)]
    public object Update(Root obj)
    {

      obj.IsolationLevel = System.Transactions.Transaction.Current.IsolationLevel.ToString();
      obj.TransactionTimeout = ApplicationContext.DefaultTransactionTimeoutInSeconds;
      obj.TransactionalType = _transactionalType;
      obj.Data = "Update";
      obj.MarkAsOld();
      return obj;
    }

    private TransactionalTypes _transactionalType;

    public void Invoke(Csla.DataPortalEventArgs args)
    {
      _transactionalType = args.DataPortalContext.TransactionalType;
    }
  }

  public class RootFactory2
  {
    [Transactional(TransactionalTypes.EnterpriseServices)]
    public object Update(Root obj)
    {
      obj.TransactionalType = _transactionalType;
      obj.Data = "Update";
      obj.MarkAsOld();
      return obj;
    }

    private TransactionalTypes _transactionalType;

    public void Invoke(Csla.DataPortalEventArgs args)
    {
      _transactionalType = args.DataPortalContext.TransactionalType;
    }
  }

  public class RootFactory3 : Csla.Server.ObjectFactory
  {
    public object Fetch()
    {
      var obj = new Root();
      LoadProperty(obj, Root.DataProperty, "Fetch");
      this.MarkOld(obj);
      return obj;
    }
  }

  public class RootFactory4
  {
    [Transactional(TransactionalTypes.TransactionScope, TransactionIsolationLevel.ReadCommitted, 100)]
    public object Update(Root obj)
    {

      obj.IsolationLevel = System.Transactions.Transaction.Current.IsolationLevel.ToString();
      obj.TransactionTimeout = 100;
      obj.TransactionalType = _transactionalType;
      obj.Data = "Update";
      obj.MarkAsOld();
      return obj;
    }

    private TransactionalTypes _transactionalType;

    public void Invoke(Csla.DataPortalEventArgs args)
    {
      _transactionalType = args.DataPortalContext.TransactionalType;
    }
  }

  public class RootFactory5
  {
    [Transactional(TransactionalTypes.TransactionScope)]
    public object Update(Root obj)
    {

      obj.IsolationLevel = System.Transactions.Transaction.Current.IsolationLevel.ToString();
      obj.TransactionTimeout = ApplicationContext.DefaultTransactionTimeoutInSeconds;
      obj.TransactionalType = _transactionalType;
      obj.Data = "Update";
      obj.MarkAsOld();
      return obj;
    }

    private TransactionalTypes _transactionalType;

    public void Invoke(Csla.DataPortalEventArgs args)
    {
      _transactionalType = args.DataPortalContext.TransactionalType;
    }
  }

  public class RootFactory6
  {
    [Transactional(TransactionalTypes.EnterpriseServices, TransactionIsolationLevel.ReadCommitted)]
    public object Update(Root obj)
    {

      obj.IsolationLevel = System.EnterpriseServices.ContextUtil.SystemTransaction.IsolationLevel.ToString();
      obj.TransactionalType = _transactionalType;
      obj.Data = "Update";
      obj.MarkAsOld();
      return obj;
    }

    private TransactionalTypes _transactionalType;

    public void Invoke(Csla.DataPortalEventArgs args)
    {
      _transactionalType = args.DataPortalContext.TransactionalType;
    }
  }

  public class RootFactory7
  {
    [Transactional(TransactionalTypes.EnterpriseServices)]
    public object Update(Root obj)
    {
      obj.IsolationLevel = System.EnterpriseServices.ContextUtil.SystemTransaction.IsolationLevel.ToString();
      
      obj.TransactionalType = _transactionalType;
      obj.Data = "Update";
      obj.MarkAsOld();
      return obj;
    }

    private TransactionalTypes _transactionalType;

    public void Invoke(Csla.DataPortalEventArgs args)
    {
      _transactionalType = args.DataPortalContext.TransactionalType;
    }
  }

}