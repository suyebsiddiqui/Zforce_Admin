using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace r4idadmin
{
    public class DataTemplateSelectorBase : DataTemplateSelector
    {
        protected dashboard GetMainWindow(DependencyObject inContainer)
        {
            DependencyObject c = inContainer;
            while (true)
            {
                DependencyObject p = VisualTreeHelper.GetParent(c);

                if (c is dashboard)
                {
                    return c as dashboard;
                }
                else
                {
                    c = p;
                }
            }
        }

        public override DataTemplate SelectTemplate(object inItem, DependencyObject inContainer)
        {
            DataRowView row = inItem as DataRowView;

            if (row != null)
            {
                if (row.DataView.Table.Columns.Contains("Status"))
                {
                    dashboard w = GetMainWindow(inContainer);
                    return (DataTemplate)w.FindResource("StatusImage");
                }
            }
            return null;
        }

    }
}
