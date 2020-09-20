using DLS.UI;
using DLS.Utils;
using Rage;
using System.Drawing;

namespace DLS.Threads
{
    internal static class UIManager
    {
        internal static int sizeX = 550;
        internal static int sizeY = 220;

        internal static int offsetX = 1920;
        internal static int offsetY = 1080;

        internal static bool IsUIOn = false;

        // Background (printed first)
        internal static Sprite Background;
        // Horn
        internal static Sprite Horn_On;
        internal static Sprite Horn_Off;
        // Manual
        internal static Sprite Manual_On;
        internal static Sprite Manual_Off;
        // Wail
        internal static Sprite Wail_On;
        internal static Sprite Wail_Off;
        // Yelp
        internal static Sprite Yelp_On;
        internal static Sprite Yelp_Off;
        // Hi-lo
        internal static Sprite Ext1_On;
        internal static Sprite Ext1_Off;
        // Phaser
        internal static Sprite Ext2_On;
        internal static Sprite Ext2_Off;
        // Hazards
        internal static Sprite Hazard_On;
        internal static Sprite Hazard_Off;
        // Left Indicator
        internal static Sprite Lind_On;
        internal static Sprite Lind_Off;
        // Right Indicator
        internal static Sprite Rind_On;
        internal static Sprite Rind_Off;
        // Stage 1
        internal static Sprite S1_On;
        internal static Sprite S1_Off;
        // Stage 2
        internal static Sprite S2_On;
        internal static Sprite S2_Off;
        // Stage 3
        internal static Sprite S3_On;
        internal static Sprite S3_Off;
        // TA Left
        internal static Sprite Taleft_On;
        internal static Sprite Taleft_Off;
        // TA Right
        internal static Sprite Taright_On;
        internal static Sprite Taright_Off;
        // TA Diverge
        internal static Sprite Tadiv_On;
        internal static Sprite Tadiv_Off;
        // TA Warn
        internal static Sprite Tawarn_On;
        internal static Sprite Tawarn_Off;
        // Interior Light
        internal static Sprite Intlt_On;
        internal static Sprite Intlt_Off;
        // Steady Burn
        internal static Sprite SB_On;
        internal static Sprite SB_Off;
        // Steady Burn
        internal static Sprite Blkt_On;
        internal static Sprite Blkt_Off;
        // Traffic Advisories
        internal static Sprite _3_l_On;
        internal static Sprite _3_l_Off;
        internal static Sprite _3_c_On;
        internal static Sprite _3_c_Off;
        internal static Sprite _3_r_On;
        internal static Sprite _3_r_Off = new Sprite(Properties.Resources._3_r_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));

        internal static Sprite _4_l_On = new Sprite(Properties.Resources._4_l_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _4_l_Off = new Sprite(Properties.Resources._4_l_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _4_cl_On = new Sprite(Properties.Resources._4_cl_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _4_cl_Off = new Sprite(Properties.Resources._4_cl_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _4_cr_On = new Sprite(Properties.Resources._4_cr_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _4_cr_Off = new Sprite(Properties.Resources._4_cr_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _4_r_On = new Sprite(Properties.Resources._4_r_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _4_r_Off = new Sprite(Properties.Resources._4_r_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));

        internal static Sprite _5_l_On = new Sprite(Properties.Resources._5_l_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _5_l_Off = new Sprite(Properties.Resources._5_l_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _5_cl_On = new Sprite(Properties.Resources._5_cl_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _5_cl_Off = new Sprite(Properties.Resources._5_cl_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _5_c_On = new Sprite(Properties.Resources._5_c_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _5_c_Off = new Sprite(Properties.Resources._5_c_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _5_cr_On = new Sprite(Properties.Resources._5_cr_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _5_cr_Off = new Sprite(Properties.Resources._5_cr_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _5_r_On = new Sprite(Properties.Resources._5_r_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _5_r_Off = new Sprite(Properties.Resources._5_r_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));

        internal static Sprite _6_l_On = new Sprite(Properties.Resources._6_l_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_l_Off = new Sprite(Properties.Resources._6_l_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_el_On = new Sprite(Properties.Resources._6_el_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_el_Off = new Sprite(Properties.Resources._6_el_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_cl_On = new Sprite(Properties.Resources._6_cl_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_cl_Off = new Sprite(Properties.Resources._6_cl_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_cr_On = new Sprite(Properties.Resources._6_cr_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_cr_Off = new Sprite(Properties.Resources._6_cr_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_er_On = new Sprite(Properties.Resources._6_er_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_er_Off = new Sprite(Properties.Resources._6_er_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_r_On = new Sprite(Properties.Resources._6_r_on, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));
        internal static Sprite _6_r_Off = new Sprite(Properties.Resources._6_r_off, new Point(1920 - sizeX, 1080 - sizeY), new Size(sizeX, sizeY));

        internal static void Process()
        {
            sizeX = Settings.UI_WIDTH;
            sizeY = Settings.UI_HEIGHT;
            offsetX = Settings.UI_OFFSETX;
            offsetY = Settings.UI_OFFSETY;

            Background = new Sprite(Properties.Resources.background, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Horn_On = new Sprite(Properties.Resources.horn_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Horn_Off = new Sprite(Properties.Resources.horn_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Horn_On = new Sprite(Properties.Resources.horn_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Manual_On = new Sprite(Properties.Resources.manual_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Manual_Off = new Sprite(Properties.Resources.manual_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Wail_On = new Sprite(Properties.Resources.wail_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Wail_Off = new Sprite(Properties.Resources.wail_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Yelp_On = new Sprite(Properties.Resources.yelp_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Yelp_Off = new Sprite(Properties.Resources.yelp_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Ext1_On = new Sprite(Properties.Resources.ext1_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Ext1_Off = new Sprite(Properties.Resources.ext1_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Ext2_On = new Sprite(Properties.Resources.ext2_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Ext2_Off = new Sprite(Properties.Resources.ext2_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Hazard_On = new Sprite(Properties.Resources.hazard_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Hazard_Off = new Sprite(Properties.Resources.hazard_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Lind_On = new Sprite(Properties.Resources.lind_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Lind_Off = new Sprite(Properties.Resources.lind_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Rind_On = new Sprite(Properties.Resources.rind_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Rind_Off = new Sprite(Properties.Resources.rind_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            S1_On = new Sprite(Properties.Resources.s1_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            S1_Off = new Sprite(Properties.Resources.s1_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            S2_On = new Sprite(Properties.Resources.s2_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            S2_Off = new Sprite(Properties.Resources.s2_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            S3_On = new Sprite(Properties.Resources.s3_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            S3_Off = new Sprite(Properties.Resources.s3_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Taleft_On = new Sprite(Properties.Resources.taleft_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Taleft_Off = new Sprite(Properties.Resources.taleft_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Taright_On = new Sprite(Properties.Resources.taright_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Taright_Off = new Sprite(Properties.Resources.taright_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Tadiv_On = new Sprite(Properties.Resources.tadiv_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Tadiv_Off = new Sprite(Properties.Resources.tadiv_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Tawarn_On = new Sprite(Properties.Resources.tawarn_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Tawarn_Off = new Sprite(Properties.Resources.tawarn_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Intlt_On = new Sprite(Properties.Resources.intlt_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Intlt_Off = new Sprite(Properties.Resources.intlt_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            SB_On = new Sprite(Properties.Resources.sb_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            SB_Off = new Sprite(Properties.Resources.sb_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Blkt_On = new Sprite(Properties.Resources.blkt_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            Blkt_Off = new Sprite(Properties.Resources.blkt_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _3_l_On = new Sprite(Properties.Resources._3_l_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _3_l_Off = new Sprite(Properties.Resources._3_l_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _3_c_On = new Sprite(Properties.Resources._3_c_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _3_c_Off = new Sprite(Properties.Resources._3_c_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _3_r_On = new Sprite(Properties.Resources._3_r_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _3_r_Off = new Sprite(Properties.Resources._3_r_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _4_l_On = new Sprite(Properties.Resources._4_l_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _4_l_Off = new Sprite(Properties.Resources._4_l_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _4_cl_On = new Sprite(Properties.Resources._4_cl_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _4_cl_Off = new Sprite(Properties.Resources._4_cl_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _4_cr_On = new Sprite(Properties.Resources._4_cr_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _4_cr_Off = new Sprite(Properties.Resources._4_cr_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _4_r_On = new Sprite(Properties.Resources._4_r_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _4_r_Off = new Sprite(Properties.Resources._4_r_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _5_l_On = new Sprite(Properties.Resources._5_l_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _5_l_Off = new Sprite(Properties.Resources._5_l_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _5_cl_On = new Sprite(Properties.Resources._5_cl_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _5_cl_Off = new Sprite(Properties.Resources._5_cl_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _5_c_On = new Sprite(Properties.Resources._5_c_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _5_c_Off = new Sprite(Properties.Resources._5_c_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _5_cr_On = new Sprite(Properties.Resources._5_cr_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _5_cr_Off = new Sprite(Properties.Resources._5_cr_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _5_r_On = new Sprite(Properties.Resources._5_r_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _5_r_Off = new Sprite(Properties.Resources._5_r_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_l_On = new Sprite(Properties.Resources._6_l_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_l_Off = new Sprite(Properties.Resources._6_l_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_el_On = new Sprite(Properties.Resources._6_el_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_el_Off = new Sprite(Properties.Resources._6_el_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_cl_On = new Sprite(Properties.Resources._6_cl_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_cl_Off = new Sprite(Properties.Resources._6_cl_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_cr_On = new Sprite(Properties.Resources._6_cr_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_cr_Off = new Sprite(Properties.Resources._6_cr_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_er_On = new Sprite(Properties.Resources._6_er_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_er_Off = new Sprite(Properties.Resources._6_er_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_r_On = new Sprite(Properties.Resources._6_r_on, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));
            _6_r_Off = new Sprite(Properties.Resources._6_r_off, new Point(offsetX - sizeX, offsetY - sizeY), new Size(sizeX, sizeY));

            Importer.GetCustomSprites();
            _ = UIHelper.player;
            Game.RawFrameRender += RawFrameRender;
        }

        static void RawFrameRender(object sender, GraphicsEventArgs e)
        {
            int w = Game.Resolution.Width;
            int h = Game.Resolution.Height;
            string text = "Warning - DLS Key Lock Active";
            float size = 16.0f;
            SizeF graphicSize = Rage.Graphics.MeasureText(text, "Arial Bold", size);
            if (!Game.IsPaused && Entrypoint.keysLocked)
            {
                if (IsUIOn)
                    e.Graphics.DrawText(text, "Arial Bold", size, new PointF(w - graphicSize.Width - 8, h + graphicSize.Height - sizeY), Color.DarkRed);
                else
                    e.Graphics.DrawText(text, "Arial Bold", size, new PointF(w - graphicSize.Width - 8, h - graphicSize.Height - 8), Color.DarkRed);
            }
            Ped player = UIHelper.player;
            if (IsUIOn && UIHelper.IsUIAbleToDisplay && UIHelper.IsInAnyVehicle)
            {
                Vehicle veh = UIHelper.currentVehicle;
                if (UIHelper.IsUIAbleToDisplay && UIHelper.IsVehicleValid && UIHelper.IsPlayerDriver
                    && (UIHelper.dlsModel != null || Entrypoint.SCforNDLS)
                    && UIHelper.activeVeh != null)
                {
                    ActiveVehicle aVeh = UIHelper.activeVeh;
                    Background.Draw(e.Graphics);
                    if (Entrypoint.IndEnabled)
                    {
                        switch (aVeh.IndStatus)
                        {
                            case IndStatus.Off:
                                Hazard_Off.Draw(e.Graphics);
                                Lind_Off.Draw(e.Graphics);
                                Rind_Off.Draw(e.Graphics);
                                break;
                            case IndStatus.Both:
                                Hazard_On.Draw(e.Graphics);
                                Lind_Off.Draw(e.Graphics);
                                Rind_Off.Draw(e.Graphics);
                                break;
                            case IndStatus.Left:
                                Hazard_Off.Draw(e.Graphics);
                                Lind_On.Draw(e.Graphics);
                                Rind_Off.Draw(e.Graphics);
                                break;
                            case IndStatus.Right:
                                Hazard_Off.Draw(e.Graphics);
                                Lind_Off.Draw(e.Graphics);
                                Rind_On.Draw(e.Graphics);
                                break;
                            default:
                                Hazard_Off.Draw(e.Graphics);
                                Lind_Off.Draw(e.Graphics);
                                Rind_Off.Draw(e.Graphics);
                                break;
                        }
                    }
                    else
                    {
                        Hazard_Off.Draw(e.Graphics);
                        Lind_Off.Draw(e.Graphics);
                        Rind_Off.Draw(e.Graphics);
                    }
                    switch (aVeh.SirenStage)
                    {
                        case SirenStage.Off:
                            Wail_Off.Draw(e.Graphics);
                            Yelp_Off.Draw(e.Graphics);
                            Ext1_Off.Draw(e.Graphics);
                            Ext2_Off.Draw(e.Graphics);
                            break;
                        case SirenStage.One:
                            Wail_On.Draw(e.Graphics);
                            Yelp_Off.Draw(e.Graphics);
                            Ext1_Off.Draw(e.Graphics);
                            Ext2_Off.Draw(e.Graphics);
                            break;
                        case SirenStage.Two:
                            Wail_Off.Draw(e.Graphics);
                            Yelp_On.Draw(e.Graphics);
                            Ext1_Off.Draw(e.Graphics);
                            Ext2_Off.Draw(e.Graphics);
                            break;
                        case SirenStage.Warning:
                            Wail_Off.Draw(e.Graphics);
                            Yelp_Off.Draw(e.Graphics);
                            Ext1_On.Draw(e.Graphics);
                            Ext2_Off.Draw(e.Graphics);
                            break;
                        case SirenStage.Warning2:
                            Wail_Off.Draw(e.Graphics);
                            Yelp_Off.Draw(e.Graphics);
                            Ext1_Off.Draw(e.Graphics);
                            Ext2_On.Draw(e.Graphics);
                            break;
                        case SirenStage.Horn:
                            Wail_Off.Draw(e.Graphics);
                            Yelp_Off.Draw(e.Graphics);
                            Ext1_Off.Draw(e.Graphics);
                            Ext2_Off.Draw(e.Graphics);
                            break;
                        default:
                            Wail_Off.Draw(e.Graphics);
                            Yelp_Off.Draw(e.Graphics);
                            Ext1_Off.Draw(e.Graphics);
                            Ext2_Off.Draw(e.Graphics);
                            break;
                    }
                    switch (aVeh.LightStage)
                    {
                        case LightStage.Off:
                            S1_Off.Draw(e.Graphics);
                            S2_Off.Draw(e.Graphics);
                            S3_Off.Draw(e.Graphics);
                            break;
                        case LightStage.One:
                            S1_On.Draw(e.Graphics);
                            S2_Off.Draw(e.Graphics);
                            S3_Off.Draw(e.Graphics);
                            break;
                        case LightStage.Two:
                            S1_Off.Draw(e.Graphics);
                            S2_On.Draw(e.Graphics);
                            S3_Off.Draw(e.Graphics);
                            break;
                        case LightStage.Three:
                            S1_Off.Draw(e.Graphics);
                            S2_Off.Draw(e.Graphics);
                            S3_On.Draw(e.Graphics);
                            break;
                        default:
                            S1_Off.Draw(e.Graphics);
                            S2_Off.Draw(e.Graphics);
                            S3_Off.Draw(e.Graphics);
                            break;
                    }
                    switch (aVeh.TAStage)
                    {
                        case TAStage.Off:
                            Taleft_Off.Draw(e.Graphics);
                            Tadiv_Off.Draw(e.Graphics);
                            Taright_Off.Draw(e.Graphics);
                            Tawarn_Off.Draw(e.Graphics);
                            break;
                        case TAStage.Left:
                            Taleft_On.Draw(e.Graphics);
                            Tadiv_Off.Draw(e.Graphics);
                            Taright_Off.Draw(e.Graphics);
                            Tawarn_Off.Draw(e.Graphics);
                            break;
                        case TAStage.Diverge:
                            Taleft_Off.Draw(e.Graphics);
                            Tadiv_On.Draw(e.Graphics);
                            Taright_Off.Draw(e.Graphics);
                            Tawarn_Off.Draw(e.Graphics);
                            break;
                        case TAStage.Right:
                            Taleft_Off.Draw(e.Graphics);
                            Tadiv_Off.Draw(e.Graphics);
                            Taright_On.Draw(e.Graphics);
                            Tawarn_Off.Draw(e.Graphics);
                            break;
                        case TAStage.Warn:
                            Taleft_Off.Draw(e.Graphics);
                            Tadiv_Off.Draw(e.Graphics);
                            Taright_Off.Draw(e.Graphics);
                            Tawarn_On.Draw(e.Graphics);
                            break;
                        default:
                            Taleft_Off.Draw(e.Graphics);
                            Tadiv_Off.Draw(e.Graphics);
                            Taright_Off.Draw(e.Graphics);
                            Tawarn_Off.Draw(e.Graphics);
                            break;
                    }
                    if (PlayerController.hornButtonDown)
                        Horn_On.Draw(e.Graphics);
                    else
                        Horn_Off.Draw(e.Graphics);
                    if (aVeh.SBOn)
                        SB_On.Draw(e.Graphics);
                    else
                        SB_Off.Draw(e.Graphics);
                    if (aVeh.IntLightOn)
                        Intlt_On.Draw(e.Graphics);
                    else
                        Intlt_Off.Draw(e.Graphics);
                    if (PlayerController.manButtonDown)
                        Manual_On.Draw(e.Graphics);
                    else
                        Manual_Off.Draw(e.Graphics);
                    if (PlayerController.blktOn)
                        Blkt_On.Draw(e.Graphics);
                    else
                        Blkt_Off.Draw(e.Graphics);
                    if (UIHelper.dlsModel != null)
                    {
                        DLSModel dlsModel = UIHelper.dlsModel;
                        switch (aVeh.TAType)
                        {
                            case "three":
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.l.ToInt32() - 1] == SirenStatus.On)
                                    _3_l_On.Draw(e.Graphics);
                                else
                                    _3_l_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.c.ToInt32() - 1] == SirenStatus.On)
                                    _3_c_On.Draw(e.Graphics);
                                else
                                    _3_c_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.r.ToInt32() - 1] == SirenStatus.On)
                                    _3_r_On.Draw(e.Graphics);
                                else
                                    _3_r_Off.Draw(e.Graphics);
                                break;
                            case "four":
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.l.ToInt32() - 1] == SirenStatus.On)
                                    _4_l_On.Draw(e.Graphics);
                                else
                                    _4_l_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.cl.ToInt32() - 1] == SirenStatus.On)
                                    _4_cl_On.Draw(e.Graphics);
                                else
                                    _4_cl_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.cr.ToInt32() - 1] == SirenStatus.On)
                                    _4_cr_On.Draw(e.Graphics);
                                else
                                    _4_cr_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.r.ToInt32() - 1] == SirenStatus.On)
                                    _4_r_On.Draw(e.Graphics);
                                else
                                    _4_r_Off.Draw(e.Graphics);
                                break;
                            case "five":
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.l.ToInt32() - 1] == SirenStatus.On)
                                    _5_l_On.Draw(e.Graphics);
                                else
                                    _5_l_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.cl.ToInt32() - 1] == SirenStatus.On)
                                    _5_cl_On.Draw(e.Graphics);
                                else
                                    _5_cl_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.c.ToInt32() - 1] == SirenStatus.On)
                                    _5_c_On.Draw(e.Graphics);
                                else
                                    _5_c_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.cr.ToInt32() - 1] == SirenStatus.On)
                                    _5_cr_On.Draw(e.Graphics);
                                else
                                    _5_cr_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.r.ToInt32() - 1] == SirenStatus.On)
                                    _5_r_On.Draw(e.Graphics);
                                else
                                    _5_r_Off.Draw(e.Graphics);
                                break;
                            case "six":
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.l.ToInt32() - 1] == SirenStatus.On)
                                    _6_l_On.Draw(e.Graphics);
                                else
                                    _6_l_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.el.ToInt32() - 1] == SirenStatus.On)
                                    _6_el_On.Draw(e.Graphics);
                                else
                                    _6_el_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.cl.ToInt32() - 1] == SirenStatus.On)
                                    _6_cl_On.Draw(e.Graphics);
                                else
                                    _6_cl_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.cr.ToInt32() - 1] == SirenStatus.On)
                                    _6_cr_On.Draw(e.Graphics);
                                else
                                    _6_cr_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.er.ToInt32() - 1] == SirenStatus.On)
                                    _6_er_On.Draw(e.Graphics);
                                else
                                    _6_er_Off.Draw(e.Graphics);
                                if (aVeh.TAStage != TAStage.Off && UIHelper.IsSirenOn[dlsModel.TrafficAdvisory.r.ToInt32() - 1] == SirenStatus.On)
                                    _6_r_On.Draw(e.Graphics);
                                else
                                    _6_r_Off.Draw(e.Graphics);
                                break;
                            default:
                                _6_l_Off.Draw(e.Graphics);
                                _6_el_Off.Draw(e.Graphics);
                                _6_cl_Off.Draw(e.Graphics);
                                _6_cr_Off.Draw(e.Graphics);
                                _6_er_Off.Draw(e.Graphics);
                                _6_r_Off.Draw(e.Graphics);
                                break;
                        }
                    }
                    else
                    {
                        _6_l_Off.Draw(e.Graphics);
                        _6_el_Off.Draw(e.Graphics);
                        _6_cl_Off.Draw(e.Graphics);
                        _6_cr_Off.Draw(e.Graphics);
                        _6_er_Off.Draw(e.Graphics);
                        _6_r_Off.Draw(e.Graphics);
                    }
                }
            }
        }
    }
}