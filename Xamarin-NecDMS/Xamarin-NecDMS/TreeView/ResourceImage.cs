﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Xamarin.Forms;

namespace NecDMS.TreeView
{
    public class ResourceImage : Image
    {
        public static readonly BindableProperty ResourceProperty = BindableProperty.Create(nameof(Resource), typeof(string), typeof(string), null, BindingMode.OneWay, null, ResourceChanged);

        private static void ResourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var resourceString = (string)newvalue;
            var imageControl = (Image)bindable;

            //This code is nicer but doesn't tell you when the resource doesn't exist :/
            imageControl.Source = ImageSource.FromResource(resourceString, Assembly.GetAssembly(typeof(ResourceImage)));
        }

        public string Resource
        {
            get => (string)GetValue(ResourceProperty);
            set => SetValue(ResourceProperty, value);
        }
    }
}
