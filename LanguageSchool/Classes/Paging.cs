﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LanguageSchool.Classes;

namespace PagingWPFDataGrid
{
    /// <summary>
    ///     Performs Paging operations on a given List and Outputs a DataTable
    /// </summary>
    internal class Paging
    {
        private DataTable PagedList = new DataTable(); //Initialize a DataTable Locally

        /// <summary>
        ///     Current Page Index Number
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///     Show the next set of Items based on page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable Next(IList<Client> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage) PageIndex = ListToPage.Count / RecordsPerPage;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        ///     Show the previous set of items base on page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable Previous(IList<Client> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if (PageIndex <= 0) PageIndex = 0;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        ///     Show first the set of Items in the page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable First(IList<Client> ListToPage, int RecordsPerPage)
        {
            PageIndex = 0;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        ///     Show the last set of items in the page index
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable Last(IList<Client> ListToPage, int RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        /// <summary>
        ///     Performs a LINQ Query on the List and returns a DataTable
        /// </summary>
        /// <param name="ListToPage"></param>
        /// <param name="RecordsPerPage"></param>
        /// <returns>DataTable</returns>
        public DataTable SetPaging(IList<Client> ListToPage, int RecordsPerPage)
        {
            var PageGroup = PageIndex * RecordsPerPage;

            IList<Client> PagedList = new List<Client>();

            PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage)
                .ToList(); //This is where the Magic Happens. If you have a Specific sort or want to return ONLY a specific set of columns, add it to this LINQ Query.

            var FinalPaging = PagedTable(PagedList);

            return FinalPaging;
        }

        //If youre paging say 30,000 rows and you know the processors of the users will be slow you can ASync thread both of these to allow the UI to update after they finish and prevent a hang.

        /// <summary>
        ///     Internal Method: Performs the Work of converting the Passed in list to a DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="SourceList"></param>
        /// <returns>DataTable</returns>
        private DataTable PagedTable<T>(IList<T> SourceList)
        {
            var columnType = typeof(T);
            var TableToReturn = new DataTable();

            foreach (var Column in columnType.GetProperties())
                TableToReturn.Columns.Add(Column.Name, Nullable.GetUnderlyingType(
                    Column.PropertyType) ?? Column.PropertyType);

            foreach (object item in SourceList)
            {
                var ReturnTableRow = TableToReturn.NewRow();
                foreach (var Column in columnType.GetProperties()) ReturnTableRow[Column.Name] = Column.GetValue(item);
                TableToReturn.Rows.Add(ReturnTableRow);
            }

            return TableToReturn;
        }
    }
}