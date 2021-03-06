﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Windows.Forms.VisualStyles;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using Client.Models;
using Newtonsoft.Json;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        public string Encrypt(string strToEncrypt, string strKey)               //암호화
        {
            TripleDESCryptoServiceProvider objDESCrypto = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objHashMD5 = new MD5CryptoServiceProvider();
            byte[] byteHash, byteBuff;
            string strTempKey = strKey;
            byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey));
            objHashMD5 = null;
            objDESCrypto.Key = byteHash;
            objDESCrypto.Mode = CipherMode.ECB; //CBC, CFB
            byteBuff = ASCIIEncoding.ASCII.GetBytes(strToEncrypt);
            return Convert.ToBase64String(objDESCrypto.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length)); ;
        }

        public void Init()
        {
            ID_Box.Text = "아이디 입력";
            //PW_Box_Text.Visibility = Visibility.Visible;
            //PW_Box.Visibility = Visibility.Hidden;                      //원래대로
        }

        private void Forget_btn_Button_Click(object sender, RoutedEventArgs e)
        {
            // 비밀번호 찾기 기능
            // 추후에 아이디 찾기 기능 부가
        }

        private void Geust_Login_Button_Click(object sender, RoutedEventArgs e)
        {
            App.ID = "Guest";
            App.guest = true;
            App.MS.Show();                                              //메인 화면 띄우기
            this.Hide();                                                //로그인창 hide
            Init();
        }

        private void SingUP_btn_Click(object sender, RoutedEventArgs e)         //회원가입 화면 열기
        {
            SignUp SU = new SignUp();
            SU.Show();
        }

      
        private void Login_btn_Click(object sender, RoutedEventArgs e)
        {
            PW_Box.Opacity = 0;
            PW_Box_Text.Opacity = 100;
            Login_PW();
            Init();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)       // 프로그램 아무곳이나 누르면 lostfocus
        {
            if (ID_Box.IsFocused == true || PW_Box.IsFocused == true)
                temp.Focus();
        }


        private void Login_PW()
        {
            string urlBase = @"https://allcleapp.azurewebsites.net/api/Users"; //기본 url
            string url = null;  //json으로 쓰일 url
            url = urlBase + "/" + ID_Box.Text;                              //id 있는지 확인
            var json = new WebClient().DownloadData(url);
            string Unicode = Encoding.UTF8.GetString(json);
            if (Unicode == "true")                                          //있다
            {
                App.ID = ID_Box.Text;
                url = urlBase + "/" + App.ID;
                String postData = "";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);// 인코딩 UTF-8
                byte[] sendData = UTF8Encoding.UTF8.GetBytes(postData);
                httpWebRequest.ContentType = "application/json; charset=UTF-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = sendData.Length;
                Stream requestStream = httpWebRequest.GetRequestStream();
                requestStream.Write(sendData, 0, sendData.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
                Unicode = streamReader.ReadToEnd().ToString();
                User result = JsonConvert.DeserializeObject<User>(Unicode); //유저 정보 post로 가져온거
                streamReader.Close();
                httpWebResponse.Close();

                // result : 새로 암호화하기 전, DB에 있던 값
                string encryptedPW = Encrypt(PW_Box.Password, result.EncryptKey);   //비밀번호 암호화하기
                string id = result.Id;
                string yearofentry = result.YearOfEntry;
                string college = result.College;
                string major = result.Major;

                if (encryptedPW == result.Password)      //기존꺼랑 비교
                {
                    String callUrl = "http://allcleapp.azurewebsites.net/api/Users";
                    string setkey = null;
                    Random rand = new Random();
                    int len = rand.Next(6, 10);
                    for (int i = 0; i < len; i++)
                    {
                        setkey += (char)rand.Next(65, 122);  // 랜덤으로 대문자 암호화키 생성
                    }
                    string NewEncryptedPW = Encrypt(PW_Box.Password, setkey);

                    // NewEncryptedPW, setkey업데이트
                    // String NewpostData = String.Format("Password={0}&EncryptKey={1}&Id={2}", NewEncryptedPW, setkey, result.Id);
                    String NewpostData = "{ \"Password\" : \"" + NewEncryptedPW + "\", \"EncryptKey\" : \"" + setkey + "\", \"Id\" : \"" + id + "\", \"YearOfEntry\" : \"" + yearofentry + "\", \"College\" : \"" + college + "\", \"Major\" : \"" + major + "\"}";

                    int check_error = 1;
                    try
                    {
                        Login_Encrypt(NewpostData);
                        check_error = 0;
                    }
                    catch
                    {
                        check_error = 1;
                    }
                    if (check_error == 0)
                    {
                        App.ID = ID_Box.Text;
                        App.guest = false;
                        ID_Box.Text = "아이디 입력";
                        if (App.first)
                            App.MS.Show();
                        else
                            App.MS.Visibility = Visibility.Visible;
                        this.Hide();
                        ID_Box.Text = "";
                        PW_Box.Password = "";
                    }
                    else if (check_error != 0)
                    {
                        // 새로운 암호 저장에 문제가 발생.
                        // 기존 암호로 저장하자.
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("서버 접속에 문제가 있습니다. 종료 후 다시 이용해주십시오.");
                        //메세지창 띄우기
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            try
                            {
                                String NewpostData2 = "{ \"Password\" : \"" + result.Password + "\", \"EncryptKey\" : \"" + result.EncryptKey + "\", \"Id\" : \"" + id + "\", \"YearOfEntry\" : \"" + yearofentry + "\", \"College\" : \"" + college + "\", \"Major\" : \"" + major + "\"}";
                                try
                                {
                                    Login_Encrypt(NewpostData2);
                                }
                                catch{
                                    MessageBoxResult messageBoxResult2 = System.Windows.MessageBox.Show("비밀번호가 유실되었을 가능성이 큽니다. 비밀번호를 재 설정 하십시오..");
                                }

                                /*밑의 2줄이 try문 안에 있어야하는 기본 구문.*/
                                /*위는 계정 정보가 에러뜨는 문제를 해결하기 위한 실험. 문제가 계속되면 180~187줄은 지워야된다.*/
                                App.MS.Close();
                                this.Close();
                            }
                            catch
                            {
                                PW_Box.Password = "";
                                this.Close();
                            }
                        }
                    }
                }
                else
                    System.Windows.MessageBox.Show("잘못된 비밀번호입니다.");
            }
            else if (ID_Box.Text == "")                                      //id 입력을 안 했을 때
                System.Windows.MessageBox.Show("아이디를 입력해주세요");
            else                                                            //없다
                System.Windows.MessageBox.Show(ID_Box.Text + "는 존재하지 않는 아이디 입니다");
        }

        private void Login_Encrypt(String NewpostData)
        {
            String callUrl = "http://allcleapp.azurewebsites.net/api/Users";

            HttpWebRequest httpWebRequest2 = (HttpWebRequest)WebRequest.Create(callUrl);// 인코딩 UTF-8
            byte[] sendData2 = UTF8Encoding.UTF8.GetBytes(NewpostData);
            httpWebRequest2.ContentType = "application/json; charset=UTF-8";
            httpWebRequest2.Method = "PUT";

            httpWebRequest2.ContentLength = sendData2.Length;
            Stream requestStream2 = httpWebRequest2.GetRequestStream();
            requestStream2.Write(sendData2, 0, sendData2.Length);
            requestStream2.Close();
            HttpWebResponse httpWebResponse2 = (HttpWebResponse)httpWebRequest2.GetResponse();
            StreamReader streamReader2 = new StreamReader(httpWebResponse2.GetResponseStream(), Encoding.GetEncoding("UTF-8"));
            streamReader2.ReadToEnd();
            streamReader2.Close();
            httpWebResponse2.Close();
        }

        private void PW_Box_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Login_PW();
            }
        }
        
        private void ID_Box_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ID_Box.Text == "")
                ID_Box.Text = "아이디 입력";
        }

        private void ID_Box_GotFocus(object sender, RoutedEventArgs e)
        {
            if(ID_Box.Text == "아이디 입력")
                ID_Box.Text = "";
        }

        private void PW_Box_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PW_Box.Password == "")
            {
                //PW_Box.Visibility = Visibility.Collapsed;
                //PW_Box_Text.Visibility = Visibility.Visible;
                PW_Box.Opacity = 0;
                PW_Box_Text.Opacity = 100;
            }
            else if (PW_Box.Password != "")
            {
                //PW_Box_Text.Visibility = Visibility.Collapsed;
                PW_Box.Opacity = 100;
                PW_Box_Text.Opacity = 0;
            }
        }

        private void PW_Box_GotFocus(object sender, RoutedEventArgs e)
        {
            //PW_Box_Text.Visibility = Visibility.Collapsed;
            //PW_Box.Visibility = Visibility.Visible;
            PW_Box.Opacity = 100;
            PW_Box_Text.Opacity = 0;
        }
        //    <!--PW_Box_Text에 올라올때, 무조건 PW_Box를 보이게하고, PW_Box_Text를 안보이게-->
        //    <!--PW_Box에서 나갈 때 PW_Box.text==""이면 PW_Box를 숨기고, PW_Box_Text를 보이게-->
        //    <!--PW_Box에서 나갈 때 PW_Box.text!=""이면 PW_Box를 계속 보이도록-->
    }
}
