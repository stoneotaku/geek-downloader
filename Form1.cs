using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;

namespace geek_downloader
{
    public struct sub_article
    {
        public string title;
        public string id;
        public string mp3_url;
        public string mp4_url;
        public string markdown;
    }
    public struct article
    {
        public sub_article[] sub_Articles;
        public string title;
        public int num;
    }
    public struct coursera
    {
        public article[] articles;
        public string title;
        public string sku;
        public int n;
    }
    public struct courseras
    {
        public coursera[] Courses;
        public int count;
        public string cookie;
        public string user;
        public string phone;
    }
    public partial class Form1 : Form
    {
        public static string geekid;
        public static CoreWebView2HttpRequestHeaders headers;
        public static ulong login_navid = 999999;
        public const string ua = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36";
        public static string username = "";
        public static string userphone = "";
        public static string cookies = null;
        public static string savefolder = "";



        public courseras Courseras = new courseras();
        public Form1()
        {
            InitializeComponent();
            //Control.CheckForIllegalCrossThreadCalls = false;
        }
        private void getUserInfo()
        {
            if (cookies == null || cookies == "")
            {
                Console.WriteLine("cookie is empty");
                return;
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://b.geekbang.org/app/v1/mine/page");
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Referer = "https://b.geekbang.org/member/user";
            request.UserAgent = ua;
            request.Headers.Add("Origin", "https://b.geekbang.org");
            request.Headers.Add("Cookie", cookies);

            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var result = streamReader.ReadToEnd();
                    JObject jo = JObject.Parse(result);
                    Console.WriteLine(jo.ToString());
                    if ((string)jo["data"]["user_info"]["es_nick_name"] != null)
                    {

                        username = (string)jo["data"]["user_info"]["es_nick_name"];
                        userphone = (string)jo["data"]["user_info"]["cellphone"];
                    }
                }
                else if (((int)httpResponse.StatusCode) == 452)
                {
                    System.IO.File.Delete("cookie.txt");
                    DialogResult dialogResult = MessageBox.Show("Downloader encountered an error", "Error!", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //do something
                        Application.Exit();
                    }
                }

                if (username != "")
                {
                    label_name.Text = username;
                    label_phone.Text = userphone;
                    label_login.Text = "ON";
                    label_login.ForeColor = Color.Green;
                }
                miniWebView();
                return;

            }
        }
        private void miniWebView()
        {
            /*
            for(int i = webView21.Width ; i >= 0; i--)
            {
                webView21.Width = i - 20;
                webView21.Height = i - 20;
            }*/
            webView21.Visible = false;
        }
        private void showListView()
        {
            listView_course.Visible = true;

            listView_course.Columns.Add("sku", 60, HorizontalAlignment.Left);
            listView_course.Columns.Add("course", 200, HorizontalAlignment.Left);
            listView_course.Columns.Add("article", 600, HorizontalAlignment.Left);
            listView_course.Columns.Add("title", 100, HorizontalAlignment.Left);
            listView_course.Columns.Add("url", 100, HorizontalAlignment.Left);

        }
        private void getArticle(string articleid, int index, ref sub_article sub)
        {
            string content = "{\"article_id\":\"" + articleid + "\"}";
            Console.WriteLine(content);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://b.geekbang.org/app/v1/article/detail");
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Referer = "https://b.geekbang.org/member/course/detail/" + articleid;
            request.UserAgent = ua;
            request.Headers.Add("Origin", "https://b.geekbang.org");
            request.Headers.Add("Cookie", cookies);
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(content);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                Console.WriteLine("get article httpResponse.StatusCode:" + httpResponse.StatusCode);
                Console.WriteLine("###############################");
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var result = streamReader.ReadToEnd();
                    JObject jo = JObject.Parse(result);
                    Console.WriteLine(jo.ToString());

                    var audio = jo["data"]["audio"];
                    var video = jo["data"]["video"];
                    var article = jo["data"]["article"];
                    string title = (string)article["title"];
                    string markdown = (string)article["content_md"];
                    string mp3_url = (string)audio["download_url"];
                    string mp4_url = (string)video["download_url"];

                    sub.mp3_url = mp3_url;
                    sub.mp4_url = mp4_url;
                    sub.markdown = markdown;
                    title = title.Replace("|", "-");
                    title = title.Replace("?", "£¿");
                    title = title.Replace("/", "");

                    sub.title = title;
                    //this.listView_course.BeginUpdate();
                    //MessageBox.Show("sub:\n"+JsonConvert.SerializeObject(sub));

                    listView_course.Items[index].SubItems[2].Text = title;
                    listView_course.Items[index].SubItems[3].Text = mp3_url;
                    listView_course.Items[index].SubItems[4].Text = mp4_url;


                    //this.listView_course.EndUpdate();
                }
            }
        }
        private void getSingleCourse(string courseid, int groupid, ref coursera cour)
        {
            string content = "{\"id\":" + courseid + "}";
            Console.WriteLine(content);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://b.geekbang.org/app/v1/course/articles");
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Referer = "https://b.geekbang.org/member/user/lesson";
            request.UserAgent = ua;
            request.Headers.Add("Origin", "https://b.geekbang.org");
            request.Headers.Add("Cookie", cookies);
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(content);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                //MessageBox.Show("Single httpResponse.StatusCode:" + httpResponse.StatusCode);
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var result = streamReader.ReadToEnd();
                    JObject jo = JObject.Parse(result);
                    Console.WriteLine(jo.ToString());
                    if (jo["data"]["list"] != null)
                    {
                        var aritcles = jo["data"]["list"];
                        Console.WriteLine(aritcles);
                        Console.WriteLine("count:", aritcles.Count());

                        int i = 0;
                        cour.articles = new article[aritcles.Count()];
                        //Courseras.Courses[Courseras.count - 1].articles = new article[aritcles.Count()];
                        foreach (var article in aritcles)
                        {
                            cour.articles[i].title = (string)article["column_title"];
                            cour.n += 1;
                            //Courseras.Courses[Courseras.count - 1].articles[i].title = (string)article["column_title"];
                            //Courseras.Courses[Courseras.count - 1].n += i+1;
                            int j = 0;
                            int sub_count = article["article_list"].Count();
                            //MessageBox.Show("sub count:" + sub_count);
                            //Courseras.Courses[Courseras.count].articles[i].sub_Articles = new sub_article[sub_count];
                            cour.articles[i].sub_Articles = new sub_article[sub_count];

                            foreach (var sub_article in article["article_list"])
                            {
                                cour.articles[i].sub_Articles[j].id = (string)sub_article["id"];
                                cour.articles[i].num += 1;
                                //Courseras.Courses[Courseras.count - 1].articles[i].sub_Articles[j].id = (string)sub_article["id"];
                                //Courseras.Courses[Courseras.count - 1].articles[i].num += j+1;
                                Console.WriteLine("-------------------");
                                Console.WriteLine(sub_article);

                                ListViewItem lvi = new ListViewItem();

                                lvi.Text = ((string)sub_article["id"]);
                                lvi.SubItems.Add((string)sub_article["column_title"]);
                                lvi.SubItems.Add("N/A");
                                lvi.SubItems.Add("N/A");
                                lvi.SubItems.Add("N/A");

                                //group.Items.Add(lvi);
                                var item = listView_course.Items.Add(lvi);
                                getArticle((string)sub_article["id"], item.Index, ref cour.articles[i].sub_Articles[j]);
                                //ref Courseras.Courses[Courseras.count].articles[i].sub_Articles[j]);

                                item.Group = listView_course.Groups[groupid];
                                j++;
                            }

                            i++;
                        }
                    }
                }

                if (username != "")
                {
                    label_name.Text = username;
                    label_phone.Text = userphone;
                    label_login.Text = "ON";
                    label_login.ForeColor = Color.Green;
                }
                miniWebView();
                return;

            }
        }

        private void getCourseList()
        {
            while (cookies == null)
            {
                Thread.Sleep(1000);
            }
            //showListView();

            var content = "{\"course_type\":0,\"learn_status\":0,\"expire_status\":1,\"hide_status\":1,\"order_by\":\"learn\",\"sort\":\"\",\"total\":0,\"page\":1,\"size\":10}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://b.geekbang.org/app/v1/user/center/course");
            request.Method = "POST";
            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Referer = "https://b.geekbang.org/member/user/lesson";
            request.UserAgent = ua;
            request.Headers.Add("Origin", "https://b.geekbang.org");
            request.Headers.Add("Cookie", cookies);
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(content);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                //MessageBox.Show("StatusCode:" + httpResponse.StatusCode);
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var result = streamReader.ReadToEnd();
                    JObject jo = JObject.Parse(result);
                    if (jo["data"]["list"] != null)
                    {

                        var course_list = jo["data"]["list"];
                        Console.WriteLine(jo.ToString());
                        //this.listView_course.BeginUpdate();   
                        int count = course_list.Count();
                        Courseras.Courses = new coursera[count];
                        Courseras.cookie = cookies;
                        Courseras.user = username;
                        Courseras.phone = userphone;

                        int i = 0;
                        foreach (var course in course_list)
                        {

                            ListViewGroup group = new ListViewGroup((string)course["sku"] + (string)course["title"], HorizontalAlignment.Center);
                            //group.Items.Add(lvi);
                            group.Name = (string)course["title"];
                            //group.Header = "this is header";
                            group.Tag = "this is tag";

                            var groupid = listView_course.Groups.Add(group);

                            Courseras.count += 1;
                            Courseras.Courses[i].title = (string)course["title"];
                            Courseras.Courses[i].sku = (string)course["sku"];
                            //MessageBox.Show("courseras:\n" + JsonConvert.SerializeObject(Courseras));

                            getSingleCourse((string)course["sku"], groupid, ref Courseras.Courses[i]);
                            i++;
                            //listView_course.Items.Add(lvi);   

                        }
                        //this.listView_course.EndUpdate();
                    }

                }

                if (username != "")
                {
                    label_name.Text = username;
                    label_phone.Text = userphone;
                    label_login.Text = "ON";
                    label_login.ForeColor = Color.Green;
                }
                miniWebView();
                return;

            }
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            //try to load cookie
            //if cookie is null or is expire, show webview
            cookies = loadCookie("cookie.txt");
            if (cookies != null)
            {
                //test cookie
                getUserInfo();
            }
            this.webView21.Source = new Uri("https://b.geekbang.org");
            await webView21.EnsureCoreWebView2Async(null);
            this.webView21.CoreWebView2.Navigate("https://account.geekbang.org/member/signin?redirect=https%3A%2F%2Fb.geekbang.org%2Fmember%2F");


            webView21.CoreWebView2.AddWebResourceRequestedFilter("*",
                                               CoreWebView2WebResourceContext.All);
            webView21.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;

            showListView();
            //Thread thread = new Thread(getCourseList);
            //thread.IsBackground = true;
            //thread.Start();

            backgroundWorker1.RunWorkerAsync();
        }
        private string loadCookie(string fileName)
        {
            if (System.IO.File.Exists(fileName))
            {
                FileStream fs = System.IO.File.OpenRead(fileName);
                StreamReader reader = new StreamReader(fs);
                string cookie_str = reader.ReadToEnd();
                reader.Close();
                fs.Close();
                return cookie_str;
            }
            else return null;

        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            string cookie = await getcookieAsync(webView21.CoreWebView2.CookieManager);
            webView21.CoreWebView2.AddWebResourceRequestedFilter("https://b.geekbang.org/app/v1/*",
                                               CoreWebView2WebResourceContext.All);
            webView21.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;
            MessageBox.Show(cookie);


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "txt";
            if (DialogResult.OK == saveFileDialog.ShowDialog())
            {
                MessageBox.Show(saveFileDialog.FileName);
                //FileStream fs = System.IO.File.OpenWrite(saveFileDialog.FileName);
                StreamWriter writer = new StreamWriter(saveFileDialog.FileName, false);
                writer.Write(cookie);
                writer.Flush();
                writer.Close();
                //fs.Close();

                //FileStream fs2 = System.IO.File.OpenWrite(saveFileDialog.FileName+".h");
                StreamWriter writer2 = new StreamWriter(saveFileDialog.FileName + ".h", false);
                writer2.Write(geekid);
                writer2.Flush();
                writer2.Close();
                //fs2.Close();
            }

            //webView21.CoreWebView2.WebResourceRequested += CoreWebView2_WebResourceRequested;


        }

        private void CoreWebView2_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            foreach (var header in e.Request.Headers)
            {
                if (string.IsNullOrEmpty(header.Value) ||
                    header.Key.StartsWith("sec-ch")) continue;
                if (header.Key == "X-GEEK-REQ-ID")
                {
                    //MessageBox.Show("Name:" + header.Key + " Value:" + header.Value);
                    geekid = header.Value;
                }

            }
            headers = e.Request.Headers;
            //if (e.Request.Headers.Contains("X-GEEK-REQ-ID"))
            //    MessageBox.Show(e.Request.Headers.GetHeader("X-GEEK-REQ-ID"));
            //throw new NotImplementedException();
        }

        private void CoreWebView2_WebResourceResponseReceived(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            if (e.Response != null && e.Response.Headers.Contains("X-GEEK-REQ-ID"))
            {
                int statusCode = e.Response.StatusCode;
                string header = e.Response.Headers.GetHeader("X-GEEK-REQ-ID");
                //MessageBox.Show(header);
            }
        }

        private static async Task<string> getcookieAsync(CoreWebView2CookieManager cookieManager)
        {
            string cookie_str = "";
            var cookies = await GetCookieManager(cookieManager).GetCookiesAsync("https://b.geekbang.org");
            foreach (var cookie in cookies)
            {
                //MessageBox.Show($"Cookie: {cookie.Name} = {cookie.Value}");
                //Console.WriteLine($"Cookie: {cookie.Name} = {cookie.Value}");
                cookie_str += cookie.Name + "=" + cookie.Value + "; ";
            }

            return cookie_str;
        }


        private static CoreWebView2CookieManager GetCookieManager(CoreWebView2CookieManager cookieManager)
        {
            return cookieManager;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cookie_str = "";
            //MessageBox.Show("load cookie");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Load cookie";
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                FileStream fs = System.IO.File.OpenRead(openFileDialog.FileName);
                StreamReader reader = new StreamReader(fs);
                cookie_str = reader.ReadToEnd();
                reader.Close();
                fs.Close();

                FileStream fs2 = System.IO.File.OpenRead(openFileDialog.FileName + ".h");
                StreamReader reader2 = new StreamReader(fs2);
                geekid = reader2.ReadToEnd();
                reader2.Close();
                fs2.Close();
            }
            //MessageBox.Show(cookie_str);
            //MessageBox.Show(geekid);
            Console.WriteLine("Cookie:" + cookie_str);



            var content = "{\"course_type\":0,\"learn_status\":0,\"expire_status\":1,\"hide_status\":1,\"order_by\":\"learn\",\"sort\":\"\",\"total\":0,\"page\":1,\"size\":10}";


            CookieContainer cookies = new CookieContainer();
            Uri uri = new Uri("https://b.geekbang.org");
            cookies.SetCookies(uri, cookie_str);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://b.geekbang.org/app/v1/user/center/course");
            request.Method = "POST";

            request.ContentType = "application/json; charset=UTF-8";
            request.Accept = "application/json";
            request.ContentType = "application/json";
            request.Referer = "https://b.geekbang.org/member/user/lesson";
            request.UserAgent = ua;
            request.Headers.Add("Origin", "https://b.geekbang.org");
            request.Headers.Add("Cookie", cookie_str);
            //request.CookieContainer = cookies;
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(content);
                streamWriter.Flush();
                streamWriter.Close();
            }
            var httpResponse = (HttpWebResponse)request.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();

                //MessageBox.Show(result);
                return;

            }

#if false
            var handler = new HttpClientHandler
            {
                CookieContainer = cookies,
                UseCookies = true, // Ensure that the handler uses the CookieContainer
            };
            //HttpClientHandler handler = new HttpClientHandler();


            //handler.CookieContainer = cookies;
            //handler.UseCookies = true;

            HttpClient client = new HttpClient(handler);

            client.BaseAddress = new Uri("https://b.geekbang.org");
            /*
            foreach (var head in headers)
            {
                if ( head.Key.StartsWith("sec")|| head.Key.StartsWith("Sec"))
                    client.DefaultRequestHeaders.Add(head.Key, head.Value);
            }
            */
            
            client.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
            //client.DefaultRequestHeaders.Add("Cookie", cookie_str);
            client.DefaultRequestHeaders.Add("Referer", "https://b.geekbang.org/member/user/lesson");
            client.DefaultRequestHeaders.Add("Origin", "https://b.geekbang.org");
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            

            //client.DefaultRequestHeaders.Add("X-GEEK-REQ-ID", geekid);
            //const string ua = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36 Edg/123.0.0.0";
            client.DefaultRequestHeaders.Add("User-Agent",ua);
            
            //content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            var message = new HttpRequestMessage(HttpMethod.Post, "https://b.geekbang.org/app/v1/user/center/course");
            
            message.Content = content;
            message.Headers.Add("Accept", "application/json, text/plain, */*");
           // message.Headers.Add("Cookie",  cookie_str);
            message.Headers.Add("Referer", "https://b.geekbang.org/member/user/lesson");
            message.Headers.Add("Origin", "https://b.geekbang.org");
            message.Headers.Add("User-Agent", ua);
            //message.Headers.Add("Content-Type", "application/json");
            MessageBox.Show(content.ReadAsStringAsync().Result);

            //HttpResponseMessage resp = client.PostAsync("app/v1/user/center/course", content).Result;
            HttpResponseMessage resp = client.PostAsync("http://172.16.17.213:8880", content).Result;
            string responseBody = resp.Content.ReadAsStringAsync().Result;
            MessageBox.Show(responseBody);

#endif

        }

        private void webView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.NavigationId == login_navid)
            {
                //show login status = true
                label_login.Text = "ON";
                label_login.ForeColor = Color.Green;
                _ = saveCookieAsync("cookie.txt");
                //string cookie = loadCookie("cookie.txt");

                miniWebView();
                showListView();
            }
        }
        private async Task saveCookieAsync(string fileName)
        {
            string ck = await getcookieAsync(webView21.CoreWebView2.CookieManager);

            if (ck != null)
            {
                StreamWriter writer = new StreamWriter(fileName, false);
                writer.Write(ck);
                writer.Flush();
                writer.Close();
            }
            cookies = ck;
            getUserInfo();
        }
        private void webView21_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {

        }

        private void webView21_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            Console.WriteLine("navid:" + e.NavigationId + ",uri:" + e.Uri.ToString());
            if (e.Uri.StartsWith("https://account.geekbang.com"))
            {
                login_navid = e.NavigationId;
            }
        }

        private void listView_course_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sender.Equals(listView_course.Groups[0]))
            {
                MessageBox.Show("group 0 double clicked");
            }
            //MessageBox.Show(e.ToString());
        }

        private void listView_course_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            //MessageBox.Show(sender.ToString());
        }

        private void button_download_Click(object sender, EventArgs e)
        {

            if (savefolder == "")
            {
                chooseFolder();
            }
            if (savefolder != "")
            {
                //start download
                MessageBox.Show("Begin download to " + savefolder);
                progressBar_download.Visible = true;

                //backgroundWorker_downloader.RunWorkerAsync();
                downloader_work();
            }

        }

        private void button_save_Click(object sender, EventArgs e)
        {
            chooseFolder();
            //button_save.Visible = false;
        }
        private void chooseFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Downloader Will Save Files To This Folder!";
            if (DialogResult.OK == dialog.ShowDialog())
            {
                savefolder = dialog.SelectedPath;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            getUserInfo();
            getCourseList();
            //MessageBox.Show("do work");

            string str = JsonConvert.SerializeObject(Courseras);
            StreamWriter writer = new StreamWriter("course.db", false);
            writer.Write(str);
            writer.Flush();
            writer.Close();
            //button_download.Visible = true;
        }

        private void backgroundWorker_downloader_DoWork(object sender, DoWorkEventArgs e)
        { }
        private void downloader_work()
        {
            //do download work
            //BackgroundWorker worker = sender as BackgroundWorker;

            FileStream fs = System.IO.File.OpenRead("course.db");
            StreamReader reader = new StreamReader(fs);
            string course_db = reader.ReadToEnd();
            reader.Close();
            fs.Close();

            courseras Cour = JsonConvert.DeserializeObject<courseras>(course_db);



            int cour_num = Cour.Courses.Count();

            for (int i = 0; i < cour_num; i++)
            {
                string course_path = savefolder;
                string sku = Cour.Courses[i].sku + Cour.Courses[i].title;

                //course_path = savefolder + "/" + Cour.Courses[i].title;
                course_path = savefolder + "/" + sku;

                Console.WriteLine($"course_path:{course_path},sku:{sku},i:{i},count:{cour_num}");


                _ = Directory.CreateDirectory(course_path);

                //int article_num = Cour.Courses[i].n;
                if (Cour.Courses[i].articles == null) continue;
                int article_num = Cour.Courses[i].articles.Count();
                for (int j = 0; j < article_num; j++)
                {
                    Console.WriteLine($"in J loop, j={j},article_num = {article_num}");
                    if (Cour.Courses[i].articles[j].sub_Articles == null) continue;
                    int sub_num = Cour.Courses[i].articles[j].sub_Articles.Count();

                    //int sub_num = Cour.Courses[i].articles[j].num;
                    for (int k = 0; k < sub_num; k++)
                    {
                        //download single file
                        Console.WriteLine($"       in K loop, k={k},sub num = {sub_num}");
                        string article_title = Cour.Courses[i].articles[j].sub_Articles[k].title;
                        string mp3_path = course_path + "/" + article_title + ".mp3";
                        string mp4_path = course_path + "/" + article_title + ".mp4";
                        string md_path = course_path + "/" + article_title + ".md";
                        string mp3_url = Cour.Courses[i].articles[j].sub_Articles[k].mp3_url;
                        string mp4_url = Cour.Courses[i].articles[j].sub_Articles[k].mp4_url;
                        string markdown = Cour.Courses[i].articles[j].sub_Articles[k].markdown;
                        //MessageBox.Show("file:" + mp3_path);
                        if (System.IO.File.Exists(md_path))
                        {
                            continue;
                        }
                        WebClient client = new WebClient();
                        client.DownloadProgressChanged += (s, ee) =>
                        {
                            Console.WriteLine(ee.ProgressPercentage);
                            progressBar_download.Value = ee.ProgressPercentage;
                            progressBar_download.ResetText();
                            label_download.Visible = true;
                            label_download.Text = sku + " | " + article_title;
                            label_progress.Visible = true;
                            label_progress.Text = ee.ProgressPercentage + "%";
                            progressBar_download.Update();
                        };
                        if (mp3_url != null && mp3_url != "")
                        {
                            client.DownloadFileAsync(new Uri(mp3_url), mp3_path);
                        }
                        if (mp4_url != null && mp4_url != "")
                        {
                            client.DownloadFileAsync(new Uri(mp4_url), mp4_path);
                        }
                        if (markdown != null && markdown != "")
                        {
                            StreamWriter writer = new StreamWriter(md_path, false);
                            writer.Write(markdown);
                            writer.Flush();
                            writer.Close();
                        }

                        //download_file(worker,mp3_path, Cour.Courses[i].articles[j].sub_Articles[k].mp3_url);


                    }
                }




            }
        }
        private static void DownloadProgressCallback4(object sender, DownloadProgressChangedEventArgs e)
        {

        }

        public void download_file(BackgroundWorker worker, string path, string url)
        {
            if (url != null && url != "")
            {
                WebClient client = new WebClient();

                client.DownloadProgressChanged += (s, e) =>
                {
                    worker.ReportProgress(e.ProgressPercentage);
                };


                Console.WriteLine($"path:{path},url:{url}");
                client.DownloadFile(new Uri(url), path);
            }
            //Console.ReadLine();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Course Loading Completed");
            button_download.Visible = true;
        }

        private void backgroundWorker_downloader_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar_download.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_downloader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("download completed");
        }

    }
}