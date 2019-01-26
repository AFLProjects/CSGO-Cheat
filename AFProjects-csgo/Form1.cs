using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.DirectX;
using D3D = Microsoft.DirectX.Direct3D;
using System.Timers;
using System.Threading.Tasks;

namespace AFProjects_csgo
{
    public partial class Form1 : Form
    {
        int _bClient = 0;
        string _process = "csgo";
        bool _Modules = false;
        int bEngine = 0;

        int trigger_state = 0;
        Stopwatch stpWatch = new Stopwatch();
        int bhop_state = 0;
        Stopwatch bhopWatch = new Stopwatch();


        GlowPropt Enemy = new GlowPropt() //r=1 red, r=1 b=1 jaune, all 1 white
        {
            r = 1, //r
            g = 0, //b
            b = 1,
            a = 1,
            rwo = true,
            rwuo = true
            //rg violet
        };

        GlowPropt Team = new GlowPropt()
        {
            r = 0,
            g = 1,
            b = 1,
            a = 1,
            rwo = true,
            rwuo = true
        };

        GlowPropt Enemy_only = new GlowPropt() //r=1 red, r=1 b=1 jaune, all 1 white
        {
            r = 1, //r
            g = 1, //b
            b = 1,
            a = 1,
            rwo = true,
            rwuo = true
            //rg violet
        };

        public AFMemory afm = new AFMemory("csgo");

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int GetAsyncKeyState(int vKey);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form1()
        {
            InitializeComponent();
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            _Modules = _GetModuleClientDll() && _GetModuleEngineDll();
            if (_Modules)
            {
                Debug.WriteLine("bClient = " + _bClient);
            }
            else
            {
                Application.Exit();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        bool _GetModuleEngineDll()
        {
            try
            {
                Process[] p = Process.GetProcessesByName(_process);

                if (p.Length > 0)
                {
                    foreach (ProcessModule m in p[0].Modules)
                    {
                        if (m.ModuleName == "engine.dll")
                        {
                            bEngine = (int)m.BaseAddress;
                            return true;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public float[] GetPos(int player, AFMemory afm)
        {
            float[] head = new float[3];
            head[0] = afm.ReadFloat((IntPtr)(player + 0x134 + 0x0));
            head[1] = afm.ReadFloat((IntPtr)(player + 0x134 + 0x4));
            head[2] = afm.ReadFloat((IntPtr)(player + 0x134 + 0x8));
            return head;
        }

        public float[] GetMyANGLE(AFMemory afm)
        {
            float[] angle = new float[3];

            angle[0] = afm.ReadFloat((IntPtr)(afm.ReadInt32((IntPtr)bEngine + signatures.dwClientState) + 0x4D10 + 0x0));
            angle[1] = afm.ReadFloat((IntPtr)(afm.ReadInt32((IntPtr)bEngine + signatures.dwClientState) + 0x4D10 + 0x4));
            angle[2] = afm.ReadFloat((IntPtr)(afm.ReadInt32((IntPtr)bEngine + signatures.dwClientState) + 0x4D10 + 0x8));
            return angle;
        }

        public void WriteAngle(float x, float y, float z, AFMemory afm)
        {
            afm.WriteFloat((IntPtr)(afm.ReadInt32((IntPtr)bEngine + signatures.dwClientState) + 0x4D10 + 0x0), x);
            afm.WriteFloat((IntPtr)(afm.ReadInt32((IntPtr)bEngine + signatures.dwClientState) + 0x4D10 + 0x4), y);
            afm.WriteFloat((IntPtr)(afm.ReadInt32((IntPtr)bEngine + signatures.dwClientState) + 0x4D10 + 0x8), z);
        }


        public bool _GetModuleClientDll()
        {

            try
            {
                Process[] p = Process.GetProcessesByName(_process);

                if (p.Length > 0)
                {
                    foreach (ProcessModule m in p[0].Modules)
                    {

                        if (m.ModuleName == "client_panorama.dll")
                        {
                            Debug.WriteLine("panorama found");
                            _bClient = (int)m.BaseAddress;
                            return true;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            pictureBox3.Image = AFProjects_csgo.Properties.Resources.close2;
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Image = AFProjects_csgo.Properties.Resources.close1;
        }

        private void visuals_btn_MouseEnter(object sender, EventArgs e)
        {
            visuals_btn.Image = AFProjects_csgo.Properties.Resources.visuals;
        }

        private void visuals_btn_MouseLeave(object sender, EventArgs e)
        {
            visuals_btn.Image = AFProjects_csgo.Properties.Resources.visuals_black;
        }

        private void visuals_btn_MouseDown(object sender, MouseEventArgs e)
        {
            tabtext.Text = "Visuals";
            tabControl1.SelectedIndex = 0;
        }

        private void aimtrigger_MouseEnter(object sender, EventArgs e)
        {
            aimtrigger.Image = AFProjects_csgo.Properties.Resources.aimtrigger2;
        }

        private void aimtrigger_MouseLeave(object sender, EventArgs e)
        {
            aimtrigger.Image = AFProjects_csgo.Properties.Resources.aimtrigger1;
        }

        private void aimtrigger_MouseDown(object sender, MouseEventArgs e)
        {
            tabtext.Text = "Aimbot & Triggerbot";
            tabControl1.SelectedIndex = 1;
        }

        private void mouvement_MouseDown(object sender, MouseEventArgs e)
        {
            tabtext.Text = "Mouvement";
            tabControl1.SelectedIndex = 2;
        }

        private void mouvement_MouseEnter(object sender, EventArgs e)
        {
            mouvement.Image = AFProjects_csgo.Properties.Resources.run2;
        }

        private void mouvement_MouseLeave(object sender, EventArgs e)
        {
            mouvement.Image = AFProjects_csgo.Properties.Resources.run;
        }

        private void misceleanous_MouseDown(object sender, MouseEventArgs e)
        {
            tabtext.Text = "Miscellaneous";
            tabControl1.SelectedIndex = 3;
        }

        private void misceleanous_MouseEnter(object sender, EventArgs e)
        {
            misceleanous.Image = AFProjects_csgo.Properties.Resources.miscellaneous2;
        }

        private void misceleanous_MouseLeave(object sender, EventArgs e)
        {
            misceleanous.Image = AFProjects_csgo.Properties.Resources.miscellaneous;
        }

        private void settings_MouseDown(object sender, MouseEventArgs e)
        {
            tabtext.Text = "Settings";
            tabControl1.SelectedIndex = 4;
        }

        private void settings_MouseEnter(object sender, EventArgs e)
        {
            settings.Image = AFProjects_csgo.Properties.Resources.settings2;
        }

        private void settings_MouseLeave(object sender, EventArgs e)
        {
            settings.Image = AFProjects_csgo.Properties.Resources.settings;
        }

        private void bhop_Tick(object sender, EventArgs e)
        {
            if (bhop_enable.m_checked)
            {
                int address = _bClient + signatures.dwLocalPlayer;
                int LocalPlayer = afm.ReadInt32((IntPtr)address);

                address = LocalPlayer + netvars.m_fFlags;
                int Flags = afm.ReadInt32((IntPtr)address);

                address = _bClient + signatures.dwForceJump;
                int fjump = address;


                if (Flags == 257 && GetAsyncKeyState(0x20) < 0 && bhop_state == 0)
                {

                    afm.WriteInt32((IntPtr)fjump, 5);
                    bhopWatch.Start();
                    bhop_state = 1;

                }

                if (bhop_state == 1 && bhopWatch.ElapsedMilliseconds >= int.Parse(bhoptoggleump.Text))
                {
                    afm.WriteInt32((IntPtr)fjump, 4);
                    bhop_state = 0;
                    bhopWatch.Stop();
                    bhopWatch.Reset();
                    bhopWatch.Stop();
                }
            }



        }

        public struct GlowPropt
        {
            public float r;
            public float g;
            public float b;
            public float a;
            public bool rwo;
            public bool rwuo;
        }

        private void wh_Tick(object sender, EventArgs e)
        {


            if (wh_enable.m_checked)
            {

                
                int _address;
                int i = 1;
                int PlayerInCross;

                do
                {
                  
                    _address = _bClient + signatures.dwLocalPlayer;
                    int LocalPlayer = afm.ReadInt32((IntPtr)_address);

                    _address = LocalPlayer + netvars.m_iTeamNum;
                    int MyTeam = afm.ReadInt32((IntPtr)_address);

                    _address = _bClient + signatures.dwEntityList + (i - 1) * 0x10;
                    int EntityList = afm.ReadInt32((IntPtr)_address);

                    _address = EntityList + netvars.m_iTeamNum;
                    int HisTeam = afm.ReadInt32((IntPtr)_address);

                    _address = EntityList + netvars.m_iHealth;
                    float health = (float)afm.ReadInt32((IntPtr)_address);

                    // float[] newANGLE = { -90 , 116 ,0 };

                    //afm.WriteFloat((IntPtr)(afm.ReadInt32((IntPtr)bEngine + 0x57D894) + 0x4D10), newANGLE[1]);
                    //afm.WriteFloat((IntPtr)(afm.ReadInt32((IntPtr)bEngine + 0x57D894) + 0x4D10 + 0x4), newANGLE[0]);
                    //afm.WriteFloat((IntPtr)(afm.ReadInt32((IntPtr)bEngine + 0x57D894) + 0x4D10 + 0x8), newANGLE[2]);
                    /*
                    _address = EntityList + _oDormant;
                    bool dormant = afm.ReadBoolean((IntPtr)_address);
                    _address = EntityList + netvars.m_bSpotted;
                    bool spoted = afm.ReadBoolean((IntPtr)_address);
                    _address = EntityList + netvars.m_bSpottedByMask;
                    bool spotted = afm.ReadBoolean((IntPtr)_address);

                    */

                    int a = 0;



                    /*
                       int WeaponAdress = afm.ReadInt32((IntPtr)LocalPlayer + netvars.m_hMyWeapons);
                       afm.WriteInt32((IntPtr)WeaponAdress + m_nModelIndex, 388);
                       afm.WriteInt32((IntPtr)WeaponAdress + m_iWorldModelIndex, 389);
                       afm.WriteInt32((IntPtr)WeaponAdress + netvars.m_iItemDefinitionIndex, 507);

                   */
                    if (true)
                    {
                        _address = EntityList + netvars.m_iGlowIndex;

                        int GlowIndex = afm.ReadInt32((IntPtr)_address);
                        if (whallplayers.m_checked)
                        {
                            if (HisTeam == 3)

                            {
                                if (true)
                                {



                                    _address = _bClient + signatures.dwGlowObjectManager;
                                    int GlowObject = afm.ReadInt32((IntPtr)_address);

                                    int calculation = GlowIndex * 0x38 + 0x4;
                                    int current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Team.r);

                                    calculation = GlowIndex * 0x38 + 0x8;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Team.b);

                                    calculation = GlowIndex * 0x38 + 0xC;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Team.g);

                                    calculation = GlowIndex * 0x38 + 0x10;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Team.a);

                                    calculation = GlowIndex * 0x38 + 0x24;
                                    current = GlowObject + calculation;
                                    afm.WriteBoolean((IntPtr)current, Team.rwo);

                                    calculation = GlowIndex * 0x38 + 0x25;
                                    current = GlowObject + calculation;
                                    afm.WriteBoolean((IntPtr)current, Team.rwuo);
                                }


                            }
                            else if (HisTeam == 2)
                            {
                                if (true)
                                {
                                    _address = _bClient + signatures.dwGlowObjectManager;
                                    int GlowObject = afm.ReadInt32((IntPtr)_address);

                                    int calculation = GlowIndex * 0x38 + 0x4;
                                    int current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Enemy.r);

                                    calculation = GlowIndex * 0x38 + 0x8;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Enemy.b);

                                    calculation = GlowIndex * 0x38 + 0xC;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Enemy.g);

                                    calculation = GlowIndex * 0x38 + 0x10;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Enemy.a);

                                    calculation = GlowIndex * 0x38 + 0x24;
                                    current = GlowObject + calculation;
                                    afm.WriteBoolean((IntPtr)current, Enemy.rwo);

                                    calculation = GlowIndex * 0x38 + 0x25;
                                    current = GlowObject + calculation;
                                    afm.WriteBoolean((IntPtr)current, Enemy.rwuo);
                                }



                            }
                        }
                        else
                        {
                            if (whonlyennemies.m_checked && whennemiesspeacialcolor.m_checked)
                            {

                                if (HisTeam != MyTeam)
                                {


                                    _address = _bClient + signatures.dwGlowObjectManager;
                                    int GlowObject = afm.ReadInt32((IntPtr)_address);

                                    int calculation = GlowIndex * 0x38 + 0x4;
                                    int current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Enemy_only.r);

                                    calculation = GlowIndex * 0x38 + 0x8;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Enemy_only.b);

                                    calculation = GlowIndex * 0x38 + 0xC;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Enemy_only.g);

                                    calculation = GlowIndex * 0x38 + 0x10;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, Enemy_only.a);

                                    calculation = GlowIndex * 0x38 + 0x24;
                                    current = GlowObject + calculation;
                                    afm.WriteBoolean((IntPtr)current, Enemy_only.rwo);

                                    calculation = GlowIndex * 0x38 + 0x25;
                                    current = GlowObject + calculation;
                                    afm.WriteBoolean((IntPtr)current, Enemy_only.rwuo);
                                }
                            }
                            if (whonlyennemies.m_checked && whbyhealth.m_checked)
                            {

                                float b = health / 150;

                                GlowPropt health_glow = new GlowPropt() //r=1 red, r=1 b=1 jaune, all 1 white
                                {
                                    r = 1, //r
                                    g = 0, //b
                                    b = b,
                                    a = 1,
                                    rwo = true,
                                    rwuo = true
                                    //rg violet
                                };



                                if (HisTeam != MyTeam)
                                {
                                    _address = _bClient + signatures.dwGlowObjectManager;
                                    int GlowObject = afm.ReadInt32((IntPtr)_address);

                                    int calculation = GlowIndex * 0x38 + 0x4;
                                    int current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, health_glow.r);

                                    calculation = GlowIndex * 0x38 + 0x8;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, health_glow.b);

                                    calculation = GlowIndex * 0x38 + 0xC;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, health_glow.g);

                                    calculation = GlowIndex * 0x38 + 0x10;
                                    current = GlowObject + calculation;
                                    afm.WriteFloat((IntPtr)current, health_glow.a);

                                    calculation = GlowIndex * 0x38 + 0x24;
                                    current = GlowObject + calculation;
                                    afm.WriteBoolean((IntPtr)current, health_glow.rwo);

                                    calculation = GlowIndex * 0x38 + 0x25;
                                    current = GlowObject + calculation;
                                    afm.WriteBoolean((IntPtr)current, health_glow.rwuo);
                                }
                            }
                        }




                    }
                    i++;

                } while (i < 65);
            }
        }



        private void trigger_Tick(object sender, EventArgs e)
        {
            if (triggerbot_enable.m_checked)
            {
                int address = _bClient + signatures.dwLocalPlayer;
                int LocalPlayer = afm.ReadInt32((IntPtr)address);

                address = LocalPlayer + netvars.m_iCrosshairId;
                int crosshairId = afm.ReadInt32((IntPtr)address);


                if (crosshairId > 0 && crosshairId <= 64)
                {
                    int Enemy_ = afm.ReadInt32((IntPtr)(_bClient + signatures.dwEntityList + ((crosshairId - 1) * 0x10)));
                    int playerTeam = afm.ReadInt32((IntPtr)(LocalPlayer + netvars.m_iTeamNum));
                    int enemyTeam = afm.ReadInt32((IntPtr)(Enemy_ + netvars.m_iTeamNum));

                    if (enemyTeam != playerTeam && trigger_state == 0)
                    {
                        stpWatch.Start();
                        trigger_state = 1;
                    }

                    if (enemyTeam != playerTeam && trigger_state == 1)
                    {

                        if (stpWatch.ElapsedMilliseconds >= int.Parse(triggerbot_timbeforeshoot.Text))
                        {
                            Random rnd = new Random();
                            int num = rnd.Next(0, int.Parse(trigger_bot_shoot_probability.Text));
                            if (num < int.Parse(trigger_bot_shoot_probability.Text))
                            {
                                c.ClickLeftMouseButtonSendInput();
                            }
                            stpWatch.Stop();
                            stpWatch.Reset();
                            trigger_state = 0;
                        }
                        Debug.WriteLine("elapsed time : " + stpWatch.ElapsedMilliseconds);
                        Debug.WriteLine("required : " + int.Parse(triggerbot_timbeforeshoot.Text));
                    }
                }
                else
                {
                    trigger_state = 0;
                }
            }
        }

        private void thirdPerson_Tick(object sender, EventArgs e)
        {
            int address = _bClient + signatures.dwLocalPlayer;
            int LocalPlayer = afm.ReadInt32((IntPtr)address);

            bool tperson = tpview.m_checked;

            if (tperson)
            {
                afm.WriteInt32((IntPtr)(LocalPlayer + netvars.m_iObserverMode), 1);
            }
            else
            {
                afm.WriteInt32((IntPtr)(LocalPlayer + netvars.m_iObserverMode), 0);
            }
        }

        private void UI_Tick(object sender, EventArgs e)
        {
            if (whitetheme.m_checked)
            {
                this.BackColor = Color.FromKnownColor(KnownColor.Control);
                this.ForeColor = Color.FromArgb(255, 60, 60, 60); ;
            }
            if (blacktheme.m_checked)
            {
                this.BackColor = Color.FromArgb(255, 40, 40, 40);
                // this.ForeColor = Color.FromArgb(255, 255, 255, 255);
                foreach (Control cnt in this.Controls)
                {
                    cnt.ForeColor = Color.White;
                }
            }
        }

        public double D3Distance(float[] p1, float[] p2)
        {
            return Math.Sqrt((double)(Math.Pow(p2[0] - p1[0], 2) + Math.Pow(p2[1] - p1[1], 2) + Math.Pow(p2[2] - p1[2], 2)));
        }

        private void aimbot_Tick(object sender, EventArgs e)
        {

            int address = _bClient + signatures.dwLocalPlayer;
            int LocalPlayer = afm.ReadInt32((IntPtr)address);


            if (aimbot_enable.m_checked)
            {
                List<double> dist = new List<double>();
                List<float[]> poses = new List<float[]>();
                for (int i = 0; i < 16; i++)
                {
                    int _address = _bClient + signatures.dwEntityList + (i - 1) * 0x10;
                    int EntityList = afm.ReadInt32((IntPtr)_address);
                    float[] pos = GetPos(EntityList, afm);
                    float[] playerPos = GetPos(LocalPlayer, afm);
                    _address = LocalPlayer + netvars.m_iTeamNum;
                    int MyTeam = afm.ReadInt32((IntPtr)_address);
                    _address = EntityList + netvars.m_iTeamNum;
                    int team = afm.ReadInt32((IntPtr)_address);
                    if (pos[0] + pos[1] + pos[2] != 0 && team != MyTeam)
                    {
                        if (D3Distance(new float[] { pos[2], pos[0], pos[1] }, new float[] { playerPos[2], playerPos[0], playerPos[1] }) != 0)
                        {
                            dist.Add(D3Distance(new float[] { pos[2], pos[0], pos[1] }, new float[] { playerPos[2], playerPos[0], playerPos[1] }));
                            poses.Add(new float[] { pos[2], pos[0], pos[1] });
                        }
                    }
                }
                float[] nearestPos = poses[dist.IndexOf(dist.Min())];
                float[] incorrectPos = GetPos(LocalPlayer, afm);
                float[] myPos = new float[] { incorrectPos[2], incorrectPos[0], incorrectPos[1] };
                label32.Text = "nearest pos to you : " + nearestPos[0] + ", " + nearestPos[1] + ", " + nearestPos[2] + " => distance : " + dist.Min();
                label33.Text = "MyPos : " + myPos[0] + ", " + myPos[1] + ", " + myPos[2];
                Console.WriteLine("nearest pos to you : " + nearestPos[0] + ", " + nearestPos[1] + ", " + nearestPos[2] + " => distance : " + dist.Min());
                Console.WriteLine("MyPos : " + myPos[0] + ", " + myPos[1] + ", " + myPos[2]);
                /*
                float[] myPos2D = new float[] { myPos[2],myPos[0] };
                float[] hisPos2D = new float[] { nearestPos[2], nearestPos[0] };
                Console.WriteLine("MyPos 2D : " + myPos2D[0] + ", " + myPos2D[1]);
                Console.WriteLine("HisPos 2D : " + hisPos2D[0] + ", " + hisPos2D[1]);
                float[] playerViewAngle = GetMyANGLE(afm);
                playerViewAngle = new float[] { playerViewAngle[1],playerViewAngle[0]};

                double k = Math.Sqrt(Math.Pow(hisPos2D[0] - myPos2D[0], 2) + Math.Pow(hisPos2D[1] - myPos2D[1], 2));
                Console.WriteLine("k(distance 2d player=>enemy) :" + k);
                double nxpos = (Math.Sin(playerViewAngle[0]) * k) + myPos2D[0];
                double nypos = (Math.Cos(playerViewAngle[0]) * k) + myPos2D[1];
                Console.WriteLine("n pos 2d x:" + nxpos + ",y:" + nypos);
                double d = Math.Sqrt(Math.Pow(hisPos2D[0] - nxpos, 2) + Math.Pow(hisPos2D[1] - nypos, 2));
                Console.WriteLine("d(distance 2d n=>enemy) :" + d);
                double angle = 0;
                double haut = Math.Pow(k, 2) + Math.Pow(k, 2) - Math.Pow(d, 2);
                Console.WriteLine("haut :" + haut);
                double bas = 2 * k * k;
                Console.WriteLine("bas :" + bas);
                angle = Math.Acos(haut / bas);
                angle = angle * (180 / Math.PI);
                Console.WriteLine("angle :" + angle);
                WriteAngle(playerViewAngle[0], (float)angle, 0,afm);
                */
            }
        }

        private void fov_Tick(object sender, EventArgs e)
        {
           
            int address = _bClient + signatures.dwLocalPlayer;
            int LocalPlayer = afm.ReadInt32((IntPtr)address);



            if (no_flash.m_checked)
            {
                address = LocalPlayer + netvars.m_flFlashDuration;
                afm.WriteInt32((IntPtr)address, 0);
            }

        }

        private void set_ennemies_color_Click(object sender, EventArgs e)
        {
            colchoose.Visible = true;
            cola.Text = Enemy_only.a.ToString();
            colr.Text = Enemy_only.r.ToString();
            colb.Text = Enemy_only.b.ToString();
            colg.Text = Enemy_only.g.ToString();
            colorChangewhat.Text = "enemies color";
        }

        private void set_t_color_Click(object sender, EventArgs e)
        {
            colchoose.Visible = true;
            cola.Text = Enemy.a.ToString();
            colr.Text = Enemy.r.ToString();
            colb.Text = Enemy.b.ToString();
            colg.Text = Enemy.g.ToString();
            colorChangewhat.Text = "t color";
        }

        private void set_ct_color_Click(object sender, EventArgs e)
        {
            colchoose.Visible = true;
            cola.Text = Team.a.ToString();
            colr.Text = Team.r.ToString();
            colb.Text = Team.b.ToString();
            colg.Text = Team.g.ToString();
            colorChangewhat.Text = "ct color";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (colorChangewhat.Text == "enemies color")
            {
                Enemy_only.a = float.Parse(cola.Text);
                Enemy_only.b = float.Parse(colb.Text);
                Enemy_only.g = float.Parse(colg.Text);
                Enemy_only.r = float.Parse(colr.Text);
                colchoose.Visible = false;
            }
            if (colorChangewhat.Text == "t color")
            {
                Enemy.a = float.Parse(cola.Text);
                Enemy.b = float.Parse(colb.Text);
                Enemy.g = float.Parse(colg.Text);
                Enemy.r = float.Parse(colr.Text);
                colchoose.Visible = false;
            }
            if (colorChangewhat.Text == "ct color")
            {
                Team.a = float.Parse(cola.Text);
                Team.b = float.Parse(colb.Text);
                Team.g = float.Parse(colg.Text);
                Team.r = float.Parse(colr.Text);
                colchoose.Visible = false;
            }
        }

        private void killSoundCheck_Tick(object sender, EventArgs e)
        {
            /*
            int address = _bClient + signatures.dwLocalPlayer;
            int LocalPlayer = afm.ReadInt32((IntPtr)address);

            address = LocalPlayer + signatures.dwPlayerResource;
            int resourcePlayer = afm.ReadInt32((IntPtr)address);

            address = LocalPlayer + netvars.m_iCompetitiveWins;
            int competitiveWins = afm.ReadInt32((IntPtr)address);

            Console.WriteLine("cmpWins :" + competitiveWins);*/
        }

        private void scoped_Tick(object sender, EventArgs e)
        {
            SetMyKnife(515, 662);

            int address = _bClient + signatures.dwLocalPlayer;
            int LocalPlayer = afm.ReadInt32((IntPtr)address);

            address = LocalPlayer + netvars.m_bIsScoped;
            int isScoped = afm.ReadInt32((IntPtr)address);

            if (isScoped == 0)
            {
                address = LocalPlayer + netvars.m_iFOV;
                afm.WriteInt32((IntPtr)address, (int)fovnum.Value);
            }
        }

        private void skinandknife_MouseEnter(object sender, EventArgs e)
        {
            skinandknife.Image = AFProjects_csgo.Properties.Resources.knife2;
        }

        private void skinandknife_MouseLeave(object sender, EventArgs e)
        {
            skinandknife.Image = AFProjects_csgo.Properties.Resources.knife;
        }

        private void skinandknife_MouseDown(object sender, MouseEventArgs e)
        {
            tabtext.Text = "Skins & Knifes";
            tabControl1.SelectedIndex = 5;
        }

        public class weapons_ids
        {
            public const int usp = 4095;//61;//262205;//new Weapon(262205,504);
            public const int m4a4 = 16;//new Weapon(16, 309);
            public const int deagle = 1; //new Weapon(1,711);
            public const int glock = 4;//new Weapon(4,12);
            public const int ak47 = 7;//new Weapon(7,675);
            public const int awp = 9;
            public const int dbaretas = 2;
            public const int p250 = 36;
            public const int fiveSeven = 3;//262147; //3
            public const int nova = 35;
            public const int xm = 25;
            public const int mag7 = 27;
            public const int mp9 = 34;//262178; //34
            public const int mp7 = 33;// 262177; //33
        }

        public class weapons_settings_paintkits
        {
            public const int usp = 504;//new Weapon(262205,504);
            public const int m4a4 = 309;//new Weapon(16, 309);
            public const int deagle = 711; //new Weapon(1,711);
            public const int glock = 38;//new Weapon(4,12);
            public const int ak47 = 302;//new Weapon(7,675);
            public const int awp = 279;
            public const int dbaretas = 658;
            public const int p250 = 678;
            public const int fiveSeven = 660;
            public const int nova = 62;
            public const int xm = 393;
            public const int mag7 = 703;
            public const int mp9 = 262;
            public const int mp7 = 481;
        }

        public class knife_indexes
        {
            public const int knife_t = 59;
            public const int knife_ct = 42;
            public const int bayonet = 500;
            public const int flip = 505;
            public const int gut = 506;
            public const int karam = 507;
            public const int m9 = 508;
            public const int hunt = 509;
            public const int falshion = 512;
            public const int bowie = 514;
            public const int butterfly = 515;
            public const int daggers = 516;
            public const int ursus = 519;
            public const int navaja = 520;
            public const int stileto = 522;
            public const int talon = 523;
        }

        public void SetSkin(int weaponEnt, int paintKit, AFMemory afm)
        {
            Random _rand = new Random();
            afm.WriteInt32((IntPtr)(weaponEnt + netvars.m_iItemIDHigh), -1);
            afm.WriteInt32((IntPtr)(weaponEnt + netvars.m_OriginalOwnerXuidLow), 0);
            afm.WriteInt32((IntPtr)(weaponEnt + netvars.m_OriginalOwnerXuidHigh), 0);
            afm.WriteInt32((IntPtr)(weaponEnt + netvars.m_nFallbackPaintKit), paintKit);
            afm.WriteInt32((IntPtr)(weaponEnt + netvars.m_nFallbackSeed), _rand.Next(1, int.MaxValue - 1));
            afm.WriteFloat((IntPtr)(weaponEnt + netvars.m_flFallbackWear), 0.001f);
        }


        public void SetMyKnife(int id, int model)
        {
            int address = _bClient + signatures.dwLocalPlayer;
            int LocalPlayer = afm.ReadInt32((IntPtr)address);

            int m_iViewModelIndex = 0x3220;
            int m_iWorldDroppedModelIndex = 0x3228;
            int m_iWorldModelIndex = 0x3224;
            int m_nModelIndex = 0x258;
            int m_hViewModel = 0x32F8;

            if (skinsenable.m_checked)
            {
                
                int curWeaponIndex = afm.ReadInt32((IntPtr)(LocalPlayer + netvars.m_hActiveWeapon)) & 0xFFF;
                int curWeaponEntity = afm.ReadInt32((IntPtr)(_bClient + signatures.dwEntityList + (curWeaponIndex - 1) * 0x10));

              
                int curWeaponID = afm.ReadInt32((IntPtr)(curWeaponEntity + netvars.m_iItemDefinitionIndex));
                if (curWeaponID == 42 || curWeaponID == 59 || curWeaponID == id)
                {
                    afm.WriteShort((IntPtr)(curWeaponEntity + netvars.m_iItemDefinitionIndex), (short)id);
                    afm.WriteInt32((IntPtr)(curWeaponEntity + m_iViewModelIndex), model);
                    afm.WriteInt32((IntPtr)(curWeaponEntity + m_iWorldModelIndex), model + 1);
                    afm.WriteInt32((IntPtr)(curWeaponEntity + m_iWorldDroppedModelIndex), model + 2);
                    if (afm.ReadInt32((IntPtr)_bClient + signatures.dwEntityList + ((afm.ReadInt32((IntPtr)LocalPlayer + m_hViewModel) & 0xFFF) - 1) * 0x10) + m_nModelIndex != model)
                    {
                        afm.WriteInt32((IntPtr)afm.ReadInt32((IntPtr)_bClient + signatures.dwEntityList + ((afm.ReadInt32((IntPtr)LocalPlayer + m_hViewModel) & 0xFFF) - 1) * 0x10) + m_nModelIndex, model);
                    }
                    

                }


            }
        }

        private void knife_Tick(object sender, EventArgs e)
        {/*
            for (int i = 0; i < 75; i++)
            {
                SetMyKnife(509, 418);
            }
            GameMath.Matrix3x4 MAT = new GameMath.Matrix3x4();
            */

            int address = _bClient + signatures.dwLocalPlayer;
            int LocalPlayer = afm.ReadInt32((IntPtr)address);

            for (int x = 0; x < 50; x++)
            {
                Random _rand = new Random();
                for (int i = 1; i <= 7; i++)
                {
                    int curWeaponIndex = (int)afm.ReadShort((IntPtr)(LocalPlayer + netvars.m_hMyWeapons + ((i - 1) * 0x4))) & 0xfff;
                    Console.WriteLine("current weapon indes : " 
                        + curWeaponIndex);
                    int curWeaponEnt = afm.ReadInt32((IntPtr)(_bClient + signatures.dwEntityList + (curWeaponIndex - 1) * 0x10));
                    int curWeaponID = afm.ReadInt32((IntPtr)(curWeaponEnt + netvars.m_iItemDefinitionIndex));
                    if (curWeaponID == 0)
                        break;
                    int pKit = 0;
                    switch (curWeaponID)
                    {
                        case weapons_ids.deagle:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.deagle, afm);
                            pKit = weapons_settings_paintkits.deagle;
                            break;
                        case weapons_ids.ak47:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.ak47, afm);
                            pKit = weapons_settings_paintkits.ak47;
                            break;
                        case weapons_ids.glock:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.glock, afm);
                            pKit = weapons_settings_paintkits.glock;
                            break;
                        case weapons_ids.m4a4:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.m4a4, afm);
                            pKit = weapons_settings_paintkits.m4a4;
                            break;
                        case weapons_ids.usp:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.usp, afm);
                            pKit = weapons_settings_paintkits.usp;
                            break;
                        case weapons_ids.awp:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.awp, afm);
                            pKit = weapons_settings_paintkits.awp;
                            break;
                        case weapons_ids.dbaretas:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.dbaretas, afm);
                            pKit = weapons_settings_paintkits.dbaretas;
                            break;
                        case weapons_ids.p250:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.p250, afm);
                            pKit = weapons_settings_paintkits.p250;
                            break;
                        case weapons_ids.fiveSeven:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.fiveSeven, afm);
                            pKit = weapons_settings_paintkits.fiveSeven;
                            break;
                        case weapons_ids.nova:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.nova, afm);
                            pKit = weapons_settings_paintkits.nova;
                            break;
                        case weapons_ids.xm:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.xm, afm);
                            pKit = weapons_settings_paintkits.xm;
                            break;
                        case weapons_ids.mag7:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.mag7, afm);
                            pKit = weapons_settings_paintkits.mag7;
                            break;
                        case weapons_ids.mp9:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.mp9, afm);
                            pKit = weapons_settings_paintkits.mp9;
                            break;
                        case weapons_ids.mp7:
                            //SetSkin(curWeaponEnt, weapons_settings_paintkits.mp7, afm);
                            pKit = weapons_settings_paintkits.mp7;
                            break;
                    }
                    SetSkin(curWeaponEnt, pKit, afm);

                }

            }
}
}
}


public static class c
{
#region click method
#region Fields, DLL Imports and Constants

public static List<Point> Points { get; set; } //Hold the list of points in the queue
public static int Iterations { get; set; } //Hold the number of iterations/repeats
public static List<string> ClickType { get; set; } //Is each point right click or left click
public static List<int> Times { get; set; } //Holds sleep times for after each click

//Import unmanaged functions from DLL library
[DllImport("user32.dll")]
public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

[DllImport("user32.dll", SetLastError = true)]
public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbSize);

/// <summary>
/// Structure for SendInput function holding relevant mouse coordinates and information
/// </summary>
public struct INPUT
{
public uint type;
public MOUSEINPUT mi;

};

/// <summary>
/// Structure for SendInput function holding coordinates of the click and other information
/// </summary>
public struct MOUSEINPUT
{
public int dx;
public int dy;
public int mouseData;
public int dwFlags;
public int time;
public IntPtr dwExtraInfo;
};

//Constants for use in SendInput and mouse_event
public const int INPUT_MOUSE = 0x0000;
public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
public const int MOUSEEVENTF_LEFTUP = 0x0004;
public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
public const int MOUSEEVENTF_RIGHTUP = 0x0010;
public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
public const int MOUSEEVENTF_MIDDLEUP = 0x0040;

#endregion
public static void ClickLeftMouseButtonSendInput()
{
//Initialise INPUT object with corresponding values for a left click
INPUT input = new INPUT();
input.type = INPUT_MOUSE;
input.mi.dx = 0;
input.mi.dy = 0;
input.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
input.mi.dwExtraInfo = IntPtr.Zero;
input.mi.mouseData = 0;
input.mi.time = 0;

//Send a left click down followed by a left click up to simulate a 
//full left click
SendInput(1, ref input, Marshal.SizeOf(input));
input.mi.dwFlags = MOUSEEVENTF_LEFTUP;
SendInput(1, ref input, Marshal.SizeOf(input));

}
/*
public static void DubbleClickLeftMouseButtonSendInput()
{
//Initialise INPUT object with corresponding values for a left click
INPUT input = new INPUT();
input.type = INPUT_MOUSE;
input.mi.dx = 0;
input.mi.dy = 0;
input.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
input.mi.dwExtraInfo = IntPtr.Zero;
input.mi.mouseData = 0;
input.mi.time = 0;

//Send a left click down followed by a left click up to simulate a 
//full left click
SendInput(1, ref input, Marshal.SizeOf(input));
input.mi.dwFlags = MOUSEEVENTF_LEFTUP;
SendInput(1, ref input, Marshal.SizeOf(input));

Task.WaitAll(Task.Delay(100));


SendInput(1, ref input, Marshal.SizeOf(input));
input.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
SendInput(1, ref input, Marshal.SizeOf(input));

//Send a left click down followed by a left click up to simulate a 
//full left click
SendInput(1, ref input, Marshal.SizeOf(input));
input.mi.dwFlags = MOUSEEVENTF_LEFTUP;
SendInput(1, ref input, Marshal.SizeOf(input));
}
*/
                            /// <summary>
                            /// Click the left mouse button at the current cursor position using
                            /// the imported SendInput function
                            /// </summary>
                            public static void ClickRightMouseButtonSendInput()
    {
        //Initialise INPUT object with corresponding values for a right click
        INPUT input = new INPUT();
        input.type = INPUT_MOUSE;
        input.mi.dx = 0;
        input.mi.dy = 0;
        input.mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
        input.mi.dwExtraInfo = IntPtr.Zero;
        input.mi.mouseData = 0;
        input.mi.time = 0;

        //Send a right click down followed by a right click up to simulate a 
        //full right click
        SendInput(1, ref input, Marshal.SizeOf(input));
        input.mi.dwFlags = MOUSEEVENTF_RIGHTUP;
        SendInput(1, ref input, Marshal.SizeOf(input));
    }
    /*
    public static void DubbleClickRightMouseButtonSendInput()
    {
        //Initialise INPUT object with corresponding values for a right click
        INPUT input = new INPUT();
        input.type = INPUT_MOUSE;
        input.mi.dx = 0;
        input.mi.dy = 0;
        input.mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
        input.mi.dwExtraInfo = IntPtr.Zero;
        input.mi.mouseData = 0;
        input.mi.time = 0;

        //Send a right click down followed by a right click up to simulate a 
        //full right click
        SendInput(1, ref input, Marshal.SizeOf(input));
        input.mi.dwFlags = MOUSEEVENTF_RIGHTUP;
        SendInput(1, ref input, Marshal.SizeOf(input));

        Task.WaitAll(Task.Delay(100));

        SendInput(1, ref input, Marshal.SizeOf(input));
        input.mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
        SendInput(1, ref input, Marshal.SizeOf(input));

        SendInput(1, ref input, Marshal.SizeOf(input));
        input.mi.dwFlags = MOUSEEVENTF_RIGHTUP;
        SendInput(1, ref input, Marshal.SizeOf(input));
    }*/
    #endregion
}

public static class netvars
{
    public const Int32 cs_gamerules_data = 0x0;
    public const Int32 m_ArmorValue = 0xB328;
    public const Int32 m_Collision = 0x31C;
    public const Int32 m_CollisionGroup = 0x474;
    public const Int32 m_Local = 0x2FBC;
    public const Int32 m_MoveType = 0x25C;
    public const Int32 m_OriginalOwnerXuidHigh = 0x31B4;
    public const Int32 m_OriginalOwnerXuidLow = 0x31B0;
    public const Int32 m_SurvivalGameRuleDecisionTypes = 0x1318;
    public const Int32 m_SurvivalRules = 0xCF0;
    public const Int32 m_aimPunchAngle = 0x302C;
    public const Int32 m_aimPunchAngleVel = 0x3038;
    public const Int32 m_bBombPlanted = 0x99D;
    public const Int32 m_bFreezePeriod = 0x20;
    public const Int32 m_bGunGameImmunity = 0x3928;
    public const Int32 m_bHasDefuser = 0xB338;
    public const Int32 m_bHasHelmet = 0xB31C;
    public const Int32 m_bInReload = 0x3285;
    public const Int32 m_bIsDefusing = 0x3914;
    public const Int32 m_bIsQueuedMatchmaking = 0x74;
    public const Int32 m_bIsScoped = 0x390A;
    public const Int32 m_bIsValveDS = 0x75;
    public const Int32 m_bSpotted = 0x93D;
    public const Int32 m_bSpottedByMask = 0x980;
    public const Int32 m_clrRender = 0x70;
    public const Int32 m_dwBoneMatrix = 0x26A8;
    public const Int32 m_fAccuracyPenalty = 0x3304;
    public const Int32 m_fFlags = 0x104;
    public const Int32 m_flC4Blow = 0x2990;
    public const Int32 m_flDefuseCountDown = 0x29AC;
    public const Int32 m_flDefuseLength = 0x29A8;
    public const Int32 m_flFallbackWear = 0x31C0;
    public const Int32 m_flFlashDuration = 0xA3E0;
    public const Int32 m_flFlashMaxAlpha = 0xA3DC;
    public const Int32 m_flNextPrimaryAttack = 0x3218;
    public const Int32 m_flTimerLength = 0x2994;
    public const Int32 m_hActiveWeapon = 0x2EF8;
    public const Int32 m_hMyWeapons = 0x2DF8;
    public const Int32 m_hObserverTarget = 0x3388;
    public const Int32 m_hOwner = 0x29CC;
    public const Int32 m_hOwnerEntity = 0x14C;
    public const Int32 m_iAccountID = 0x2FC8;
    public const Int32 m_iClip1 = 0x3244;
    public const Int32 m_iCompetitiveRanking = 0x1A84;
    public const Int32 m_iCompetitiveWins = 0x1B88;
    public const Int32 m_iCrosshairId = 0xB394;
    public const Int32 m_iEntityQuality = 0x2FAC;
    public const Int32 m_iFOV = 0x31E4;
    public const Int32 m_iFOVStart = 0x31E8;
    public const Int32 m_iGlowIndex = 0xA3F8;
    public const Int32 m_iHealth = 0x100;
    public const Int32 m_iItemDefinitionIndex = 0x2FAA;
    public const Int32 m_iItemIDHigh = 0x2FC0;
    public const Int32 m_iObserverMode = 0x3374;
    public const Int32 m_iShotsFired = 0xA370;
    public const Int32 m_iState = 0x3238;
    public const Int32 m_iTeamNum = 0xF4;
    public const Int32 m_lifeState = 0x25F;
    public const Int32 m_nFallbackPaintKit = 0x31B8;
    public const Int32 m_nFallbackSeed = 0x31BC;
    public const Int32 m_nFallbackStatTrak = 0x31C4;
    public const Int32 m_nForceBone = 0x268C;
    public const Int32 m_nTickBase = 0x342C;
    public const Int32 m_rgflCoordinateFrame = 0x444;
    public const Int32 m_szCustomName = 0x303C;
    public const Int32 m_szLastPlaceName = 0x35B0;
    public const Int32 m_thirdPersonViewAngles = 0x31D8;
    public const Int32 m_vecOrigin = 0x138;
    public const Int32 m_vecVelocity = 0x114;
    public const Int32 m_vecViewOffset = 0x108;
    public const Int32 m_viewPunchAngle = 0x3020;
}
public static class signatures
{
    public const Int32 clientstate_choked_commands = 0x4CB0;
    public const Int32 clientstate_delta_ticks = 0x174;
    public const Int32 clientstate_last_outgoing_command = 0x4CAC;
    public const Int32 clientstate_net_channel = 0x9C;
    public const Int32 convar_name_hash_table = 0x2F0F8;
    public const Int32 dwClientState = 0x58ACFC;
    public const Int32 dwClientState_GetLocalPlayer = 0x180;
    public const Int32 dwClientState_IsHLTV = 0x4CC8;
    public const Int32 dwClientState_Map = 0x28C;
    public const Int32 dwClientState_MapDirectory = 0x188;
    public const Int32 dwClientState_MaxPlayer = 0x310;
    public const Int32 dwClientState_PlayerInfo = 0x5240;
    public const Int32 dwClientState_State = 0x108;
    public const Int32 dwClientState_ViewAngles = 0x4D10;
    public const Int32 dwEntityList = 0x4CCDC3C;
    public const Int32 dwForceAttack = 0x30FF2E0;
    public const Int32 dwForceAttack2 = 0x30FF2EC;
    public const Int32 dwForceBackward = 0x30FF328;
    public const Int32 dwForceForward = 0x30FF334;
    public const Int32 dwForceJump = 0x5170DF0;
    public const Int32 dwForceLeft = 0x30FF34C;
    public const Int32 dwForceRight = 0x30FF340;
    public const Int32 dwGameDir = 0x630E70;
    public const Int32 dwGameRulesProxy = 0x51E3124;
    public const Int32 dwGetAllClasses = 0xCE19AC;
    public const Int32 dwGlobalVars = 0x58AA00;
    public const Int32 dwGlowObjectManager = 0x520DA80;
    public const Int32 dwInput = 0x51189D0;
    public const Int32 dwInterfaceLinkList = 0x89E3E4;
    public const Int32 dwLocalPlayer = 0xCBD6A4;
    public const Int32 dwMouseEnable = 0xCC31F0;
    public const Int32 dwMouseEnablePtr = 0xCC31C0;
    public const Int32 dwPlayerResource = 0x30FD69C;
    public const Int32 dwRadarBase = 0x510297C;
    public const Int32 dwSensitivity = 0xCC308C;
    public const Int32 dwSensitivityPtr = 0xCC3060;
    public const Int32 dwSetClanTag = 0x895C0;
    public const Int32 dwViewMatrix = 0x4CBF654;
    public const Int32 dwWeaponTable = 0x5119494;
    public const Int32 dwWeaponTableIndex = 0x323C;
    public const Int32 dwYawPtr = 0xCC2E50;
    public const Int32 dwZoomSensitivityRatioPtr = 0xCC8090;
    public const Int32 dwbSendPackets = 0xD210A;
    public const Int32 dwppDirect3DDevice9 = 0xA3FC0;
    public const Int32 interface_engine_cvar = 0x3E9EC;
    public const Int32 m_bDormant = 0xED;
    public const Int32 m_pStudioHdr = 0x294C;
    public const Int32 m_pitchClassPtr = 0x5102C30;
    public const Int32 m_yawClassPtr = 0xCC2E50;
    public const Int32 model_ambient_min = 0x58DD1C;
}
