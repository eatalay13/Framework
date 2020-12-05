using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Mvc.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWeb.Framework.Extensions
{
    public static class DevExpressDataGridExtensions
    {
        public static DataGridBuilder<T> AdvantagGridOptions<T>(this DataGridBuilder<T> dataGridBuilder)
        {
            dataGridBuilder.Paging(opt => opt.Enabled(true))
                            .SearchPanel(opt => opt.Visible(true))
                            .FilterPanel(filter => filter.Visible(true))
                            .HeaderFilter(filter => filter.Visible(true))
                            .AllowColumnResizing(true)
                            .ColumnResizingMode(ColumnResizingMode.NextColumn)
                            .ColumnAutoWidth(true)
                            .AllowColumnReordering(true)
                            .FilterRow(filterRow => filterRow
                                .Visible(true)
                                .ApplyFilter(GridApplyFilterMode.Auto))
                            .Sorting(sorting => sorting.Mode(GridSortingMode.Multiple))
                            .RowAlternationEnabled(true)
                            .Export(export => export.Enabled(true).AllowExportSelectedData(true))
                            .ColumnChooser(c => c.Enabled(true).Mode(GridColumnChooserMode.DragAndDrop))
                            .Selection(select => select.Mode(SelectionMode.Multiple))
                            .Paging(paging => paging.PageSize(10))
                            .Pager(pager =>
                            {
                                pager.ShowPageSizeSelector(true);
                                pager.AllowedPageSizes(new[] { 5, 10, 20, 50, 100 });
                                pager.ShowInfo(true);
                            });

            return dataGridBuilder;
        }
    }
}
