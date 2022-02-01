using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;

namespace Vibrodiagnostic
{
    public class MyHookClass : NativeWindow
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint RegisterWindowMessage(string lpString);

        uint simpr, simpr2;
        public Status solver;
        Form1 f;
        public MyHookClass(IntPtr hWnd, Form1 form)
        {
            //simpr = 49917;
            //simpr2 = 50054;
            simpr = RegisterWindowMessage("MyMessage");
            simpr2 = RegisterWindowMessage("MyMessage1");
            this.AssignHandle(hWnd);
            solver = new Status();
            f = form;
        }      
        protected override void WndProc(ref Message m) // в эту функцию приходят все сообщения от СИМПРА
        {
            int wparamhi, wparamlo, wparam;
            long lParam = Convert.ToInt64("" + m.LParam);

            if (m.Msg == simpr)
            {
                wparam = Convert.ToInt32("" + m.WParam);
                wparamhi = wparam / 65536;
                wparamlo = wparam - wparamhi * 65536;

                    if (wparamlo == 1)// таблица 1 
                    {
                        #region условия таблицы 1 - РРПН
                        switch (lParam)
                        {
                            case 1: 
                                {
                                if (solver.trend)
                                        m.Result = new IntPtr(1);
                                    else
                                        m.Result = new IntPtr(0);
                                }
                                break;
                            case 2: 
                                {
                                    if (solver.time_Less_24)
                                        m.Result = new IntPtr(1);
                                    else
                                        m.Result = new IntPtr(0);
                                }
                                break;
                            case 3: 
                                {
                                    if (solver.regr_out_of_range)
                                        m.Result = new IntPtr(1);
                                    else
                                        m.Result = new IntPtr(0);
                                }
                                break;
                            case 4: 
                                {
                                    if (solver.antiphase_vect)
                                        m.Result = new IntPtr(1);
                                    else
                                        m.Result = new IntPtr(0);
                                }
                                break;
                        }
                        #endregion
                    }
                    else if (wparamlo == 3)// таблица 2 
                    {
                        #region условия таблицы 2 
                        switch (lParam)
                        {
                            case 1: 
                                {
                                    if (solver.not_continue)
                                        m.Result = new IntPtr(1);
                                    else
                                        m.Result = new IntPtr(0);
                                }
                                break;

                        }
                        #endregion
                    }
                    else if (wparamlo == 2)// таблица 3 
                    {
                        #region условия таблицы 3
                        switch (lParam)
                        {
                            case 1: // Уровни 1-х составляющих вибрации на основных критических частотах вращения выходят за пределы допустимых значений
                                {
                                    if (solver.levels_1_comp)
                                        m.Result = new IntPtr(1);
                                    else
                                        m.Result = new IntPtr(0);
                                }
                                break;
                            case 2: // Уровни 2-х составляющих вибрации на критических частотах вращения 2-го рода выходят за пределы допустимых значений
                                {
                                    if (solver.levels_2_comp)
                                        m.Result = new IntPtr(1);
                                    else
                                        m.Result = new IntPtr(0);
                                }
                                break;

                        }
                        #endregion
                    }
                    if (wparamlo == 4)// таблица 4 
                    {
                        #region условия таблицы 4
                        switch (lParam)
                        {
                            case 1:
                                {
                                    if (solver.not_continue)
                                        m.Result = new IntPtr(1);
                                    else
                                        m.Result = new IntPtr(0);
                                }
                                break;
                        }
                        #endregion
                    }
            }
            else
            {
                if (m.Msg == simpr2)
                {
                    wparam = Convert.ToInt32("" + m.WParam);
                    wparamhi = wparam / 65536;
                    wparamlo = wparam - wparamhi * 65536;

                    if (wparamlo == 1)// таблица 1 
                    {
                        solver.not_continue = true;
                        f.DrawRVDMuftpercent("");
                        //f.DrawRSDMuftpercent("");
                        #region действия таблицы 1 - РППН
                        switch (lParam)
                        {
                            case 1: // Подозрение на обрыв болтов муфты РВД/РСД
                                {
                                    solver.answer = "Переход к анализу в режиме выбега \n";

                                    solver.overswing = true;
                                    f.textBox1.Text += "Перейти к анализу на режиме выбега\n" + Environment.NewLine;

                                    f.DrawMuftRVD(Color.Yellow); //все еще норм, но был желтый
                                }
                                break;
                            case 2:
                                {
                                    solver.answer = "Продолжается анализ на режиме работы под нагрузкой...";

                                    f.DrawMuftRVD(Color.Green);
                                    f.DrawRVDMuftpercent("");

                                    solver.overswing = false;

                                    solver.not_continue = false; //да!

                                    f.textBox1.Text = "Продолжить анализ на режиме работы под нагрузкой\n" + Environment.NewLine;
                                }
                                break;
                            case 3:  // 40%
                                {

                                    solver.answer = "Сделан вывод о возможности обрыва болтов со степенью уверенности 90%\n"; //Сделан вывод о возможности обрыва болтов со степенью уверенности 90%
                                    solver.overswing = true;

                                    solver.not_continue = true;
                                    if (f.textBox1.Text.Contains("Сделан вывод")) f.textBox1.Text = "";
                                    f.textBox1.Text += "Сделан вывод о возможности обрыва болтов со степенью уверенности 90%\n" + Environment.NewLine; //Вероятность обрыва болтов муфты - 90 %

                                    //f.DrawRVDMuftpercent("90%");
                                }
                                break;
                            case 4: // 35%
                                {

                                    solver.answer = "Сделан вывод о возможности обрыва болтов со степенью уверенности 50%\n";
                                    solver.overswing = true;
                                    if (f.textBox1.Text.Contains("Сделан вывод")) f.textBox1.Text = "";
                                    f.textBox1.Text += "Сделан вывод о возможности обрыва болтов со степенью уверенности 50%\n" + Environment.NewLine;

                                    //f.DrawRVDMuftpercent("50%");
                                }
                                break;
                            case 5: // 50%
                                {

                                    solver.answer = "Сделан вывод о возможности обрыва болтов со степенью уверенности 40%";
                                    solver.overswing = true;
                                    if (f.textBox1.Text.Contains("Сделан вывод")) f.textBox1.Text = "";
                                    f.textBox1.Text += "Сделан вывод о возможности обрыва болтов со степенью уверенности 40%" + Environment.NewLine;
                                    //f.DrawRVDMuftpercent("40%");
                                }
                                break;
                            case 6: // 30%
                                {

                                    solver.answer = "Сделан вывод о возможности обрыва болтов со степенью уверенности 35%";
                                    solver.overswing = true;
                                    if (f.textBox1.Text.Contains("Сделан вывод")) f.textBox1.Text = "";
                                    f.textBox1.Text += "Сделан вывод о возможности обрыва болтов со степенью уверенности 35%" + Environment.NewLine;

                                    //f.DrawRVDMuftpercent("35%");
                                }
                                break;
                            case 7: // 20%
                                {

                                    solver.answer = "Сделан вывод о возможности обрыва болтов со степенью уверенности 30%\n";
                                    solver.overswing = true;
                                    if (f.textBox1.Text.Contains("Сделан вывод")) f.textBox1.Text = "";
                                    f.textBox1.Text += "Сделан вывод о возможности обрыва болтов со степенью уверенности 30%\n" + Environment.NewLine;

                                    //f.DrawRVDMuftpercent("30%");
                                }
                                break;
                            case 8: // 10%
                                {

                                    solver.answer = "Сделан вывод о возможности обрыва болтов со степенью уверенности 20%";
                                    solver.overswing = true;
                                    if (f.textBox1.Text.Contains("Сделан вывод")) f.textBox1.Text = "";
                                    f.textBox1.Text += "Сделан вывод о возможности обрыва болтов со степенью уверенности 20%" + Environment.NewLine;

                                    //f.DrawRVDMuftpercent("20%");
                                }
                                break;
                            case 9:
                                {
                                    solver.answer = "Сделан вывод о возможности обрыва болтов со степенью уверенности 10%";
                                    solver.overswing = true;
                                    if (f.textBox1.Text.Contains("Сделан вывод")) f.textBox1.Text = "";
                                    f.textBox1.Text += "Сделан вывод о возможности обрыва болтов со степенью уверенности 10%" + Environment.NewLine;

                                    //f.DrawRVDMuftpercent("10%");
                                }
                                break;
                        }
                        #endregion
                        f.textBox2.Text = ""; // ОЧИЩЕНИЕ ПОЛЯ ПРО ВЫБЕГ, КОГДА НОВАЯ ДИАГНОСТИКА
                    }
                    else if (wparamlo == 2)// таблица 2 
                    {
                        solver.not_continue = true;
                        f.DrawRVDMuftpercent("");
                        //f.DrawRSDMuftpercent("");
                        #region действия таблицы 2
                        switch (lParam)
                        {
                            case 1: // Обрыв болтов муфты
                                {
                                    solver.overswing = false;

                                    solver.answer = "Сделан вывод о наличии обрыва болтов муфты со степенью уверенности 60%"; //Вероятность наличия обрыва болтов муфты РВД-РСД 60%

                                    f.DrawMuftRVD(Color.Red);
                                    //f.DrawRVDMuftpercent("60%");

                                    f.textBox2.Text += "Сделан вывод о наличии обрыва болтов муфты РВД-РСД со степенью уверенности 60%" + Environment.NewLine;
                                }
                                break;
                            case 2:
                                {
                                    //f.DrawRVDMuftpercent("30%");
                                    f.DrawMuftRVD(Color.Red);

                                    solver.answer = "Сделан вывод о наличии обрыва болтов муфты со степенью уверенности 30%";
                                    solver.overswing = true;
                                    f.textBox2.Text += "Сделан вывод о наличии обрыва болтов муфты РВД-РСД со степенью уверенности 30%" + Environment.NewLine;
                                    solver.not_continue = true;
                                }
                                break;
                            case 3:
                                {
                                    //f.DrawRVDMuftpercent("20%");
                                    f.DrawMuftRVD(Color.Red);

                                    solver.answer = "Сделан вывод о наличии обрыва болтов муфты со степенью уверенности 20%";
                                    solver.overswing = true;
                                    f.textBox2.Text += "Сделан вывод о наличии обрыва болтов муфты РВД-РСД со степенью уверенности 20%" + Environment.NewLine;
                                }
                                break;
                            case 4:
                                {
                                    //f.DrawRVDMuftpercent("не выявлена");
                                    solver.answer = "Подозрение на обрыв болтов, продолжается анализ в режиме выбега...";
                                    solver.overswing = true;
                                    f.textBox2.Text = "Продолжить анализ в режиме выбега" + Environment.NewLine;
                                    solver.not_continue = false;
                                }
                                break;
                        }
                        #endregion
                    }
                    else if (wparamlo == 3)// таблица 3 
                    {
                        #region действия таблицы 3 
                        switch (lParam)
                        {
                            case 1:
                                {
                                    //solver.overswing = true;
                                }
                                break;
                            case 2:
                                {
                                    //solver.overswing = true;
                                }
                                break;
                        }
                        #endregion
                    }
                    else if (wparamlo == 4)// таблица 4 
                    {
                        #region действия таблицы 4
                        switch (lParam)
                        {
                            case 1:
                                {
                                    //solver.overswing = false;
                                }
                                break;
                            case 2:
                                {
                                    //solver.overswing = false;
                                }
                                break;
                        }
                        #endregion
                    }
                    f.label1.Text = "Состояние: " + solver.answer; //"Результат диагностики: " + 

                    Application.DoEvents();
                    
                    Thread.Sleep(500); // если у нас есть визуальное отображение, то задержку можно установить здесь      

                    m.Result = new IntPtr(1); // ответом на запрос действия со стороны СИМПР должна быть единица

                }
                else base.WndProc(ref m); // для всех действий не связанных с СИМПР возвращаем управление программе
            }

        } 
    }

}
