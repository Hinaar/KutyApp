using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Widget;
using KutyApp.Client.Xam.Controls;
using KutyApp.Client.Xam.Droid.Renderers;
using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtraPicker), typeof(ExtraPickerRendererDroid))]
namespace KutyApp.Client.Xam.Droid.Renderers
{
    public class ExtraPickerRendererDroid : PickerRenderer
    {
        private Android.Graphics.Color? backColor;

        public ExtraPickerRendererDroid(Context context) : base(context)
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null) return;

            var customEntry = e.NewElement as ExtraPicker;
            if (customEntry == null) return;

            GradientDrawable gd = new GradientDrawable();
            gd.SetCornerRadius(customEntry.CornerRadius);
            gd.SetStroke(customEntry.BorderThickness, customEntry.BorderColor.ToAndroid());
            gd.SetColor(customEntry.MyBackgroundColor.ToAndroid());
            backColor = customEntry.MyBackgroundColor.ToAndroid();
            Control.SetBackground(gd);

            Control.Click += Control_Click;
        }

        private string[] customRenderValues = { nameof(ExtraPicker.MyBackgroundColor), nameof(ExtraPicker.CornerRadius), nameof(ExtraPicker.BorderThickness), nameof(ExtraPicker.BorderColor) };

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            var customEntry = sender as ExtraPicker;

            if (customRenderValues.Contains(e.PropertyName))
            {
                GradientDrawable gd = new GradientDrawable();
                gd.SetCornerRadius(customEntry.CornerRadius);
                gd.SetStroke(customEntry.BorderThickness, customEntry.BorderColor.ToAndroid());
                gd.SetColor(customEntry.MyBackgroundColor.ToAndroid());
                Control.SetBackground(gd);
            }
        }

        IElementController ElementController => Element as IElementController;

        private AlertDialog _dialog;

        //protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        //{
        //    base.OnElementChanged(e);

        //    if (e.NewElement == null || e.OldElement != null)
        //        return;

        //    Control.Click += Control_Click;
        //}

        protected override void Dispose(bool disposing)
        {
            Control.Click -= Control_Click;
            base.Dispose(disposing);
        }

        private void Control_Click(object sender, EventArgs e)
        {
            Picker model = Element;

            var picker = new NumberPicker(Context);
            if (model.Items != null && model.Items.Any())
            {
                // set style here
                picker.MaxValue = model.Items.Count - 1;
                picker.MinValue = 0;
                //picker.SetBackgroundColor(Android.Graphics.Color.Pink);
                picker.SetDisplayedValues(model.Items.ToArray());
                picker.WrapSelectorWheel = false;
                picker.Value = model.SelectedIndex;
                picker.ShowDividers = ShowDividers.None;

                try
                {
                    //for (int i = 0; i < picker.ChildCount+1; i++)
                    //{
                    //    var tmp = picker.GetChildAt(i);
                    //    if (tmp is EditText texte)
                    //    {
                    //        texte.SetTextColor(Android.Graphics.Color.Green);
                    //    }
                    //}
                    

                    var numberPickerType = Java.Lang.Class.FromType(typeof(NumberPicker));
                    var divider = numberPickerType.GetDeclaredField("mSelectionDivider");
                    divider.Accessible = true;

                    var dividerDraw = new ColorDrawable(Android.Graphics.Color.Transparent);
                    divider.Set(picker, dividerDraw);
                }
                catch
                {
                    // ignored
                }
            }

            var layout = new LinearLayout(Context) { Orientation = Orientation.Vertical, ClipToOutline = true, ShowDividers = ShowDividers.None };
            layout.AddView(picker);
            ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, true);

            var builder = new AlertDialog.Builder(Context, Resource.Style.DialogeTheme);
            builder.SetView(layout);

            builder.SetTitle(model.Title ?? "");
            builder.SetNegativeButton("Cancel  ", (s, a) =>
            {
                ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                // It is possible for the Content of the Page to be changed when Focus is changed.
                // In this case, we'll lose our Control.
                Control?.ClearFocus();
                _dialog = null;
            });
            builder.SetPositiveButton("Ok ", (s, a) =>
            {
                ElementController.SetValueFromRenderer(Picker.SelectedIndexProperty, picker.Value);
                // It is possible for the Content of the Page to be changed on SelectedIndexChanged.
                // In this case, the Element & Control will no longer exist.
                if (Element != null)
                {
                    if (model.Items.Count > 0 && Element.SelectedIndex >= 0)
                        Control.Text = model.Items[Element.SelectedIndex];
                    ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                    // It is also possible for the Content of the Page to be changed when Focus is changed.
                    // In this case, we'll lose our Control.
                    Control?.ClearFocus();
                }
                _dialog = null;
            });

            _dialog = builder.Create();
            _dialog.DismissEvent += (ssender, args) =>
            {
                ElementController?.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            };
            _dialog.Show();
            //_dialog.GetButton((int)DialogButtonType.Positive).SetBackgroundColor(Android.Graphics.Color.Green);
        }
    }
}