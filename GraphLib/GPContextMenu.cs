using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GraphLib
{
    public class GpContextMenu
    {
        private readonly GraphPanel _graphPanel;
        private readonly Params _graphParams;
        private readonly GrTabPanel _graphTabPanel;  
        private bool _tickVisible = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        public GpContextMenu(GraphPanel g)
        {
            _graphPanel = g;
            _graphParams = _graphPanel.GraphParameters;
            _graphTabPanel = _graphPanel.GraphParameters.GraphTabPanel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctxMenu"></param>
        /// <param name="graphTag"></param>
        /// <param name="setIt"></param>
        internal static void SetHighlight(ContextMenu ctxMenu, int graphTag, bool setIt)
        {         
            foreach(MenuItem t in ctxMenu.MenuItems)
                SetHighlight2(t, graphTag, setIt);
        }

        internal static void SetSolid(ContextMenu ctxMenu, int graphTag, bool setIt)
        {
            foreach (MenuItem t in ctxMenu.MenuItems)
                SetSolid2(t, graphTag, setIt);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="graphTag"></param>
        /// <param name="setIt"></param>
        private static void SetHighlight2(MenuItem m, int graphTag, bool setIt)
        {
           
            if (m.MenuItems.Count <= 0)
            {
                var mTag = (MenuTag) m.Tag;

                if (mTag.ItemTag != GraphOption.Highlight)
                    return;
                if (mTag.GraphTag == graphTag)
                    m.Checked = setIt;
            }
            else
            {
                foreach (MenuItem t in m.MenuItems)
                    SetHighlight2(t, graphTag, setIt);                
            }
        }

        private static void SetSolid2(MenuItem m, int graphTag, bool setIt)
        {
            var cnt = m.MenuItems.Count;
            if (cnt <= 0)
            {
                var mTag = (MenuTag)m.Tag;

                if (mTag.ItemTag != GraphOption.AsPoint)
                    return;
                if (mTag.GraphTag == graphTag)
                    m.Checked = setIt;
            }
            else
            {
                foreach (MenuItem t in m.MenuItems)
                    SetSolid2(t, graphTag, setIt);               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gTag"></param>
        /// <param name="col"></param>
        internal void SetGraphMenuColourTick(int gTag, Pen col)
        {
            if (col.Equals(Pens.Red))
                SetMenuColorTick(gTag, GraphColour.Red);
            else if (col.Equals(Pens.Blue))
                SetMenuColorTick(gTag, GraphColour.Blue);
            else if (col.Equals(Pens.Green))
                SetMenuColorTick(gTag, GraphColour.Green);
            else if (col.Equals(Pens.LightGreen))
                SetMenuColorTick(gTag, GraphColour.LightGreen);
            else if (col.Equals(Pens.Black))
                SetMenuColorTick(gTag, GraphColour.Black);
            else if (col.Equals(Pens.Brown))
                SetMenuColorTick(gTag, GraphColour.Brown);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gTag"></param>
        /// <param name="col"></param>
        private void SetMenuColorTick(int gTag, GraphColour col)
        {
            foreach (MenuItem t in _graphTabPanel.Cst.MainPan.ConMenu.MenuItems)
            {
                if (t.MenuItems.Count > 0)
                    SetColTick(gTag, t, col);
            }
    
        }
       
        private static void SetColTick(int gTag, Menu mi, GraphColour col)
        {
            foreach (MenuItem t in mi.MenuItems)
            {                          
                var tag = (MenuTag)t.Tag;
                if (tag.GraphTag != gTag)
                    continue;

                if (tag.ItemTag == GraphOption.Colour)
                {
                    foreach (MenuItem tt in t.MenuItems)                      
                    {                      
                        tag = (MenuTag)tt.Tag;                       
                        if (tag.ItemTag != GraphOption.Colour)
                            continue;
                        tt.Checked = tag.ColourTag == col;
                    }
                }
                else
                {
                    if (t.MenuItems.Count > 0)
                        SetColTick(gTag, t, col);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int UpdateMarkerBoundary()
        {
           
            var marMid = -1;
          

            if (!_graphTabPanel.Cst.MainPan.MarkerMenuSet)
            {
                var m1 = new MenuItem("Markers/Boundarys");
                _graphTabPanel.Cst.MainPan.MarkerMenuSet = true;

                m1.Tag = CreateMenuTag(_graphPanel.Gdata.AbsoluteGraphTag, 0, GraphOption.MarkerBoundaryMenu);
                _graphTabPanel.Cst.MainPan.ConMenu.MenuItems.Add(m1);
                marMid = _graphTabPanel.Cst.MainPan.ConMenu.MenuItems.Count - 1;

                if (_graphTabPanel.Cst.SubPan == null)
                    return marMid;
                var id = 1;
                foreach (var t in _graphTabPanel.Cst.SubPan)
                {
                    m1 = new MenuItem("Markers/Boundarys");
                    t.MarkerMenuSet = true;
                    m1.Tag = CreateMenuTag(_graphPanel.Gdata.AbsoluteGraphTag, id, GraphOption.MarkerBoundaryMenu);
                    t.ConMenu.MenuItems.Add(m1);
                    ++id;
                }
            }
            else
            {
                 
                for (var i = 0; i < _graphTabPanel.Cst.MainPan.ConMenu.MenuItems.Count; i++)
                {                 
                    var mt = (MenuTag) _graphTabPanel.Cst.MainPan.ConMenu.MenuItems[i].Tag;
                    if (mt.ItemTag != GraphOption.MarkerBoundaryMenu)
                        continue;
                    marMid = i;
                    break;
                }
            }
            return marMid;
        }

        private void UpdateContextBoundary(string title)
        {
            
            var titList = title.Split(',');
            var marMid = UpdateMarkerBoundary();

            var menuItem1 = titList.Length > 1 ? new MenuItem(titList[1]) : new MenuItem(title);
            menuItem1.Tag = CreateMenuTag(_graphPanel.Gdata.AbsoluteGraphTag, 0, GraphOption.BoundaryGraph);
            menuItem1.Checked = false;
            AddCMenuItems(menuItem1, _graphPanel.Gdata.AbsoluteGraphTag, 0, false);
            menuItem1.MenuItems[0].Checked = _graphParams.InitiallyVisible;
            if (marMid == -1)
                return;
            _graphTabPanel.Cst.MainPan.ConMenu.MenuItems[marMid].MenuItems.Add(menuItem1);
            if (_graphTabPanel.Cst.SubPan == null)
                return;
            for (var i = 0; i < _graphTabPanel.Cst.SubPan.Length; i++)
            {
                menuItem1 = new MenuItem(title)
                {
                    Tag = CreateMenuTag(_graphPanel.Gdata.AbsoluteGraphTag, i + 1, GraphOption.BoundaryGraph)
                };
                AddCMenuItems(menuItem1, _graphPanel.Gdata.AbsoluteGraphTag, i + 1, false);
                _graphTabPanel.Cst.SubPan[i].ConMenu.MenuItems[marMid].MenuItems.Add(menuItem1);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gType"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        internal void UpdateContextMenu(GraphType gType, string title)
        {
            if (_graphParams.MissFromContextMenu)
                return;

            if (GrBoundary.IsAnyMarkerOrBoundary(gType))
            {
                UpdateContextBoundary(title);
                return;              
            }
                 
            var menuI = new MenuItem(title)
            {
                Tag = CreateMenuTag(_graphPanel.Gdata.AbsoluteGraphTag, 0, GraphOption.TopLevelGraph)
          
            };
          
            AddCMenuItems(menuI, _graphPanel.Gdata.AbsoluteGraphTag, 0, true);
            menuI.MenuItems[0].Checked = _graphParams.InitiallyVisible;                        
            _graphTabPanel.Cst.MainPan.ConMenu.MenuItems.Add(menuI);

            if (_graphTabPanel.Cst.SubPan == null)
                return;
            var panelTag = 1;
            foreach (var t in _graphTabPanel.Cst.SubPan)
            {
                menuI = new MenuItem(title)
                {
                    Tag = CreateMenuTag(_graphPanel.Gdata.AbsoluteGraphTag, panelTag, GraphOption.TopLevelGraph)
                };
                AddCMenuItems(menuI, _graphPanel.Gdata.AbsoluteGraphTag, panelTag, true);
                menuI.MenuItems[0].Checked = _graphParams.InitiallyVisible;
                t.ConMenu.MenuItems.Add(menuI);
                ++panelTag;
            }
         
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="panelTag"></param>
        internal void CopyMenu(ContextMenu dest, ContextMenu src, int panelTag)
        {
            if (src.MenuItems.Count <= 0)
                return;

            if (dest.MenuItems.Count > 0)
                dest.MenuItems.Clear();

            for (var i = 0; i < src.MenuItems.Count; i++)
            {
                var sm = src.MenuItems[i];
                var m = new MenuItem(sm.Text);
                var mt = new MenuTag();
                var smt = (MenuTag) sm.Tag;              
                mt.GraphTag = smt.GraphTag;
                mt.PanelTag = panelTag + 1;
                mt.ItemTag = smt.ItemTag;
                m.Tag = mt;

                switch (mt.ItemTag)
                {
                    case GraphOption.AsYAxis:
                        m.Click += _graphTabPanel.AxisMenuContextClick;
                        break;
                    case GraphOption.Legend:
                        m.Click += _graphTabPanel.LegendMenuContextClick;
                        break;
                    case GraphOption.ShowAllGraphs:
                        m.Click += _graphTabPanel.ShowAllMenuContextClick;
                        break;
                    case GraphOption.HideAllGraphs:
                        m.Click += _graphTabPanel.HideAllMenuContextClick;
                        break;                  
                    default:
                        m.Click += MenuContextClick;
                        break;
                }

                dest.MenuItems.Add(m);
                if (sm.MenuItems.Count <= 0)
                    continue;
              
               
                CopyMenu2(m, sm, panelTag);
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="panelTag"></param>
        private void CopyMenu2(Menu dest, Menu src, int panelTag)
        {
            foreach(MenuItem sm in src.MenuItems)       
            {              
                var m = new MenuItem(sm.Text);
                var mt = new MenuTag();
                var smt = (MenuTag) sm.Tag;               
                mt.GraphTag = smt.GraphTag;
                mt.ColourTag = smt.ColourTag;
                mt.PanelTag = panelTag + 1;
                mt.ItemTag = smt.ItemTag;
                m.Tag = mt;
            
                switch (mt.ItemTag)
                {
                    case GraphOption.AsYAxis:
                        m.Click += _graphTabPanel.AxisMenuContextClick;
                        break;
                    case GraphOption.Legend:
                        m.Click += _graphTabPanel.LegendMenuContextClick;
                        break;
                    case GraphOption.ShowAllGraphs:
                        m.Click += _graphTabPanel.ShowAllMenuContextClick;
                        break;
                    case GraphOption.HideAllGraphs:
                        m.Click += _graphTabPanel.HideAllMenuContextClick;
                        break;                    
                    default:
                        m.Click += MenuContextClick;
                        break;
                }

                dest.MenuItems.Add(m);
                if (sm.MenuItems.Count > 0)
                    CopyMenu2(m, sm, panelTag);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctxMenu"></param>
        /// <param name="graphTag"></param>
        internal static void SetMaster(ContextMenu ctxMenu, int graphTag)
        {
            foreach (MenuItem m in ctxMenu.MenuItems)
                SetMaster2(m, graphTag);           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="graphTag"></param>
        private static void SetMaster2(MenuItem m, int graphTag)
        {
            if (m.MenuItems.Count <= 0)
            {
                var mTag = (MenuTag) m.Tag;
                if (mTag.ItemTag != GraphOption.Master)
                    return;
                m.Checked = mTag.GraphTag == graphTag;
            }
            else
            {
                foreach (MenuItem mm in m.MenuItems)
                    SetMaster2(mm, graphTag);               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuItem1"></param>
        /// <param name="graphTag"></param>
        /// <param name="panelTag"></param>
        /// <param name="fullMenu"></param>
        private void AddCMenuItems(MenuItem menuItem1, int graphTag, int panelTag, bool fullMenu)
        {       
            menuItem1.MenuItems.Add(CreateSubMenu("Visible", graphTag, panelTag, GraphOption.Visible));
           
            if (fullMenu)
            {
                menuItem1.MenuItems.Add(CreateSubMenu("Master Controller", graphTag, panelTag, GraphOption.Master));
                menuItem1.MenuItems.Add(CreateSubMenu("HighLighted", graphTag, panelTag, GraphOption.Highlight));
                menuItem1.MenuItems.Add(CreateSubMenu("Drawn As Points", graphTag, panelTag, GraphOption.AsPoint));
            }
            else
                menuItem1.MenuItems.Add(CreateSubMenu("HighLighted", graphTag, panelTag, GraphOption.Highlight));

            menuItem1.MenuItems.Add(CreateColourSubMenu(graphTag, panelTag));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="gTag"></param>
        /// <param name="pTag"></param>
        /// <param name="iTag"></param>
        /// <returns></returns>
        private MenuItem CreateSubMenu(string title, int gTag, int pTag, GraphOption iTag)
        {
            // ReSharper disable once IntroduceOptionalParameters.Local
            return CreateSubMenu(title, gTag, pTag, iTag, GraphColour.Black);
        }

        private MenuItem CreateSubMenu(string title, int gTag, int pTag, GraphOption iTag, GraphColour cTag)
        {
            var mTag = new MenuTag
            {
                GraphTag = gTag,               
                PanelTag = pTag,
                ItemTag = iTag,
                ColourTag = cTag              
            };
           
            var menuItem = new MenuItem(title) {Tag = mTag, Checked = false};
            menuItem.Click += MenuContextClick;            
            return menuItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuContextClick(object sender, EventArgs e)
        {
            var id = -1;
            var it = (MenuItem) sender;
            var mTag = (MenuTag) it.Tag;
            var pan = _graphTabPanel.GetPanelControlFromTag(mTag.PanelTag);
            var pm = it.Parent;

            it.Checked = !it.Checked;

            if (pan.TagList == null)
                return;
          
            for (var i = 0; i < pan.TagList.Length; i++)
            {
                if (pan.TagList[i].Tag != mTag.GraphTag) 
                    continue;
                id = i;
                break;
            }
            if (id == -1)
            {
                if (mTag.ItemTag == GraphOption.Visible && it.Checked)
                {
                    TagGraph(pan, mTag, it.Checked);
                    if (pan.Legend.Visible)
                        pan.Legend.UpdateLegend(pan);
                    _graphPanel.RedrawGraphics();
                }
                return;
            }
               

            switch (mTag.ItemTag)
            {
                case GraphOption.Visible:
                    TagGraph(pan, mTag, it.Checked);
                    break;
                case GraphOption.Highlight:
                    pan.TagList[id].Highlight = it.Checked;
                    break;
                case GraphOption.AsPoint:
                    pan.TagList[id].AsPoint = it.Checked;
                    break;
                case GraphOption.Master:
                    foreach (var t in pan.TagList)
                        t.Master = false;
                    if (!it.Checked)
                        id = 0;
                    SetMaster(pan.ConMenu, pan.TagList[id].Tag);
                    pan.TagList[id].Master = true;
                    _graphParams.MasterCallBack();
                    break;
                case GraphOption.Colour:
                    for (var j = 0; j < pm.MenuItems.Count; j++)
                        pm.MenuItems[j].Checked = false;
                    it.Checked = true;
                    pan.TagList[id].Colour = Params.GetPenColour(mTag.ColourTag);
                    break;
            }
            if(pan.Legend.Visible)
                pan.Legend.UpdateLegend(pan);
            _graphPanel.RedrawGraphics();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gTag"></param>
        /// <param name="pTag"></param>
        /// <returns></returns>
        private MenuItem CreateColourSubMenu(int gTag, int pTag)
        {
            var cMenu = CreateSubMenu("Graph Colour", gTag, pTag, GraphOption.Colour, GraphColour.Black);

            cMenu.MenuItems.Add(CreateSubMenu("Black", gTag, pTag, GraphOption.Colour, GraphColour.Black));
            cMenu.MenuItems.Add(CreateSubMenu("Blue", gTag, pTag, GraphOption.Colour, GraphColour.Blue));
            cMenu.MenuItems.Add(CreateSubMenu("Brown", gTag, pTag, GraphOption.Colour, GraphColour.Brown));
            cMenu.MenuItems.Add(CreateSubMenu("Green", gTag, pTag, GraphOption.Colour, GraphColour.Green));
            cMenu.MenuItems.Add(CreateSubMenu("Purple", gTag, pTag, GraphOption.Colour, GraphColour.Purple));
            cMenu.MenuItems.Add(CreateSubMenu("Red", gTag, pTag, GraphOption.Colour, GraphColour.Red));
            cMenu.MenuItems.Add(CreateSubMenu("White", gTag, pTag, GraphOption.Colour, GraphColour.White));
            cMenu.MenuItems.Add(CreateSubMenu("Yellow", gTag, pTag, GraphOption.Colour, GraphColour.Yellow));
            return cMenu;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="mTag"></param>
        /// <param name="tagIt"></param>
        private void TagGraph(PanelControl pan, MenuTag mTag, bool tagIt)
        {
            var matchedId = -1;

            if (pan.TagList == null)
            {
                if (tagIt)
                {
                    pan.TagList = new GraphControl[1];
                    pan.TagList[0] = new GraphControl(mTag.GraphTag,_graphPanel.GetGraphColour(mTag.GraphTag));
                }

                SetMaster(pan.ConMenu, mTag.GraphTag);
                _graphParams.MasterCallBack();
                _graphParams.GraphicsSet = true;
                return;
            }

            foreach (var t in pan.TagList)
                t.Master = false;

            var cnt = pan.TagList.Length;
            for (var i = 0; i < cnt; i++)
            {
                if (pan.TagList[i].Tag == mTag.GraphTag)
                {
                    matchedId = i;
                    break;
                }
            }

            if (matchedId == -1 && tagIt)
            {
                for (var i = 0; i < cnt; i++)
                    pan.TagList[i].Master = false;


                Array.Resize(ref pan.TagList, cnt + 1);
                pan.TagList[cnt] = new GraphControl(mTag.GraphTag, _graphPanel.GetGraphColour(mTag.GraphTag));

                SetMaster(pan.ConMenu, mTag.GraphTag);
                pan.TagList[cnt].Master = true;
                _graphParams.MasterCallBack();
            }
            else if (matchedId != -1)
            {
                pan.TagList[matchedId].Visible = tagIt;
                pan.TagList[matchedId].CanBeVisible = tagIt;
                if (tagIt)
                {
                    SetMaster(pan.ConMenu, pan.TagList[matchedId].Tag);
                    pan.TagList[matchedId].Master = true;
                    _graphParams.MasterCallBack();
                }
                else
                {
                    var found = pan.TagList.Any(t => t.Master);
                    if (!found)
                    {

                        foreach (var t in pan.TagList.Where(t => t.Visible))
                        {
                            found = true;
                            SetMaster(pan.ConMenu, t.Tag);
                            t.Master = true;
                            _graphParams.MasterCallBack();
                            break;
                        }

                        if (!found)
                        {
                            SetMaster(pan.ConMenu, pan.TagList[0].Tag);
                            pan.TagList[0].Master = true;
                            _graphParams.MasterCallBack();
                        }

                        Params.SetGraphMasterPan(pan, mTag.GraphTag, true);

                    }
                }
            }
            _graphParams.GraphicsSet = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="tagV"></param>
        public void SetAsubtag(PanelControl pan, GraphControl tagV)
        {
            var cnt = 0;

            if (pan.TagList != null)
            {
                cnt = pan.TagList.Length;
                for (var i = 0; i < cnt; i++)
                {
                    if (pan.TagList[i].Tag == tagV.Tag)
                        return;
                }
            }

            Array.Resize(ref pan.TagList, cnt + 1);

            pan.TagList[cnt] = new GraphControl(tagV.Tag, tagV.Colour) {Highlight = tagV.Highlight};

            SetTickVisible(true);
            SetVisibleTick(ref pan.ConMenu, pan.TagList[cnt].Tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pan"></param>
        /// <param name="tag"></param>
        /// <param name="visible"></param>
        /// <returns></returns>
        public bool ResetAsubtag(PanelControl pan, int tag, bool visible)
        {
            if (pan.TagList == null)
                return false;

            var asMaster = false;

            if (tag < 0)
            {
                tag = Math.Abs(tag);
                asMaster = true;
            }


            foreach (var t in pan.TagList.Where(t => t.Tag == tag))
            {
                t.Master = asMaster;
                t.Visible = visible;
                t.CanBeVisible = visible;
                SetTickVisible(visible);
                SetVisibleTick(ref pan.ConMenu, tag);
                SetTickVisible(true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vis"></param>
        private void SetTickVisible(bool vis)
        {
            _tickVisible = vis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctxMenu"></param>
        /// <param name="graphTag"></param>
        private void SetVisibleTick(ref ContextMenu ctxMenu, int graphTag)
        {
            for (var i = 0; i < ctxMenu.MenuItems.Count; i++)
                SetVisibleTick2(ctxMenu.MenuItems[i], graphTag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        /// <param name="graphTag"></param>
        private void SetVisibleTick2(MenuItem m, int graphTag)
        {
            var cnt = m.MenuItems.Count - 1;
            if (cnt < 0)
            {
                var mTag = (MenuTag) m.Tag;
                if (mTag.ItemTag != GraphOption.Visible) return;
                if (mTag.GraphTag == graphTag)
                    m.Checked = _tickVisible;
                return;
            }

            for (var i = 0; i < cnt; i++)
                SetVisibleTick2(m.MenuItems[i], graphTag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphTag"></param>
        /// <param name="panelTag"></param>
        /// <param name="itemTag"></param>
        /// <returns></returns>
        private static MenuTag CreateMenuTag(int graphTag, int panelTag, GraphOption itemTag)
        {
            var m = new MenuTag {GraphTag = graphTag, PanelTag = panelTag, ItemTag = itemTag};
            return m;
        }
    }
}