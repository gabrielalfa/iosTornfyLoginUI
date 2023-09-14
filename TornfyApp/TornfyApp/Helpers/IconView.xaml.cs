using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TornfyApp.Helpers
{
    public partial class IconView
    {
        public IconView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty IconNameProperty =
        BindableProperty.Create("IconName", typeof(string), typeof(IconView), null, propertyChanged: iconNamePropertyChanged);
        private static void iconNamePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (IconView)bindable;
            control.FontImage.Glyph = newValue.ToString();
        }
        public string IconName
        {
            get { return (string)GetValue(IconNameProperty); }
            set
            {
                SetValue(IconNameProperty, value);
                FontImage.Glyph = value;
            }
        }
        public static readonly BindableProperty IconColorProperty =
        BindableProperty.Create("IconColor", typeof(Color), typeof(IconView), Color.Black,propertyChanged: iconColorPropertyChanged);
        private static void iconColorPropertyChanged(BindableObject bindable , object oldValue, object newValue)
        {
            var control = (IconView)bindable;
            control.FontImage.Color = (Color)newValue;
        }
        public Color IconColor
        {
            get { return (Color)GetValue(IconColorProperty); }
            set
            {
                SetValue(IconColorProperty, value);
                FontImage.Color = value;
            }
        }


    }
}
