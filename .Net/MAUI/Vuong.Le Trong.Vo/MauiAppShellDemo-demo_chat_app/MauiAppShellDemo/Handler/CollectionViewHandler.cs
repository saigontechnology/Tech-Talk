﻿using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIAppDemo.Handler
{
    /// <summary>
    /// Solves <see href="https://github.com/dotnet/maui/issues/4116">this issue.</see>
    /// </summary>
    public class CollectionViewHandler : Microsoft.Maui.Controls.Handlers.Items.CollectionViewHandler
    {
        ///// <inheritdoc/>
        //protected override void ConnectHandler(ListViewBase platformView)
        //{
        //    base.ConnectHandler(platformView);
        //    platformView.Items.VectorChanged += Items_VectorChanged;
        //}

        ///// <inheritdoc/>
        //protected override void DisconnectHandler(ListViewBase platformView)
        //{
        //    base.DisconnectHandler(platformView);
        //    platformView.Items.VectorChanged -= Items_VectorChanged;
        //}

        //private void Items_VectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs @event)
        //{
        //    if (sender is ItemCollection item)
        //    {
        //        if (item?.Count > 5 && Element.ItemsUpdatingScrollMode == Microsoft.Maui.Controls.ItemsUpdatingScrollMode.KeepLastItemInView)//Number of items on a large screen
        //            PlatformView.ScrollIntoView(item.LastOrDefault());
        //    }
        //}
    }
}
