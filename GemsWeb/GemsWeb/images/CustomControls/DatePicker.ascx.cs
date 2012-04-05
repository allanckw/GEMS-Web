using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace GemsWeb.CustomControls
{
    public partial class DatePicker : System.Web.UI.UserControl
    {

        int minYear = 80;
        int maxYear = 0;
        bool fDate = false;

        //public DateTime CalDate
        //{
        //    get { return myCalendar.SelectedDate; }
        //    set
        //    {
        //        dTable.Visible = true;
        //        myCalendar.SelectedDate = value;

        //    }
        //}

        public DateTime CalDate
        {
            get { return myCalendar.SelectedDate; }
            set { myCalendar.SelectedDate = value; }
        }

        public enum Months
        {
            January,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December

        }

        public int MinimumYear
        {
            get { return this.minYear; }
            set { this.minYear = value; }
        }

        public int MaximumYear
        {
            get { return this.maxYear; }
            set { this.maxYear = value; }
        }

        public bool DisplayFutureDate
        {
            get { return this.fDate; }
            set { this.fDate = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Hide the title of the calendar control
            myCalendar.ShowTitle = false;
            if (!Page.IsPostBack)
            {
                Populate_YearList();
                Populate_MonthList();

                myCalendar.SelectedDate = System.DateTime.Now;
                setCalDate();
            }

        }

        protected void Populate_MonthList()
        {
            if (DisplayFutureDate)
            {
                for (int i = 0; i <= 11; i++)
                {
                    drpCalMonth.Items.Add(Enum.GetName(typeof(Months), i));
                }
            }
            else
            {
                for (int i = 0; i <= DateTime.Now.Month - 1; i++)
                {
                    drpCalMonth.Items.Add(Enum.GetName(typeof(Months), i));
                }
            }
        }

        protected void Calendar1_DayRender(object sender, System.Web.UI.WebControls.DayRenderEventArgs e)
        {
            if (!DisplayFutureDate)
            {
                e.Day.IsSelectable = e.Day.Date <= DateTime.Now;
            }
        }


        protected void Populate_YearList()
        {
            //Year list can be extended
            int year = 0;

            for (year = DateTime.Now.Year - minYear; year <= DateTime.Now.Year + maxYear; year++)
            {
                drpCalYear.Items.Add(year.ToString());
            }

            drpCalYear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;

        }


        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
            setCalDate();

        }
        protected void drpCalMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            Set_Calendar();
        }

        protected void drpCalYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            Set_Calendar();
            if (!fDate)
            {
                drpCalMonth.Items.Clear();
                Populate_MonthList();

            }
        }

     

        private void setCalDate()
        {
            txtDate.Text = myCalendar.SelectedDate.ToString("dd MMM yyyy");
            dTable.Visible = false;
        }


        protected void Set_Calendar()
        {
            //Whenever month or year selection changes display the calendar for that month/year

            myCalendar.TodaysDate = Convert.ToDateTime(drpCalMonth.SelectedItem.Value + " 1, " + drpCalYear.SelectedItem.Value);

        }
        public DatePicker()
        {
            Load += Page_Load;
        }

        protected void btnChangeDate_Click(object sender, EventArgs e)
        {
            if (dTable.Visible == true)
            {
                dTable.Visible = false;
            }
            else
            {
                dTable.Visible = true;
            }
        }

        protected void myCalendar_SelectionChanged(object sender, EventArgs e)
        {
            txtDate.Text = myCalendar.SelectedDate.ToString("dd MMM yyyy");
            dTable.Visible = false;
        }
    }
}
