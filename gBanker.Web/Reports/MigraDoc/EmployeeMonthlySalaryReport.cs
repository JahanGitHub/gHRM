using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using gHRM.Web.Helpers;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using Newtonsoft.Json;

namespace gHRM.Web.Reports.MigraDoc
{
    public class EmployeeMonthlySalaryReport
    {
        public const double PAGE_WIDTH = 14;
        public const double PAGE_MARGIN_SIDE = 0.25;
        public const double INCHES_TO_PIXEL_RATIO = 96;
        private List<EmployeeMonthlySalaryReport_Data> Data;
        private List<string> TransactionTypeList;
        private List<EmployeeMonthlySalaryReport_Component> ComponentList;
        private int ColsCount, TblTotalColCount, TableColMaxChar;
        public EmployeeMonthlySalaryReport_Settings Settings, DefaultSettings;
        private string RootPath;
        public int SalaryYear = 0, SalaryMonth = 0;
        public string[] MonthNameList = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

        public EmployeeMonthlySalaryReport(HttpContextBase Context)
        {
            Data = new List<EmployeeMonthlySalaryReport_Data>();
            TransactionTypeList = new List<string>();
            ComponentList = new List<EmployeeMonthlySalaryReport_Component>();
            ColsCount = 0;
            RootPath = Context.Server.MapPath("~");
            DefaultSettings = new EmployeeMonthlySalaryReport_Settings
            {
                TopMargin = 1.2,
                HeaderFontSize = 12,
                TableHeaderHeight = 1,
                BodyColWidth = 0.5,
                BodyFontSize = 7
            };
            ApplySettings();
            TableColMaxChar = GetTableColMaxChar();
        }

        public Document GetPdfDocument(string FileName, DataSet DSet)
        {
            Data = new List<EmployeeMonthlySalaryReport_Data>();
            foreach (DataRow DataRow in DSet.Tables[0].Rows)
            {
                string TransactionTypeName = DataRow["TransactionTypeName"].ToString();
                string ComponentName = DataRow["ComponentName"].ToString();
                decimal PRComponentAmount = 0, TotalEarning = 0, TotalDeduction = 0, NetPay = 0;
                decimal.TryParse(DataRow["PRComponentAmount"].ToString(), out PRComponentAmount);
                decimal.TryParse(DataRow["TotalEarning"].ToString(), out TotalEarning);
                decimal.TryParse(DataRow["TotalDeduction"].ToString(), out TotalDeduction);
                decimal.TryParse(DataRow["NetPay"].ToString(), out NetPay);
                Data.Add(new EmployeeMonthlySalaryReport_Data
                {
                    OfficeShortOrder = DataRow["OfficeShortOrder"].ToString(),
                    EmployeeCode = DataRow["EmployeeCode"].ToString(),
                    EmployeeDetail = DataRow["EmployeeDetail"].ToString(),
                    GrossSalary = Convert.ToDecimal(DataRow["GrossSalary"]),
                    SalaryArrears = Convert.ToDecimal(DataRow["SalaryArrears"]),
                    TransactionTypeName = TransactionTypeName,
                    ComponentName = ComponentName,
                    PRComponentAmount = PRComponentAmount,
                    TotalEarning = TotalEarning,
                    TotalDeduction = TotalDeduction,
                    NetPay = NetPay,
                    SignAEmpName = DataRow["SignAEmpName"].ToString(),
                    SignAEmpDesName = DataRow["SignAEmpDesName"].ToString(),
                    SignBEmpName = DataRow["SignBEmpName"].ToString(),
                    SignBEmpDesName = DataRow["SignBEmpDesName"].ToString(),
                    SignCEmpName = DataRow["SignCEmpName"].ToString(),
                    SignCEmpDesName = DataRow["SignCEmpDesName"].ToString(),
                    SignDEmpName = DataRow["SignDEmpName"].ToString(),
                    SignDEmpDesName = DataRow["SignDEmpDesName"].ToString()
                });
                if (!TransactionTypeList.Contains(TransactionTypeName)) TransactionTypeList.Add(TransactionTypeName);
                if (ComponentList.Where(x => x.TransactionTypeName == TransactionTypeName && x.ComponentName == ComponentName).Count() == 0)
                {
                    ComponentList.Add(new EmployeeMonthlySalaryReport_Component
                    {
                        TransactionTypeName = TransactionTypeName,
                        ComponentName = ComponentName
                    });
                }
            }
            TransactionTypeList = TransactionTypeList.OrderByDescending(x => x).ToList();
            ColsCount = ComponentList.Count() + 5;

            Document Doc = new Document();
            Section Sec = Doc.AddSection();
            PageSetup(Doc, Sec);
            AddHeader(Sec);
            AddBody(Sec);
            AddFooter(Sec, RootPath);
            return Doc;
        }

        private void PageSetup(Document Doc, Section Sec)
        {
            Sec.PageSetup = Doc.DefaultPageSetup.Clone();
            Sec.PageSetup.Orientation = Orientation.Landscape;
            Sec.PageSetup.PageHeight = new Unit(PAGE_WIDTH, UnitType.Inch);
            Sec.PageSetup.PageWidth = new Unit(8.5, UnitType.Inch);
            Sec.PageSetup.TopMargin = new Unit(Settings.TableHeaderHeight + Settings.TopMargin, UnitType.Inch);
            Sec.PageSetup.BottomMargin = new Unit(0.5, UnitType.Inch);
            Sec.PageSetup.LeftMargin = new Unit(PAGE_MARGIN_SIDE, UnitType.Inch);
            Sec.PageSetup.RightMargin = new Unit(PAGE_MARGIN_SIDE, UnitType.Inch);
            Sec.PageSetup.HeaderDistance = new Unit(0.25, UnitType.Inch);
            Sec.PageSetup.FooterDistance = new Unit(0.25, UnitType.Inch);
        }

        private void AddHeader(Section Sec)
        {
            Paragraph CName = Sec.Headers.Primary.AddParagraph();
            CName.Format.Alignment = ParagraphAlignment.Center;
            CName.Format.Font.Size = Settings.HeaderFontSize;
            CName.AddText(SessionHelper.CompanyName);

            Sec.Headers.Primary.AddParagraph().Format.Font.Size = 8;

            Paragraph CAddress = Sec.Headers.Primary.AddParagraph();
            CAddress.Format.Alignment = ParagraphAlignment.Center;
            CAddress.Format.Font.Size = Settings.HeaderFontSize;
            CAddress.AddText(SessionHelper.CompanyAddress);

            Sec.Headers.Primary.AddParagraph().Format.Font.Size = 8;

            Paragraph RHeader = Sec.Headers.Primary.AddParagraph();
            RHeader.AddFormattedText("Salary Sheet " + MonthNameList[SalaryMonth - 1] + "-" + SalaryYear);
            RHeader.Format.Font.Size = Settings.HeaderFontSize;
            RHeader.Format.Font.Bold = true;
            RHeader.Format.Alignment = ParagraphAlignment.Center;

            Sec.Headers.Primary.AddParagraph().Format.Font.Size = 8;

            Table Tbl = Sec.Headers.Primary.AddTable();
            Tbl.Borders.Width = 0.25;
            Tbl.Format.Font.Size = Settings.BodyFontSize;
            Tbl.LeftPadding = 1;
            Tbl.RightPadding = 1;
            AddTableHeader(Tbl);
        }

        private void AddFooter(Section Sec, string RootDir)
        {
            Table Tbl = Sec.Footers.Primary.AddTable();
            Tbl.Format.Font.Size = 8;
            for (int i = 0; i < 3; i++) Tbl.AddColumn(new Unit(4.5, UnitType.Inch));
            var Row = Tbl.AddRow();

            Paragraph LeftCol = Row.Cells[0].AddParagraph();
            LeftCol.AddText(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());

            Paragraph MiddleCol = Row.Cells[1].AddParagraph();
            MiddleCol.Format.Alignment = ParagraphAlignment.Center;
            Image Img = MiddleCol.AddImage(RootDir + "Images\\gHRM-logo.png");
            Img.Width = 35;

            Paragraph RightCol = Row.Cells[2].AddParagraph();
            RightCol.Format.Alignment = ParagraphAlignment.Right;
            RightCol.AddPageField();
            RightCol.AddText(" of ");
            RightCol.AddNumPagesField();
        }

        private void AddBody(Section Sec)
        {
            Table Tbl = Sec.AddTable();
            Tbl.Borders.Width = 0.25;
            Tbl.Format.Font.Size = Settings.BodyFontSize;
            Tbl.LeftPadding = 1;
            Tbl.RightPadding = 1;
            AddTableBody(Tbl);
            AddTableBodyGrandTotal(Tbl);
            AddSignature(Sec);
        }

        private void AddTableCols(Table Tbl)
        {
            double EachColWidth = Settings.BodyColWidth;
            TblTotalColCount = 5 + ComponentList.Count() + TransactionTypeList.Count();
            for (int i = 0; i < TblTotalColCount; i++)
            {
                double ColWidth = EachColWidth;
                if (1 == i)
                {
                    ColWidth = SecWidth() - ((TblTotalColCount - 1) * EachColWidth);
                }
                Tbl.AddColumn(new Unit(ColWidth, UnitType.Inch));
            }
        }

        private void AddTableHeader(Table Tbl)
        {
            AddTableCols(Tbl);
            int CurrentIndex = 0;
            var Row1 = Tbl.AddRow();
            Row1.Format.Alignment = ParagraphAlignment.Center;
            Row1.Format.Font.Bold = true;

            Cell CodeCell = Row1.Cells[CurrentIndex];
            CodeCell.MergeDown = 1;
            CodeCell.AddParagraph().AddText("Code");
            CurrentIndex++;

            Cell NameDesCell = Row1.Cells[CurrentIndex];
            NameDesCell.MergeDown = 1;
            NameDesCell.AddParagraph().AddText("Name & Designation");
            CurrentIndex++;

            Cell GrossCell = Row1.Cells[CurrentIndex];
            GrossCell.MergeDown = 1;
            GrossCell.AddParagraph().AddText("Gross");
            CurrentIndex++;

            Cell ArrearsCell = Row1.Cells[CurrentIndex];
            ArrearsCell.MergeDown = 1;
            ArrearsCell.AddParagraph().AddText("Arrears");
            CurrentIndex++;

            foreach (var TransactionType in TransactionTypeList)
            {
                int ComponentCount = ComponentList.Where(x => x.TransactionTypeName == TransactionType).Count();
                Cell TTCell = Row1.Cells[CurrentIndex];
                TTCell.MergeRight = ComponentCount;
                TTCell.AddParagraph().AddText(TransactionType);
                CurrentIndex += ComponentCount + 1;
            }

            Cell NetPayCell = Row1.Cells[CurrentIndex];
            NetPayCell.MergeDown = 1;
            NetPayCell.AddParagraph().AddText("Net Pay");

            var Row2 = Tbl.AddRow();
            Row2.Format.Alignment = ParagraphAlignment.Center;
            Row2.Format.Font.Bold = true;
            Row2.HeightRule = RowHeightRule.Exactly;
            Row2.Height = new Unit(Settings.TableHeaderHeight, UnitType.Inch);

            CurrentIndex = 4;

            foreach (var TransactionType in TransactionTypeList)
            {
                var TTComponentList = ComponentList.Where(x => x.TransactionTypeName == TransactionType).ToList();
                foreach (var TTComponent in TTComponentList)
                {
                    Row2.Cells[CurrentIndex].AddParagraph().AddText(WordWrap(TTComponent.ComponentName, TableColMaxChar));
                    CurrentIndex++;
                }
                Row2.Cells[CurrentIndex].AddParagraph().AddText(WordWrap("Total " + TransactionType, TableColMaxChar));
                CurrentIndex++;
            }
        }

        private void AddTableBody(Table Tbl)
        {
            AddTableCols(Tbl);
            int CurrentIndex = 0;
            List<string> OfficeList = Data.Select(x => x.OfficeShortOrder).Distinct().ToList();
            foreach (string OfficeDes in OfficeList)
            {
                var OfficeGHeaderRow = Tbl.AddRow();
                Cell OfficeGHeaderCell = OfficeGHeaderRow.Cells[0];
                OfficeGHeaderCell.Format.Font.Bold = true;
                OfficeGHeaderCell.AddParagraph().AddText(OfficeDes);
                OfficeGHeaderCell.MergeRight = TblTotalColCount - 1;

                var EmployeeList = Data.Where(x => x.OfficeShortOrder == OfficeDes).Select(x => new
                {
                    x.EmployeeCode,
                    x.EmployeeDetail,
                    x.GrossSalary,
                    x.SalaryArrears,
                    x.TotalEarning,
                    x.TotalDeduction,
                    x.NetPay
                }).Distinct().ToList();
                foreach (var Employee in EmployeeList)
                {
                    CurrentIndex = 0;
                    decimal SalaryAllowanceTotal = 0, DeductionTotal = 0;
                    var Row = Tbl.AddRow();
                    (Row.Cells[CurrentIndex]).AddParagraph().AddText(Employee.EmployeeCode);
                    CurrentIndex++;
                    (Row.Cells[CurrentIndex]).AddParagraph().AddText(Employee.EmployeeDetail);
                    CurrentIndex++;
                    Cell GrossCell = Row.Cells[CurrentIndex];
                    GrossCell.Format.Alignment = ParagraphAlignment.Right;
                    GrossCell.AddParagraph().AddText(string.Format("{0:N0}", Employee.GrossSalary));
                    CurrentIndex++;
                    Cell ArrearsCell = Row.Cells[CurrentIndex];
                    ArrearsCell.Format.Alignment = ParagraphAlignment.Right;
                    ArrearsCell.AddParagraph().AddText(string.Format("{0:N0}", Employee.SalaryArrears));
                    CurrentIndex++;

                    foreach (var TransactionType in TransactionTypeList)
                    {
                        var TTComponentList = ComponentList.Where(x => x.TransactionTypeName == TransactionType).ToList();
                        foreach (var TTComponent in TTComponentList)
                        {
                            decimal Amount = Data.Where(x => x.EmployeeCode == Employee.EmployeeCode && x.ComponentName == TTComponent.ComponentName).Select(x => x.PRComponentAmount).FirstOrDefault();
                            Cell ComponentAmountCell = Row.Cells[CurrentIndex];
                            ComponentAmountCell.Format.Alignment = ParagraphAlignment.Right;
                            ComponentAmountCell.AddParagraph().AddText(string.Format("{0:N0}", Amount));
                            CurrentIndex++;
                        }
                        bool IsDeduction = !TransactionType.StartsWith("S");
                        if (!IsDeduction) SalaryAllowanceTotal = Employee.TotalEarning;
                        else DeductionTotal = Employee.TotalDeduction;
                        Cell TotalComponentAmountCell = Row.Cells[CurrentIndex];
                        TotalComponentAmountCell.Format.Alignment = ParagraphAlignment.Right;
                        TotalComponentAmountCell.AddParagraph().AddText(string.Format("{0:N0}", IsDeduction ? Employee.TotalDeduction : Employee.TotalEarning));
                        CurrentIndex++;
                    }
                    decimal NetPayAmount = SalaryAllowanceTotal - DeductionTotal;
                    Cell NetPayCell = Row.Cells[CurrentIndex];
                    NetPayCell.Format.Alignment = ParagraphAlignment.Right;
                    NetPayCell.AddParagraph().AddText(string.Format("{0:N0}", NetPayAmount));
                }
                CurrentIndex = 0;
                var OfficeGFooterRow = Tbl.AddRow();
                Cell OfficeGFooterCell = OfficeGFooterRow.Cells[CurrentIndex];
                OfficeGFooterCell.Format.Alignment = ParagraphAlignment.Right;
                OfficeGFooterCell.MergeRight = 1;
                OfficeGFooterCell.Format.Font.Bold = true;
                OfficeGFooterCell.AddParagraph().AddText("Sub Total");
                CurrentIndex += 2;

                decimal GrossSalaryOffTotal = EmployeeList.Sum(x => x.GrossSalary);
                Cell GrossSalaryOffTotalCell = OfficeGFooterRow.Cells[CurrentIndex];
                GrossSalaryOffTotalCell.Format.Alignment = ParagraphAlignment.Right;
                GrossSalaryOffTotalCell.Format.Font.Bold = true;
                GrossSalaryOffTotalCell.AddParagraph().AddText(string.Format("{0:N0}", GrossSalaryOffTotal));
                CurrentIndex++;

                decimal ArrearsOffTotal = EmployeeList.Sum(x => x.SalaryArrears);
                Cell ArrearsOffTotalCell = OfficeGFooterRow.Cells[CurrentIndex];
                ArrearsOffTotalCell.Format.Alignment = ParagraphAlignment.Right;
                ArrearsOffTotalCell.Format.Font.Bold = true;
                ArrearsOffTotalCell.AddParagraph().AddText(string.Format("{0:N0}", ArrearsOffTotal));
                CurrentIndex++;

                foreach (var TransactionType in TransactionTypeList)
                {
                    var TTComponentList = ComponentList.Where(x => x.TransactionTypeName == TransactionType).ToList();
                    decimal TotalAmount = 0;
                    foreach (var TTComponent in TTComponentList)
                    {
                        decimal Amount = Data.Where(x => x.OfficeShortOrder == OfficeDes && x.ComponentName == TTComponent.ComponentName).Sum(x => x.PRComponentAmount);
                        Cell ComponentAmountCell = OfficeGFooterRow.Cells[CurrentIndex];
                        ComponentAmountCell.Format.Alignment = ParagraphAlignment.Right;
                        ComponentAmountCell.Format.Font.Bold = true;
                        ComponentAmountCell.AddParagraph().AddText(string.Format("{0:N0}", Amount));
                        TotalAmount += Amount;
                        CurrentIndex++;
                    }
                    bool IsDeduction = !TransactionType.StartsWith("S");
                    if (!IsDeduction)
                    {
                        decimal TotalEarningOffTotal = EmployeeList.Sum(x => x.TotalEarning);
                        Cell TotalEarningOffTotalCell = OfficeGFooterRow.Cells[CurrentIndex];
                        TotalEarningOffTotalCell.Format.Alignment = ParagraphAlignment.Right;
                        TotalEarningOffTotalCell.Format.Font.Bold = true;
                        TotalEarningOffTotalCell.AddParagraph().AddText(string.Format("{0:N0}", TotalEarningOffTotal));
                        CurrentIndex++;
                    }
                    else
                    {
                        decimal TotalDeductionOffTotal = EmployeeList.Sum(x => x.TotalDeduction);
                        Cell TotalDeductionOffTotalCell = OfficeGFooterRow.Cells[CurrentIndex];
                        TotalDeductionOffTotalCell.Format.Alignment = ParagraphAlignment.Right;
                        TotalDeductionOffTotalCell.Format.Font.Bold = true;
                        TotalDeductionOffTotalCell.AddParagraph().AddText(string.Format("{0:N0}", TotalDeductionOffTotal));
                        CurrentIndex++;
                    }
                }
                decimal NetPayOffTotal = EmployeeList.Sum(x => x.NetPay);
                Cell NetPayOffTotalCell = OfficeGFooterRow.Cells[CurrentIndex];
                NetPayOffTotalCell.Format.Alignment = ParagraphAlignment.Right;
                NetPayOffTotalCell.Format.Font.Bold = true;
                NetPayOffTotalCell.AddParagraph().AddText(string.Format("{0:N0}", NetPayOffTotal));
                CurrentIndex++;
            }
        }

        private void AddTableBodyGrandTotal(Table Tbl)
        {
            int CurrentIndex = 0;
            decimal SalaryAllowanceTotal = 0, DeductionTotal = 0;
            var OfficeGFooterRow = Tbl.AddRow();
            Cell OfficeGFooterCell = OfficeGFooterRow.Cells[CurrentIndex];
            OfficeGFooterCell.Format.Alignment = ParagraphAlignment.Right;
            OfficeGFooterCell.MergeRight = 1;
            OfficeGFooterCell.Format.Font.Bold = true;
            OfficeGFooterCell.AddParagraph().AddText("Grand Total");
            CurrentIndex += 2;

            var EmployeeList = Data.Select(x => new
            {
                x.EmployeeCode,
                x.EmployeeDetail,
                x.GrossSalary,
                x.SalaryArrears
            }).Distinct().ToList();

            decimal GrossSalaryTotal = EmployeeList.Sum(x => x.GrossSalary);
            Cell GrossSalaryTotalCell = OfficeGFooterRow.Cells[CurrentIndex];
            GrossSalaryTotalCell.Format.Alignment = ParagraphAlignment.Right;
            GrossSalaryTotalCell.Format.Font.Bold = true;
            GrossSalaryTotalCell.AddParagraph().AddText(string.Format("{0:N0}", GrossSalaryTotal));
            CurrentIndex++;

            decimal ArrearsOffTotal = EmployeeList.Sum(x => x.SalaryArrears);
            Cell ArrearsOffTotalCell = OfficeGFooterRow.Cells[CurrentIndex];
            ArrearsOffTotalCell.Format.Alignment = ParagraphAlignment.Right;
            ArrearsOffTotalCell.Format.Font.Bold = true;
            ArrearsOffTotalCell.AddParagraph().AddText(string.Format("{0:N0}", ArrearsOffTotal));
            CurrentIndex++;

            foreach (var TransactionType in TransactionTypeList)
            {
                var TTComponentList = ComponentList.Where(x => x.TransactionTypeName == TransactionType).ToList();
                decimal TotalAmount = 0;
                foreach (var TTComponent in TTComponentList)
                {
                    decimal Amount = Data.Where(x => x.ComponentName == TTComponent.ComponentName).Sum(x => x.PRComponentAmount);
                    Cell ComponentAmountCell = OfficeGFooterRow.Cells[CurrentIndex];
                    ComponentAmountCell.Format.Alignment = ParagraphAlignment.Right;
                    ComponentAmountCell.Format.Font.Bold = true;
                    ComponentAmountCell.AddParagraph().AddText(string.Format("{0:N0}", Amount));
                    TotalAmount += Amount;
                    CurrentIndex++;
                }
                if (TransactionType.StartsWith("S")) SalaryAllowanceTotal = TotalAmount;
                else DeductionTotal = TotalAmount;
                Cell TotalComponentAmountCell = OfficeGFooterRow.Cells[CurrentIndex];
                TotalComponentAmountCell.Format.Alignment = ParagraphAlignment.Right;
                TotalComponentAmountCell.Format.Font.Bold = true;
                TotalComponentAmountCell.AddParagraph().AddText(string.Format("{0:N0}", TotalAmount));
                CurrentIndex++;
            }
            decimal NetPayAmount = SalaryAllowanceTotal - DeductionTotal;
            Cell NetPayCell = OfficeGFooterRow.Cells[CurrentIndex];
            NetPayCell.Format.Alignment = ParagraphAlignment.Right;
            NetPayCell.Format.Font.Bold = true;
            NetPayCell.AddParagraph().AddText(string.Format("{0:N0}", NetPayAmount));
        }

        private void AddSignature(Section Sec)
        {
            if (Data.Count() == 0 ||
                (string.IsNullOrEmpty(Data[0].SignAEmpName)
                && string.IsNullOrEmpty(Data[0].SignBEmpName)
                && string.IsNullOrEmpty(Data[0].SignCEmpName)
                && string.IsNullOrEmpty(Data[0].SignDEmpName))) return;
            int CurrentIndex = 0;
            Sec.AddParagraph().Format.Font.Size = 60;
            Table Tbl = Sec.AddTable();
            double ColWidth = Math.Round(SecWidth() / 4, 2);
            for (int i = 0; i < 4; i++) Tbl.AddColumn(new Unit(ColWidth, UnitType.Inch));
            var Row = Tbl.AddRow();
            int ColMaxChar = Convert.ToInt32((ColWidth * INCHES_TO_PIXEL_RATIO) / Settings.BodyFontSize);
            Row.Cells[CurrentIndex].AddParagraph().AddText(WordWrap(Data[0].SignAEmpName + Environment.NewLine + Data[0].SignAEmpDesName, ColMaxChar));
            CurrentIndex++;
            Row.Cells[CurrentIndex].AddParagraph().AddText(WordWrap(Data[0].SignBEmpName + Environment.NewLine + Data[0].SignBEmpDesName, ColMaxChar));
            CurrentIndex++;
            Row.Cells[CurrentIndex].AddParagraph().AddText(WordWrap(Data[0].SignCEmpName + Environment.NewLine + Data[0].SignCEmpDesName, ColMaxChar));
            CurrentIndex++;
            Row.Cells[CurrentIndex].AddParagraph().AddText(WordWrap(Data[0].SignDEmpName + Environment.NewLine + Data[0].SignDEmpDesName, ColMaxChar));
        }

        private double SecWidth()
        {
            return PAGE_WIDTH - (PAGE_MARGIN_SIDE * 2) - 0.15;
        }

        private void ApplySettings()
        {
            try
            {
                string SettingStr = File.ReadAllText(RootPath + "App_Data\\EmployeeMonthlySalaryReport_Settings.json");
                Settings = JsonConvert.DeserializeObject<EmployeeMonthlySalaryReport_Settings>(SettingStr);
            }
            catch { Settings = DefaultSettings; }
        }

        private string WordWrap(string text, int width)
        {
            string[] words = text.Split(new string[] { " " },
                        StringSplitOptions.None);

            int curLineLength = 0;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < words.Length; i += 1)
            {
                if (i > 0) strBuilder.Append(" ");
                string word = words[i];
                // If adding the new word to the current line would be too long,
                // then put it on a new line (and split it up if it's too long).
                if (curLineLength + word.Length > width)
                {
                    // Only move down to a new line if we have text on the current line.
                    // Avoids situation where wrapped whitespace causes emptylines in text.
                    if (curLineLength > 0)
                    {
                        strBuilder.Append(Environment.NewLine);
                        curLineLength = 0;
                    }

                    // If the current word is too long to fit on a line even on it's own then
                    // split the word up.
                    while (word.Length > width)
                    {
                        strBuilder.Append(word.Substring(0, width - 1) + "-");
                        word = word.Substring(width - 1);

                        strBuilder.Append(Environment.NewLine);
                    }

                    // Remove leading whitespace from the word so the new line starts flush to the left.
                    word = word.TrimStart();
                }
                strBuilder.Append(word);
                curLineLength += word.Length;
            }
            return strBuilder.ToString();
        }

        private int GetTableColMaxChar()
        {
            return Convert.ToInt32((Settings.BodyColWidth * INCHES_TO_PIXEL_RATIO) / Settings.BodyFontSize);
        }

        public class EmployeeMonthlySalaryReport_Data
        {
            public string OfficeShortOrder { get; set; }
            public string EmployeeCode { get; set; }
            public string EmployeeDetail { get; set; }
            public decimal GrossSalary { get; set; }
            public decimal SalaryArrears { get; set; }
            public string TransactionTypeName { get; set; }
            public string ComponentName { get; set; }
            public decimal PRComponentAmount { get; set; }
            public decimal TotalEarning { get; set; }
            public decimal TotalDeduction { get; set; }
            public decimal NetPay { get; set; }
            public string SignAEmpName { get; set; }
            public string SignAEmpDesName { get; set; }
            public string SignBEmpName { get; set; }
            public string SignBEmpDesName { get; set; }
            public string SignCEmpName { get; set; }
            public string SignCEmpDesName { get; set; }
            public string SignDEmpName { get; set; }
            public string SignDEmpDesName { get; set; }
        }

        public class EmployeeMonthlySalaryReport_Component
        {
            public string TransactionTypeName { get; set; }
            public string ComponentName { get; set; }
        }

        public class EmployeeMonthlySalaryReport_Settings
        {
            public double TopMargin { get; set; }
            public double HeaderFontSize { get; set; }
            public double TableHeaderHeight { get; set; }
            public double BodyColWidth { get; set; }
            public double BodyFontSize { get; set; }
        }
    }
}