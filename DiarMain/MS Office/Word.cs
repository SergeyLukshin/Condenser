using System;
using System.Collections.Generic;
using System.Text;
using MSWord = Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections;

namespace Condenser
{
    public class Word
    {
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public class FontParams
	    {
		    public string	fontName;
            public string styleName;
		    public long	fontHeight;
		    public bool? bold;
		    public bool? underline;		
		    public bool? italic;

		    public FontParams(bool bClean = false)
		    {
			    if (bClean)
			    {
				    fontName	= "";
				    fontHeight	= -1;
				    bold		= null;
				    underline	= null;
				    italic		= null;
			    }
			    else
			    {
				    fontName	= "Times New Roman";
				    fontHeight	= 11;
				    bold		= false;
				    underline	= false;
				    italic		= false;
			    }
		    }

		    public FontParams(string fontName_, 
							    long fontHeight_ = -1, 
							    bool? bold_ = null,
                                bool? italic_ = null,
                                bool? underline_ = null,
                                string styleName_ = "")
		    {
			    fontName = fontName_;
			    fontHeight = fontHeight_;
			    bold = bold_;
			    underline = underline_;
			    italic = italic_;
                styleName = styleName_;
		    }
	    };

        private MSWord.Application m_pApp = null;
        private MSWord.Document m_pDoc = null;
        private MSWord.Table m_pTable = null;
        private MSWord.Selection m_pSelection = null;
        private int old_table_index = -1;

        public class CellInfo
        {
            public CellInfo()
            {
                strText = "";
                color = null;
                text_color = null;
            }
            public string strText;
            public MSWord.WdColor? color;
            public MSWord.WdColor? text_color;
        }

        public class ColumnInfo
        {
            public List<CellInfo> m_Cells = new List<CellInfo>();
        }
        public List<ColumnInfo> m_Table = new List<ColumnInfo>();

        public void DestroyWord ()
        {
            if (m_pApp == null) return;
            if (!m_pApp.Visible)
                Quit();
            else
                Release();
        }

        public void Quit()
        {
            if (m_pApp != null)
            {
		        m_pApp.Quit(false);
            }
        }

        public void Release()
        {
            if (m_pSelection != null)
            {
	            m_pSelection = null;
            }

            if (m_pTable != null)
            {
	            m_pTable = null;
            }

            if (m_pDoc != null)
            {
	            m_pDoc = null;
            }

            if (m_pApp != null)
            {
                m_pApp = null;
            }
        }

        public bool Start (string strFileName)
        {
            m_pApp = null;
            m_pDoc = null;
            m_pTable = null;
            m_pSelection = null;

            old_table_index = 0;

            try
            {
                m_pApp = new MSWord.Application();

                if (strFileName == "")
                {
                    m_pDoc = m_pApp.Documents.Add();
                }
                else
                {
                    m_pDoc = m_pApp.Documents.Add(strFileName);
                }

                return true;
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }

        public void WordDocumentToFront()
        {
            string s = m_pApp.ActiveDocument.ActiveWindow.Caption + " - " + m_pApp.Caption;
            IntPtr h = FindWindow(null, s);
            if ((int)h != 0) SetForegroundWindow(h);
        }

        public void SetPageBreak()
        {
            if (m_pSelection != null)
                m_pSelection.InsertBreak(7);
        }

        public void SetVisible(bool bVisible)
        {
	        if (m_pApp != null)
            {
                WordDocumentToFront();
	            m_pApp.Visible = true;
	            m_pApp.Activate();
            }
        }

        public void InsertRowsInVirtualTable(int iRowCnt)
        {
            for (int i = 0; i < m_Table.Count; i++)
            {
                for (int j = 0; j < iRowCnt; j++)
                    m_Table[i].m_Cells.Add(new CellInfo());
            }
        }

        public void SetTextInVirtualCell(int iCol, int iRow, string strText, string strDefault = "")
        {
            if (strText == "") strText = strDefault;
            m_Table[iCol - 1].m_Cells[iRow - 1].strText = strText;
        }

        public void SetVirtualCellColor(int iCol, int iRow, MSWord.WdColor color, MSWord.WdColor text_color = MSWord.WdColor.wdColorAutomatic)
        {
            m_Table[iCol - 1].m_Cells[iRow - 1].color = color;
            m_Table[iCol - 1].m_Cells[iRow - 1].text_color = text_color;
        }

        public void SetVirtualCellColor(int FirstColumn, int LastColumn, int FirstRow, int LastRow, MSWord.WdColor color, MSWord.WdColor text_color)
        {
            for (int iCol = FirstColumn; iCol <= LastColumn; iCol++)
            {
                for (int iRow = FirstRow; iRow <= LastRow; iRow++)
                {
                    m_Table[iCol - 1].m_Cells[iRow - 1].color = color;
                    m_Table[iCol - 1].m_Cells[iRow - 1].text_color = text_color;
                }
            }
        }

        public void InitVirtualTable(int cols, int rows)
        {
            m_Table.Clear();
            for (int i = 0; i < cols; i++)
            {
                ColumnInfo ci = new ColumnInfo();
                for (int j = 0; j < rows; j++)
                    ci.m_Cells.Add(new CellInfo());

                m_Table.Add(ci);
            }
        }

        /*public void ConvertToTable(string strTable)
        {
            if (m_pSelection == null)
            {
                if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
                if (m_pApp == null) return;

                m_pDoc.Select();
                m_pSelection = m_pApp.Selection;
                if (m_pSelection == null) return;
            }

	        if (m_pSelection == null) return;
	        m_pSelection.Collapse(0);

            object varInfo = m_pSelection.get_Information(MSWord.WdInformation.wdWithInTable);
            if (Convert.ToBoolean(varInfo) == true)
            {
                m_pDoc.Select();
                m_pSelection = m_pApp.Selection;
                if (m_pSelection == null) return;
                m_pSelection.Collapse(0);
            }

	        m_pSelection.Font.Bold = 0;
	        m_pSelection.Text = strTable;

            m_pSelection.Paragraphs.SpaceAfter = 0;
            m_pSelection.Paragraphs.SpaceBefore = 0;

            m_pTable = m_pSelection.ConvertToTable(1, Type.Missing, Type.Missing, Type.Missing, Type.Missing, 
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, 0, 0);

            if (m_pTable == null) return;
	
	        MSWord.Rows pRows = m_pSelection.Rows;
	        pRows.AllowBreakAcrossPages = 0;
            //m_pApp.WordBasic.OpenOrCloseParaBelow;
            //m_pApp.WordBasic.OpenOrCloseParaAbove;

	        m_pSelection.Collapse();

	        MSWord.Borders pBorders = m_pTable.Borders;
	        if (pBorders == null) return;
	        pBorders.Enable = 1;
        }*/

        public void SetTableFont(FontParams fp, int indTable = -1)
        {
            GetTableReference(indTable);
            
            m_pTable.Select();
            PutFont(fp, m_pApp.Selection);

            if (fp.styleName != "")
            {
                m_pSelection = m_pApp.Selection;
                m_pSelection.Paragraphs.SpaceAfter = 0;
                m_pSelection.Paragraphs.SpaceBefore = 0;
                m_pSelection.Paragraphs.LineSpacingRule = MSWord.WdLineSpacing.wdLineSpaceSingle;
            }
        }

        public void DisplayVirtualTable()
        {
            string strTable = "";
            int indCell = 0;

            char chTab = (char)9;
            char chN = (char)13;

            if (m_Table.Count > 0)
            {
                while (true)
                {
                    for (int i = 0; i < m_Table.Count; i++)
                    {
                        if (i == 0)
                            strTable += m_Table[i].m_Cells[indCell].strText;
                        else
                            strTable += chTab + m_Table[i].m_Cells[indCell].strText;
                    }
                    strTable += chN;

                    indCell++;
                    if (indCell >= m_Table[0].m_Cells.Count) break;
                }

                ConvertToTable(strTable);

                for (int i = 0; i < m_Table.Count; i++)
                {
                    for (int j = 0; j < m_Table[i].m_Cells.Count; j++)
                    {
                        if (m_Table[i].m_Cells[j].color != null && m_Table[i].m_Cells[j].text_color != null)
                        {
                            SetCellColor(i + 1, j + 1, (MSWord.WdColor)m_Table[i].m_Cells[j].color, (MSWord.WdColor)m_Table[i].m_Cells[j].text_color, -1);
                        }
                    }
                }
            }
        }

        void PutFont(FontParams fp, MSWord.Selection sel)
        {
            MSWord.Font pFont = sel.Font;

            if (fp.styleName != "")
            {
                sel.set_Style(m_pDoc.Styles[fp.styleName]);
            }
            
            if (fp.fontName != "")
	        {
		        pFont.Name = fp.fontName;
	        }

            if (fp.bold != null)
	        {
		        if ((bool)fp.bold)
			        pFont.Bold = 1;
		        else
                    pFont.Bold = 0;
	        }

            if (fp.italic != null)
	        {
                if ((bool)fp.italic)
			        pFont.Italic = 1;
		        else
                    pFont.Italic = 0;
	        }

	        if (fp.underline != null)
	        {
                if ((bool)fp.underline)
			        pFont.Underline = MSWord.WdUnderline.wdUnderlineSingle;
		        else
                    pFont.Underline = MSWord.WdUnderline.wdUnderlineNone;
	        }

	        if (fp.fontHeight > 0 )
	        {
		        pFont.Size = fp.fontHeight;
	        }
        }

        public void SetTableHeadingRows(long CountHeadingRows, int indTable = -1)
        {
            GetTableReference(indTable);

	        MSWord.Rows pRows = m_pTable.Rows;
	        if (pRows == null) return;

	        for (int i = 0; i < CountHeadingRows; i++)
	        {
		        pRows[i + 1].HeadingFormat = -1;
	        }
        }

        public void SetText(string strText, MSWord.WdParagraphAlignment hAlignment, FontParams font = null)
        {
            if (m_pSelection == null)
            {
                if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
                if (m_pApp == null) return;

                m_pDoc.Select();
                m_pSelection = m_pApp.Selection;
                if (m_pSelection == null) return;
            }

            object varInfo = m_pSelection.get_Information(MSWord.WdInformation.wdWithInTable);
	        if (Convert.ToBoolean(varInfo) == true)
	        {
		        m_pDoc.Select();
                m_pSelection = m_pApp.Selection;
		        if (m_pSelection == null) return;
		        m_pSelection.Collapse(0);
	        }

            MSWord.ParagraphFormat pf = m_pSelection.ParagraphFormat;
            pf.Alignment = hAlignment;
            m_pSelection.Text = strText;
            if (font != null)
                PutFont(font, m_pSelection);

            m_pSelection.Collapse(0);
        }

        public void GotoBookmark(string strBookmark)
        {
            if (m_pSelection == null)
            {
                if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
                if (m_pApp == null) return;

                m_pDoc.Select();
                m_pSelection = m_pApp.Selection;
                if (m_pSelection == null) return;
            }

            m_pSelection.GoTo(-1, Type.Missing, Type.Missing, strBookmark);
            m_pSelection.Collapse(0);
        }

        public void SetBookmarkText(List<KeyValuePair<string, string>> mBookMark)
        {
	        if (mBookMark.Count == 0) return;

	        /*if (m_pSelection == null)
            {
		        if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
		        if (m_pApp == null) return;

		        m_pDoc.Select();
		        m_pSelection = m_pApp.Selection;
		        if (m_pSelection == null) return;	        
            }*/

            for (int i = 0; i < mBookMark.Count; i++)
            {
                if (mBookMark[i].Key == "" /*|| mBookMark[i].Value == ""*/) continue;

                //if (m_pDoc.Bookmarks.Exists(mBookMark[i].Key))
                {
                    m_pDoc.Bookmarks[mBookMark[i].Key].Range.Text = mBookMark[i].Value;
                }
                /*
                m_pSelection.GoTo(-1, Type.Missing, Type.Missing, mBookMark[i].Key);
		        m_pSelection.Text = mBookMark[i].Value;*/
            }
        }

        public void InsertRowsInTable (int indTable, int nCount)
        {
            if (nCount <= 0) return;

            if (m_pApp != null)
            {
	            if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
	            if (m_pDoc == null) return;

                MSWord.Tables tables = m_pDoc.Tables;
                if (tables.Count == 0) return;

                m_pTable = tables[indTable];
	            if (m_pTable == null) return;

	            m_pTable.Select();

	            MSWord.Selection pSel = m_pApp.Selection;
	            pSel.InsertRowsBelow(nCount);

	            m_pDoc.Select();
	            m_pSelection = m_pApp.Selection;
	            if (m_pSelection == null) return;
	            m_pSelection.Collapse(0);
            }
        }

        public void DeleteRowInTable(int indTable, int index)
        {
            if (m_pApp != null)
            {
                if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
                if (m_pDoc == null) return;

                MSWord.Tables tables = m_pDoc.Tables;
                if (tables.Count == 0) return;

                m_pTable = tables[indTable];
                if (m_pTable == null) return;

                m_pTable.Rows[index].Delete();                
            }
        }

        public void SetTextInCell(int Column, int Row, string strText, int indTable = -1, string strDefault = "", bool bAdd = false)
        {
            if (strText == "")
            {
                strText = strDefault;
                if (strText == "") return;
            }

            int top = 1;
            int left = 1;
            //MSWord.Columns pCols = null;
            //MSWord.Rows pRows = null;
            MSWord.Cell pCell = null;
            MSWord.Selection pSel = null;

            GetTableReference(indTable);

            //pCols = m_pTable.Columns;
            //if (pCols == null) return;
            //pRows = m_pTable.Rows;
            //if (pRows == null) return;

            if (Column >= 0) left = Column;
            if (Row >= 0) top = Row;

            // проверяем правильность установки границ диапазона
            // получаем верхнюю левую ячейку диапазона
            pCell = m_pTable.Cell(top, left);
            if (pCell == null) return;

            if (!bAdd)
            {
                pCell.Range.Text = strText;
            }
            else
            {
                pCell.Select();
                pSel = m_pApp.Selection;
                pSel.EndKey();

                pSel.Text = strText;
                pSel.Collapse(0);
            }
        }

        public void GetTableReference(int indTable)
        {
            if (indTable == -1)
            {
	            if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
	            if (m_pDoc == null) return;

                MSWord.Tables tables = m_pDoc.Tables;
                if (tables.Count == 0) return;

                m_pTable = tables[m_pDoc.Tables.Count];
	            if (m_pTable == null) return;

                old_table_index = tables.Count;
	            return;
            }

            if (indTable != old_table_index) 
            {
	            if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
	            if (m_pDoc == null) return;

                MSWord.Tables tables = m_pDoc.Tables;
                if (tables.Count == 0 || tables.Count < indTable) return;

                m_pTable = tables[indTable];
                if (m_pTable == null) return;

	            old_table_index = indTable;
	            return;
            }

            if (m_pTable == null)
            {
	            if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
	            if (m_pDoc == null) return;

                MSWord.Tables tables = m_pDoc.Tables;
                if (tables.Count == 0) return;

                m_pTable = tables[tables.Count];
                if (m_pTable == null) return;

                old_table_index = tables.Count;
                return;
            }
        }

        public void ConvertToTable(string strText, int row_cnt = -1, int col_cnt = -1, bool bSetBorders = true, string bookmark = "")
        {
            string strReturn = "" + (char)10;
            bool bUseReturn = false;
            if (strText.IndexOf(strReturn) >= 0)
                bUseReturn = true;

            strText = strText.Replace(strReturn, "#enter#");

            if (bookmark == "")
            {
 		        if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
		        if (m_pDoc == null) return;

		        m_pDoc.Select();
		        m_pSelection = m_pApp.Selection;
		        if (m_pSelection == null) return;

  	            m_pSelection.Collapse(0);

                object varInfo = m_pSelection.get_Information(MSWord.WdInformation.wdWithInTable);
                if (Convert.ToBoolean(varInfo) == true)
                {
                    m_pDoc.Select();
                    m_pSelection = m_pApp.Selection;
                    if (m_pSelection == null) return;
                    m_pSelection.Collapse(0);
                }
            }
            else 
            {
                m_pSelection.GoTo(-1, Type.Missing, Type.Missing, bookmark);
            };

            m_pSelection.Font.Bold = 0;
            m_pSelection.Text = strText;

            m_pSelection.Paragraphs.SpaceAfter = 0;
            m_pSelection.Paragraphs.SpaceBefore = 0;
            m_pSelection.Paragraphs.LineSpacingRule = MSWord.WdLineSpacing.wdLineSpaceSingle;


            if (row_cnt < 0 && col_cnt < 0)
            {
                m_pTable = m_pSelection.ConvertToTable(1, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, 0, 0);
            }
            else
            {
                m_pTable = m_pSelection.ConvertToTable(1, row_cnt, col_cnt, 0, 0,
                    Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                    Type.Missing, 0, 0);
            }

	        // выравниваем талицу (не содержимое!!!)
	        if (m_pTable == null) return;

	        MSWord.Rows pRows = m_pSelection.Rows;
	        pRows.AllowBreakAcrossPages = 0;

	        m_pSelection.Collapse(0);

	        MSWord.Borders pBorders = m_pTable.Borders;
	        if (pBorders == null) return;
            if (bSetBorders)
	            pBorders.Enable = 1;
            else
                pBorders.Enable = 0;

            if (bUseReturn)
            {
                m_pTable.Select();
                MSWord.Selection sel = m_pApp.Selection;

                MSWord.Find find = sel.Find;
                find.ClearFormatting();
                MSWord.Replacement replace = find.Replacement;
                replace.ClearFormatting();
                find.Text = "#enter#";
                replace.Text = "^p";
                find.MatchCase = false;
                find.MatchWholeWord = false;
                find.MatchWildcards = false;
                find.MatchSoundsLike = false;
                find.MatchAllWordForms = false;
                find.Wrap = MSWord.WdFindWrap.wdFindContinue;
                MSWord.WdReplace repl = MSWord.WdReplace.wdReplaceAll;
                find.Execute(find.Text, find.MatchCase, find.MatchWholeWord, find.MatchWildcards,
                    find.MatchSoundsLike, find.MatchAllWordForms, Type.Missing, find.Wrap, Type.Missing, replace.Text, repl); 
            }
        }

        public void SplitCell(int Column, int FirstRow, int LastRow, int iCntCols, int iCntRows, int indTable = -1)
        {
            GetTableReference(indTable);
            for (int i = FirstRow; i <= LastRow; i++)
            {
                MSWord.Cell pCellBegin = m_pTable.Cell(i, Column);
                pCellBegin.Select();
                m_pApp.Selection.Cells.Split(iCntRows, iCntCols, false);
            }
        }
        
        public void DeleteColumn(int indCol, int indTable)
        {
            GetTableReference(indTable);
            MSWord.Columns pCols = m_pTable.Columns;
            if (pCols == null) return;
            MSWord.Column pCol = pCols[indCol];
            pCol.Delete();
        }

    
        public void SetCellsMerge(int FirstColumn, int LastColumn, int FirstRow, int LastRow, MSWord.WdCellVerticalAlignment vAlignment, int indTable = -1)
        {
            if (FirstColumn == LastColumn && FirstRow == LastRow) return;

	        GetTableReference(indTable);

	        MSWord.Cell pCellBegin = m_pTable.Cell(FirstRow, FirstColumn);
	        MSWord.Cell pCellEnd = m_pTable.Cell(LastRow, LastColumn);
	        if (pCellBegin == null) return;
            if (pCellEnd == null) return;

            pCellBegin.Merge(pCellEnd);
	        pCellBegin.VerticalAlignment = vAlignment;
        }

        public void SetTableAlignment(MSWord.WdParagraphAlignment hAlign, MSWord.WdCellVerticalAlignment vAlign, int indTable = -1)
        {
            GetTableReference(indTable);

            m_pTable.Select();
            MSWord.Selection sel = m_pApp.Selection;
            sel.Cells.VerticalAlignment = vAlign;
            sel.ParagraphFormat.Alignment = hAlign;
        }

        public void SetCellsFont(int FirstColumn, int LastColumn, int FirstRow, int LastRow, FontParams fp, int indTable = -1)
        {
            GetTableReference(indTable);

            MSWord.Cell pCellBegin = m_pTable.Cell(FirstRow, FirstColumn);
            MSWord.Cell pCellEnd = m_pTable.Cell(LastRow, LastColumn);
            if (pCellBegin == null) return;
            if (pCellEnd == null) return;

            MSWord.Range range = m_pDoc.Range(pCellBegin.Range.Start, pCellEnd.Range.End);
            range.Select();
            PutFont(fp, m_pApp.Selection);
        }

        public void SetCellsFont(int FirstColumn, int LastColumn, int FirstRow, int LastRow, string strName, int bold, float size, int indTable)
        {
            GetTableReference(indTable);

            MSWord.Cell pCellBegin = m_pTable.Cell(FirstRow, FirstColumn);
            MSWord.Cell pCellEnd = m_pTable.Cell(LastRow, LastColumn);
            if (pCellBegin == null) return;
            if (pCellEnd == null) return;

            MSWord.Range range = m_pDoc.Range(pCellBegin.Range.Start, pCellEnd.Range.End);
            range.Select();
            MSWord.Selection sel = m_pApp.Selection;
            //MSWord.Font old_font = sel.Font;
            if (strName != "") sel.Font.Name = strName;
            if (bold != -1) sel.Font.Bold = bold;
            if (size > 0) sel.Font.Size = size;
            //sel.Font = old_font;
        }

        public void SetCellColor(int iCol, int iRow, MSWord.WdColor color, MSWord.WdColor text_color = MSWord.WdColor.wdColorAutomatic, int indTable = -1)
        {
            if (text_color == MSWord.WdColor.wdColorAutomatic && color == MSWord.WdColor.wdColorAutomatic) return;

            GetTableReference(indTable);

            MSWord.Cell pCell = m_pTable.Cell(iRow, iCol);
            if (pCell == null) return;
            pCell.Select();
            MSWord.Selection sel = m_pApp.Selection;
            sel.Shading.Texture = MSWord.WdTextureIndex.wdTextureNone;
            sel.Shading.ForegroundPatternColor = MSWord.WdColor.wdColorAutomatic;
            sel.Shading.BackgroundPatternColor = color;
            if (text_color != MSWord.WdColor.wdColorAutomatic)
                sel.Font.Color = text_color;
        }

        public void SetCellsColor(int iCol1, int iCol2, int iRow1, int iRow2, MSWord.WdColor color, MSWord.WdColor text_color = MSWord.WdColor.wdColorAutomatic, int indTable = -1)
        {
            GetTableReference(indTable);

            MSWord.Cell pCell1 = m_pTable.Cell(iRow1, iCol1);
            if (pCell1 == null) return;
            MSWord.Cell pCell2 = m_pTable.Cell(iRow2, iCol2);
            if (pCell2 == null) return;

            MSWord.Range r = m_pDoc.Range(pCell1.Range.Start, pCell2.Range.End);
            r.Select();
            MSWord.Selection sel = m_pApp.Selection;
            sel.Shading.Texture = MSWord.WdTextureIndex.wdTextureNone;
            sel.Shading.ForegroundPatternColor = MSWord.WdColor.wdColorAutomatic;
            sel.Shading.BackgroundPatternColor = color;
            if (text_color != MSWord.WdColor.wdColorAutomatic)
                sel.Font.Color = text_color;
        }

        public string Conv2Char(int col, int row)
        // Процедура преобразования числовых координат, используемых в CGridCtrl,
        // в текстовый вид типа "A3", используемый в Excel. 
        // Входные параметры:
        // X - колонка, Y - строка в CGridCtrl,
        // buf - указатель на строку, в которую должен быть записан текстовый вид координат.
        // Возвращает функция этот же указатель.
        // Ограничения на X: X<676. Ограничений на Y нет.
        // Размер буффера желательно не меньше 10.
        {
	        // в Excel - 256 ячеек (IV)
	        // в Calc  - 1024 ячеек (AMJ)

	        if(col >= 676)
	        {
                byte ch = Encoding.ASCII.GetBytes("A")[0];
                ch += (byte)((col / 676) - 1);

                byte ch2 = Encoding.ASCII.GetBytes("A")[0];
                ch2 += (byte)(((col % 676) / 26) - 1);

                byte ch3 = Encoding.ASCII.GetBytes("A")[0];
                ch3 += (byte)((col % 676) % 26);

                return Convert.ToChar(ch) + Convert.ToChar(ch2) + Convert.ToChar(ch3) + (row + 1).ToString();
	        }
	        else
	        {
                if (col >= 26)
                {
                    byte ch = Encoding.ASCII.GetBytes("A")[0];
                    ch += (byte)((col / 26) - 1);

                    byte ch2 = Encoding.ASCII.GetBytes("A")[0];
                    ch2 += (byte)(col % 26);

                    return Convert.ToChar(ch) + Convert.ToChar(ch2) + (row + 1).ToString();
                }
                else
                {
                    byte ch = Encoding.ASCII.GetBytes("A")[0];
                    ch += (byte)col;

                    return Convert.ToChar(ch) + (row + 1).ToString();
                }
	        }
        }

        public string GetCellsText(int iCol1, int iCol2, int iRow1, int iRow2, int indTable = -1)
        {
            GetTableReference(indTable);

            MSWord.Cell pCell1 = m_pTable.Cell(iRow1, iCol1);
            if (pCell1 == null) return "";
            MSWord.Cell pCell2 = m_pTable.Cell(iRow2, iCol2);
            if (pCell2 == null) return "";

            MSWord.Range r = m_pDoc.Range(pCell1.Range.Start, pCell2.Range.End);
            r.Select();
            MSWord.Selection sel = m_pApp.Selection;

            return sel.Text;
        }

        public void DeleteVirtualRow(int iRow)
        {
            for (int iCol = 0; iCol < m_Table.Count; iCol++)
            {
                m_Table[iCol].m_Cells.RemoveAt(iRow - 1);
            }
        }

        public string GetVirtualCellsText(int iCol1, int iCol2, int iRow1, int iRow2, int indTable = -1)
        {
            string strText = "";
            for (int iCol = iCol1; iCol <= iCol2; iCol++)
            {
                for (int iRow = iRow1; iRow <= iRow2; iRow++)
                {
                    strText += m_Table[iCol - 1].m_Cells[iRow - 1].strText;
                }
            }

            return strText;
        }

        public void SetCellsAlignment(int iCol1, int iCol2, int iRow1, int iRow2, MSWord.WdParagraphAlignment hAlign, MSWord.WdCellVerticalAlignment vAlign, int indTable = -1)
        {
            GetTableReference(indTable);

            MSWord.Cell pCell1 = m_pTable.Cell(iRow1, iCol1);
            if (pCell1 == null) return;
            MSWord.Cell pCell2 = m_pTable.Cell(iRow2, iCol2);
            if (pCell2 == null) return;

            MSWord.Range r = m_pDoc.Range(pCell1.Range.Start, pCell2.Range.End);
            r.Select();
            MSWord.Selection sel = m_pApp.Selection;
            sel.ParagraphFormat.Alignment = hAlign;
            sel.Cells.VerticalAlignment = vAlign;
        }

        public void InsertPicture(string strFileName, MSWord.WdParagraphAlignment hAlign, string bookmark)
        {
	        if (bookmark == "") 
            {
 		        if (m_pDoc == null) m_pDoc = m_pApp.ActiveDocument;
		        if (m_pDoc == null) return;

		        m_pDoc.Select();
		        m_pSelection = m_pApp.Selection;
                if (m_pSelection == null) return;

              	m_pSelection.Collapse(0);
            }
            else 
            {
                GotoBookmark(bookmark);
            }

	        MSWord.ParagraphFormat pParFormat = m_pSelection.ParagraphFormat;
            if (pParFormat == null) return;
            pParFormat.Alignment = hAlign;
            MSWord.InlineShapes shapes = m_pSelection.InlineShapes;
            if (shapes == null) return;
            shapes.AddPicture(strFileName, false, true);
        }

        public void DeleteRowEx(int Row, int indTable = -1)
        {
            GetTableReference(indTable);

            MSWord.Cell pCell = m_pTable.Cell(Row, 1);
            pCell.Select();
            m_pSelection = m_pApp.Selection;
            if (m_pSelection == null) return;

            //m_pSelection.MoveDown(MSWord.WdUnits.wdLine, 2);
            m_pSelection.Rows.Delete();
        }

        public void DeleteColumnEx(int Column, int indTable)
        {
            GetTableReference(indTable);

            MSWord.Cell pCell = m_pTable.Cell(1, Column);
            pCell.Select();
            m_pSelection = m_pApp.Selection;
            if (m_pSelection == null) return;

            //m_pSelection.MoveDown(MSWord.WdUnits.wdLine, 2);
            m_pSelection.Columns.Delete();
        }

        public void SetColumnWidth(int Column, double Width_, int indTable = -1)
        {
            GetTableReference(indTable);

	        MSWord.Columns pColumns = m_pTable.Columns;
	        if (pColumns == null) return;
	        int cntColumns = pColumns.Count;
            if (Column >= 1 && Column <= cntColumns) 
            {
    		    MSWord.Column pColumn = pColumns[Column];
		        if (pColumn == null) return;
                float w = m_pApp.MillimetersToPoints((float)Width_);
                if (Math.Abs(w) >= 0.009)
                    pColumn.Width = w;
                else
                    pColumn.Delete();
                //pColumns[Column].SetWidth(w, MSWord.WdRulerStyle.wdAdjustSameWidth);
            }
        }


        public class BorderInfo
        {
            public BorderInfo(MSWord.WdBorderType type, MSWord.WdLineStyle style, MSWord.WdLineWidth width, MSWord.WdColor color)
            {
                m_type = type;
                m_style = style;
                m_width = width;
                m_color = color;
            }

            public MSWord.WdBorderType m_type;
            public MSWord.WdLineStyle m_style;
            public MSWord.WdLineWidth m_width;
            public MSWord.WdColor m_color;
        }

        public void SetTableBordersColor(long color, int indTable = -1)
        {
            GetTableReference(indTable);

            m_pTable.Select();

            m_pSelection = m_pApp.Selection;
            if (m_pSelection == null) return;

            MSWord.Borders pBorders = m_pSelection.Borders;
            pBorders.Enable = 1;
            pBorders[MSWord.WdBorderType.wdBorderBottom].Color = (MSWord.WdColor)color;
            pBorders[MSWord.WdBorderType.wdBorderHorizontal].Color = (MSWord.WdColor)color;
            pBorders[MSWord.WdBorderType.wdBorderLeft].Color = (MSWord.WdColor)color;
            pBorders[MSWord.WdBorderType.wdBorderRight].Color = (MSWord.WdColor)color;
            pBorders[MSWord.WdBorderType.wdBorderTop].Color = (MSWord.WdColor)color;
            pBorders[MSWord.WdBorderType.wdBorderVertical].Color = (MSWord.WdColor)color;
        }

        public void SetCellsBorders(int iCol1, int iCol2, int iRow1, int iRow2, List<BorderInfo> listBorders, int indTable = -1 )
        {
	        GetTableReference(indTable);

	        MSWord.Cell pCell1 = m_pTable.Cell(iRow1, iCol1);
            if (pCell1 == null) return;
            MSWord.Cell pCell2 = m_pTable.Cell(iRow2, iCol2);
            if (pCell2 == null) return;

            MSWord.Range r = m_pDoc.Range(pCell1.Range.Start, pCell2.Range.End);
	        r.Select();

            m_pSelection = m_pApp.Selection;
            if (m_pSelection == null) return;

            MSWord.Borders borders = m_pSelection.Borders;

	        for ( short index = 0; index < listBorders.Count; index++ )
	        {
                if (listBorders[index].m_type == MSWord.WdBorderType.wdBorderHorizontal && iRow1 == iRow2)
			        continue;

                if (listBorders[index].m_type == MSWord.WdBorderType.wdBorderVertical && iCol1 == iCol2)
			        continue;

                borders[listBorders[index].m_type].LineStyle = listBorders[index].m_style;
                if (listBorders[index].m_style != MSWord.WdLineStyle.wdLineStyleNone)
                {
                    borders[listBorders[index].m_type].LineWidth = listBorders[index].m_width;
                    borders[listBorders[index].m_type].Color = listBorders[index].m_color;
                }		        
	        }
        }

        public void SetCellList(int iCol, int iRow, int indTable)
        {
            GetTableReference(indTable);
            MSWord.Cell pCell = m_pTable.Cell(iRow, iCol);
            if (pCell == null) return;

            pCell.Select();

            m_pSelection = m_pApp.Selection;
            if (m_pSelection == null) return;

            m_pApp.ListGalleries[MSWord.WdListGalleryType.wdBulletGallery].ListTemplates[1].ListLevels[1].NumberFormat = "-";
            m_pSelection.Range.ListFormat.ApplyListTemplateWithLevel(m_pApp.ListGalleries[MSWord.WdListGalleryType.wdBulletGallery].ListTemplates[1], false,
                MSWord.WdListApplyTo.wdListApplyToWholeList, MSWord.WdDefaultListBehavior.wdWord10ListBehavior);
        }
    }
}
