using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;


namespace GraphLib
{

    public struct DPoint
    {
        public float X;
        public float Y;
        public bool StartPt;
    }

    public enum XAxisType
    {
        None = 0,
        Auto = 1,
        SetSpan = 2,
        MinMax = 3
    }

    public enum YAxisType
    {
        None = 0,
        Auto = 1,
        Free = 2,
        MinMax = 3
    }

    public enum GraphManipulationType
    {
        None = 0,
        WbWaferStartStop = 1,
        GeneralMarker = 2,
        NotSet = 3,
        MeasurementMarker = 4
    }

    public enum GraphType
    {
        None = 0,
        Graph = 1,
        MoveableMarker = 2,
        Boundary = 3,
        Live = 4,
        FixedMarker = 5
    }

 
    [Flags]
    public enum GraphOption
    {
        None = 0,
        Master = 1,
        Visible = 2,
        Highlight = 3,
        AsPoint = 4,
        AsYAxis = 5,
        Colour = 6,

        MarkerBoundaryMenu = 8,
        TopLevelGraph = 9,
        BoundaryGraph = 10,
        Legend = 13,
        ShowAllGraphs = 14,
        HideAllGraphs = 15
    }

    public enum GraphColour
    {
        Black = 0,
        Red = 1,
        Green = 2,
        Blue = 3,
        Yellow = 4,
        White = 5,
        Brown = 6,
        Purple = 7,
        LightGreen = 8,
        Gray = 9,
        GoldenRod = 10
    }       

    public class GraphControl
    {
        public bool UseForYScale { get; set; }
        public bool Visible;
        public bool CanBeVisible;
        public bool AsPoint;
        public bool AsBar;
        public bool Highlight;
        public bool Master;
        public int Tag;
        public int BarOffset;  
        public Pen Colour;          
        public DPoint[] DisplayPoints;
        public Point[] ScrPoints;
        public int ScrPCount;
        public int MaxScreenPCount;
        public double DisplayYScale;
        public GraphControl(int tag,Pen col)
        {
            Tag = tag;
            Colour = col;
            AsPoint = false;
            AsBar = false;
            Highlight = false;
            Master = true;
            Visible = true;
            CanBeVisible = true;
            UseForYScale = true;
            BarOffset = 0;       
            DisplayYScale = 1.0;
            MaxScreenPCount = 3000;
            ScrPCount = 0;
            ScrPoints = new Point[3000];
            DisplayPoints = new DPoint[3000]; 
        }
    }
  
    public class GraphSurface
    {
        public DPoint[] DPts;
        public int PtCount;
        public int MaxPtCount;         
        public double MaxD;
        public double MinD;
        public double XScale;
       // public double YScale;
        public string Name;
        public string Source;
        public GraphType GType;
        public GraphManipulationTag ObjectTag;
        public int TagId;
        public string GxAxisTitle;
        public string GyAxisTitle;
        public double DyScale;
    }

    public class PanelControl : IDisposable
    {
        public Panel GPan;
        public double PercentHeight;
        public ContextMenu ConMenu;
        public ContextMenu BoundaryConMenu;         
        public ContextMenu MarkerConMenu;
        public Graphics PanSurface;
        public Graphics GrSurface;
        public Graphics HCursorSurface;
        public Graphics VCursorSurface;
        public Graphics RectHSurface;
        public Graphics RectVSurface;
        public Graphics CrossSurface;
        public Bitmap HCursorImage;
        public Bitmap VCursorImage;
        public Bitmap RectHImage;
        public Bitmap RectVImage;
        public Bitmap DrawingImage;
        public Bitmap CrossImage;
        public bool Zooming;
        public double YScale;
        public double XScale;
        public double XOffset;
        public double YOffset;
        public int XPan;
        public int YPan;
        public int XPanStart;
        public int YPanStart;
        public YAxisType YAxisType;         
        public XAxisType XAxisType;
        public string XAxisSpanLabel;          
        public double XAxisSpan;
        public double XAxisMin;
        public double XAxisMax;
        public double YAxisMin;
        public double YAxisMax;
        public bool NeedToSetScale;
        public GraphControl[] TagList;
        public Button ExpandYTop;
        public Button ContractYTop;
        public Button ExpandYBottom;
        public Button ContractYBottom;
        public Button Reset;
        public bool MarkerMenuSet;
        public Legend Legend;
        public string XAxisTitle;
        public string YAxisTitle;
        public string LegendTitle;


        public PanelControl(GraphPanel gPanel,int tag)
        {

            ConMenu = new ContextMenu();
            
            BoundaryConMenu = new ContextMenu();
            MarkerConMenu = new ContextMenu();
            Legend = new Legend(gPanel);
            GPan = new Panel
            {
                Tag = tag,
                BackColor = Color.WhiteSmoke,              
                BorderStyle = BorderStyle.Fixed3D
            };

            LegendTitle = gPanel.PanelName;
            MarkerMenuSet = true;
            PanSurface = null;
            GrSurface = null;
            HCursorSurface = null;
            VCursorSurface = null;
            RectHSurface = null;
            RectVSurface = null;
            CrossSurface = null;
            HCursorImage = null;
            VCursorImage = null;
            RectHImage = null;
            RectVImage = null;
            DrawingImage = null;
            CrossImage = null;
               
            PercentHeight = 100.0;
               
            Zooming = false;
            YScale = 1.0;
            XScale = 1.0;
            XOffset = 0;
            YOffset = 0;
            XPan = 0;
            YPan = 0;
            XPanStart = 0;
            YPanStart = 0;
            YAxisType = YAxisType.Auto;
            XAxisType = XAxisType.Auto;
            XAxisSpanLabel = "In Seconds";
            XAxisSpan = 500.0;
            XAxisMin = 0.0;
            XAxisMax = 500.0;
            YAxisMin = 0.0;
            YAxisMax = 500.0;
            NeedToSetScale = true;
               
            XAxisTitle = "Time(secs)";
            YAxisTitle = "Counts";
            TagList = null;
              
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            try
            {
                ConMenu.Dispose();
                BoundaryConMenu.Dispose();
                MarkerConMenu.Dispose();
                Legend.Dispose();
                GPan.Dispose();
            }
            // ReSharper disable once EmptyGeneralCatchClause               
            catch
            {                  
            }
        }
    }
      
    public class GrTabControl
    {
        public GraphSurface[] Graphs;
        public TabPage Page;
        public Graphics TabSurface;
        public Graphics GrSurface;
        public Bitmap DrawingImage;       
        public PanelControl[] SubPan;
        public PanelControl MainPan;       
        public Button ExpandX;
        public Button ContractX;
        public Button ResetX;
    }

       

    public struct LiveData
    {
        public ArrayList LivePoints;
        public int TabId;
        public int GraphId;
        public bool Started;
        public int CurrentPt;
    }

    public struct MenuTag
    {
        public int GraphTag;
        public int PanelTag;           
        public GraphOption ItemTag;
        public GraphColour ColourTag;    
    }

    public class GraphManipulationTag
    {
        public int MyId;
        public int GroupId;
        public int GraphTag;
        public GraphManipulationType TypeId;          
        public double[] Pts;
        public double[] Values;

        public GraphManipulationTag()
        {
            MyId = 0;
            TypeId = GraphManipulationType.NotSet;             
        }
    }

    public struct Ep
    {
        public int X;
        public int Y;
        public GraphSurface Gs;
        public Color Col;
    }

    // ReSharper disable once IdentifierTypo
    public struct Anam
    {
        public double X;
        public int Gid;
    }


}
