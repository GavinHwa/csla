﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Csla.Xaml;

namespace SilverlightUI
{
  public class OrderVm : ViewModel<BusinessLibrary.Order>
  {
    public OrderVm()
    {
      //BeginRefresh(BusinessLibrary.Order.NewOrder);
      BeginRefresh(callback => BusinessLibrary.Order.GetOrder(441, callback));
    }

    protected override void OnError(Exception error)
    {
      Bxf.Shell.Instance.ShowError(error.Message, "Error");
    }

    public override void AddNew(object sender, ExecuteEventArgs e)
    {
      Model.LineItems.AddNew();
    }   
  }
}
